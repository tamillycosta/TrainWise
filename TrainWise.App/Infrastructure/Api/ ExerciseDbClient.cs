using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.Extensions.Options;
using TrainWise.Core.Application.Interfaces;
using TrainWise.Core.Application.Dtos;
using Api.Configuration;
using System.Net.Http.Json;
using Newtonsoft.Json;


namespace Infrastructure.Api.DbClient
{
    public class ExerciseDbClient : IExerciseApiClient
    {
        private readonly HttpClient _httpClient;
        

         private const string ExercisesJsonUrl =
            "https://raw.githubusercontent.com/yuhonas/free-exercise-db/main/dist/exercises.json";

        // Base URL das imagens
        private const string ImageBaseUrl =
            "https://raw.githubusercontent.com/yuhonas/free-exercise-db/main/exercises/";

        public ExerciseDbClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            
           
        }

        public async Task<List<ExternalExerciseDto>> GetAllExercisesAsync()
        {
            var response = await _httpClient.GetAsync(ExercisesJsonUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var exercises = JsonConvert.DeserializeObject<List<ExternalExerciseDto>>(content);

            return exercises ?? new List<ExternalExerciseDto>();
        }

        public async Task<ExternalExerciseDto> GetExerciseByIdAsync(string id)
        {
              var all = await GetAllExercisesAsync();
             return all.FirstOrDefault(e => e.Id == id);
        }

        public async Task<byte[]> DownloadGifAsync(string exerciseId)
        {
            var exercise = await GetExerciseByIdAsync(exerciseId);

            if (exercise.Images == null || !exercise.Images.Any())
            {
                throw new Exception($"Exercicio {exerciseId} não possui imagem");
            }
            var imagePath = exercise.Images.Count > 1 
                ? exercise.Images[1] 
                : exercise.Images[0];

            return await DownloadImageAsync(imagePath);

        }

        public async Task<byte[]> DownloadImageAsync(string imageRelativePath)
        {
            var url =  $"{ImageBaseUrl}{imageRelativePath}";
        
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}