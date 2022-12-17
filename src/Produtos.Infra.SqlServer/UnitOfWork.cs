using Produtos.Domain.Model.Interfaces;

namespace Produtos.Infra.SqlServer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Complete()
        {
            _context.SaveChanges();
        }

        public Task CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
