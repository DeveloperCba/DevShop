using System.Net;
using DevShop.Core.DomainObjects.Exceptions;

namespace DevShop.WebAPI.Core.Extensions;

public class CustomHttpRequestException : Exception
{
    public HttpStatusCode StatusCode;

    public CustomHttpRequestException() { }

    public CustomHttpRequestException(string message, Exception innerException)
        : base(message, innerException) { }

    public CustomHttpRequestException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
        ThrowException(statusCode);
    }

    private void ThrowException(HttpStatusCode statusCode)
    {
        switch (statusCode)
        {
            case HttpStatusCode.BadRequest:
                break;
            case HttpStatusCode.Unauthorized:
                throw new UnauthorizedException("Acesso não autorizado.");
            case HttpStatusCode.Forbidden:
                throw new ForbiddenException("Não tem permissão para usar este recurso.");
            case HttpStatusCode.NotFound:
                break;
            case HttpStatusCode.InternalServerError:
                break;
            case HttpStatusCode.NotImplemented:
                break;
            case HttpStatusCode.BadGateway:
                break;

            case HttpStatusCode.GatewayTimeout:
                break;
            default:
                break;
        }
    }
}