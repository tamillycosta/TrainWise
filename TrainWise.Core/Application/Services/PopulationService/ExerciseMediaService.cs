using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using TrainWise.Core.Application.Interfaces;
using TrainWise.Core.Domain.Entities.WorkoutComponents;

namespace Application.Services.PopulationService
{
    public class ExerciseMediaService
    {
        private readonly IExerciseApiClient _apiClient;
        private readonly ICloudStorageService _cloudStorage;

        private readonly ILogger _logger;

        public ExerciseMediaService(IExerciseApiClient apiClient, ICloudStorageService cloudStorage, ILogger logger)
        {
            _apiClient = apiClient;
            _cloudStorage = cloudStorage;
            _logger = logger;
        }


        public async Task<bool> DownloadMediaAsync(Exercise exercise)
        {
            try
            {
                _logger.LogInformation($"Baixando mídia para '{exercise.Name}'...");


                var exerciseData = await _apiClient.GetExerciseByIdAsync(exercise.ExternalId);

                if (exerciseData?.Images == null || !exerciseData.Images.Any())
                {
                    _logger.LogWarning($"'{exercise.Name}' não tem imagens disponíveis");
                    return false;
                }


                var thumbnailBytes = await _apiClient.DownloadImageAsync(exerciseData.Images[0]);
                var thumbnailUrl = await _cloudStorage.UploadImageAsync(
                    thumbnailBytes,
                    $"{exercise.ExternalId}_thumb.jpg",
                    "exercises/thumbnails");


                var gifUrl = thumbnailUrl; // SE N TIVER GIF
                if (exerciseData.Images.Count > 1)
                {
                    var gifBytes = await _apiClient.DownloadImageAsync(exerciseData.Images[1]);
                    gifUrl = await _cloudStorage.UploadGifAsync(
                        gifBytes,
                        $"{exercise.ExternalId}.gif",
                        "exercises/gifs");
                }

                
                exercise.SetMedia(thumbnailUrl, gifUrl, MediaType.Gif);

                _logger.LogInformation($"Mídia processada para '{exercise.Name}'");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Erro ao processar mídia de '{exercise.Name}'");
                return false;
            }
        }
       
    }

}
