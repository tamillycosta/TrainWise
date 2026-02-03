using TrainWise.Core.Domain.Entities.UsersComponets;
using TrainWise.Core.Domain.Entities.WorkoutComponents;

namespace TrainWise.Core.Domain.Entities.WorkoutAnalysis
{
    public class ProgressAnalysis
    {


        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int ExerciseId { get; private set; }

        public DateTime AnalysisDate { get; private set; }
        public ProgressStatus ProgressStatus { get; private set; }

        public double VolumeChangePercent { get; private set; }
        public double StrengthChangePercent { get; private set; }
        public int WeeksStagnant { get; private set; }

        public IReadOnlyCollection<string> Observations { get; private set; }

        // Relacionamentos
        public User User { get; private set; }
        public Exercise Exercise { get; private set; }
        

       protected ProgressAnalysis() { } 

        public ProgressAnalysis(int userId,int exerciseId,ProgressStatus status,double volumeChangePercent,
            double strengthChangePercent,
            int weeksStagnant,
            IReadOnlyCollection<string> observations)
        {
            UserId = userId;
            ExerciseId = exerciseId;
            ProgressStatus = status;
            VolumeChangePercent = volumeChangePercent;
            StrengthChangePercent = strengthChangePercent;
            WeeksStagnant = weeksStagnant;
            Observations = observations.ToList();
            AnalysisDate = DateTime.UtcNow;
        }
    }

    public enum ProgressStatus
    {
        Progressing,        // Progredindo bem
        Stagnant,           // Estagnado (sem mudança)
        Regressing,         // Regredindo
        Deloading,          // Em deload
        Recovering          // Recuperando
    }
}
