using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LolEsportsMatches.DataAccess.EntityFrameworkCore.Context
{
    public class DesignTimeMatchHistoryContextFactory : IDesignTimeDbContextFactory<MatchHistoryContext>
    {
        public MatchHistoryContext CreateDbContext(string[] args)
        {
            const string connectionStringName = "MATCH_HISTORY";
            const string connectioStringPrefix = "SQLCONNSTR_";

            IConfigurationRoot? configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
            string? connectionString = configuration.GetConnectionString(connectionStringName);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception($"{connectioStringPrefix}{connectionStringName} environment variable is not set.");
            }

            Console.WriteLine($"Using {connectioStringPrefix}{connectionStringName} environment variable as a connection string.");

            DbContextOptions<MatchHistoryContext>? builderOptions = new DbContextOptionsBuilder<MatchHistoryContext>().UseSqlServer(connectionString).Options;
            return new MatchHistoryContext(builderOptions);
        }
    }
}
