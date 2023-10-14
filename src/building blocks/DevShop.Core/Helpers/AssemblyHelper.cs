using System.Reflection;

namespace DevShop.Core.Helpers;

public class AssemblyHelper
{
    public static string GetAssemblyName()
    {
        return Assembly.GetEntryAssembly()?.GetName().Name;
    }
    public static string GetAssemblyVersion()
    {
        var version = Assembly.GetEntryAssembly()?.GetName().Version;
        return version?.ToString();
    }

    public static string GetAssemblyCompany()
    {
        var company = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyCompanyAttribute>();
        return company?.Company.ToString();
    }

    public static string GetAssemblyProduct()
    {
        var product = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyProductAttribute>();
        return product?.Product.ToString();
    }
}