using Microsoft.Extensions.DependencyInjection;
using Produtos.CrossCutting.AutoMapper;

namespace Produtos.CrossCutting.IoC.Configurations
{
    public static class AutoMapperConfiguration
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            var config = AutoMapperConfig.RegisterMappings();
            
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
