using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DevShop.Core.DomainObjects;
using DevShop.Identity.Application.Features.User.Commands;
using DevShop.Identity.Application.Features.User.Dtos;
using DevShop.Identity.Application.Features.User.Queries;
using DevShop.Identity.Application.Models.Dtos;
using DevShop.Identity.Application.Models.Requesties;
using DevShop.WebAPI.Core.Controllers;
using DevShop.WebAPI.Core.Responses;


namespace DevShop.Identity.API.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/users")]
public class UserController : MainController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserController(
        INotify notification,
        IMediator mediator,
        IMapper mapper
    ) : base(notification)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtêm os dados do usuário.
    /// </summary>
    /// <param name="filter">Informe o parâmetro do filtro.</param>
    /// <returns>Retornar os dados do usuário.</returns>
    [HttpGet()]
    [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.UnprocessableEntity)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAllUser(string filter = null)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var userResponse = await _mediator.Send(new GetAllUserQuery { Filter = filter });
        return CustomResponse(userResponse);
    }

    /// <summary>
    /// Obtêm os dados do usuário paginado.
    /// </summary>
    /// <param name="filter">Informe o parâmetro do filtro.</param>
    /// <param name="pageNumber">Informe o número da página.</param>
    /// <param name="pageSize">informe o total da página</param>
    /// <returns>Retorna os dados do usuário paginado.</returns>
    [HttpGet("pagination")]
    [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.UnprocessableEntity)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAllUserPagination(string filter, int pageNumber = 0, int pageSize = 10)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var userResponse = await _mediator.Send(new GetAllUserPaginationQuery { Filter = filter, PageNumber = pageNumber, PageSize = pageSize });
        return CustomResponse(userResponse);
    }

    /// <summary>
    /// Obtêm o usuário através do código.
    /// </summary>
    /// <param name="userId">Informe o código do usuário.</param>
    /// <returns>Retornar os dados do usuário.</returns>
    [HttpGet("by-userid/{userid}")]
    [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.UnprocessableEntity)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UserById(string userId)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var userResponse = await _mediator.Send(new GetUserByIdQuery { UserId = userId });
        return CustomResponse(userResponse);
    }

    /// <summary>
    /// Obtêm o usuário através do email.
    /// </summary>
    /// <param name="email">Informe o email do usuário.</param>
    /// <returns>Retornar os dados do usuário.</returns>
    [HttpGet("by-email/{email}")]
    [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.UnprocessableEntity)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UserByEmail(string email)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var userResponse = await _mediator.Send(new GetUserByEmailQuery { Email = email });
        return CustomResponse(userResponse);
    }

    /// <summary>
    /// Obtêm o usuário através do login.
    /// </summary>
    /// <param name="userName">Informe o login do usuário.</param>
    /// <returns>Retornar os dados do usuário.</returns>
    [HttpGet("by-username/{userName}")]
    [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.UnprocessableEntity)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UserByUserName(string userName)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var userResponse = await _mediator.Send(new GetUserByUserNameQuery { UserName = userName });
        return CustomResponse(userResponse);
    }

    /// <summary>
    /// Cria um novo usuário.
    /// </summary>
    /// <param name="request">Informe os dados do usuário.</param>
    /// <returns>Retornar os dados do usuário.</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ConfirmEmailDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.UnprocessableEntity)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> CreateUser(UserRequest request)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var userResponse = await _mediator.Send(_mapper.Map<CreateUserCommand>(request));
        return CustomResponse(userResponse);
    }

    /// <summary>
    ///  Envia o token para confirmação do email.
    /// </summary>
    /// <param name="request">Informe os dados do usuário.</param>
    /// <returns>Retornar os dados do usuário.</returns>
    [HttpPost("send-token-confirm-email")]
    [ProducesResponseType(typeof(ConfirmEmailDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.UnprocessableEntity)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> SendTokenConfirmEmail(SendTokenConfirmEmailRequest request)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var userSendTokenConfirmEmail = await _mediator.Send(_mapper.Map<SendTokenConfirmEmailCommand>(request));
        return CustomResponse(userSendTokenConfirmEmail);
    }

    /// <summary>
    /// Atualiza os dados do usuario.
    /// </summary>
    /// <param name="request">Informe os dados do usuário.</param>
    /// <returns>Retorna a informação da alteração.</returns>
    [HttpPut("update")]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.UnprocessableEntity)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UpdateUser(UpdateRequest request)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var userResponse = await _mediator.Send(_mapper.Map<UpdateUserCommand>(request));
        return CustomResponse(userResponse);
    }

    /// <summary>
    /// Bloqueia o acesso do usuário.
    /// </summary>
    /// <param name="request">Informe código do usuário.</param>
    /// <returns>Retorna a informação do bloqueio.</returns>
    [HttpPut("lock")]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.UnprocessableEntity)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> LockUser(LockUserRequest request)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var userResponse = await _mediator.Send(_mapper.Map<LockUserCommand>(request));
        return CustomResponse(userResponse);
    }

    /// <summary>
    /// Desbloqueia o acesso do usuário.
    /// </summary>
    /// <param name="request">Informe o código do usuário.</param>
    /// <returns>Retorna a informação do bloqueio.</returns>
    [HttpPut("unlock")]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.UnprocessableEntity)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UnlockUser(UnlockUserRequest request)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var userResponse = await _mediator.Send(_mapper.Map<UnlockUserCommand>(request));
        return CustomResponse(userResponse);
    }

    /// <summary>
    /// Exclui um determinado usuário e suas configurações de acesso.
    /// </summary>
    /// <param name="request">Informe os dados do usuário.</param>
    /// <returns>Retorna a informação da exclusão.</returns>
    [HttpDelete("delete")]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.UnprocessableEntity)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResultResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> DeleteUser(UserRemoveRequest request)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var userResponse = await _mediator.Send(_mapper.Map<RemoveUserCommand>(request));
        return CustomResponse(userResponse);
    }
}