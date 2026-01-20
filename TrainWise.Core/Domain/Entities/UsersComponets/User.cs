using System;
using TrainWise.Core.Domain.Entities.WorkoutAnalysis;
using TrainWise.Core.Domain.Entities.WorkoutComponents;

namespace TrainWise.Core.Domain.Entities.UsersComponets
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime CreateAt { get; set; }
        public UserPreferences Preferences { get; set; }

        public ICollection<WorkoutPlan> WorkoutPlans { get; set; }
        public ICollection<WorkoutSession> WorkoutSessions { get; set; }
        public ICollection<AiRecommendation> AIRecommendations { get; set; }
        public ICollection<ProgressAnalysis> ProgressAnalyses { get; set; }
        public ICollection<VolumeAnalysis> VolumeAnalyses { get; set; }

        protected User() {}
        public User(string name)

        {
       
            Name = name;
            CreateAt = DateTime.UtcNow;
            WorkoutPlans = new List<WorkoutPlan>();
            WorkoutSessions = new List<WorkoutSession>();
            AIRecommendations = new List<AiRecommendation>();
            ProgressAnalyses = new List<ProgressAnalysis>();
            VolumeAnalyses = new List<VolumeAnalysis>();
        }
    } 
}
