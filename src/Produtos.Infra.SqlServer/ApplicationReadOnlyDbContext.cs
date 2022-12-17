using Microsoft.EntityFrameworkCore;

namespace Produtos.Infra.SqlServer
{
    public class ApplicationReadOnlyDbContext : ApplicationDbContext
    {
        public ApplicationReadOnlyDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions) { }
    }
}
