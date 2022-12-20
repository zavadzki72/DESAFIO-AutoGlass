using Microsoft.AspNetCore.Mvc.ModelBinding;
using Produtos.Domain.Model.Enumerators;

namespace Produtos.Domain.Model
{
    public class ServiceResult
    {
        public string? RouteLocation { get; protected set; }
        public bool Success { get; protected set; }
        public ServiceResultStatus Status { get; protected set; }
        public List<DomainNotification> Notifications { get; protected set; } = new();

        public static ServiceResult OkEmpty()
        {
            return new ServiceResult
            {
                Status = ServiceResultStatus.OK,
                Success = true
            };
        }

        public static ServiceResult BadRequestByModelState(ModelStateDictionary modelState)
        {
            var result = new ServiceResult
            {
                Status = ServiceResultStatus.ERROR,
                Success = false
            };

            foreach(var e in modelState)
            {
                var notification = new DomainNotification(DomainNotificationKey.FLUENT_VALIDATION, "INVALID_VIEWMODEL", $"{e.Key} : {e.Value}");
                result.Notifications.Add(notification);
            }

            return result;
        }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Model { get; private set; }

        public static ServiceResult<T> Ok(T model)
        {
            return new ServiceResult<T>
            {
                Model = model,
                Status = ServiceResultStatus.OK,
                Success = true
            };
        }

        public static ServiceResult<T> Created(string routeLocation)
        {
            return new ServiceResult<T>
            {
                Model = default,
                RouteLocation = routeLocation,
                Status = ServiceResultStatus.OK,
                Success = true
            };
        }

        public static ServiceResult<T> Error(params DomainNotification[] notifications)
        {
            var result = new ServiceResult<T>
            {
                Model = default,
                Status = ServiceResultStatus.ERROR,
                Success = false
            };

            result.Notifications.AddRange(notifications);
            return result;
        }

        public static ServiceResult<T> NotFound(params DomainNotification[] notifications)
        {
            var result = new ServiceResult<T>
            {
                Model = default,
                Status = ServiceResultStatus.NOT_FOUND,
                Success = false
            };

            result.Notifications.AddRange(notifications);
            return result;
        }

        public static ServiceResult<T> Personalized(bool success, T model, ServiceResultStatus status, params DomainNotification[] notifications)
        {
            var result = new ServiceResult<T>
            {
                Model = model,
                Status = status,
                Success = success
            };

            result.Notifications.AddRange(notifications);

            return result;
        }
    }
}
