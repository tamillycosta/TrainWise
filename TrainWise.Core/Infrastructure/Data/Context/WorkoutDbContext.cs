using Microsoft.EntityFrameworkCore;
using TrainWise.Core.Infrastructure.Data.Configurations;
using TrainWise.Core.Domain.Entities.WorkoutComponents;
using TrainWise.Core.Domain.Entities.UsersComponets;
using TrainWise.Core.Domain.Entities.WorkoutAnalysis;

namespace TrainWise.Core.Infrastructure.Data.Context
{
    public class WorkoutDbContext : DbContext
    {
        public WorkoutDbContext(DbContextOptions<WorkoutDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<UserPreferences> UserPreferences => Set<UserPreferences>();
        public DbSet<WorkoutPlan> WorkoutPlans => Set<WorkoutPlan>();
        public DbSet<WorkoutTemplate> WorkoutTemplates => Set<WorkoutTemplate>();
        public DbSet<WorkoutTemplateExercise> WorkoutTemplateExercises => Set<WorkoutTemplateExercise>();
        public DbSet<Exercise> Exercises => Set<Exercise>();
        public DbSet<WorkoutSession> WorkoutSessions => Set<WorkoutSession>();
        public DbSet<SetExecution> SetExecutions => Set<SetExecution>();
        public DbSet<PerformanceSnapshot> PerformanceSnapshots => Set<PerformanceSnapshot>();
        public DbSet<ProgressAnalysis> ProgressAnalyses => Set<ProgressAnalysis>();
        public DbSet<AiRecommendation> AIRecommendations => Set<AiRecommendation>();
        public DbSet<VolumeAnalysis> VolumeAnalyses => Set<VolumeAnalysis>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WorkoutDbContext).Assembly);
        }
    }
}
