using System.Diagnostics;
using Application.Interfaces;
using Application.Services.PopulationService;
using Microsoft.AspNetCore.Mvc;
using TrainWise.Core.Application.Interfaces;
using TrainWise.Core.Application.Services;

namespace TrainWise.App.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly ExercisePopulationService _populationService;

        private readonly ILogger<SeedController> _logger;

        private readonly IExerciseRepository _repository;
        private readonly ExerciseMediaService _mediaService;

        public SeedController(
            IExerciseApiClient apiClient,
            ICloudStorageService cloudStorage,
            IExerciseRepository repository,
            ILogger<SeedController> logger)
        {

            _logger = logger;
            _repository = repository;
            var filterService = new ExerciseFilterService(logger);
            var mappingService = new ExerciseMappingService();
            _mediaService = new ExerciseMediaService(apiClient, cloudStorage, logger);

            _populationService = new ExercisePopulationService(
            apiClient,
            repository,
            filterService,
            mappingService,
            _mediaService,
            logger);
        }


        /// <summary>
        /// Popula o banco com todos os exercícios da API externa
        /// </summary>
        /// <param name="downloadMedia">Se deve fazer download e upload dos GIFs</param>
        /// <param name="limit">Limite de exercícios (null = todos)</param>
        [HttpPost("populate-exercises")]
        public async Task<IActionResult> PopulateExercises(
            [FromQuery] bool downloadMedia = false,
            [FromQuery] int? limit = null)
        {
            _logger.LogInformation(
                "Iniciando população de exercícios. DownloadMedia: {DownloadMedia}, Limit: {Limit}", downloadMedia, limit);

            var result = await _populationService.PopulateDatabaseAsync(downloadMedia, limit);

            if (result.IsSuccess)
            {
                return Ok(new
                {
                    message = "População concluída com sucesso!",
                    data = result
                });
            }

            return BadRequest(new
            {
                message = "População concluída com erros",
                data = result
            });
        }

        /// <summary>
        /// Popula um exercício específico pelo ID externo
        /// </summary>
        [HttpPost("populate-exercise/{exerciseId}")]
        public async Task<IActionResult> PopulateExercise(string exerciseId)
        {
            _logger.LogInformation("Populando exercício: {ExerciseId}", exerciseId);

            var result = await _populationService.PopulateExerciseAsync(exerciseId);

            if (result.IsSuccess)
            {
                return Ok(new
                {
                    message = $"Exercício {exerciseId} populado com sucesso!",
                    data = result
                });
            }

            return BadRequest(new
            {
                message = $"Erro ao popular exercício {exerciseId}",
                data = result
            });
        }

        

        /// <summary>
        /// Aprova o seed de exercícios no banco
        /// </summary>
        [HttpPost("populate-approved")]
        public async Task<IActionResult> PopulateApprovedWithMedia()
        {
            var stopwatch = Stopwatch.StartNew();
            var successCount = 0;
            var failureCount = 0;
            var errors = new List<string>();

            try
            {
                var approvedData = await _repository.GetApprovedAsync();

                _logger.LogInformation($"Processando mídia para {approvedData.Count} exercícios aprovados");
                if (!approvedData.Any())
                {
                    return Ok(new
                    {
                        message = "nenhum exercicio foi aprovado",
                        totalProcess = 0,
                    });
                }

                foreach (var exercise in approvedData)
                {
                    try
                    {
                        _logger.LogInformation($"Baixando mídia para '{exercise.Name}'...");
                        var success = await _mediaService.DownloadMediaAsync(exercise);
                        if (success)
                        {
                            await _repository.UpdateAsync(exercise);
                            successCount++;
                        }
                        else
                        {
                            failureCount++;
                            errors.Add($"{exercise.Name}: Falha ao processar mídia");
                        }

                    }
                    catch (Exception ex)
                    {
                        failureCount++;
                        var errorMsg = $"{exercise.Name}: {ex.Message}";
                        errors.Add(errorMsg);
                        _logger.LogError(ex, $"Erro ao processar mídia de '{exercise.Name}'");
                    }
                }

                stopwatch.Stop();


                 return Ok(new
                {
                    message = "Processamento de mídia concluído",
                    totalProcessed = approvedData.Count,
                    successCount,
                    failureCount,
                    errors,
                    duration = stopwatch.Elapsed
                });
            } catch (Exception ex) {
                _logger.LogError(ex, "Erro crítico ao processar mídias");
                stopwatch.Stop();
                return StatusCode(500, new { error = ex.Message });

            }
                    
                

            }
        }
        
    }
