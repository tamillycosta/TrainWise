using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IExercisePopulationService
    {
        Task<ExercisePopulationResult> PopulateDatabaseAsync(
            bool downloadMedia = true, 
            int? limit = null);
        
        Task<ExercisePopulationResult> PopulateExerciseAsync(string exerciseId);
    }

    public class ExercisePopulationResult {
        public int TotalProcessed { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<string> Errors { get; set; } = new();
        public TimeSpan Duration { get; set; }
        public bool IsSuccess => FailureCount == 0;
    }
}