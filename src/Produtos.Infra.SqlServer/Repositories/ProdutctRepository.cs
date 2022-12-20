using Microsoft.EntityFrameworkCore;
using Produtos.Domain.Model.Dtos.Filters;
using Produtos.Domain.Model.Entities;
using Produtos.Domain.Model.Interfaces.Repositories;
using Produtos.Domain.Core;
using Produtos.Domain.Model.ViewModels.Products;
using Produtos.Domain.Model.ViewModels.Suppliers;
using Produtos.Domain.Model;

namespace Produtos.Infra.SqlServer.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }

        public override async Task<Product?> GetById(int id)
        {
            return await _applicationDbContext.Set<Product>()
                .Include(x => x.Supplier)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedRepositoryResponse<PaginatedProductResponseViewModel>> GetByFilter(ProductFilter filter)
        {
            var query = (
                from p in _applicationDbContext.Set<Product>()
                    join s in _applicationDbContext.Set<Supplier>() on p.SupplierId equals s.Id
                where p.IsActive
                select new PaginatedProductResponseViewModel
                {
                    Id = p.Id,
                    DefaultOrderId = p.Id,
                    Description = p.Description,
                    ManufacturingDate = p.ManufacturingDate,
                    ValidDate = p.ValidDate,
                    Supplier = new SupplierResponseViewModel
                    {
                        Id = s.Id,
                        Cnpj = s.Cnpj,
                        Description = s.Description
                    }
                }
            );

            if (filter.Ids.Any())
            {
                query = query.Where(x => filter.Ids.Contains(x.Id));
            }

            if (filter.Descriptions.Any())
            {
                query = query.Where(x => filter.Descriptions.Contains(x.Description));
            }

            if (filter.SupplierIds.Any())
            {
                query = query.Where(x => filter.SupplierIds.Contains(x.Supplier.Id));
            }

            if (filter.SupplierCnpjs.Any())
            {
                query = query.Where(x => filter.SupplierCnpjs.Contains(x.Supplier.Cnpj));
            }

            if (filter.SupplierDescriptions.Any())
            {
                query = query.Where(x => filter.SupplierDescriptions.Contains(x.Supplier.Description));
            }

            if (filter.MinManufactureDate.HasValue)
            {
                query = query.Where(x => x.ManufacturingDate > filter.MinManufactureDate);
            }

            if (filter.MaxManufactureDate.HasValue)
            {
                query = query.Where(x => x.ManufacturingDate < filter.MaxManufactureDate);
            }

            if (filter.MinValidDate.HasValue)
            {
                query = query.Where(x => x.ValidDate > filter.MinValidDate);
            }

            if (filter.MaxValidDate.HasValue)
            {
                query = query.Where(x => x.ValidDate < filter.MaxValidDate);
            }

            var result = await query.GetPaginatedResult(filter);

            return result;
        }
    }
}
