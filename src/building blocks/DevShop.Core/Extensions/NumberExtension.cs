namespace DevShop.Core.Extensions;

public static class NumberExtension
{
    public static bool IsInteger(this object value)
    {
        if (value == null)
            return false;

        int intValue;
        return int.TryParse(value.ToString(), out intValue);
    }

    public static bool IsShort(this object value)
    {
        if (value == null)
            return false;

        short intValue;
        return short.TryParse(value.ToString(), out intValue);
    }

    public static bool IsLong(this object value)
    {
        if (value == null)
            return false;

        long intValue;
        return long.TryParse(value.ToString(), out intValue);
    }

    public static bool IsDouble(this object value)
    {
        if (value == null)
            return false;

        double doubleValue;
        return double.TryParse(value.ToString(), out doubleValue);
    }

    public static bool IsDecimal(this object value)
    {
        if (value == null)
            return false;

        decimal decimalValue;
        return decimal.TryParse(value.ToString(), out decimalValue);
    }
}