namespace DevShop.Core.Validations;

public class VerifyingDigit
{
    private string _number;
    private const int _module = 11;
    private readonly List<int> _multipliers = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9 };
    private readonly IDictionary<int, string> _substitutions = new Dictionary<int, string>();
    private bool _complementOfModule = true;

    public VerifyingDigit(string number)
    {
        _number = number;
    }

    public VerifyingDigit WithAteMultipliers(int firstMultiplier, int lastMultiplier)
    {
        _multipliers.Clear();
        for (var i = firstMultiplier; i <= lastMultiplier; i++)
            _multipliers.Add(i);

        return this;
    }

    public VerifyingDigit Replacing(string substitute, params int[] digits)
    {
        foreach (var i in digits)
        {
            _substitutions[i] = substitute;
        }
        return this;
    }

    public void AddDigit(string digit)
    {
        _number = string.Concat(_number, digit);
    }

    public string CalculateDigit()
    {
        return !(_number.Length > 0) ? string.Empty : GetDigitSum();
    }

    private string GetDigitSum()
    {
        var soma = 0;
        for (int i = _number.Length - 1, m = 0; i >= 0; i--)
        {
            var produto = (int)char.GetNumericValue(_number[i]) * _multipliers[m];
            soma += produto;

            if (++m >= _multipliers.Count) m = 0;
        }

        var mod = soma % _module;
        var resultado = _complementOfModule ? _module - mod : mod;

        return _substitutions.ContainsKey(resultado) ? _substitutions[resultado] : resultado.ToString();
    }
}