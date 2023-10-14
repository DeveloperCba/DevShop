using DevShop.Core.DomainObjects.Exceptions;

namespace DevShop.Core.Validations;

public class CellPhone
{
    private const int JustNumberLenght = 9;
    private const int NumberWithDddLenght = 11;
    public const int MaxLength = 14;

    public string FullNumber { get; private set; }

    public CellPhone() { }

    public CellPhone(string number)
    {
        if (!Validate(number)) throw new DomainException("Número do celular inválido");
        FullNumber = number.Length == NumberWithDddLenght ? $"+55{number}" : number;
    }

    public string GetDdd() =>
        FullNumber.Length switch
        {
            NumberWithDddLenght => FullNumber[..2],
            MaxLength => FullNumber.Substring(3, 2),
            _ => string.Empty
        };

    public string GetNumber() =>
        FullNumber.Length switch
        {
            NumberWithDddLenght => FullNumber[2..],
            MaxLength => FullNumber[5..],
            _ => FullNumber
        };

    public static bool Validate(string number)
    {
        if (string.IsNullOrEmpty(number))
            return false;

        return number.Length switch
        {
            MaxLength or JustNumberLenght or NumberWithDddLenght => true,
            _ => false,
        };
    }
}