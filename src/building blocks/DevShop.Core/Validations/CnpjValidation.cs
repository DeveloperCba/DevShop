using DevShop.Core.Extensions;

namespace DevShop.Core.Validations;

public class CnpjValidation
{
    public const int SizeCNPJ = 14;

    public static bool Validate(string cpnj)
    {
        var cpnjNumeros = cpnj.OnlyNumber();

        if (!SizeValid(cpnjNumeros)) return false;
        return !HasRepeatedDigits(cpnjNumeros) && HaveValidDigits(cpnjNumeros);
    }

    private static bool SizeValid(string valor) => valor.Length == SizeCNPJ;

    private static bool HasRepeatedDigits(string valor)
    {
        string[] invalidNumbers =
        {
            "00000000000",
            "11111111111",
            "22222222222",
            "33333333333",
            "44444444444",
            "55555555555",
            "66666666666",
            "77777777777",
            "88888888888",
            "99999999999"
        };
        return invalidNumbers.Contains(valor);
    }

    private static bool HaveValidDigits(string value)
    {
        var number = value.Substring(0, SizeCNPJ - 2);
        var verifyingDigit = new VerifyingDigit(number)
            .WithAteMultipliers(2, 9)
            .Replacing("0", 10, 11);

        var firstDigit = verifyingDigit.CalculateDigit();
        verifyingDigit.AddDigit(firstDigit);
        var secondDigit = verifyingDigit.CalculateDigit();

        return string.Concat(firstDigit, secondDigit) == value.Substring(SizeCNPJ - 2, 2);
    }
}