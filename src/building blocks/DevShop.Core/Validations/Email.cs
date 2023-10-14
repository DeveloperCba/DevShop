using System.Text.RegularExpressions;
using DevShop.Core.DomainObjects.Exceptions;

namespace DevShop.Core.Validations;

public class Email
{
    public const int MaxLenght = 254;
    public const int MinLenght = 5;

    public string Address { get; }

    protected Email() { }

    public Email(string address)
    {
        if (!Validate(address)) throw new DomainException("Email inválido");
        Address = address;
    }

    public static bool Validate(string email)
    {
        var regex = new Regex("[a-z0-9!#$%&’*+/=?^_`{|}~-]+(?:.[a-z0-9!#$%&’*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
        return regex.IsMatch(email);
    }
}