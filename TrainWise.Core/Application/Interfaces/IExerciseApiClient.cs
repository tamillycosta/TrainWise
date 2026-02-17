using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainWise.Core.Application.Dtos;

namespace TrainWise.Core.Application.Interfaces
{
    public interface IExerciseApiClient
    {
        Task<List<ExternalExerciseDto>> GetAllExercisesAsync();
        Task<ExternalExerciseDto> GetExerciseByIdAsync(string id);
        Task<byte[]> DownloadGifAsync(string exerciseId);
        Task<byte[]> DownloadImageAsync(string imageRelativePath);
        
    }
}   