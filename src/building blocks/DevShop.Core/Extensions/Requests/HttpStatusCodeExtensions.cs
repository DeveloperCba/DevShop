using System.Net;

namespace DevShop.Core.Extensions.Requests;

public static class HttpStatusCodeExtensions
{
    public static bool IsSuccess(this HttpStatusCode statusCode)
    {
        return ((int)statusCode).ToString().StartsWith("2");
    }

    public static bool IsAuthenticationFailed(this HttpStatusCode statusCode)
    {
        return statusCode == HttpStatusCode.Unauthorized || statusCode == HttpStatusCode.Forbidden;
    }

    public static bool IsNotFound(this HttpStatusCode statusCode)
    {
        return statusCode == HttpStatusCode.NotFound || statusCode == HttpStatusCode.BadRequest;
    }

    public static bool IsInternalError(this HttpStatusCode statusCode)
    {
        return ((int)statusCode).ToString().StartsWith("5");
    }
}