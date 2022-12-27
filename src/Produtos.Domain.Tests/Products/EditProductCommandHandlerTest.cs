using Microsoft.Extensions.Logging;
using Moq;
using Produtos.Domain.Core;
using Produtos.Domain.Model;
using Produtos.Domain.Model.Interfaces;
using Produtos.Domain.Model.Interfaces.Repositories;
using Produtos.Domain.Products.Edit;
using Produtos.Tests.Domain.Mocks;
using Xunit;

namespace Produtos.Domain.Tests.Products
{
    public class EditProductCommandHandlerTest
    {
        private readonly Mock<ISupplierRepository> _supplierRepository;
        private readonly Mock<IProductRepository> _produtctRepository;
        private readonly Mock<IMediatorHandler> _bus;
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<ILogger<CommandHandler>> _log;

        private readonly EditProductCommandHandler _editProductCommandHandler;

        public EditProductCommandHandlerTest()
        {
            _supplierRepository = new Mock<ISupplierRepository>();
            _produtctRepository = new Mock<IProductRepository>();
            _bus = new Mock<IMediatorHandler>();
            _uow = new Mock<IUnitOfWork>();
            _log = new Mock<ILogger<CommandHandler>>();

            _editProductCommandHandler = new(_supplierRepository.Object, _produtctRepository.Object, _bus.Object, _log.Object, _uow.Object);
        }

        [Fact]
        public async Task Handle_Ok_WithoutSupplierInRequest()
        {
            //ARRANGE
            var product = ProductMock.Get(1).First();
            var command = new EditProductCommand(product.Id, product.Description, product.ManufacturingDate, product.ValidDate, string.Empty, string.Empty);

            _produtctRepository.Setup(x => x.GetById(product.Id))
                .ReturnsAsync(product);

            //ACTION
            await _editProductCommandHandler.Handle(command, CancellationToken.None);

            //ASSERT
            _produtctRepository.Verify(x => x.GetById(product.Id), Times.Once);
            _supplierRepository.Verify(x => x.GetByCnpj(product.Supplier.Cnpj), Times.Never);
            _uow.Verify(x => x.CompleteAsync(), Times.Once);
            _bus.Verify(x => x.RaiseEvent(It.IsAny<DomainNotification>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Ok_WithSupplierInRequest()
        {
            //ARRANGE
            var product = ProductMock.Get(1).First();
            var command = new EditProductCommand(product.Id, product.Description, product.ManufacturingDate, product.ValidDate, "Novo fornecedor", "12345678910125");

            _produtctRepository.Setup(x => x.GetById(product.Id))
                .ReturnsAsync(product);

            _supplierRepository.Setup(x => x.GetByCnpj(product.Supplier.Cnpj))
                .ReturnsAsync(product.Supplier);

            //ACTION
            await _editProductCommandHandler.Handle(command, CancellationToken.None);

            //ASSERT
            _produtctRepository.Verify(x => x.GetById(product.Id), Times.Once);
            _supplierRepository.Verify(x => x.GetByCnpj("12345678910125"), Times.Once);
            _uow.Verify(x => x.CompleteAsync(), Times.Once);
            _bus.Verify(x => x.RaiseEvent(It.IsAny<DomainNotification>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Ok_WithSupplierInRequestAndNotInDatabase()
        {
            //ARRANGE
            var product = ProductMock.Get(1).First();
            var command = new EditProductCommand(product.Id, product.Description, product.ManufacturingDate, product.ValidDate, "Novo fornecedor", "12345678910125");

            _produtctRepository.Setup(x => x.GetById(product.Id))
                .ReturnsAsync(product);

            //ACTION
            await _editProductCommandHandler.Handle(command, CancellationToken.None);

            //ASSERT
            _produtctRepository.Verify(x => x.GetById(product.Id), Times.Once);
            _supplierRepository.Verify(x => x.GetByCnpj("12345678910125"), Times.Once);
            _uow.Verify(x => x.CompleteAsync(), Times.Once);
            _bus.Verify(x => x.RaiseEvent(It.IsAny<DomainNotification>()), Times.Never);
        }


        [Fact]
        public async Task Handle_ProductNotFound()
        {
            //ARRANGE
            var product = ProductMock.Get(1).First();
            var command = new EditProductCommand(product.Id, product.Description, product.ManufacturingDate, product.ValidDate, "Novo fornecedor", "12345678910125");

            //ACTION
            await _editProductCommandHandler.Handle(command, CancellationToken.None);

            //ASSERT
            _produtctRepository.Verify(x => x.GetById(product.Id), Times.Once);
            _supplierRepository.Verify(x => x.GetByCnpj("12345678910125"), Times.Never);
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
        public async Task Handle_SupplierDescriptionMandatory()
        {
            //ARRANGE
            var product = ProductMock.Get(1).First();
            var command = new EditProductCommand(product.Id, product.Description, product.ManufacturingDate, product.ValidDate, string.Empty, "12345678910125");

            _produtctRepository.Setup(x => x.GetById(product.Id))
                .ReturnsAsync(product);

            //ACTION
            await _editProductCommandHandler.Handle(command, CancellationToken.None);

            //ASSERT
            _produtctRepository.Verify(x => x.GetById(product.Id), Times.Once);
            _supplierRepository.Verify(x => x.GetByCnpj("12345678910125"), Times.Once);
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
        public async Task Handle_InvalidCommand()
        {
            //ARRANGE
            var product = ProductMock.Get(1).First();
            var command = new EditProductCommand(product.Id, product.Description, product.ManufacturingDate, DateTime.Today.AddMonths(-10), product.Supplier.Description, product.Supplier.Cnpj);

            //ACTION
            await _editProductCommandHandler.Handle(command, CancellationToken.None);

            //ASSERT
            _produtctRepository.Verify(x => x.GetById(product.Id), Times.Never);
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
