using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TrainWise.Core.Application.Dtos;

namespace Application.Services.PopulationService
{
    public class ExerciseFilterService
    {
        private readonly ILogger _logger;

        public ExerciseFilterService(ILogger logger)
        {
            _logger = logger;
        }

        public List<ExternalExerciseDto> ApplySmartFilter(List<ExternalExerciseDto> exercises)
{
    return exercises.Where(e =>
    {
        var equipment = e.Equipment?.ToLower() ?? "";
        var level = e.Level?.ToLower() ?? "";
        var name = e.Name.ToLower() ?? "";

        
        var allowedEquipment = new[]
        {
            "barbell", "dumbbell", "cable", "body only",
            "machine", "e-z curl bar", "kettlebells"
        };

        if (!allowedEquipment.Any(eq => equipment.Contains(eq)))
            return false;

      
        if (level == "expert")
            return false;

        if (HasBlockedKeywords(name, equipment))
                        {
                            return false;
                        }

        return true;
    }).ToList();
}

    

    
        private bool HasBlockedKeywords(string name, string equipment)
        {
            var blockedKeywords = new[]
            {
                "assisted",
                "suspended",
                "bosu",
                "stability ball",
                "resistance band",
                "roller",
                "stick",
                "towel",
                "wheel",
                "medicine ball",
                "band",
                "rope",
                "wheel roller",
                "stretch"
            };

            return blockedKeywords.Any(keyword => 
                name.Contains(keyword) || equipment.Contains(keyword));
        }

    }
}