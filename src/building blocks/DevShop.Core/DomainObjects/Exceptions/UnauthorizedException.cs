namespace DevShop.Core.DomainObjects.Exceptions;

public class UnauthorizedException : ApplicationException
{
    public UnauthorizedException(string message) : base(message)
    {
    }
}