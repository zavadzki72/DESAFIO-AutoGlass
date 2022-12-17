using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Produtos.Infra.SqlServer;

namespace Produtos.CrossCutting.IoC.Configurations
{
    public static class EntityFrameworkConfiguration
    {
        public static void AddEntityFrameworkConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["SQL_CONNECTION"];
            var readOnlyConnectionString = configuration["SQL_READONLY_CONNECTION"];

            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(connectionString).UseSnakeCaseNamingConvention();
            }, ServiceLifetime.Scoped);

            services.AddDbContext<ApplicationReadOnlyDbContext>(options => {
                options.UseSqlServer(readOnlyConnectionString).UseSnakeCaseNamingConvention();
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, ServiceLifetime.Scoped);
        }
    }
}
