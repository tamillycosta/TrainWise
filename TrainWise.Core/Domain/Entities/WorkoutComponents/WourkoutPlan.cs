using System;
using System.Collections.Generic;
using  TrainWise.Core.Domain.Entities.UsersComponets;

namespace TrainWise.Core.Domain.Entities.WorkoutComponents
{
    public class WorkoutPlan
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsAIGenerated { get; set; }
        public WorkoutSplitType SplitType { get; set; }
        
        // Relacionamentos
        public User User { get; set; }
        public ICollection<WorkoutTemplate> WorkoutTemplates { get; set; }

        protected WorkoutPlan(){}
        public WorkoutPlan(string name,int userId, WorkoutSplitType workoutSplit, bool isAIGenerated, DateTime?  endDate)

        {
            Name = name;
            UserId = userId;
            SplitType = workoutSplit;
            StartDate = DateTime.UtcNow;
            EndDate = endDate;
            IsAIGenerated = isAIGenerated;
            IsActive = true;
            WorkoutTemplates = new List<WorkoutTemplate>();
        }
    }

    public enum WorkoutSplitType
    {
        FullBody,           // Corpo todo (3x semana)
        UpperLower,         // Superior/Inferior (4x semana)
        PushPullLegs,       // Push/Pull/Pernas (5-6xx semana)
        BodyPartSplit,      // Divisão por parte do corpo (5-6x semana)
        TorsoLimbs,          // Divisão de troncos e braços com perna (4x semana)
        FrontBackSplit,      // Divisão entre musculos da frente e atrás do corpo (4x semana)
        Custom              // Personalizado
    }
}