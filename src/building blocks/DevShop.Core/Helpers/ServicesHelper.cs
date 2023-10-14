namespace DevShop.Core.Helpers;

public static class ServicesHelper
{
    public static string GetServiceName()
    {
        var assemblyName = AssemblyHelper.GetAssemblyName();
        var serviceName = assemblyName.Split(".").Last();
        return serviceName;
    }
}