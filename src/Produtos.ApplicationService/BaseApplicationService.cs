using MediatR;
using Produtos.Domain.Core;
using Produtos.Domain.Model;
using Produtos.Domain.Model.Enumerators;

namespace Produtos.ApplicationService
{
    public abstract class BaseApplicationService
    {
        public readonly DomainNotificationHandler _notifications;

        public BaseApplicationService(INotificationHandler<DomainNotification> notifications)
        {
            _notifications = (DomainNotificationHandler)notifications;
        }

        public ServiceResult<T> ProcessServiceResult<T>(T response, string notFoundMessage)
        {
            if(response == null)
            {
                var notifications = _notifications.AddNotificationToReturn(DomainNotificationKey.NOT_FOUND, "NOT_FOUND", notFoundMessage);
                return ServiceResult<T>.NotFound(notifications);
            }

            return ServiceResult<T>.Ok(response, _notifications.GetNotifications());
        }
    }
}
