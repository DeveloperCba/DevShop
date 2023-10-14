using System.Text.RegularExpressions;

namespace DevShop.Core.Validations;

public static class CheckCaracterValidation
{
    public static bool ContainsSpecialCharacters(this string text)
    {
        return Regex.IsMatch(text, @"[!@#$%^&*(),.?""':{}|<>]");
    }

    public static bool ContainsUppercaseLetters(this string text)
    {
        return Regex.IsMatch(text, @"[A-Z]");
    }

    public static bool ContainsUppercaseLettersAndContainsSpecialCharacters(this string text)
    {
        var hasUppercase = text.Any(char.IsUpper);
        var hasLowercase = text.Any(char.IsLower);
        var hasDigit = text.Any(char.IsDigit);
        var hasSpecialChar = text.Any(IsSpecialChar);
        var isValid = hasUppercase && hasLowercase && hasDigit && hasSpecialChar;

        return isValid;
    }

    public static bool IsSpecialChar(char c)
    {
        return !char.IsLetterOrDigit(c);
    }
}