using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TrainWise.Core.Infrastructure.Data.Context
{
    public class WorkoutDbContextFactory : IDesignTimeDbContextFactory<WorkoutDbContext>
    {
        public WorkoutDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WorkoutDbContext>();
            optionsBuilder.UseSqlite("Data Source=trainwise.db");

            return new WorkoutDbContext(optionsBuilder.Options);
        }
    }
}
