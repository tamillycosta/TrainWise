using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TrainWise.Core.Application.Dtos;
using TrainWise.Core.Domain.Entities.WorkoutComponents;

namespace Application.Services.PopulationService
{
    public class ExerciseMappingService
    {


        public ExerciseMappingService()
        {

        }


        public Exercise MapToEntity(ExternalExerciseDto dto)
        {
            var exercise = new Exercise(dto.Name)
            {
                ExternalId = dto.Id,
             
                IsCompound = dto.Mechanic?.ToLower() == "compound",
                IsApproved = false
            };

          
            if (dto.PrimaryMuscles?.Any() == true)
            {
                var primaryMuscle = MapMuscleGroup(dto.PrimaryMuscles.First());
                if (primaryMuscle.HasValue)
                    exercise.PrimaryMuscleGroup = primaryMuscle.Value;
            }

          
            if (dto.SecondaryMuscles?.Any() == true)
            {
                foreach (var muscle in dto.SecondaryMuscles)
                {
                    var muscleType = MapMuscleGroup(muscle);
                    if (muscleType.HasValue && !exercise.SecondaryMuscles.Contains(muscleType.Value))
                        exercise.SecondaryMuscles.Add(muscleType.Value);
                }
            }

            return exercise;
        }

        private MuscleGroupType? MapMuscleGroup(string? target)
        {
            return target?.ToLower() switch
            {
                "abdominals" or "abs" => MuscleGroupType.Abs,
                "biceps" => MuscleGroupType.Biceps,
                "triceps" => MuscleGroupType.Triceps,
                "chest" => MuscleGroupType.Chest,
                "lats" or "middle back" or "upper back" => MuscleGroupType.Back,
                "shoulders" or "front deltoid" or "deltoid" => MuscleGroupType.Shoulders,
                "quadriceps" => MuscleGroupType.Quadriceps,
                "hamstrings" => MuscleGroupType.Hamstrings,
                "glutes" => MuscleGroupType.Glutes,
                "calves" => MuscleGroupType.Calves,
                "forearms" => MuscleGroupType.Forearms,
                "traps" => MuscleGroupType.Traps,
                "lower back" => MuscleGroupType.LowerBack,
                _ => null
            };
        }

    }
}