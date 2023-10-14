using System.Text;
using System.Text.Encodings.Web;
using DevShop.Core.DomainObjects;
using DevShop.Core.Validations.Services.Email;
using DevShop.Identity.Application.Features.Auth.Commands;
using DevShop.Identity.Application.Features.Auth.Dtos;
using DevShop.Identity.Application.Models;
using DevShop.Identity.Application.Models.Dtos;
using DevShop.Identity.Application.Services;
using DevShop.WebAPI.Core.Services;
using MediatR;
using Microsoft.Extensions.Options;

namespace DevShop.Identity.Application.Features.Auth.CommandHandlers;

public class ResetPasswordCommandHandler : BaseService,
    IRequestHandler<ResetPasswordCommand, ResetPasswordDto>,
    IRequestHandler<ResetPasswordConfirmationCommand, MessageDto>
{
    private readonly IEmailService _emailService;
    private readonly IAutenticationService _autenticationService;
    private readonly ExternalEmailSettings _externalEmailSettings;

    public ResetPasswordCommandHandler(
        IEmailService emailService,
        INotify notification,
        IAutenticationService autenticationService,
        IOptions<ExternalEmailSettings> externalEmailSettings) : base(notification)
    {
        _emailService = emailService;
        _autenticationService = autenticationService;
        _externalEmailSettings = externalEmailSettings.Value;
    }

    public async Task<ResetPasswordDto> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _autenticationService.UserManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            NotificationEvent("Usuário não encontrado.");
            return default!;
        }

        var token = await _autenticationService.UserManager.GeneratePasswordResetTokenAsync(user);
        var callBackUrl = $@"{_externalEmailSettings.Url}/UserIdentity/EsqueciSenha?userId={user.Id}&token={UrlEncoder.Default.Encode(token)}";

        await SendEmailReset(user.Name, user.Email, callBackUrl);

        return new ResetPasswordDto
        {
            CallBack = callBackUrl,
            Token = token,
            UserId = user.Id
        };
    }

    public async Task<MessageDto> Handle(ResetPasswordConfirmationCommand request, CancellationToken cancellationToken)
    {
        var response = new MessageDto();
        var user = await _autenticationService.UserManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            NotificationEvent("Usuário não encontrado.");
            return default!;
        }

        var password = request.Password;
        var checkPassword = await _autenticationService.UserManager.CheckPasswordAsync(user, password);
        if (checkPassword)
        {
            response.Message = "Atenção, está senha já foi utilizada, tente uma nova senha.";
            return response;
        }

        var result = await _autenticationService.UserManager.ResetPasswordAsync(user, request.Token, password);
        if (result.Succeeded)
        {
            response.Message = "Senha alterada com sucesso!!!";

            await SendEmailConfirm(user.Name, user.Email);
            return response;
        }

        if (result.Errors.ToList().Any())
            foreach (var item in result.Errors)
                response.Message += "\n" + item.Description;
        else
            response.Message += "\nO link não existe, tente novamente.";

        return response;
    }


    private async Task SendEmailReset(string name, string email, string callBack)
    {
        var title = "Redifinição de Senha.";
        var message = "Clique no link para resetar sua senha.";
        var empresa = "Teste";
        var htmlEmail = new StringBuilder();
        htmlEmail.AppendLine("<div style=\"font-family:calibri; width:60%; border:1px solid #ccc; margin:0 auto; height:400px; padding:10px\">");
        htmlEmail.AppendLine($"<h1> <div style=\"float:right; width:160px !important; \"><img src=\"cid:Image\" width=\"160px !important\" /></div> {title} </h1>");
        htmlEmail.AppendLine(string.Format("            Olá <strong>{0}</strong>, <br>", name));
        htmlEmail.AppendLine(string.Format("            Você solicitou a redefinição de senha. Por favor, clique no link abaixo para redefinir sua senha.<br>"));
        htmlEmail.AppendLine("<br>");
        htmlEmail.AppendLine(string.Format("<div style=\"text-align:center\" ><a style=\"vertical-align:middle\" href=\"{0}\"> Clique Aqui</a> <strong>para resetar a senha</strong></div>", callBack));

        htmlEmail.AppendLine($"<b>Política de Senhas {empresa}</b> <br>");
        htmlEmail.AppendLine("A senha deve atender aos requisitos de complexidade aplicados quando as senhas são alteradas ou criadas. <br><br>");
        htmlEmail.AppendLine("<b>NÃO forneça as suas senhas para outra pessoa.</b> <br><br>");
        htmlEmail.AppendLine("- Não conter o nome da conta do usuário ou partes do nome completo do usuário que excedam dois caracteres consecutivos;<br>");
        htmlEmail.AppendLine("- Deve possuir pelo menos 8 caracteres ;<br>");
        htmlEmail.AppendLine("- Contêm caracteres maiúsculos (A a Z) e minúsculos (a a z); <br>");
        htmlEmail.AppendLine("- Dígitos de base 10 (0 a 9) e caracteres não alfabéticos (por exemplo,!, $, #,%); <br>");
        htmlEmail.AppendLine("- Aplicado histórico de senhas. Não é possível reaproveitar das últimas 5 senhas; <br>");

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

    private async Task SendEmailConfirm(string name, string email)
    {
        var title = "Senha alterada.";
        var message = "Sua senha foi alterada com sucesso.";
        var empresa = "Teste";
        var htmlEmail = new StringBuilder();
        htmlEmail.AppendLine("<div style=\"font-family:calibri; width:70%; border:1px solid #ccc; margin:0 auto; height:400px; padding:10px\">");
        htmlEmail.AppendLine($"<h1>");
        htmlEmail.AppendLine($"    <div style=\"float:right; width:160px !important; \"><img src=\"cid:Image\" width=\"160px !important\" /></div> {title} ");
        htmlEmail.AppendLine("</h1>");
        htmlEmail.AppendLine($"     Olá <strong>{name}</strong>, <br>");
        htmlEmail.AppendLine($"     <p>{message}</p><br>");
        htmlEmail.AppendLine("<br>");
        htmlEmail.AppendLine("<br><br><br>");
        htmlEmail.AppendLine("  <div style=\"bottom:0px; text-align:center\">");
        htmlEmail.AppendLine("      <p>© " + DateTime.Now.Year.ToString() + $" - {empresa}</p>");
        htmlEmail.AppendLine("  </div>");
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
}