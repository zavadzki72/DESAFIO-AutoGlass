using MediatR;
using Microsoft.Extensions.Logging;
using Produtos.Domain.Core;
using Produtos.Domain.Model.Interfaces;
using Produtos.Domain.Model.Interfaces.Repositories;

namespace Produtos.Domain.Products.Delete
{
    public class DeleteProductCommandHandler : CommandHandler, IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository, IMediatorHandler bus, ILogger<CommandHandler> logger, IUnitOfWork unitOfWork) : base(bus, logger, unitOfWork)
        {
            _productRepository = productRepository;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await NotifyValidationErrors(request);
                return Unit.Value;
            }

            var product = await _productRepository.GetById(request.Id);

            if(product == null)
            {
                await NotifyError("NOT_FOUND", $"The product with id {request.Id} is not found");
                return Unit.Value;
            }

            await _productRepository.Delete(product);
            await Commit();

            return Unit.Value;
        }
    }
}
