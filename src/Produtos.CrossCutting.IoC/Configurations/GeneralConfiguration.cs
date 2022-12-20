using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Produtos.ApplicationService;
using Produtos.CrossCutting.bus;
using Produtos.Domain.Core;
using Produtos.Domain.Model;
using Produtos.Domain.Model.Interfaces;
using Produtos.Domain.Model.Interfaces.ApplicationServices;
using Produtos.Domain.Model.Interfaces.Repositories;
using Produtos.Domain.Model.ViewModels.Products;
using Produtos.Domain.Model.ViewModels.Products.Validator;
using Produtos.Domain.Products.Delete;
using Produtos.Domain.Products.Edit;
using Produtos.Domain.Products.Register;
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

            services.AddTransient<IValidator<GetProductsByFilter>, GetProductsByFilterValidator>();
        }

        private static void AddDomainConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IMediatorHandler, InMemoryBus>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            services.AddScoped<IRequestHandler<RegisterProductCommand, int>, RegisterProductCommandHandler>();
            services.AddScoped<IRequestHandler<EditProductCommand>, EditProductCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteProductCommand>, DeleteProductCommandHandler>();
        }

        private static void AddInfraConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
        }
    }
}
