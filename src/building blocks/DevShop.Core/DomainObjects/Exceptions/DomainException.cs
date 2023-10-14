namespace DevShop.Core.DomainObjects.Exceptions;

public class DomainException : ApplicationException
{
    public DomainException()
    { }

    public DomainException(string message) : base(message)
    { }

    public DomainException(string message, Exception innerException) : base(message, innerException)
    { }
}