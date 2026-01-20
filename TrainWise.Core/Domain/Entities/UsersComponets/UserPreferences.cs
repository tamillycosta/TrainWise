using System.Collections.Generic;


namespace TrainWise.Core.Domain.Entities.UsersComponets
{
    public class UserPreferences
    {
        public int Id;
        public int UserId;

        public int MinRpeForValidSet;

        public int TrainingDaysPerWeek { get; set; }
       
        public List<string> AvailableEquipment { get; set; }
        public int PreferredSessionDuration { get; set; } 

        public User User { get; set; }
        protected UserPreferences() { } 
        public UserPreferences(int trainingDaysPerWeek, int minRpeForValidSet)
        {
            TrainingDaysPerWeek = trainingDaysPerWeek;
            MinRpeForValidSet = minRpeForValidSet;
            AvailableEquipment = new List<string>();
        }
    }
  } 