using System.Globalization;

namespace DevShop.Core.Extensions;

public static class DecimalExtensions
{
    private static readonly NumberFormatInfo Nfi = new NumberFormatInfo
    {
        NumberDecimalSeparator = ",",
        NumberGroupSeparator = ".",
        NumberDecimalDigits = 2
    };

    public static string ToCoin(this decimal value)
    {
        return string.Format(Nfi, "{0:N}", value);
    }

    public static bool IsBetween(this decimal value, decimal min, decimal max)
    {
        return value >= min && value <= max;
    }

    public static int ToCent(this decimal value)
    {
        var decimalParsed = value.ToString();
        decimalParsed = decimalParsed.Contains('.') ? decimalParsed : decimalParsed + ".00";

        decimalParsed = decimalParsed.Replace(".", "");

        return int.Parse(decimalParsed);
    }

    public static decimal ToDecimal(this int value)
    {
        var intParsed = value.ToString();
        intParsed = intParsed.Insert(intParsed.Length - 2, ".");
        return decimal.Parse(intParsed);
    }

    public static decimal ToString(this string text)
    {
        decimal.TryParse(text, out decimal result);
        return decimal.Round(result, 2);
    }
}