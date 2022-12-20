using MediatR;
using Produtos.Domain.Model;
using Produtos.Domain.Model.Enumerators;

namespace Produtos.Domain.Core
{
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {
        private readonly List<DomainNotification> _notifications;

        public DomainNotificationHandler()
        {
            _notifications = new List<DomainNotification>();
        }

        public Task Handle(DomainNotification notification, CancellationToken cancellationToken)
        {
            _notifications.Add(notification);
            return Task.CompletedTask;
        }

        public List<DomainNotification> GetNotifications()
        {
            return _notifications;
        }

        public bool HasValidationErrors()
        {
            return _notifications.Any(x => x.DomainNotificationKey == DomainNotificationKey.FLUENT_VALIDATION);
        }

        public bool HasNotificationsErrors()
        {
            return _notifications.Any(x => x.DomainNotificationKey == DomainNotificationKey.FLUENT_VALIDATION || x.DomainNotificationKey == DomainNotificationKey.ERROR || x.DomainNotificationKey == DomainNotificationKey.NOT_FOUND);
        }

        public List<DomainNotification> AddNotificationToReturn(DomainNotificationKey domainNotificationKey, string key, string error)
        {
            _notifications.Add(new DomainNotification(domainNotificationKey, key, error));
            return _notifications;
        }
    }
}
