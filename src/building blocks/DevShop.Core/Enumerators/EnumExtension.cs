using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DevShop.Core.Enumerators;

public static class EnumExtension
{
    public static string ShortName(this Enum enumerador)
    {
        return enumerador.GetDisplayAttribute()?.GetShortName() ?? string.Empty;
    }

    public static string Name(this Enum enumerador)
    {
        return enumerador.GetDisplayAttribute()?.GetName() ?? string.Empty;
    }

    public static string Description(this Enum enumerador)
    {
        return enumerador.GetDisplayAttribute()?.GetDescription() ?? string.Empty;
    }

    public static string GroupName(this Enum enumerador)
    {
        return enumerador.GetDisplayAttribute()?.GetGroupName() ?? string.Empty;
    }

    public static int GetIndex(this Enum value)
    {
        var values = Enum.GetValues(value.GetType());
        return Array.IndexOf(values, value);
    }



    public static T GetEnumByShortName<T>(this string shortName)
    {
        var values = from opcao in typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public)
            let attribute = Attribute.GetCustomAttribute(opcao, typeof(DisplayAttribute)) as DisplayAttribute
            where attribute != null && attribute.ShortName == shortName
            select (T)opcao.GetValue(null);

        return (T)(object)values.FirstOrDefault();
    }

    private static DisplayAttribute GetDisplayAttribute(this Enum enumerador)
    {
        try
        {
            return enumerador.GetType()
                .GetMember(enumerador.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>();
        }
        catch
        {
            return null;
        }
    }
}