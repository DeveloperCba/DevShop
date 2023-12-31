using FluentValidation.Results;
using MediatR;

namespace DevShop.Core.Messages;

public abstract class Command : Message, IRequest<ValidationResult>
{
    public DateTime Timestamp { get; private set; }
    public ValidationResult ValidationResult { get; set; }

    protected Command()
    {
        Timestamp = DateTime.Now;
    }

    public virtual bool IsValid()
    {
        throw new NotImplementedException();
    }
}

public abstract class Command<T> : Message, IRequest<T>
{
    public DateTime Timestamp { get; private set; }
    public ValidationResult ValidationResult { get; set; }

    protected Command()
    {
        Timestamp = DateTime.Now;
    }

    public virtual bool IsValid()
    {
        throw new NotImplementedException();
    }
}