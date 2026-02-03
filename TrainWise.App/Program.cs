using Microsoft.Extensions.Configuration;
using TrainWise.Core.Infrastructure.Config;
using TrainWise.Core.Application.Interfaces;
using Api.Configuration;
using Infrastructure.Api.DbClient;
using Microsoft.Extensions.Options;

DatabaseConfig.Start();

// Carrega configurações manualmente
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsetings.json", optional: false)
    .Build();

var exerciseDbSettings = new ExerciseDbSettings();
configuration.GetSection("ExerciseDb").Bind(exerciseDbSettings);

// Cria o client manualmente
var httpClient = new HttpClient();
var options = Options.Create(exerciseDbSettings);
var exerciseClient = new ExerciseDbClient(httpClient, options);

// Testa buscar exercícios
Console.WriteLine("Buscando exercícios da API...");
try
{
    var exercises = await exerciseClient.GetAllExercisesAsync();
    Console.WriteLine($"Total de exercícios encontrados: {exercises.Count}");

    if (exercises.Any())
    {
        var first = exercises.First();
        Console.WriteLine($"\nPrimeiro exercício:");
        Console.WriteLine($"Nome: {first.Name}");
        Console.WriteLine($"Músculo alvo: {first.Target}");

        Console.WriteLine("\nBaixando GIF do exercício...");
        var gifBytes = await exerciseClient.DownloadGifAsync(first.Id);
        Console.WriteLine($"GIF baixado! Tamanho: {gifBytes.Length / 1024} KB");
    }

    var exercise = await exerciseClient.GetExerciseByIdAsync("0002");
    Console.WriteLine($"\nExercicio buscado pelo id");
    Console.WriteLine($"Nome: {exercise.Name}");
    Console.WriteLine($"Músculo alvo: {exercise.Target}");
    Console.WriteLine($"GIF URL: {exercise.GifUrl}");
}
catch (Exception ex)
{
    Console.WriteLine($"Erro: {ex.Message}");
}