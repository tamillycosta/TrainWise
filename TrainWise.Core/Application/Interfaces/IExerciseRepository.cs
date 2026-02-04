using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainWise.Core.Domain.Entities.WorkoutComponents;

namespace Application.Interfaces
{
    public interface IExerciseRepository
    {
        Task<Exercise?> GetByIdAsync(int id);
        Task<Exercise?> GetByExternalIdAsync(string externalId);
        Task<Exercise?> GetByNameAsync(string name);
        Task<List<Exercise>> GetAllAsync();
        
        // Queries filtradas
        Task<List<Exercise>> GetByMuscleGroupAsync(MuscleGroupType muscleGroup);
        Task<List<Exercise>> GetCompoundExercisesAsync();
        Task<List<Exercise>> GetIsolationExercisesAsync();
        
        // Paginação
        Task<(List<Exercise> exercises, int totalCount)> GetPagedAsync(
            int pageNumber, 
            int pageSize,
            MuscleGroupType? muscleGroupFilter = null);
        
        // Comandos
        Task<Exercise> AddAsync(Exercise exercise);
        Task<Exercise> UpdateAsync(Exercise exercise);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByExternalIdAsync(string externalId);
        
        // Bulk operations 
        Task AddRangeAsync(IEnumerable<Exercise> exercises);
        Task<int> CountAsync();
    }
}