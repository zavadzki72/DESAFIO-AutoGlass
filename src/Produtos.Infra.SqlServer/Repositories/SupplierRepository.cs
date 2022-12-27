using Microsoft.EntityFrameworkCore;
using Produtos.Domain.Model.Entities;
using Produtos.Domain.Model.Interfaces.Repositories;

namespace Produtos.Infra.SqlServer.Repositories
{
    public class SupplierRepository : BaseRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(ApplicationDbContext applicationDbContext, ApplicationReadOnlyDbContext applicationReadOnlyDbContext) : base(applicationDbContext, applicationReadOnlyDbContext) { }

        public async Task<Supplier?> GetByCnpj(string cnpj)
        {
            return await _applicationDbContext.Set<Supplier>().FirstOrDefaultAsync(x => x.Cnpj.Equals(cnpj));
        }
    }
}
