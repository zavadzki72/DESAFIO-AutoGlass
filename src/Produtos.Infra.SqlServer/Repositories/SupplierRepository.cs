using Produtos.Domain.Model.Entities;
using Produtos.Domain.Model.Interfaces.Repositories;

namespace Produtos.Infra.SqlServer.Repositories
{
    public class SupplierRepository : BaseRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }
    }
}
