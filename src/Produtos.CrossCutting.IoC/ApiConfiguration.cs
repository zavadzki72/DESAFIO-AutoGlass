using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Produtos.CrossCutting.IoC.Configurations;

namespace Produtos.CrossCutting.IoC
{
    public static class ApiConfiguration
    {
        public static void ApplyApiConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkConfiguration(configuration);
            services.AddAutoMapperConfiguration();

            services.AddGeneralConfiguration();
        }
    }
}
