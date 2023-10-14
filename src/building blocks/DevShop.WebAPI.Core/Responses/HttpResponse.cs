using System.Net;

namespace DevShop.WebAPI.Core.Responses;

public class HttpResponse
{
    public HttpStatusCode StatusCode { get; set; }

    public string Content { get; set; }

    public byte[] ContentBytes { get; set; }
}