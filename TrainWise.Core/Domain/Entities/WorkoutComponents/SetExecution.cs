using System;

namespace TrainWise.Core.Domain.Entities.WorkoutComponents
{
    public class SetExecution
    {
    
        public int Id { get; private set; }
        public int WorkoutSessionId { get; private set; }
        public int WorkoutTemplateExerciseId { get; private set; }
        public int SetNumber { get; private set; }

        public double Weight { get; private set; }
        public int Reps { get; private set; }
        public int? RPE { get; private set; }
        public SetType SetType { get; private set; }

        public DateTime ExecutedAt { get; private set; }
        public string Notes { get; private set; }

        // Volume bruto (neutro)
        public double LoadVolume => Weight * Reps;

        // Relacionamentos
        public WorkoutSession WorkoutSession { get; private set; }
        public WorkoutTemplateExercise WorkoutTemplateExercise { get; private set; }

        protected SetExecution() { } 

        public SetExecution(int workoutSessionId,int workoutTemplateExerciseId,
            int setNumber,double weight,int reps,SetType setType,
            int? rpe,
            string notes = null){
            WorkoutSessionId = workoutSessionId;
            WorkoutTemplateExerciseId = workoutTemplateExerciseId;
            SetNumber = setNumber;
            Weight = weight;
            Reps = reps;
            SetType = setType;
            RPE = rpe;
            Notes = notes;
            ExecutedAt = DateTime.UtcNow;
        }


        // A série sabe se ela é válida ou não, MAS não soma nada
        public bool IsValidForVolume(int minRpe)
        {
            if (SetType == SetType.Failure)
                return true;

            if (SetType == SetType.CloseToFailure && RPE.HasValue && RPE >= minRpe)
                return true;

            return false;
        }

        
    }

    public enum SetType
    {
        WarmUp,
        Prepare,
        BackOffSet,
        DropSet,
        RestPause,
        CloseToFailure,
        Failure
    }
}
