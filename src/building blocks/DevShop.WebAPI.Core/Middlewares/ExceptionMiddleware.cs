using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DevShop.Core.Datas.Interfaces;
using DevShop.Core.DomainObjects;
using DevShop.Core.DomainObjects.Exceptions;
using DevShop.WebAPI.Core.Responses;

namespace DevShop.WebAPI.Core.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _hostEnvironment;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment hostEnvironment
    )
    {
        _next = next;
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ILogErrorRepository logErroRepository
    )
    {
        try
        {
            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                throw new UnauthorizedException(string.Empty);

            if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                throw new ForbiddenException(string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            var statusCode = (int)HttpStatusCode.InternalServerError;

            var result = string.Empty;
            var errors = new List<string>();
            switch (ex)
            {
                case DomainException:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                case NotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    errors.Add(ex.Message);
                    break;
                case FluentValidation.ValidationException validationException:
                    statusCode = (int)HttpStatusCode.UnprocessableEntity;
                    errors.AddRange(validationException.Errors.Select(x => x.ErrorMessage));
                    break;
                case BadRequestException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    errors.Add("Não encontrado");
                    break;
                case UnprocessableException:
                    statusCode = (int)HttpStatusCode.UnprocessableEntity;
                    errors.Add(ex.Message);
                    break;
                case ForbiddenException:
                    statusCode = (int)HttpStatusCode.Forbidden;
                    errors.Add("Sem Permissão para acessar o sistema, entre em contato com o suporte!");
                    break;
                case UnauthorizedException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    errors.Add("Não Autorizado, entre em contato com o suporte!");
                    break;

                case InternalServerErrorException:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    errors.Add("Ocorreu um erro interno.");
                    break;
                default:
                    errors.Add(ex.Message);
                    break;
            }

            context.Response.StatusCode = statusCode;

            await logErroRepository.AddAsync(new LogError
            {
                ErroCompleto = ex.StackTrace,
                Erro = ex.Message,
                Method = context.Request?.Method.ToString().Trim(),
                Path = context.Request?.Path.ToString().Trim(),
                Query = WebUtility.UrlDecode(context.Request?.QueryString.ToString().Trim()),
            });

            var resultErrors = new ResultResponse
            {
                Status = statusCode,
                Title = "Ocorreu um erro.",
                Errors = new ErrorMessageResponse
                {
                    Messages = errors.ToList(),
                }
            };
            var json = JsonConvert.SerializeObject(resultErrors);
            await context.Response.WriteAsync(json);
        }
    }
}