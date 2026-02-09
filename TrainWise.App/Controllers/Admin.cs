using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TrainWise.Core.Application.Interfaces;

namespace TrainWise.App.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly IExercisePopulationService _populationService;
        private readonly ILogger<SeedController> _logger;

        public SeedController(
            IExercisePopulationService populationService,
            ILogger<SeedController> logger)
        {
            _populationService = populationService;
            _logger = logger;
        }

        /// <summary>
        /// Popula o banco com todos os exercícios da API externa
        /// </summary>
        /// <param name="downloadMedia">Se deve fazer download e upload dos GIFs</param>
        /// <param name="limit">Limite de exercícios (null = todos)</param>
        [HttpPost("populate-exercises")]
        public async Task<IActionResult> PopulateExercises(
            [FromQuery] bool downloadMedia = true,
            [FromQuery] int? limit = null)
        {
            _logger.LogInformation(
                "Iniciando população de exercícios. DownloadMedia: {DownloadMedia}, Limit: {Limit}", 
                downloadMedia, 
                limit);

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
        /// Teste rápido - popula apenas 5 exercícios sem fazer upload de mídia
        /// </summary>
        [HttpPost("test-populate")]
        public async Task<IActionResult> TestPopulate()
        {
            _logger.LogInformation("Executando teste de população (5 exercícios, sem mídia)");

            var result = await _populationService.PopulateDatabaseAsync(
                downloadMedia: false, 
                limit: 5);

            return Ok(new
            {
                message = "Teste concluído",
                data = result
            });
        }
    }
}