using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using TrainWise.Core.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TrainWise.Core.Application.Dtos;
using TrainWise.Core.Domain.Entities.WorkoutComponents;
namespace Application.Services
{
    public class ExercisePopulationService : IExercisePopulationService
    {

        private readonly IExerciseApiClient _apiClient;
        private readonly ICloudStorageService _cloudStorage;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ILogger<ExercisePopulationService> _logger;

        public ExercisePopulationService(IExerciseApiClient apiClient, ICloudStorageService cloudStorage, IExerciseRepository exerciseRepository, ILogger<ExercisePopulationService> logger)
        {
            _apiClient = apiClient;
            _cloudStorage = cloudStorage;
            _exerciseRepository = exerciseRepository;
            _logger = logger;
        }

        public async Task<ExercisePopulationResult> PopulateDatabaseAsync(bool downloadMedia = true, int? limit = null)
        {
            var stopWatch = Stopwatch.StartNew();
            var result = new ExercisePopulationResult();

            try
            {
                _logger.LogInformation("Iniciando população de exercicios ...");

                var externalExercises = await _apiClient.GetAllExercisesAsync();

                if (limit.HasValue)
                {
                    externalExercises = externalExercises.Take(limit.Value).ToList();
                }
                result.TotalProcessed = externalExercises.Count();
                _logger.LogInformation($"Total de exercícios a processar: {result.TotalProcessed}");

                foreach (var externalExercise in externalExercises)
                {

                    try
                    {
                        await ProcessExerciseAsync(externalExercise, downloadMedia);
                        result.SuccessCount++;

                        _logger.LogInformation(
                        $"[{result.SuccessCount}/{result.TotalProcessed}] Exercício '{externalExercise.Name}' processado com sucesso");
                    }


                    catch (Exception ex)
                    {
                        result.FailureCount++;
                        var errorMsg = $"Erro ao processar exercício '{externalExercise.Name}': {ex.Message}";
                        result.Errors.Add(errorMsg);
                        _logger.LogError(ex, errorMsg);
                    }
                }
                stopWatch.Stop();
                result.Duration = stopWatch.Elapsed;

                _logger.LogInformation(
                    $"População concluída: {result.SuccessCount} sucessos, {result.FailureCount} falhas em {result.Duration.TotalSeconds:F2}s");

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro crítico na população de exercícios");
                stopWatch.Stop();
                result.Duration = stopWatch.Elapsed;
                result.Errors.Add($"Erro crítico: {ex.Message}");
                return result;
            }


        }

       public async Task<ExercisePopulationResult> PopulateExerciseAsync(string exerciseId)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = new ExercisePopulationResult { TotalProcessed = 1 };

            try
            {
                var externalExercise = await _apiClient.GetExerciseByIdAsync(exerciseId);
                await ProcessExerciseAsync(externalExercise, downloadMedia: true);
                
                result.SuccessCount = 1;
                stopwatch.Stop();
                result.Duration = stopwatch.Elapsed;
                
                return result;
            }
            catch (Exception ex)
            {
                result.FailureCount = 1;
                result.Errors.Add(ex.Message);
                stopwatch.Stop();
                result.Duration = stopwatch.Elapsed;
                
                _logger.LogError(ex, $"Erro ao processar exercício {exerciseId}");
                return result;
            }
        }


        private async Task ProcessExerciseAsync(ExternalExerciseDto externalExercise, bool downloadMedia)
        {
            var existing = await _exerciseRepository.GetByExternalIdAsync(externalExercise.Id);
            if (existing != null)
            {
                _logger.LogInformation($"Exercício '{externalExercise.Name}' já existe no banco. Pulando...");
                return;
            }

            var exercise = new Exercise(externalExercise.Name)
            {
                PrimaryMuscleGroup = (MuscleGroupType)MapMuscleGroup(externalExercise.Target),
                IsCompound = DetermineIfCompound(externalExercise),
                ExternalId = externalExercise.Id
            };

            if (externalExercise.SecondaryMuscles?.Any() == true)
            {
                foreach (var muscle in externalExercise.SecondaryMuscles)
                {
                    var muscleType = MapMuscleGroup(muscle);
                    if (muscleType.HasValue && !exercise.SecondaryMuscles.Contains(muscleType.Value))
                    {
                        exercise.SecondaryMuscles.Add(muscleType.Value);
                    }
                    ;
                }
            }
            if (downloadMedia)
            {
                try
                {
                    var gifBytes = await _apiClient.DownloadGifAsync(externalExercise.Id);

                    // UPLOUD CLOUDINARY
                    var gifFileName = $"{externalExercise.Id}.gif";
                    var gifUrl = await _cloudStorage.UploadGifAsync(gifBytes, gifFileName, "exercises/gifs");

                    exercise.SetMedia(gifUrl, gifUrl, MediaType.Gif);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"Erro ao fazer upload de mídia para '{externalExercise.Name}'. Salvando sem mídia.");
                }
            }
            await _exerciseRepository.AddAsync(exercise);
        }

         private MuscleGroupType? MapMuscleGroup(string target)
        {
            return target?.ToLower() switch
            {
                "abs" or "abdominals" => MuscleGroupType.Abs,
                "biceps" => MuscleGroupType.Biceps,
                "triceps" => MuscleGroupType.Triceps,
                "chest" or "pectorals" => MuscleGroupType.Chest,
                "back" or "lats" => MuscleGroupType.Back,
                "shoulders" or "delts" => MuscleGroupType.Shoulders,
                "quads" or "quadriceps" => MuscleGroupType.Quadriceps,
                "hamstrings" => MuscleGroupType.Hamstrings,
                "glutes" => MuscleGroupType.Glutes,
                "calves" => MuscleGroupType.Calves,
                "forearms" => MuscleGroupType.Forearms,
                "traps" or "upper back" => MuscleGroupType.Traps,
                "lower back" => MuscleGroupType.LowerBack,
                _ => null
            };
        }

        private bool DetermineIfCompound(ExternalExerciseDto exercise)
        {
            return exercise.SecondaryMuscles?.Count > 2;
        }
    }
}