namespace DevShop.Core.DomainObjects.Exceptions;

public class UnprocessableException : ApplicationException
{
    public UnprocessableException(string message) : base(message)
    {
    }
}