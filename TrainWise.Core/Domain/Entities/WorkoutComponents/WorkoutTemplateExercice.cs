namespace TrainWise.Core.Domain.Entities.WorkoutComponents
{
    public class WorkoutTemplateExercise
    {
        public int Id { get; set; }
        public int WorkoutTemplateId { get; set; }
        public int ExerciseId { get; set; }
        public int Order { get; set; }
        public int TargetSets { get; set; }
        public int TargetRepsMin { get; set; }
        public int TargetRepsMax { get; set; }
        public string Notes { get; set; }
        public int? RestTime { get; set; } // Tempo de descanso em segundos
        
        // Relacionamentos
        public WorkoutTemplate WorkoutTemplate { get; set; }
        public Exercise Exercise { get; set; }
        public ICollection<SetExecution> SetExecutions { get; set; }

        protected WorkoutTemplateExercise(){}
        public WorkoutTemplateExercise(int targetSets, int targetRepsMin, int targetRepsMax, int restTime)
        {
            SetExecutions = new List<SetExecution>();
            TargetSets = targetSets;
            TargetRepsMin = targetRepsMin;
            TargetRepsMax = targetRepsMax;
            RestTime = restTime;
        }
    }
    
}