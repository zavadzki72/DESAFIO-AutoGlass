using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Produtos.Domain.Model.Dtos.Filters;
using Produtos.Infra.SqlServer.Repositories;
using Produtos.Tests.Domain.Mocks;
using Xunit;

namespace Produtos.Infra.SqlServer.Tests
{
    public class ProductRepositoryTest
    {
        [Fact]
        public async Task GetById_Ok()
        {
            //ARRANGE
            var repository = await GetInstance(FillDatabase);

            //ACTION
            var response = await repository.GetById(1);

            //ASSERT
            Assert.NotNull(response);
            Assert.Equal(1, response.Id);
        }

        [Fact]
        public async Task GetByFilter_Ok()
        {
            //ARRANGE
            var repository = await GetInstance(FillDatabase);
            var filter = new ProductFilter
            {
                Page = 0,
                Size = 10,
            };

            //ACTION
            var response = await repository.GetByFilter(filter);

            //ASSERT
            Assert.NotNull(response);
            Assert.Equal(10, response.CountData);
        }

        [Fact]
        public async Task GetByFilter_Ok_FilterById()
        {
            //ARRANGE
            var repository = await GetInstance(FillDatabase);
            var filter = new ProductFilter
            {
                Page = 0,
                Size = 10,
                Ids = new List<int> { 1, 2 }
            };

            //ACTION
            var response = await repository.GetByFilter(filter);

            //ASSERT
            Assert.NotNull(response);
            Assert.Equal(2, response.CountData);
        }

        private static async Task FillDatabase(ApplicationDbContext applicationDbContext, ApplicationReadOnlyDbContext applicationReadOnlyDbContext)
        {
            var products = ProductMock.Get(10);

            await applicationDbContext.AddRangeAsync(products);
            await applicationReadOnlyDbContext.AddRangeAsync(products);

            await applicationDbContext.SaveChangesAsync();
            await applicationReadOnlyDbContext.SaveChangesAsync();
        }

        private static async Task<ProductRepository> GetInstance(Func<ApplicationDbContext, ApplicationReadOnlyDbContext, Task> FillDatabase)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var context = new ApplicationDbContext(options);
            var readOnlyContext = new ApplicationReadOnlyDbContext(options);

            await FillDatabase(context, readOnlyContext);

            return new ProductRepository(context, readOnlyContext);
        }
    }
}