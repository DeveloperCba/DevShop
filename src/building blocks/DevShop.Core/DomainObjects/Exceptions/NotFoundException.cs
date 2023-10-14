namespace DevShop.Core.DomainObjects.Exceptions;

public class NotFoundException : ApplicationException
{
    public NotFoundException() : base($"não foi encontrado.")
    {
    }

    public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({key})  não foi encontrado.")
    {
    }
}