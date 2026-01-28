using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainWise.Core.Domain.Entities.WorkoutComponents;
using TrainWise.Core.Domain.Entities.UsersComponets;
using TrainWise.Core.Domain.Entities.WorkoutAnalysis;
using Newtonsoft.Json;


// configurations workoutComponentes 
namespace TrainWise.Core.Infrastructure.Data.Configurations{


public class WorkoutPlainConfiguration : IEntityTypeConfiguration<WorkoutPlan>{
    public void Configure(EntityTypeBuilder<WorkoutPlan> builder){
      

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(200);

        builder.HasOne(e => e.User)
                    .WithMany(u => u.WorkoutPlans)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
    }
}


public class WorkoutTemplateConfiguration :  IEntityTypeConfiguration<WorkoutTemplate>{
    public void Configure(EntityTypeBuilder<WorkoutTemplate> builder){
          builder.HasKey(e => e.Id);
          builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
                
                // Serializar lista de grupos musculares
            builder.Property(e => e.TargetMuscleGroups)
                    .HasConversion(
                        v => JsonConvert.SerializeObject(v),
                        v => JsonConvert.DeserializeObject<List<MuscleGroupType>>(v) ?? new List<MuscleGroupType>()
                    );
                
            builder.HasOne(e => e.WorkoutPlan)
                    .WithMany(p => p.WorkoutTemplates)
                    .HasForeignKey(e => e.WorkoutPlanId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
    }


public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>{
    public void Configure(EntityTypeBuilder<Exercise> builder){
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
                
        builder.Property(e => e.SecondaryMuscles)
                    .HasConversion(
                        v => JsonConvert.SerializeObject(v),
                        v => JsonConvert.DeserializeObject<List<MuscleGroupType>>(v) ?? new List<MuscleGroupType>()
                    );
            }
    
}

public class WorkoutTemplateExerciseConfiguration : IEntityTypeConfiguration<WorkoutTemplateExercise>
{
     public void Configure(EntityTypeBuilder<WorkoutTemplateExercise> builder){
          builder.HasKey(e => e.Id);
                
            builder.HasOne(e => e.WorkoutTemplate)
                    .WithMany(t => t.Exercises)
                    .HasForeignKey(e => e.WorkoutTemplateId)
                    .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasOne(e => e.Exercise)
                    .WithMany(ex => ex.WorkoutTemplateExercises)
                    .HasForeignKey(e => e.ExerciseId)
                    .OnDelete(DeleteBehavior.Restrict);
            }

    }




public class WorkoutSessionConfiguration : IEntityTypeConfiguration<WorkoutSession>
{
    public void Configure(EntityTypeBuilder<WorkoutSession> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.User)
            .WithMany(u => u.WorkoutSessions)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.WorkoutTemplate)
            .WithMany(t => t.Sessions)
            .HasForeignKey(e => e.WorkoutTemplateId)
            .OnDelete(DeleteBehavior.Restrict);

       
        builder.Ignore(e => e.Duration);
    }
    }



public class SetExecutionConfiguration : IEntityTypeConfiguration<SetExecution>{
    public void Configure(EntityTypeBuilder<SetExecution> builder){
            builder.HasKey(e => e.Id);
                
            builder.HasOne(e => e.WorkoutSession)
                    .WithMany(s => s.SetExecutions)
                    .HasForeignKey(e => e.WorkoutSessionId)
                    .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasOne(e => e.WorkoutTemplateExercise)
                    .WithMany(te => te.SetExecutions)
                    .HasForeignKey(e => e.WorkoutTemplateExerciseId)
                    .OnDelete(DeleteBehavior.Restrict);
                
            builder.Ignore(e => e.LoadVolume);
    }
}

















}


