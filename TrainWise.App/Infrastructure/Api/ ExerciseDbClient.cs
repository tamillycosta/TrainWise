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
        private readonly ExerciseDbSettings _configs;


        public ExerciseDbClient(HttpClient httpClient, IOptions<ExerciseDbSettings> configs)
        {
            _httpClient = httpClient;
            _configs = configs.Value;

            _httpClient.BaseAddress = new Uri(_configs.BaseUrl);
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", _configs.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", "exercisedb.p.rapidapi.com");
        }

        public async Task<List<ExternalExerciseDto>> GetAllExercisesAsync()
        {
            var response = await _httpClient.GetAsync("exercises");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var exercises = JsonConvert.DeserializeObject<List<ExternalExerciseDto>>(content);

            return exercises ?? new List<ExternalExerciseDto>();
        }

        public async Task<ExternalExerciseDto> GetExerciseByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"exercises/exercise/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ExternalExerciseDto>(content) ?? new ExternalExerciseDto();
        }

        public async Task<byte[]> DownloadGifAsync(string exerciseId, string resolution = "360")
        {
        var url = $"image?exerciseId={exerciseId}&resolution={resolution}&rapidapi-key={_configs.ApiKey}";
        
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsByteArrayAsync();
         }
    }
}