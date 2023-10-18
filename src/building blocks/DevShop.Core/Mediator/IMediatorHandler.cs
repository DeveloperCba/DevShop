using DevShop.Core.Messages;
using FluentValidation.Results;

namespace DevShop.Core.Mediator;
public interface IMediatorHandler
{
    Task Publish<T>(T @event) where T : Event;
    Task<ValidationResult> Send<T>(T command) where T : Command;
}
