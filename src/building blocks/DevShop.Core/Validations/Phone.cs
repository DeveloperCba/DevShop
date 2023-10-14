using DevShop.Core.DomainObjects.Exceptions;
using DevShop.Core.Extensions;

namespace DevShop.Core.Validations;

public class Phone
{
    public const int MaxLength = 10;
    private const int MinLenght = 8;

    public string FullNumber { get; private set; }

    protected Phone() { }

    public Phone(string number)
    {
        if (!Validate(number)) throw new DomainException("Número do telefone inválido");
        FullNumber = number;
    }

    public string GetDdd() => FullNumber.Length == MaxLength ? FullNumber[..2] : string.Empty;

    public string GetNumber() => FullNumber.Length == MaxLength ? FullNumber[2..] : FullNumber;

    public static bool Validate(string number)
    {
        var onlyNumbers = number.OnlyNumber();

        return onlyNumbers.Length switch
        {
            MaxLength or MinLenght => true,
            _ => false,
        };
    }
}