using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Produtos.ApplicationService;
using Produtos.CrossCutting.bus;
using Produtos.Domain.Core;
using Produtos.Domain.Model;
using Produtos.Domain.Model.Interfaces;
using Produtos.Domain.Model.Interfaces.ApplicationServices;
using Produtos.Domain.Model.Interfaces.Repositories;
using Produtos.Infra.SqlServer;
using Produtos.Infra.SqlServer.Repositories;

namespace Produtos.CrossCutting.IoC.Configurations
{
    public static class GeneralConfiguration
    {
        public static void AddGeneralConfiguration(this IServiceCollection services)
        {
            services.AddApplicationConfiguration();
            services.AddDomainConfiguration();
            services.AddInfraConfiguration();
        }

        private static void AddApplicationConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IProductApplicationService, ProductApplicationService>();
        }

        private static void AddDomainConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IMediatorHandler, InMemoryBus>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
        }

        private static void AddInfraConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IProdutctRepository, ProdutctRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
        }
    }
}
