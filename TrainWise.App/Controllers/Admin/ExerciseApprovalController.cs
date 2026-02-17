using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TrainWise.App.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class ExerciseApprovalController : ControllerBase
    {
        private readonly ILogger<ExerciseApprovalController> _logger;
        private readonly IExerciseRepository _repository;

        public ExerciseApprovalController(ILogger<ExerciseApprovalController> logger, IExerciseRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// Lista Exercicios pendentes de aprovação
        /// </summary>
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var (exercises, totalCount) = await _repository.GetPedingPageAsync(page, pageSize);

            return Ok(new
            {
                exercises = exercises.Select(e => new
                {
                    e.Id,
                    e.ExternalId,
                    e.Name,
                    primaryMuscleGroup = e.PrimaryMuscleGroup.ToString(),
                    secondaryMuscles = e.SecondaryMuscles.Select(m => m.ToString()),
                    e.IsCompound,
                    e.ImagePath,
                    e.MediaPath
                }),
                pagination = new
                {
                    currentPage = page,
                    pageSize,
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                    totalCount
                }
            });
        }

        /// <summary>
        /// Remove exercícios em lote (rejeita)
        /// </summary>
        [HttpDelete("reject")]
        public async Task<IActionResult> RejectBatch([FromBody] List<int> exerciseIds)
        {
            if (!exerciseIds.Any())
            {
                return BadRequest("Nenhum exercicio fornecido");
            }
            await _repository.DeleteBatchAsync(exerciseIds);

            _logger.LogInformation($"{exerciseIds.Count} exercícios removidos");
            return Ok(new
            {
                message = $"{exerciseIds.Count} exercício(s) removido(s) com sucesso",
                rejectedIds = exerciseIds
            });
        }

        /// <summary>
        /// Exporta IDs externos dos exercícios aprovados (para JSON)
        /// </summary>
        [HttpGet("export-approved")]
        public async Task<IActionResult> ExportApproved()
        {
            var approvedIds = await _repository.GetApprovedExternalIdsAsync();

            var export = new
            {
                exportDate = DateTime.UtcNow,
                totalApproved = approvedIds.Count,
                exerciseIds = approvedIds.OrderBy(id => id).ToList()
            };

            return Ok(export);
        }

        /// <summary>
        /// Estatísticas de aprovação
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var total = await _repository.CountAsync();
            var approved = (await _repository.GetApprovedAsync()).Count;
            var pending = total - approved;

             return Ok(new
            {
                total,
                approved,
                pending,
               
            });

        }
    }
}