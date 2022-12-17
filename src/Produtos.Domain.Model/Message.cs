using MediatR;

namespace Produtos.Domain.Model
{
    public abstract class Message : IRequest
    {
        protected Message()
        {
            MessageType = GetType().Name;
        }

        public string MessageType { get; protected set; }
    }
}
