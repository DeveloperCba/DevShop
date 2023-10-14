namespace DevShop.Core.Extensions;

public static class DateTimeExtensions
{
    public static string ToPatternFormat(this DateTime date)
    {
        return date.ToString("yyyy-MM-dd'T'HH:mm:ss.fffZ");
    }

    public static bool IsBetween(this IComparable a, IComparable b, IComparable c)
    {
        return a.CompareTo(b) >= 0 && a.CompareTo(c) <= 0;
    }
}