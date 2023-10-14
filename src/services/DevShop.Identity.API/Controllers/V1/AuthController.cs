using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DevShop.Core.DomainObjects;
using DevShop.Identity.Application.Features.Auth.Commands;
using DevShop.Identity.Application.Features.User.Dtos;
using DevShop.Identity.Application.Models.Dtos;
using DevShop.Identity.Application.Models.Requesties;
using DevShop.WebAPI.Core.Controllers;
using DevShop.WebAPI.Core.Responses;


namespace DevShop.Identity.API.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : MainController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthController(
        INotify notification,
        IMediator mediator,
        IMapper mapper
    ) : base(notification)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtêm os dados de autorização.
    /// </summary>
    /// <param name="request">Informe os dados de autenticação.</param>
    /// <returns>Retornar os dados de autenticação.</returns>
    [AllowAnonymous]
    [HttpPost()]
    [ProducesResponseType(typeof(UserResponseLoginDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.UnprocessableEntity)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var loginResponse = await _mediator.Send(_mapper.Map<LoginCommand>(request));
        return CustomResponse(loginResponse);
    }

    /// <summary>
    /// Envia um email para resetar a senha.
    /// </summary>
    /// <param name="request">Informe o email para resetar a senha.</param>
    /// <returns>Envia um email para resetar a senha..</returns>
    [AllowAnonymous]
    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.UnprocessableEntity)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var resetPasswordResponse = await _mediator.Send(_mapper.Map<ResetPasswordCommand>(request));
        return CustomResponse(resetPasswordResponse);
    }

    /// <summary>
    ///  Confirma a nova senha.
    /// </summary>
    /// <param name="request">Informe o código do usuário, token e a nova senha.</param>
    /// <returns>Retorna uma mensagem com o resultado da opereração.</returns>
    [AllowAnonymous]
    [HttpPut("reset-password-confirmation")]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.UnprocessableEntity)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> ResetPassword(ResetPasswordConfirmationRequest request)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var resetPasswordResponse = await _mediator.Send(_mapper.Map<ResetPasswordConfirmationCommand>(request));
        return CustomResponse(resetPasswordResponse);
    }

    /// <summary>
    /// Confirma o email antes de fazer a autênticação.
    /// </summary>
    /// <param name="request">Informe o email para resetar a senha.</param>
    /// <returns>Envia um email para resetar a senha..</returns>
    [AllowAnonymous]
    [HttpPut("confirm-email")]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.UnprocessableEntity)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailRequest request)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var confirmEmailResponse = await _mediator.Send(_mapper.Map<ConfirmEmailCommand>(request));
        return CustomResponse(confirmEmailResponse);
    }
}