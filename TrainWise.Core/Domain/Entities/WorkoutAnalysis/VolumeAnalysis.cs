using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainWise.Core.Domain.Entities.WorkoutComponents;
using TrainWise.Core.Domain.Entities.UsersComponets;

namespace TrainWise.Core.Domain.Entities.WorkoutAnalysis
{
    public class VolumeAnalysis
    {
        protected VolumeAnalysis() { }

        public int Id { get; private set; }
        public int UserId { get; private set; }
        public MuscleGroupType MuscleGroup { get; private set; }
        public DateTime WeekStartDate { get; private set; }

        public int TotalSets { get; private set; }
        public double TotalVolume { get; private set; }
        public double AverageIntensity { get; private set; }
        public int WorkoutsCount { get; private set; }

        public User User { get; private set; }


        public VolumeAnalysis(int userId,  MuscleGroupType muscleGroup,  DateTime weekStartDate, int totalSets,
            double totalVolume,
            double averageIntensity,
            int workoutsCount)
        {
            UserId = userId;
            MuscleGroup = muscleGroup;
            WeekStartDate = weekStartDate;
            TotalSets = totalSets;
            TotalVolume = totalVolume;
            AverageIntensity = averageIntensity;
            WorkoutsCount = workoutsCount;
        }
    }
}