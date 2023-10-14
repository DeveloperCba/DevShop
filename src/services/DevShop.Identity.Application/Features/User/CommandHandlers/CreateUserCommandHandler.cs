using System.Text;
using System.Text.Encodings.Web;
using DevShop.Core.DomainObjects;
using DevShop.Core.Validations.Services.Email;
using DevShop.Identity.Application.Features.User.Commands;
using DevShop.Identity.Application.Features.User.Dtos;
using DevShop.Identity.Application.Models;
using DevShop.Identity.Application.Services;
using DevShop.Identity.Domain.Interfaces;
using DevShop.Identity.Domain.Models;
using DevShop.WebAPI.Core.Services;
using MediatR;
using Microsoft.Extensions.Options;

namespace DevShop.Identity.Application.Features.User.CommandHandlers;

public class CreateUserCommandHandler : BaseService,
    IRequestHandler<CreateUserCommand, ConfirmEmailDto>,
    IRequestHandler<SendTokenConfirmEmailCommand, ConfirmEmailDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IAutenticationService _autenticationService;
    private readonly IEmailService _emailService;
    private readonly ExternalEmailSettings _externalEmailSettings;
    public CreateUserCommandHandler(
        IUserRepository userRepository,
        INotify notification,
        IAutenticationService autenticationService,
        IEmailService emailService,
        IOptions<ExternalEmailSettings> externalEmailSettings) : base(notification)
    {
        _userRepository = userRepository;
        _autenticationService = autenticationService;
        _emailService = emailService;
        _externalEmailSettings = externalEmailSettings.Value;
    }

    public async Task<ConfirmEmailDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByFilterAsync(x => x.Document == request.Document || x.Email == request.Email);
        if (user is not null)
        {
            NotificationEvent("Já existe um login com o CodigoSgv ou CpfCnpj ou Email.");
            return default!;
        }

        user = GetUserMap(request);
        var result = await _autenticationService.UserManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            var token = await _autenticationService.UserManager.GenerateEmailConfirmationTokenAsync(user);
            var callBackUrl = $@"{_externalEmailSettings.Url}/UserIdentity/ConfirmacaoEmail?userId={user.Id}&token={UrlEncoder.Default.Encode(token)}";

            await SendConfirmEmail(user.Name, user.Email, callBackUrl);

            await _autenticationService.SignInManager.SignInAsync(user, isPersistent: false);

            return new ConfirmEmailDto
            {
                CallBack = callBackUrl,
                Token = token,
                UserId = user.Id
            };

            // return await _autenticationService.GenereateJwt(user.Email);
        }

        foreach (var error in result.Errors)
            NotificationEvent(error.Description);

        return default!;
    }

    public async Task<ConfirmEmailDto> Handle(SendTokenConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _autenticationService.UserManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            NotificationEvent("Usuário não encontrado.");
            return default!;
        }

        var token = await _autenticationService.UserManager.GenerateEmailConfirmationTokenAsync(user);
        var callBackUrl = $@"{_externalEmailSettings.Url}/UserIdentity/ConfirmacaoEmail?userId={user.Id}&token={UrlEncoder.Default.Encode(token)}";

        //TODO: REMOVER O EMAIL CHUMBADO - SOMENTE PARA TESTE
        await SendConfirmEmail(user.Name, "teste@teste.com.br", callBackUrl);
        //await SendConfirmEmail(user.Nome, user.Email, callBackUrl);

        await _autenticationService.SignInManager.SignInAsync(user, isPersistent: false);

        return new ConfirmEmailDto
        {
            CallBack = callBackUrl,
            Token = token,
            UserId = user.Id
        };
    }

    private async Task SendConfirmEmail(string name, string email, string callBack)
    {
        var title = "Confirmação de Email.";
        var message = "Clique no link para confirmar seu cadastro.";
        var empresa = "Teste";
        var htmlEmail = new StringBuilder();
        htmlEmail.AppendLine("<div style=\"font-family:calibri; width:60%; border:1px solid #ccc; margin:0 auto; height:400px; padding:10px\">");
        htmlEmail.AppendLine($"<h1> <div style=\"float:right; width:160px !important; \"><img src=\"cid:Image\" width=\"160px !important\" /></div> {title} </h1>");
        htmlEmail.AppendLine(string.Format("            Olá <strong>{0}</strong>, <br>", name));
        htmlEmail.AppendLine(string.Format("            Por favor, clique no link abaixo para confirmar seu cadastro.<br>"));
        htmlEmail.AppendLine("<br>");
        htmlEmail.AppendLine(string.Format("<div style=\"text-align:center\" ><a style=\"vertical-align:middle\" href=\"{0}\"> Clique Aqui</a> <strong>para confirmar seu cadastro</strong></div>", callBack));
        htmlEmail.AppendLine("<br><br><br><br><br><br>");
        htmlEmail.AppendLine("<div style=\"bottom:0px; text-align:center\" ><p>© " + DateTime.Now.Year.ToString() + $" - {empresa}</p></div>");
        htmlEmail.AppendLine("</div>");

        var emailRequest = new EmailRequest
        {
            Title = title,
            Subject = message,
            Message = htmlEmail.ToString(),
            SendImage = true
        };
        emailRequest.Recipients.Add(email);

        await _emailService.Send(emailRequest);
    }

    private static ApplicationUser GetUserMap(CreateUserCommand request)
    {
        return new ApplicationUser
        {
            Neighborhood = request.Neighborhood,
            ZipCode = request.ZipCode,
            City = request.City,
            Complement = request.Complement,
            Document = request.Document,
            Email = request.Email,
            UserName = request.Email,
            Number = request.Number,
            State = request.State,
            Street = request.Street,
            Name = request.Name,
            PhoneNumber = request.PhoneNumber,
            EmailConfirmed = true,
            TwoFactorEnabled = true
        };
 

    }
}