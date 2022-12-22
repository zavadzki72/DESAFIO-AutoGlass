﻿using Bogus;
using Produtos.Domain.Model.Entities;

namespace Produtos.Tests.Domain.Mocks
{
    public static class ProductMock
    {
        public static List<Product> Get(int quantity, int id = 1, Supplier? supplier = null)
        {
            supplier ??= SupplierMock.Get(1).First();

            var product = new Faker<Product>()
                .RuleFor(x => x.Id, id++)
                .RuleFor(x => x.Description, x => x.Commerce.Product())
                .RuleFor(x => x.ManufacturingDate, DateTime.Today)
                .RuleFor(x => x.ValidDate, DateTime.Today.AddMonths(3))
                .RuleFor(x => x.SupplierId, supplier.Id)
                .RuleFor(x => x.Supplier, supplier);

            return product.Generate(quantity);
        }
    }
}
