using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace IncidentManagement.Data
{
    public class IncidentDbContextFactory : IDesignTimeDbContextFactory<IncidentDbContext>
    {
        public IncidentDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<IncidentDbContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("IncidentConnection"));

            return new IncidentDbContext(optionsBuilder.Options);
        }
    }
}