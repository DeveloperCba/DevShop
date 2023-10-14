using DevShop.Core.Extensions;

namespace DevShop.Core.Validations;

public class CpfValidation
{
    public const int SizeCPF = 11;

    public static bool Validate(string cpf)
    {
        var cpfNumeros = cpf.OnlyNumber();

        if (!SizeValid(cpfNumeros)) return false;
        return !HasRepeatedDigits(cpfNumeros) && HaveValidDigits(cpfNumeros);
    }

    private static bool SizeValid(string valor) => valor.Length == SizeCPF;

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
        var number = value.Substring(0, SizeCPF - 2);
        var verifyingDigit = new VerifyingDigit(number)
            .WithAteMultipliers(2, 11)
            .Replacing("0", 10, 11);

        var firstDigit = verifyingDigit.CalculateDigit();
        verifyingDigit.AddDigit(firstDigit);
        var secondDigit = verifyingDigit.CalculateDigit();

        return string.Concat(firstDigit, secondDigit) == value.Substring(SizeCPF - 2, 2);
    }
}