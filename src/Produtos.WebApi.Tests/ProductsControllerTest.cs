using Bogus;
using Bogus.Extensions.Brazil;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Produtos.Domain.Model;
using Produtos.Domain.Model.ApiContracts;
using Produtos.Domain.Model.Enumerators;
using Produtos.Domain.Model.Interfaces.ApplicationServices;
using Produtos.Domain.Model.ViewModels.Products;
using Produtos.Domain.Model.ViewModels.Suppliers;
using Produtos.WebApi.Controllers;
using Xunit;

namespace Produtos.WebApi.Tests
{
    public class ProductsControllerTest
    {
        private readonly Mock<IProductApplicationService> _productApplicationService;
        private readonly Mock<IValidator<GetProductsByFilter>> _getProductsByFilterValidator;

        private readonly ProductsController _productsController;

        public ProductsControllerTest()
        {
            _productApplicationService = new Mock<IProductApplicationService>();
            _getProductsByFilterValidator = new Mock<IValidator<GetProductsByFilter>>();

            _productsController = new(_productApplicationService.Object, _getProductsByFilterValidator.Object);
        }

        [Fact]
        public async Task GetById_Ok()
        {
            //ARRANGE
            int id = 10;

            var response = GetProductResponseViewModel(10, 1);

            _productApplicationService.Setup(x => x.GetById(id)).ReturnsAsync(ServiceResult<ProductResponseViewModel>.Ok(response, new List<DomainNotification>()));

            //ACTION
            var result = await _productsController.GetById(id);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<ActionResult<ProductResponseViewModel>>(result);

            _productApplicationService.Verify(x => x.GetById(id), Times.Once);
        }

        [Fact]
        public async Task GetById_NotFound()
        {
            //ARRANGE
            int id = 10;

            _productApplicationService.Setup(x => x.GetById(id))
                .ReturnsAsync(ServiceResult<ProductResponseViewModel>.NotFound(new List<DomainNotification>() { new DomainNotification(DomainNotificationKey.NOT_FOUND, "TEST", "NOT FOUND TEST")}));

            //ACTION
            var result = await _productsController.GetById(id);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<ActionResult<ProductResponseViewModel>>(result);

            _productApplicationService.Verify(x => x.GetById(id), Times.Once);
        }

        [Fact]
        public async Task GetById_Error()
        {
            //ARRANGE
            int id = 10;

            _productApplicationService.Setup(x => x.GetById(id))
                .ReturnsAsync(ServiceResult<ProductResponseViewModel>.Error(new List<DomainNotification>() { new DomainNotification(DomainNotificationKey.ERROR, "TEST", "ERROR TEST") }));

            //ACTION
            var result = await _productsController.GetById(id);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<ActionResult<ProductResponseViewModel>>(result);

            _productApplicationService.Verify(x => x.GetById(id), Times.Once);
        }

