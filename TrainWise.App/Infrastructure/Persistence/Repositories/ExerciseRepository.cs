using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using TrainWise.Core.Domain.Entities.WorkoutComponents;
using TrainWise.Core.Infrastructure.Data.Context;

namespace TrainWise.App.Infrastructure.Persistence.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {

        private readonly WorkoutDbContext _context;


        public ExerciseRepository(WorkoutDbContext context)
        {
            _context = context;
        }


        // ============ APROVED SEEDS =============

        // ========== QUERIES BÁSICAS ==========

        public async Task<Exercise?> GetByIdAsync(int id)
        {
            return await _context.Exercises
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Exercise?> GetByExternalIdAsync(string externalId)
        {
            return await _context.Exercises
                .FirstOrDefaultAsync(e => e.ExternalId == externalId);
        }

        public async Task<Exercise?> GetByNameAsync(string name)
        {
            return await _context.Exercises
                .FirstOrDefaultAsync(e => e.Name.ToLower() == name.ToLower());
        }

        public async Task<List<Exercise>> GetAllAsync()
        {
            return await _context.Exercises
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        // ========== QUERIES FILTRADAS ==========

        public async Task<List<Exercise>> GetByMuscleGroupAsync(MuscleGroupType muscleGroup)
        {
            return await _context.Exercises
                .Where(e => e.PrimaryMuscleGroup == muscleGroup)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public async Task<List<Exercise>> GetCompoundExercisesAsync()
        {
            return await _context.Exercises
                .Where(e => e.IsCompound)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public async Task<List<Exercise>> GetIsolationExercisesAsync()
        {
            return await _context.Exercises
                .Where(e => !e.IsCompound)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        // ========== PAGINAÇÃO ==========

        public async Task<(List<Exercise> exercises, int totalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            MuscleGroupType? muscleGroupFilter = null)
        {
            var query = _context.Exercises.AsQueryable();


            if (muscleGroupFilter.HasValue)
            {
                query = query.Where(e => e.PrimaryMuscleGroup == muscleGroupFilter.Value);
            }

            var totalCount = await query.CountAsync();

            var exercises = await query
                .OrderBy(e => e.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (exercises, totalCount);
        }

        // ========== COMANDOS ==========

        public async Task<Exercise> AddAsync(Exercise exercise)
        {
            await _context.Exercises.AddAsync(exercise);
            await _context.SaveChangesAsync();
            return exercise;
        }

        public async Task<Exercise> UpdateAsync(Exercise exercise)
        {
            _context.Exercises.Update(exercise);
            await _context.SaveChangesAsync();
            return exercise;
        }

        public async Task DeleteAsync(int id)
        {
            var exercise = await GetByIdAsync(id);
            if (exercise != null)
            {
                _context.Exercises.Remove(exercise);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Exercises.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> ExistsByExternalIdAsync(string externalId)
        {
            return await _context.Exercises.AnyAsync(e => e.ExternalId == externalId);
        }

        // ========== BULK OPERATIONS ==========

        public async Task AddRangeAsync(IEnumerable<Exercise> exercises)
        {
            await _context.Exercises.AddRangeAsync(exercises);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Exercises.CountAsync();
        }


        // ========== APROVED SEEDS ==========
        public async Task<(List<Exercise> exercises, int totalCount)> GetPedingPageAsync(int page, int pageSize)
        {
            var query = _context.Exercises.Where(e => !e.IsApproved);

            var totalCount = await query.CountAsync();

            var exercises = await query
                .OrderBy(e => e.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (exercises, totalCount);
        }

        public async Task<List<string>> GetApprovedExternalIdsAsync()
        {
            return await _context.Exercises
                .Where(e => e.IsApproved)
                .Select(e => e.ExternalId)
                .ToListAsync();
        }

        public async Task<List<Exercise>> GetApprovedAsync()
        {
            return await _context.Exercises
            .Where(e => e.IsApproved && string.IsNullOrEmpty(e.MediaPath))
            .OrderBy(e => e.Name)
            .ToListAsync();
        }

        public async Task ApproveBatchAsync(List<int> ids)
        {
            var exercises = await _context.Exercises
            .Where(e => ids.Contains(e.Id))
            .ToListAsync();

            foreach (var exercise in exercises)
            {
                exercise.Approve();
            }

            await _context.SaveChangesAsync();
        }


        public async Task DeleteBatchAsync(List<int> ids)
        {
             var exercises = await _context.Exercises
            .Where(e => ids.Contains(e.Id))
            .ToListAsync();

            _context.Exercises.RemoveRange(exercises);
            await _context.SaveChangesAsync();

        }
    }
}