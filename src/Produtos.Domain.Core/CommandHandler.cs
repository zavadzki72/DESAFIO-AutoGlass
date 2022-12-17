using Microsoft.Extensions.Logging;
using Produtos.Domain.Model;
using Produtos.Domain.Model.Enumerators;
using Produtos.Domain.Model.Interfaces;

namespace Produtos.Domain.Core
{
    public abstract class CommandHandler
    {
        private readonly IMediatorHandler _bus;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        protected CommandHandler(IMediatorHandler bus, ILogger<CommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        protected async Task NotifyValidationErrors(Command message)
        {
            foreach (var error in message.ValidationResult.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(DomainNotificationKey.FLUENT_VALIDATION, "INVALID_COMMAND", $"{message.MessageType} : {error.ErrorMessage}"));
                _logger.LogError($"NotifyValidationErrors - FluentError - {error.ErrorMessage}");
            }
        }

        protected async Task NotifyValidationErrors<TResponse>(Command<TResponse> message)
        {
            foreach (var error in message.ValidationResult.Errors)
            {
                await _bus.RaiseEvent(new DomainNotification(DomainNotificationKey.FLUENT_VALIDATION, "INVALID_COMMAND", $"{message.MessageType} : {error.ErrorMessage}"));
                _logger.LogError($"NotifyValidationErrors - FluentError - {error.ErrorMessage}");
            }
        }

        protected async Task NotifyError(string key, string value)
        {
            await _bus.RaiseEvent(new DomainNotification(DomainNotificationKey.ERROR, key, value));
            _logger.LogError($"NotifyError - {key} - {value}");
        }

        protected async Task Commit()
        {
            await _unitOfWork.CompleteAsync();
        }
    }
}
