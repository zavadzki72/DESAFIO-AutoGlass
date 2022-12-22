using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Produtos.Domain.Model.Enumerators;

namespace Produtos.Domain.Model
{
    public class ServiceResult
    {
        public ServiceResult(List<DomainNotification> notifications)
        {
            Notifications = notifications;
        }

        public string? RouteLocation { get; protected set; }
        public bool Success { get; protected set; }
        public ServiceResultStatus Status { get; protected set; }
        public List<DomainNotification> Notifications { get; protected set; }

        public static ServiceResult OkEmpty(List<DomainNotification> notifications)
        {
            return new ServiceResult(notifications)
            {
                Status = ServiceResultStatus.OK,
                Success = true
            };
        }

        public static ServiceResult BadRequestByValidation(List<ValidationFailure> errors)
        {
            var result = new ServiceResult(new List<DomainNotification>())
            {
                Status = ServiceResultStatus.ERROR,
                Success = false
            };

            foreach(var e in errors)
            {
                var notification = new DomainNotification(DomainNotificationKey.FLUENT_VALIDATION, "INVALID_VIEWMODEL", $"{e.ErrorCode} : {e.ErrorMessage}");
                result.Notifications.Add(notification);
            }

            return result;
        }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public ServiceResult(List<DomainNotification> notifications) : base(notifications) { }

        public T? Model { get; private set; }

        public static ServiceResult<T> Ok(T model, List<DomainNotification> notifications)
        {
            return new ServiceResult<T>(notifications)
            {
                Model = model,
                Status = ServiceResultStatus.OK,
                Success = true
            };
        }

        public static ServiceResult<T> Created(string routeLocation, List<DomainNotification> notifications)
        {
            return new ServiceResult<T>(notifications)
            {
                Model = default,
                RouteLocation = routeLocation,
                Status = ServiceResultStatus.CREATED,
                Success = true
            };
        }

        public static ServiceResult<T> Error(List<DomainNotification> notifications)
        {
            var result = new ServiceResult<T>(notifications)
            {
                Model = default,
                Status = ServiceResultStatus.ERROR,
                Success = false
            };
            
            return result;
        }

        public static ServiceResult<T> NotFound(List<DomainNotification> notifications)
        {
            var result = new ServiceResult<T>(notifications)
            {
                Model = default,
                Status = ServiceResultStatus.NOT_FOUND,
                Success = false
            };
            
            return result;
        }

        public static ServiceResult<T> Personalized(bool success, T model, ServiceResultStatus status, List<DomainNotification> notifications)
        {
            var result = new ServiceResult<T>(notifications)
            {
                Model = model,
                Status = status,
                Success = success
            };

            return result;
        }
    }
}
