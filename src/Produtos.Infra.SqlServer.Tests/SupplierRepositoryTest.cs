using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Produtos.Domain.Model.Entities;
using Produtos.Infra.SqlServer.Repositories;
using Produtos.Tests.Domain.Mocks;
using Xunit;

namespace Produtos.Infra.SqlServer.Tests
{
    public class SupplierRepositoryTest
    {

        [Fact]
        public async Task GetByCnpj_Ok()
        {
            //ARRANGE
            var instance = await GetInstance(FillDatabase);
            var repository = instance.Item1;
            var data = instance.Item2;

            //ACTION
            var response = await repository.GetByCnpj(data.First().Cnpj);

            //ASSERT
            Assert.NotNull(response);
            Assert.Equal(data.First().Cnpj, response.Cnpj);
        }

        private static async Task<List<Supplier>> FillDatabase(ApplicationDbContext applicationDbContext, ApplicationReadOnlyDbContext applicationReadOnlyDbContext)
        {
            var suppliers = SupplierMock.Get(10);

            await applicationDbContext.AddRangeAsync(suppliers);
            await applicationReadOnlyDbContext.AddRangeAsync(suppliers);

            await applicationDbContext.SaveChangesAsync();
            await applicationReadOnlyDbContext.SaveChangesAsync();

            return suppliers;
        }

        private static async Task<(SupplierRepository, List<Supplier>)> GetInstance(Func<ApplicationDbContext, ApplicationReadOnlyDbContext, Task<List<Supplier>>> FillDatabase)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var context = new ApplicationDbContext(options);
            var readOnlyContext = new ApplicationReadOnlyDbContext(options);

            var data = await FillDatabase(context, readOnlyContext);

            return (new SupplierRepository(context, readOnlyContext), data);
        }
    }
}
