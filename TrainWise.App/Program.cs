using Api.Configuration;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Api.DbClient;
using Infrastructure.CloudStorage;
using Microsoft.EntityFrameworkCore;
using TrainWise.App.Infrastructure.Persistence.Repositories;
using TrainWise.Core.Application.Interfaces;
using TrainWise.Core.Application.Services;
using TrainWise.Core.Infrastructure.Data.Context;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration
   
    .AddJsonFile("appsetings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsetings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();


// ========== Db ==========
builder.Services.AddDbContext<WorkoutDbContext>(options =>
    options.UseSqlite("Data Source=trainwise.db;Mode=ReadWriteCreate;Cache=Shared"));




// ======== CONFIG ==========


builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("Cloudinary"));



// ========== SERVICES ==========
builder.Services.AddHttpClient<IExerciseApiClient, ExerciseDbClient>();
builder.Services.AddScoped<ICloudStorageService, CloudinaryService>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IExercisePopulationService, ExercisePopulationService>();

// ========== MVC/API ==========
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WorkoutDbContext>();
    context.Database.Migrate();
    Console.WriteLine("Banco criado/atualizado com sucesso!");
}


// ========== MIDDLEWARE ==========
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();