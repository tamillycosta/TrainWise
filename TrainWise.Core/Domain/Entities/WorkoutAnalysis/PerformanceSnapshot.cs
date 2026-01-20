using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainWise.Core.Domain.Entities.WorkoutComponents;
using TrainWise.Core.Domain.Entities.UsersComponets;

namespace TrainWise.Core.Domain.Entities.WorkoutAnalysis
{
    public class PerformanceSnapshot
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public int UserId { get; set; }
        public DateTime AnalysisDate { get; set; }

        // Métricas
        public double MaxWeight { get; set; }
        public double TotalVolume { get; set; }
        public double AverageRPE { get; set; }
        public int TotalSets { get; set; }
        public DateTime LastWorkoutDate { get; set; }

        // Relacionamentos
        public Exercise Exercise { get; set; }
        public User User { get; set; }

        protected PerformanceSnapshot(){}
        
           
       
        public PerformanceSnapshot(int userId,int exerciseId,double maxWeight,double totalVolume,
            double averageRpe,
            int totalSets,
            DateTime lastWorkoutDate)
        {
            UserId = userId;
            ExerciseId = exerciseId;
            MaxWeight = maxWeight;
            TotalVolume = totalVolume;
            AverageRPE = averageRpe;
            TotalSets = totalSets;
            LastWorkoutDate = lastWorkoutDate;
            AnalysisDate = DateTime.UtcNow;
        }
        
    }

  
    

}
