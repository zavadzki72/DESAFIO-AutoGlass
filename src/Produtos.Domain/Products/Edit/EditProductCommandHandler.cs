using MediatR;
using Microsoft.Extensions.Logging;
using Produtos.Domain.Core;
using Produtos.Domain.Model.Entities;
using Produtos.Domain.Model.Interfaces;
using Produtos.Domain.Model.Interfaces.Repositories;

namespace Produtos.Domain.Products.Edit
{
    public class EditProductCommandHandler : CommandHandler, IRequestHandler<EditProductCommand>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductRepository _productRepository;

        public EditProductCommandHandler(ISupplierRepository supplierRepository, IProductRepository productRepository, IMediatorHandler bus, ILogger<CommandHandler> logger, IUnitOfWork unitOfWork) : base(bus, logger, unitOfWork)
        {
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
        }

        public async Task<Unit> Handle(EditProductCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await NotifyValidationErrors(request);
                return Unit.Value;
            }

            var product = await _productRepository.GetById(request.Id);
            
            if (product == null)
            {
                await NotifyError("NOT_FOUND", $"The product with id {request.Id} is not found");
                return Unit.Value;
            }

            var supplier = product.Supplier;

            if(!string.IsNullOrWhiteSpace(request.SupplierCnpj) && !request.SupplierCnpj.Equals(supplier.Cnpj))
            {
                supplier = await _supplierRepository.GetByCnpj(request.SupplierCnpj);

                if (supplier == null && string.IsNullOrWhiteSpace(request.SupplierDescritpion))
                {
                    await NotifyError("INVALID_FIELD", $"The supplier description is mandatory, because the supplier cnpj does not exists.");
                    return Unit.Value;
                }

                supplier ??= new Supplier(request.SupplierDescritpion, request.SupplierCnpj);
            }
            else
            {
                supplier.Edit(request.SupplierDescritpion, request.SupplierCnpj);
            }

            product.Edit(request.Description, request.ManufacturingDate, request.ValidDate);
            product.SetSupplier(supplier);

            await Commit();

            return Unit.Value;
        }
    }
}
