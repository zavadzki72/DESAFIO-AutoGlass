using MediatR;
using Produtos.Domain.Core;
using Produtos.Domain.Model;
using Produtos.Domain.Model.Enumerators;

namespace Produtos.ApplicationService
{
    public abstract class BaseApplicationService
    {
        private readonly DomainNotificationHandler _notifications;

        public BaseApplicationService(INotificationHandler<DomainNotification> notifications)
        {
            _notifications = (DomainNotificationHandler)notifications;
        }

        public ServiceResult<T> ProcessServiceResult<T>(T response, string notFoundMessage)
        {
            if(response == null)
            {
                var notification = _notifications.AddNotificationToReturn(DomainNotificationKey.NOT_FOUND, "NOT_FOUND", notFoundMessage);
                return ServiceResult<T>.NotFound(notification);
            }

            return ServiceResult<T>.Ok(response);
        }
    }
}
