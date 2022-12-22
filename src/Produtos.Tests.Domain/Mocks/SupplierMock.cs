using Bogus;
using Bogus.Extensions.Brazil;
using Produtos.Domain.Model.Entities;

namespace Produtos.Tests.Domain.Mocks
{
    public static class SupplierMock
    {
        public static List<Supplier> Get(int quantity, int id = 1)
        {
            var supplier = new Faker<Supplier>()
                .RuleFor(x => x.Id, id++)
                .RuleFor(x => x.Description, x => x.Company.CompanyName())
                .RuleFor(x => x.Cnpj, x => x.Company.Cnpj());

            return supplier.Generate(quantity);
        }
    }
}
