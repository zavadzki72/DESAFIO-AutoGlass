using Microsoft.EntityFrameworkCore;
using Produtos.Domain.Model.Entities;
using Produtos.Domain.Model.Interfaces.Repositories;

namespace Produtos.Infra.SqlServer.Repositories
{
    public class ProdutctRepository : BaseRepository<Product>, IProdutctRepository
    {
        public ProdutctRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }

        public override async Task<Product?> GetById(int id)
        {
            return await _applicationDbContext.Set<Product>()
                .Include(x => x.Supplier)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
