using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainWise.Core.Domain.Entities.WorkoutComponents
{
    public class WorkoutTemplate
    {
        public int Id { get; set; }
        public int WorkoutPlanId { get; set; }
        public string Name { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }
        public int Order { get; set; }
        public List<MuscleGroupType> TargetMuscleGroups { get; set; }

        // Relacionamentos
        public WorkoutPlan WorkoutPlan { get; set; }
        public ICollection<WorkoutTemplateExercise> Exercises { get; set; }
        public ICollection<WorkoutSession> Sessions { get; set; }

        protected WorkoutTemplate()
        {
            
        }

        public WorkoutTemplate(int workoutPlanId, string name, DayOfWeek? dayOfWeek, int order){
            WorkoutPlanId = workoutPlanId;
            Name = name;
            DayOfWeek = dayOfWeek;
            Order = order;

            TargetMuscleGroups = new List<MuscleGroupType>();
            Exercises = new List<WorkoutTemplateExercise>();
            Sessions = new List<WorkoutSession>();
        }
    }
}
