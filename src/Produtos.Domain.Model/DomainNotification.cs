using Produtos.Domain.Model.Enumerators;

namespace Produtos.Domain.Model
{
    public class DomainNotification : Event
    {
        public DomainNotification(DomainNotificationKey domainNotificationKey, string key, string message)
        {
            DomainNotificationKey = domainNotificationKey;
            Key = key;
            Message = message;
        }

        public DomainNotificationKey DomainNotificationKey { get; private set; }
        public string Key { get; private set; }
        public string Message { get; private set; }
    }
}
