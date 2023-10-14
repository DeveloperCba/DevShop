namespace DevShop.Core.DomainObjects.Exceptions;

public class CodeErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public CodeErrorResponse(
        int statusCode,
        string message)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultMessageStatusCode(statusCode);
    }

    private string GetDefaultMessageStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "Existe erro na requisão enviada.",
            401 => "Acesso não autorizado.",
            403 => "Não tem permissão para usar este recurso.",
            404 => "Não foi encontrado o recurso solicitado.",
            500 => "Existem erros internos no servidor.",
            _ => string.Empty
        };
    }
}