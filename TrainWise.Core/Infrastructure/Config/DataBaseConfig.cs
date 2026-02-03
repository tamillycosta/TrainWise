using Microsoft.EntityFrameworkCore;
using TrainWise.Core.Infrastructure.Data.Context;

namespace TrainWise.Core.Infrastructure.Config
{
    public class DatabaseConfig  
    {
        public static void Start(){
            var options = new DbContextOptionsBuilder<WorkoutDbContext>()
            .UseSqlite("Data Source=trainwise.db")
            .Options;

            using var context = new WorkoutDbContext(options);
            context.Database.Migrate();
            Console.WriteLine("Banco criado com sucesso!");
        }
    }
}