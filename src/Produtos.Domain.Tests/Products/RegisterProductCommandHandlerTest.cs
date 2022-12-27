using Microsoft.Extensions.Logging;
using Moq;
using Produtos.Domain.Core;
using Produtos.Domain.Model;
using Produtos.Domain.Model.Entities;
using Produtos.Domain.Model.Interfaces;
using Produtos.Domain.Model.Interfaces.Repositories;
using Produtos.Domain.Products.Register;
using Produtos.Tests.Domain.Mocks;
using Xunit;

namespace Produtos.Domain.Tests.Products
{
    public class RegisterProductCommandHandlerTest
    {
        private readonly Mock<ISupplierRepository> _supplierRepository;
        private readonly Mock<IProductRepository> _produtctRepository;
        private readonly Mock<IMediatorHandler> _bus;
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<ILogger<CommandHandler>> _log;

        private readonly RegisterProductCommandHandler _registerProductCommandHandler;

        public RegisterProductCommandHandlerTest()
        {
            _supplierRepository = new Mock<ISupplierRepository>();
            _produtctRepository = new Mock<IProductRepository>();
            _bus = new Mock<IMediatorHandler>();
            _uow = new Mock<IUnitOfWork>();
            _log = new Mock<ILogger<CommandHandler>>();

            _registerProductCommandHandler = new(_supplierRepository.Object, _produtctRepository.Object, _bus.Object, _log.Object, _uow.Object);
        }

        [Fact]
        public async Task Handle_Ok_WithoutSupplier()
        {
            //ARRANGE
            var product = ProductMock.Get(1).First();
            var command = new RegisterProductCommand(product.Description, product.ManufacturingDate, product.ValidDate, product.Supplier.Description, product.Supplier.Cnpj);

            _produtctRepository.Setup(x => x.Add(It.IsAny<Product>()))
                .ReturnsAsync(product);

            //ACTION
            var result = await _registerProductCommandHandler.Handle(command, CancellationToken.None);

            //ASSERT
            Assert.False(result == 0);
            Assert.Equal(product.Id, result);

            _produtctRepository.Verify(x => x.Add(It.IsAny<Product>()), Times.Once);
            _uow.Verify(x => x.CompleteAsync(), Times.Once);
            _bus.Verify(x => x.RaiseEvent(It.IsAny<DomainNotification>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Ok_WithSupplier()
        {
            //ARRANGE
            var product = ProductMock.Get(1).First();
            var command = new RegisterProductCommand(product.Description, product.ManufacturingDate, product.ValidDate, product.Supplier.Description, product.Supplier.Cnpj);

            _produtctRepository.Setup(x => x.Add(It.IsAny<Product>()))
                .ReturnsAsync(product);

            _supplierRepository.Setup(x => x.GetByCnpj(command.SupplierCnpj))
                .ReturnsAsync(product.Supplier);

            //ACTION
            var result = await _registerProductCommandHandler.Handle(command, CancellationToken.None);

            //ASSERT
            Assert.False(result == 0);
            Assert.Equal(product.Id, result);

            _produtctRepository.Verify(x => x.Add(It.IsAny<Product>()), Times.Once);
            _uow.Verify(x => x.CompleteAsync(), Times.Once);
            _bus.Verify(x => x.RaiseEvent(It.IsAny<DomainNotification>()), Times.Never);
        }

        [Fact]
        public async Task Handle_InvalidCommand()
        {
            //ARRANGE
            var product = ProductMock.Get(1).First();
            var command = new RegisterProductCommand(product.Description, product.ManufacturingDate, DateTime.Today.AddMonths(-10), product.Supplier.Description, product.Supplier.Cnpj);

            //ACTION
            var result = await _registerProductCommandHandler.Handle(command, CancellationToken.None);

            //ASSERT
            Assert.True(result == 0);

            _produtctRepository.Verify(x => x.Add(It.IsAny<Product>()), Times.Never);
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