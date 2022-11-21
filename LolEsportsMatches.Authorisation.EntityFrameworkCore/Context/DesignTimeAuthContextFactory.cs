using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LolEsportsMatches.Authorisation.EntityFrameworkCore.Context
{
    public class DesignTimeAuthContextFactory : IDesignTimeDbContextFactory<AuthContext>
    {
        public AuthContext CreateDbContext(string[] args)
        {
            const string connectionStringName = "AUTH_BASE";
            const string connectioStringPrefix = "SQLCONNSTR_";

            IConfigurationRoot? configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
            string? connectionString = configuration.GetConnectionString(connectionStringName);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception($"{connectioStringPrefix}{connectionStringName} environment variable is not set.");
            }

            Console.WriteLine($"Using {connectioStringPrefix}{connectionStringName} environment variable as a connection string.");

            DbContextOptions<AuthContext>? builderOptions = new DbContextOptionsBuilder<AuthContext>().UseSqlServer(connectionString).Options;
            return new AuthContext(builderOptions);
        }
    }
}
