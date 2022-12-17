namespace Produtos.Domain.Model.Interfaces
{
    public interface IMediatorHandler
    {
        Task SendCommand<T>(T command) where T : Command;
        Task<TResponse> SendCommand<TRequest, TResponse>(TRequest command) where TRequest : Command<TResponse>;
        Task RaiseEvent<T>(T @event) where T : Event;
    }
}
