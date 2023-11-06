using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Project.Repositories.EfCore;

namespace WEBAPI.Factories
{
    /// <summary>
    /// IDesignTimeDbContextFactory : otomatik olarak implemente edildiği noktaları bulur.
    /// </summary>
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        /// <summary>
        /// implemented.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DatabaseContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Migrationların WEBAPI projesinde oluşmasını istiyoruz.
            var builder = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlServer(configuration.GetConnectionString("databaseConnection"),
                prj => prj.MigrationsAssembly("WEBAPI"));

            return new DatabaseContext(builder.Options);
        }
    }
}
