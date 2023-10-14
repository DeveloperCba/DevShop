using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevShop.WebAPI.Core.Middlewares;

public class UnhandledExceptionMiddleware<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionMiddleware(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(ex, "Solicitação de aplicativo: ocorreu uma exceção para a solicitação {Name} {@Request}", requestName, request);
            throw;
        }
    }
}