using System;
using System.Collections.Generic;
using System.Linq;
using TrainWise.Core.Domain.Entities.UsersComponets;

namespace TrainWise.Core.Domain.Entities.WorkoutComponents
{
    public class WorkoutSession
    {
        public int Id { get; private set; }
        public int WorkoutTemplateId { get; private set; }
        public int UserId { get; private set; }

        public DateTime StartTime { get; private set; }
        public DateTime? EndTime { get; private set; }
        public WorkoutStatus Status { get; private set; }
        public string? Notes { get; private set; }

       
        private readonly List<SetExecution> _sets = new();

      
        public IReadOnlyCollection<SetExecution> SetExecutions => _sets;

        // Relacionamentos
        public WorkoutTemplate WorkoutTemplate { get; private set; }
        public User User { get; private set; }

        protected WorkoutSession() { } 

        public WorkoutSession(int workoutTemplateId, int userId)
        {
            WorkoutTemplateId = workoutTemplateId;
            UserId = userId;
            StartTime = DateTime.UtcNow;
            Status = WorkoutStatus.InProgress;
        }

        
        public void AddSet(SetExecution set)
        {
            if (Status != WorkoutStatus.InProgress)
                throw new InvalidOperationException("Não é possível adicionar séries a um treino finalizado.");

            _sets.Add(set);
        }

        
        public int CountValidSets(UserPreferences preferences)
        {
            return _sets.Count(s =>
                s.IsValidForVolume(preferences.MinRpeForValidSet));
        }

       
        public double CalculateEffectiveVolume(UserPreferences preferences)
        {
            return _sets
                .Where(s => s.IsValidForVolume(preferences.MinRpeForValidSet))
                .Sum(s => s.LoadVolume);
        }

     
        public double CalculateTotalVolume()
        {
            return _sets.Sum(s => s.LoadVolume);
        }

        public void Finish()
        {
            Status = WorkoutStatus.Completed;
            EndTime = DateTime.UtcNow;
        }

        public TimeSpan? Duration =>
            EndTime.HasValue ? EndTime.Value - StartTime : null;
    }

    public enum WorkoutStatus
    {
        InProgress,
        Completed,
        Cancelled,
        Skipped
    }
}
