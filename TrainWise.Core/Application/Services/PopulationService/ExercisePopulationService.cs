using System.Diagnostics;
using Application.Interfaces;
using Application.Services.PopulationService;
using Microsoft.Extensions.Logging;
using TrainWise.Core.Application.Dtos;
using TrainWise.Core.Application.Interfaces;

namespace TrainWise.Core.Application.Services
{
    public class ExercisePopulationService : IExercisePopulationService
    {
        private readonly IExerciseApiClient _apiClient;
        private readonly IExerciseRepository _repository;
        private readonly ExerciseFilterService _filterService;
        private readonly ExerciseMappingService _mappingService;
        private readonly ExerciseMediaService _mediaService;
        private readonly ILogger _logger;

        public ExercisePopulationService(IExerciseApiClient apiClient,IExerciseRepository repository,ExerciseFilterService filterService,ExerciseMappingService mappingService,ExerciseMediaService mediaService,ILogger logger)
        {
            _apiClient = apiClient;
            _repository = repository;
            _filterService = filterService;
            _mappingService = mappingService;
            _mediaService = mediaService;
            _logger = logger;
        }

        public async Task<ExercisePopulationResult> PopulateDatabaseAsync(
            bool downloadMedia = true,
            int? limit = null)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = new ExercisePopulationResult();

            try
            {
                _logger.LogInformation("Iniciando população de exercícios...");

                
                var externalExercises = await _apiClient.GetAllExercisesAsync();
                _logger.LogInformation($"Total de exercícios da API: {externalExercises.Count}");

                
                externalExercises = _filterService.ApplySmartFilter(externalExercises);

               
                if (limit.HasValue)
                {
                    externalExercises = externalExercises.Take(limit.Value).ToList();
                }

                result.TotalProcessed = externalExercises.Count;
                _logger.LogInformation($"Total de exercícios a processar: {result.TotalProcessed}");

              
                foreach (var externalExercise in externalExercises)
                {
                    try
                    {
                        await ProcessSingleExerciseAsync(externalExercise, downloadMedia);
                        result.SuccessCount++;

                        _logger.LogInformation(
                            $"[{result.SuccessCount}/{result.TotalProcessed}] '{externalExercise.Name}' processado");
                    }
                    catch (Exception ex)
                    {
                        result.FailureCount++;
                        var errorMsg = $"Erro ao processar '{externalExercise.Name}': {ex.Message}";
                        result.Errors.Add(errorMsg);
                        _logger.LogError(ex, errorMsg);
                    }
                }

                stopwatch.Stop();
                result.Duration = stopwatch.Elapsed;

                _logger.LogInformation(
                    $"População concluída: {result.SuccessCount} sucessos, " +
                    $"{result.FailureCount} falhas em {result.Duration.TotalSeconds:F2}s");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro crítico na população");
                stopwatch.Stop();
                result.Duration = stopwatch.Elapsed;
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
                await ProcessSingleExerciseAsync(externalExercise, downloadMedia: true);

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

        private async Task ProcessSingleExerciseAsync(ExternalExerciseDto externalExercise, bool downloadMedia)
        {
           
            if (await ExerciseAlreadyExists(externalExercise.Id))
            {
                _logger.LogInformation($"Exercício '{externalExercise.Name}' já existe. Pulando...");
                return;
            }

          
            var exercise = _mappingService.MapToEntity(externalExercise);

           
            if (downloadMedia)
            {
                await _mediaService.DownloadMediaAsync(exercise);
            }

           
            await _repository.AddAsync(exercise);
        }

        private async Task<bool> ExerciseAlreadyExists(string externalId)
        {
            var existing = await _repository.GetByExternalIdAsync(externalId);
            return existing != null;
        }
    }
}