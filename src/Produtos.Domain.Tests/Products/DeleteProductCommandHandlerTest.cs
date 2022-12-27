using Microsoft.Extensions.Logging;
using Moq;
using Produtos.Domain.Core;
using Produtos.Domain.Model;
using Produtos.Domain.Model.Entities;
using Produtos.Domain.Model.Interfaces;
using Produtos.Domain.Model.Interfaces.Repositories;
using Produtos.Domain.Products.Delete;
using Produtos.Tests.Domain.Mocks;
using Xunit;

namespace Produtos.Domain.Tests.Products
{
    public class DeleteProductCommandHandlerTest
    {
        private readonly Mock<IProductRepository> _produtctRepository;
        private readonly Mock<IMediatorHandler> _bus;
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<ILogger<CommandHandler>> _log;

        private readonly DeleteProductCommandHandler _deleteProductCommandHandler;

        public DeleteProductCommandHandlerTest()
        {
            _produtctRepository = new Mock<IProductRepository>();
            _bus = new Mock<IMediatorHandler>();
            _uow = new Mock<IUnitOfWork>();
            _log = new Mock<ILogger<CommandHandler>>();

            _deleteProductCommandHandler = new(_produtctRepository.Object, _bus.Object, _log.Object, _uow.Object);
        }

        [Fact]
        public async Task Handle_Ok()
        {
            //ARRANGE
            var product = ProductMock.Get(1).First();
            var command = new DeleteProductCommand(product.Id);

            _produtctRepository.Setup(x => x.GetById(product.Id))
                .ReturnsAsync(product);

            //ACTION
            await _deleteProductCommandHandler.Handle(command, CancellationToken.None);

            //ASSERT
            _produtctRepository.Verify(x => x.GetById(product.Id), Times.Once);
            _produtctRepository.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
            _uow.Verify(x => x.CompleteAsync(), Times.Once);
            _bus.Verify(x => x.RaiseEvent(It.IsAny<DomainNotification>()), Times.Never);
        }

        [Fact]
        public async Task Handle_InvalidCommand()
        {
            //ARRANGE
            var command = new DeleteProductCommand(-10);

            //ACTION
            await _deleteProductCommandHandler.Handle(command, CancellationToken.None);

            //ASSERT
            _produtctRepository.Verify(x => x.GetById(-10), Times.Never);
            _produtctRepository.Verify(x => x.Update(It.IsAny<Product>()), Times.Never);
            _uow.Verify(x => x.CompleteAsync(), Times.Never);
            _bus.Verify(x => x.RaiseEvent(It.IsAny<DomainNotification>()), Times.Once);
            _log.Verify(x => x.Log(
                It.Is<LogLevel>(x => x == LogLevel.Error),
                It.Is<EventId>(x => x.Id == 0),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        }

        [Fact]
        public async Task Handle_ProductNotFound()
        {
            //ARRANGE
            var command = new DeleteProductCommand(1);

            //ACTION
            await _deleteProductCommandHandler.Handle(command, CancellationToken.None);

            //ASSERT
            _produtctRepository.Verify(x => x.GetById(1), Times.Once);
            _produtctRepository.Verify(x => x.Update(It.IsAny<Product>()), Times.Never);
            _uow.Verify(x => x.CompleteAsync(), Times.Never);
            _bus.Verify(x => x.RaiseEvent(It.IsAny<DomainNotification>()), Times.Once);
            _log.Verify(x => x.Log(
                It.Is<LogLevel>(x => x == LogLevel.Error),
                It.Is<EventId>(x => x.Id == 0),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        }
    }
}
