using Moq;
using Produtos.CrossCutting.AutoMapper;
using Produtos.Domain.Core;
using Produtos.Domain.Model;
using Produtos.Domain.Model.ApiContracts;
using Produtos.Domain.Model.Dtos.Filters;
using Produtos.Domain.Model.Interfaces;
using Produtos.Domain.Model.Interfaces.Repositories;
using Produtos.Domain.Model.ViewModels.Products;
using Produtos.Domain.Model.ViewModels.Suppliers;
using Produtos.Domain.Products.Delete;
using Produtos.Domain.Products.Edit;
using Produtos.Domain.Products.Register;
using Produtos.Tests.Domain.Mocks;
using Xunit;

namespace Produtos.ApplicationService.Tests
{
    public class ProductApplicationServiceTest
    {
        private readonly Mock<IProductRepository> _produtctRepository;
        private readonly Mock<IMediatorHandler> _bus;
        private readonly Mock<DomainNotificationHandler> _notifications;

        private readonly ProductApplicationService _productApplicationService;

        public ProductApplicationServiceTest()
        {
            _produtctRepository = new Mock<IProductRepository>();
            _bus = new Mock<IMediatorHandler>();
            _notifications = new Mock<DomainNotificationHandler>();
            
            var mapper = AutoMapperConfig.RegisterMappings().CreateMapper();

            _productApplicationService = new(_produtctRepository.Object, _bus.Object, mapper, _notifications.Object);
        }

        [Fact]
        public async Task GetById_Ok()
        {
            //ARRANGE
            var product = ProductMock.Get(1).First();

            _produtctRepository.Setup(x => x.GetById(product.Id)).ReturnsAsync(product);

            //ACTION
            var result = await _productApplicationService.GetById(product.Id);

            //ASSERT
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.False(result.Notifications.Any());
        }

        [Fact]
        public async Task GetById_NotFound()
        {
            //ARRANGE
            int id = 1;

            //ACTION
            var result = await _productApplicationService.GetById(id);

            //ASSERT
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.True(result.Notifications.Any());
            Assert.Equal($"The product with id: {id} was not found", result.Notifications.First().Message);
        }

        [Fact]
        public async Task GetByFilter_Ok()
        {
            //ARRANGE
            var product = new PaginatedProductResponseViewModel
            {
                Id = 1,
                DefaultOrderId = 1,
                Description = "Product Test",
                ManufacturingDate = DateTime.Today,
                ValidDate = DateTime.Today.AddMonths(3),
                Supplier = new SupplierResponseViewModel
                {
                    Id = 1,
                    Cnpj = "12345678910111",
                    Description = "Supplier Test"
                }
            };

            _produtctRepository.Setup(x => x.GetByFilter(It.IsAny<ProductFilter>()))
                .ReturnsAsync(new PagedRepositoryResponse<PaginatedProductResponseViewModel>(new List<PaginatedProductResponseViewModel> { product }, 1));

            //ACTION
            var result = await _productApplicationService.GetByFilter(new GetProductsByFilter { Size = 10 });

            //ASSERT
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.False(result.Notifications.Any());
            Assert.True(result.Model.Data.Count == 1);
        }

        [Fact]
        public async Task Register_Ok()
        {
            //ARRANGE
            var viewModel = new RegisterProductViewModel { 
                Description = "Test",
                ManufacturingDate = DateTime.Today,
                ValidDate = DateTime.Today.AddMonths(3),
                Supplier = new RegisterSupplierViewModel
                {
                    Cnpj = "12345678910111",
                    Description = "Supplier Test"
                }
            };

            _bus.Setup(x => x.SendCommand<RegisterProductCommand, int>(It.IsAny<RegisterProductCommand>())).ReturnsAsync(1);

            //ACTION
            var result = await _productApplicationService.Register(viewModel);

            //ASSERT
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.False(result.Notifications.Any());
            Assert.Equal("products/1", result.RouteLocation);

            _bus.Verify(x => x.SendCommand<RegisterProductCommand, int>(It.IsAny<RegisterProductCommand>()), Times.Once);
        }

        [Fact]
        public async Task Edit_Ok()
        {
            //ARRANGE
            var viewModel = new RegisterProductViewModel
            {
                Description = "Test",
                ManufacturingDate = DateTime.Today,
                ValidDate = DateTime.Today.AddMonths(3),
                Supplier = new RegisterSupplierViewModel
                {
                    Cnpj = "12345678910111",
                    Description = "Supplier Test"
                }
            };

            //ACTION
            var result = await _productApplicationService.Edit(1, viewModel);

            //ASSERT
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.False(result.Notifications.Any());

            _bus.Verify(x => x.SendCommand(It.IsAny<EditProductCommand>()), Times.Once);
        }

        [Fact]
        public async Task Delete_Ok()
        {
            //ARRANGE
            int id = 1;

            //ACTION
            var result = await _productApplicationService.Delete(id);

            //ASSERT
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.False(result.Notifications.Any());

            _bus.Verify(x => x.SendCommand(It.IsAny<DeleteProductCommand>()), Times.Once);
        }
    }
}