
using DevShop.Core.Datas.Interfaces;
using DevShop.Core.DomainObjects;
using DevShop.Core.Extensions;
using DevShop.WebAPI.Core.Responses;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Net;

namespace DevShop.WebAPI.Core.Middlewares;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private const string StopwatchKey = "StopwatchFilter.Value";

    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IHttpContextAccessor httpContext, ILogRequestRepository _logRequestRepository)
    {
        var logRequestObject = new LogRequest();
        var logRequest = await FormatRequest(httpContext, logRequestObject);

        try
        {
            if (logRequest.Path.ToRemoveLogRequest())
            {
                await _logRequestRepository.AddAsync(logRequest);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Erro em gravar a requisição", ex);
        }

        var originalBodyStream = httpContext.HttpContext.Response.Body;

        using var responseBody = new MemoryStream();
        httpContext.HttpContext.Response.Body = responseBody;

        await _next(context);

        //code dealing with response
        var logResponse = await FormatResponse(httpContext.HttpContext.Response, logRequestObject);
        await responseBody.CopyToAsync(originalBodyStream);

        try
        {
            if (logRequest.Path.ToRemoveLogRequest())
            {
                await _logRequestRepository.UpdateAsync(logRequest);
            }

            await _logRequestRepository.UnitOfWork.Commit();
        }
        catch (Exception ex)
        {
            throw new Exception("Erro em atualizar registro de requisição", ex);
        }
    }

    private static async Task<LogRequest> FormatResponse(Microsoft.AspNetCore.Http.HttpResponse response, LogRequest logRequest)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        var stopwatch = (Stopwatch)response.HttpContext.Items[StopwatchKey];

        var statusCode = response.StatusCode;
        var resultResponse = text.ToString().Trim();

        logRequest.ExecutionTime = stopwatch.Elapsed;
        logRequest.StatusCode = statusCode;
        logRequest.Response = resultResponse;

        return logRequest;
    }

    private static async Task<LogRequest> FormatRequest(IHttpContextAccessor httpContext, LogRequest logRequest)
    {
        httpContext.HttpContext.Items[StopwatchKey] = Stopwatch.StartNew();

        httpContext.HttpContext.Request.EnableBuffering();
        var bodyAsText = await new StreamReader(httpContext.HttpContext.Request.Body).ReadToEndAsync();
        httpContext.HttpContext.Request.Body.Position = 0;

        var device = httpContext.HttpContext.Request.Headers["User-Agent"].ToString().Trim();
        var host = httpContext.HttpContext.Request.Host.ToString().Trim();
        var method = httpContext.HttpContext.Request?.Method.ToString().Trim();
        var path = httpContext.HttpContext.Request?.Path.ToString().Trim();
        var query = WebUtility.UrlDecode(httpContext.HttpContext.Request?.QueryString.ToString().Trim());
        var header = Newtonsoft.Json.JsonConvert.SerializeObject(httpContext.HttpContext.Request.Headers).ToString().Trim();
        var body = bodyAsText.ToString().Trim();
        var ip = httpContext.HttpContext?.Connection?.RemoteIpAddress?.ToString().Trim();
        var url = $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host}{httpContext.HttpContext.Request.Path}{httpContext.HttpContext.Request.QueryString}";

        logRequest.Device = device;
        logRequest.Host = host;
        logRequest.Method = method;
        logRequest.Path = path;
        logRequest.Url = url;
        logRequest.Header = header;
        logRequest.Body = body;
        logRequest.Query = query;
        logRequest.Ip = ip;
        return logRequest;
    }
}