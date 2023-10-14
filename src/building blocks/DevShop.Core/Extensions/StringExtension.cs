using System.Text.RegularExpressions;

namespace DevShop.Core.Extensions;

public static class StringExtension
{

    public static string GetWithinCharacterEspecial(this string texto)
    {
        return texto
            .RemoveAcento()
            .RemoveCharacterEspecial();
    }

    public static string RemoveCharacterEspecial(this string texto)
    {
        var letra = string.Empty;
        var resultado = string.Empty;

        if (!string.IsNullOrEmpty(texto))
        {
            for (int i = 0; i < texto.ToString().Length; i++)
            {
                letra = texto[i].ToString();
                switch (letra)
                {
                    case "&": letra = string.Empty; break;
                    case "º": letra = string.Empty; break;
                    case ":": letra = string.Empty; break;
                    case "@": letra = string.Empty; break;
                    case "#": letra = string.Empty; break;
                    case "$": letra = string.Empty; break;
                    case "%": letra = string.Empty; break;
                    case "¨": letra = string.Empty; break;
                    case "*": letra = string.Empty; break;
                    case "(": letra = string.Empty; break;
                    case ")": letra = string.Empty; break;
                    case "ª": letra = string.Empty; break;
                    case "°": letra = string.Empty; break;
                    case ";": letra = string.Empty; break;
                    case "/": letra = string.Empty; break;
                    case "´": letra = string.Empty; break;
                    case "`": letra = string.Empty; break;
                    case "'": letra = string.Empty; break;
                    case "-": letra = string.Empty; break;
                    case " ": letra = string.Empty; break;
                }
                resultado += letra;
            }
        }

        return resultado;
    }

    public static string RemoveAcento(this string texto)
    {
        var letra = string.Empty;
        var resultado = string.Empty;

        for (int i = 0; i < texto.ToString().Length; i++)
        {
            letra = texto[i].ToString();
            switch (letra)
            {
                case "á": letra = "a"; break;
                case "é": letra = "e"; break;
                case "í": letra = "i"; break;
                case "ó": letra = "o"; break;
                case "ú": letra = "u"; break;
                case "à": letra = "a"; break;
                case "è": letra = "e"; break;
                case "ì": letra = "i"; break;
                case "ò": letra = "o"; break;
                case "ù": letra = "u"; break;
                case "â": letra = "a"; break;
                case "ê": letra = "e"; break;
                case "î": letra = "i"; break;
                case "ô": letra = "o"; break;
                case "û": letra = "u"; break;
                case "ä": letra = "a"; break;
                case "ë": letra = "e"; break;
                case "ï": letra = "i"; break;
                case "ö": letra = "o"; break;
                case "ü": letra = "u"; break;
                case "ã": letra = "a"; break;
                case "õ": letra = "o"; break;
                case "ñ": letra = "n"; break;
                case "ç": letra = "c"; break;
                case "Á": letra = "A"; break;
                case "É": letra = "E"; break;
                case "Í": letra = "I"; break;
                case "Ó": letra = "O"; break;
                case "Ú": letra = "U"; break;
                case "À": letra = "A"; break;
                case "È": letra = "E"; break;
                case "Ì": letra = "I"; break;
                case "Ò": letra = "O"; break;
                case "Ù": letra = "U"; break;
                case "Â": letra = "A"; break;
                case "Ê": letra = "E"; break;
                case "Î": letra = "I"; break;
                case "Ô": letra = "O"; break;
                case "Û": letra = "U"; break;
                case "Ä": letra = "A"; break;
                case "Ë": letra = "E"; break;
                case "Ï": letra = "I"; break;
                case "Ö": letra = "O"; break;
                case "Ü": letra = "U"; break;
                case "Ã": letra = "A"; break;
                case "Õ": letra = "O"; break;
                case "Ñ": letra = "N"; break;
                case "Ç": letra = "C"; break;
                case "£": letra = ""; break;
                case "©": letra = ""; break;
                case "¢": letra = ""; break;
            }
            resultado += letra;
        }
        return resultado;
    }

    public static string ReplaceCharacterSequencial(this string valor)
    {
        string resultado = null;
        try
        {
            var anterior = '0';
            var novo = '0';
            var caracteres = new char[valor.Length];
            caracteres = valor.ToCharArray();

            foreach (char caracter in caracteres)
            {
                if (caracter == anterior)
                    novo = Convert.ToChar(Convert.ToString(Convert.ToInt16(caracter) + 2 > 9 ? 1 : Convert.ToInt16(caracter) + 2));
                else
                    novo = caracter;

                anterior = novo;
                resultado += novo;
            }
        }
        catch { resultado = null; }

        return resultado;
    }

    public static string RemoveCharacters(this string input)
    {
        string resultString = null;
        try
        {
            Regex regexObj = new Regex(@"[^\d]");
            resultString = regexObj.Replace(input, "");
        }
        catch { resultString = null; }

        return resultString;
    }

    public static long RemoveCharactersToInt(this string input)
    {
        string resultString = null;
        try
        {
            Regex regexObj = new Regex(@"[^\d]");
            resultString = regexObj.Replace(input, "");
        }
        catch { resultString = null; }

        return string.IsNullOrEmpty(resultString) ? 0 : Convert.ToInt64(resultString);
    }

    public static string FormatString(this string mascara, string valor)
    {
        var novoValor = string.Empty;
        var posicao = 0;
        for (int i = 0; mascara.Length > i; i++)
        {
            if (mascara[i] == '#')
            {
                if (valor.Length > posicao)
                {
                    novoValor = novoValor + valor[posicao];
                    posicao++;
                }
                else
                    break;
            }
            else
            {
                if (valor.Length > posicao)
                    novoValor = novoValor + mascara[i];
                else
                    break;
            }
        }
        return novoValor;
    }

    public static string ToReplaceWebProtocol(this string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var webProtocol = new List<string>() { "https://", "http://" };

        webProtocol.ForEach(protocol =>
        {
            if (input.Contains(protocol))
            {
                input = input.Replace(protocol, "");
            }
        });

        return input;
    }

    public static bool ToRemoveLogRequest(this string input)
    {
        var notRemove = true;

        if (string.IsNullOrEmpty(input)) return notRemove;

        var extenssions = new List<string>() { ".json", ".css", ".js", ".gif", ".woff", ".ico", ".svg", ".png", ".map", "hangfire" };

        foreach (var extenssion in extenssions)
        {
            if (input.ToLower().Contains(extenssion))
            {
                notRemove = false;
                break;
            }
        }

        return notRemove;
    }  

    public static string GetOnlyNumber(this string texto)
    {
        return Regex.Replace(texto, "[^0-9,]", "");
    }


    public static string OnlyNumber(this string valor)
    {
        var onlyNumber = "";
        foreach (var s in valor)
        {
            if (char.IsDigit(s))
            {
                onlyNumber += s;
            }
        }
        return onlyNumber.Trim();
    }
    public static string ToSnakeCase(this string name)
        => Regex.Replace(
            name,
            @"([a-z0-9])([A-Z])",
            "$1_$2").ToLower();

}