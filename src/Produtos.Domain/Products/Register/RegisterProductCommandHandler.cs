using MediatR;
using Microsoft.Extensions.Logging;
using Produtos.Domain.Core;
using Produtos.Domain.Model.Entities;
using Produtos.Domain.Model.Interfaces;
using Produtos.Domain.Model.Interfaces.Repositories;

namespace Produtos.Domain.Products.Register
{
    public class RegisterProductCommandHandler : CommandHandler, IRequestHandler<RegisterProductCommand, int>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductRepository _productRepository;

        public RegisterProductCommandHandler(ISupplierRepository supplierRepository, IProductRepository productRepository, IMediatorHandler bus, ILogger<CommandHandler> logger, IUnitOfWork unitOfWork) : base(bus, logger, unitOfWork)
        {
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
        }

        public async Task<int> Handle(RegisterProductCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await NotifyValidationErrors(request);
                return 0;
            }

            var supplier = await _supplierRepository.GetByCnpj(request.SupplierCnpj);
            supplier ??= new Supplier(request.SupplierDescritpion, request.SupplierCnpj);
            
            var product = new Product(request.Description, request.ManufacturingDate.Value, request.ValidDate.Value);
            product.SetSupplier(supplier);

            var result = await _productRepository.Add(product);
            await Commit();

            return result.Id;
        }
    }
}
