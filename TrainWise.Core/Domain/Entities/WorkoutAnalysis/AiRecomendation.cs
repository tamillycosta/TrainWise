using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainWise.Core.Domain.Entities.UsersComponets;
using  TrainWise.Core.Domain.Entities.WorkoutComponents;

namespace TrainWise.Core.Domain.Entities.WorkoutAnalysis
{
    public class AiRecommendation
    {
    
        public int Id { get; private set; }
        public int UserId { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public RecommendationType Type { get; private set; }
        public RecommendationPriority Priority { get; private set; }

        public string Title { get; private set; }
        public string Description { get; private set; }

        // Escopo opcional
        public int? ExerciseId { get; private set; }
        public int? WorkoutPlanId { get; private set; }

        // Estado
        public bool IsApplied { get; private set; }
        public DateTime? AppliedAt { get; private set; }

        // Relacionamentos
        public User User { get; private set; }
        public Exercise Exercise { get; private set; }
        public WorkoutPlan WorkoutPlan { get; private set; }

        protected AiRecommendation() { } 

        public AiRecommendation(int userId,RecommendationType type, string title,string description,
            RecommendationPriority priority,
            int? exerciseId = null,
            int? workoutPlanId = null)
        {
            UserId = userId;
            Type = type;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Priority = priority;
            ExerciseId = exerciseId;
            WorkoutPlanId = workoutPlanId;
            CreatedAt = DateTime.UtcNow;
            IsApplied = false;
        }

        public void Apply()
        {
            if (IsApplied)
                throw new InvalidOperationException("Esta recomendação já foi aplicada.");

            IsApplied = true;
            AppliedAt = DateTime.UtcNow;
        }
    }
    

 

    public enum RecommendationType
    {
        ExerciseChange,         // Trocar exercício
        VolumeAdjustment,       // Ajustar volume
        IntensityAdjustment,    // Ajustar intensidade
        DeloadSuggestion,       // Sugestão de deload
        ProgramChange,          // Mudar programa
        TechniqueImprovement,   // Melhorar técnica
        RecoveryAdvice          // Conselho de recuperação
    }

    public enum RecommendationPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
}
