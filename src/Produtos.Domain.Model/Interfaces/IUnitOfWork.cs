namespace Produtos.Domain.Model.Interfaces
{
    public interface IUnitOfWork
    {
        void Complete();
        Task CompleteAsync();
    }
}