        [Fact]
        public async Task GetByFilter_Ok()
        {
            //ARRANGE
            var filter = new GetProductsByFilter
            {
                Ids = new List<int> { 10 }
            };

            var response = GetPaginatedProductResponseViewModelList(5);
            var paginatedResult = new PaginatedResult<List<PaginatedProductResponseViewModel>>(response, 0, 5, 10);

            _getProductsByFilterValidator.Setup(x => x.ValidateAsync(filter, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _productApplicationService.Setup(x => x.GetByFilter(filter))
                .ReturnsAsync(ServiceResult<PaginatedResult<List<PaginatedProductResponseViewModel>>>.Ok(paginatedResult, new List<DomainNotification>()));

            //ACTION
            var result = await _productsController.GetByFilter(filter);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<ActionResult<PaginatedProductResponseViewModel>>(result);

            _productApplicationService.Verify(x => x.GetByFilter(filter), Times.Once);
        }

        [Fact]
        public async Task GetByFilter_InvalidModel()
        {
            //ARRANGE
            var filter = new GetProductsByFilter
            {
                Ids = new List<int> { 10 }
            };

            var validationErrors = new List<ValidationFailure> { 
                new ValidationFailure("Test", "Test message")
            };

            _getProductsByFilterValidator.Setup(x => x.ValidateAsync(filter, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

            //ACTION
            var result = await _productsController.GetByFilter(filter);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<ActionResult<PaginatedProductResponseViewModel>>(result);

            _productApplicationService.Verify(x => x.GetByFilter(filter), Times.Never);
        }

        [Fact]
        public async Task Register_Ok()
        {
            //ARRANGE
            var viewModel = new RegisterProductViewModel
            {
                Description = "Test product",
                ManufacturingDate = DateTime.Today,
                ValidDate = DateTime.Today.AddMonths(3),
                Supplier = new RegisterSupplierViewModel
                {
                    Cnpj = "12345678910123",
                    Description = "Test supplier"
                }
            };

            _productApplicationService.Setup(x => x.Register(viewModel))
                .ReturnsAsync(ServiceResult<int>.Created("products/1", new List<DomainNotification>()));

            //ACTION
            var result = await _productsController.Register(viewModel);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<ActionResult<int>>(result);

            _productApplicationService.Verify(x => x.Register(viewModel), Times.Once);
        }

        [Fact]
        public async Task Edit_Ok()
        {
            //ARRANGE
            var viewModel = new RegisterProductViewModel
            {
                Description = "Test product",
                ManufacturingDate = DateTime.Today,
                ValidDate = DateTime.Today.AddMonths(3),
                Supplier = new RegisterSupplierViewModel
                {
                    Cnpj = "12345678910123",
                    Description = "Test supplier"
                }
            };

            _productApplicationService.Setup(x => x.Edit(1, viewModel))
                .ReturnsAsync(ServiceResult.OkEmpty(new List<DomainNotification>()));

            //ACTION
            var result = await _productsController.Edit(1, viewModel);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);

            _productApplicationService.Verify(x => x.Edit(1, viewModel), Times.Once);
        }

        [Fact]
        public async Task Delete_Ok()
        {
            //ARRANGE
            int id = 10;

            _productApplicationService.Setup(x => x.Delete(id))
                .ReturnsAsync(ServiceResult.OkEmpty(new List<DomainNotification>()));

            //ACTION
            var result = await _productsController.Delete(id);

            //ASSERT
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);

            _productApplicationService.Verify(x => x.Delete(id), Times.Once);
        }

        private static ProductResponseViewModel GetProductResponseViewModel(int id, int supplierId)
        {
            var supplier = new Faker<SupplierResponseViewModel>()
                .RuleFor(x => x.Id, supplierId)
                .RuleFor(x => x.Description, x => x.Company.CompanyName())
                .RuleFor(x => x.Cnpj, x => x.Company.Cnpj());

            var product = new Faker<ProductResponseViewModel>()
                .RuleFor(x => x.Id, id)
                .RuleFor(x => x.Description, x => x.Commerce.Product())
                .RuleFor(x => x.ManufacturingDate, DateTime.Today.AddDays(1))
                .RuleFor(x => x.ValidDate, DateTime.Today.AddMonths(3))
                .RuleFor(x => x.Supplier, supplier);

            return product;
        }

        private static List<PaginatedProductResponseViewModel> GetPaginatedProductResponseViewModelList(int quantity)
        {
            var supplier = new Faker<SupplierResponseViewModel>()
                .RuleFor(x => x.Id, x => x.Random.Int(1))
                .RuleFor(x => x.Description, x => x.Company.CompanyName())
                .RuleFor(x => x.Cnpj, x => x.Company.Cnpj());

            var suppliers = supplier.Generate(quantity);

            var product = new Faker<PaginatedProductResponseViewModel>()
                .RuleFor(x => x.Id, x => x.Random.Int(1))
                .RuleFor(x => x.Description, x => x.Commerce.Product())
                .RuleFor(x => x.ManufacturingDate, DateTime.Today.AddDays(1))
                .RuleFor(x => x.ValidDate, DateTime.Today.AddMonths(3))
                .RuleFor(x => x.Supplier, x => x.PickRandom(suppliers));

            return product.Generate(quantity);
        }
    }
}