using DevShop.Core.DomainObjects;
using DevShop.WebAPI.Core.Responses;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace DevShop.WebAPI.Core.Controllers;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Authorize]
public abstract class MainController : ControllerBase
{
    protected ICollection<string> Errors = new List<string>();
    private readonly INotify _notification;
    public MainController(INotify notification)
    {
        _notification = notification;
    }

    protected bool ValidOperation() => !_notification.HasNotification();


    protected ActionResult CustomResponse(object result = null)
    {
        if (ValidOperation())
        {
            if (string.IsNullOrEmpty(result.ToString()))
                return new CustomResponseObjectResult(result, (int)HttpStatusCode.NoContent);

            return new CustomResponseObjectResult(result, (int)HttpStatusCode.OK);
        }

        Errors = _notification.GetNotifications().Select(n => n.Message).ToArray();
        var resultErrors = new ResultResponse
        {
            Title =  "Ocorreu um erro.",
            Status = result == null ? (int)HttpStatusCode.UnprocessableEntity : (int)HttpStatusCode.BadRequest,
            Errors = new ErrorMessageResponse
            {
                Messages = Errors.ToList(),
            }
        };
        if(resultErrors.Status == (int)HttpStatusCode.UnprocessableEntity )
            return new CustomResponseObjectResult(resultErrors, (int)HttpStatusCode.UnprocessableEntity);

        return new CustomResponseObjectResult(resultErrors, (int)HttpStatusCode.BadRequest);

    }

    protected ValidationProblemDetails ConfigureMessageError(string title, string detail, int? status, string errorTitle, string[] errors)
    {
        var validationProblemDetails = new ValidationProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = status,
        };
        validationProblemDetails.Errors.Add(errorTitle, errors);

        return validationProblemDetails ?? null;
    }

    protected ActionResult CustomResponse(ValidationResult validationResult)
    {
        foreach (var erro in validationResult.Errors)
        {
            AddProcessingError(erro.ErrorMessage);
        }

        return CustomResponse();
    }
    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        var erros = modelState.Values.SelectMany(e => e.Errors);
        foreach (var erro in erros)
        {
            var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
            AddProcessingError(errorMsg);
        }
        return CustomResponse();
    }


    protected ActionResult CustomResponse(ResultResponse response)
    {
        ResponseHasErrors(response);

        return CustomResponse();
    }

    protected bool ResponseHasErrors(ResultResponse response)
    {
        if (response == null || !response.Errors.Messages.Any()) return false;

        foreach (var message in response.Errors.Messages)
        {
            AddProcessingError(message);
        }

        return true;
    }

    protected void AddProcessingError(string message) => _notification.Handler(new NotificationMessage(message));
}