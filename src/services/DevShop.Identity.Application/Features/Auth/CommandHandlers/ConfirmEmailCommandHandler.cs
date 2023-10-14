using System.Text;
using DevShop.Core.DomainObjects;
using DevShop.Core.Validations.Services.Email;
using DevShop.Identity.Application.Features.Auth.Commands;
using DevShop.Identity.Application.Models.Dtos;
using DevShop.Identity.Application.Services;
using DevShop.WebAPI.Core.Services;
using MediatR;
namespace DevShop.Identity.Application.Features.Auth.CommandHandlers;

public class ConfirmEmailCommandHandler : BaseService, IRequestHandler<ConfirmEmailCommand, MessageDto>
{
    private readonly IEmailService _emailService;
    private readonly IAutenticationService _autenticationService;

    public ConfirmEmailCommandHandler(
        IEmailService emailService,
        INotify notification,
        IAutenticationService autenticationService) : base(notification)
    {
        _emailService = emailService;
        _autenticationService = autenticationService;
    }

    public async Task<MessageDto> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var response = new MessageDto();
        var user = await _autenticationService.UserManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            NotificationEvent("Usuário não encontrado.");
            return default!;
        }

        var result = await _autenticationService.UserManager.ConfirmEmailAsync(user, request.Token);

        if (result.Succeeded)
        {
            response.Message = "Email confirmado com sucesso!!!";
            await SendEmailConfirm(user.Name, "teste@teste.com.br");
            return response;
        }

        if (result.Errors.ToList().Any())
            foreach (var item in result.Errors)
                response.Message += "\n" + item.Description;
        else
            response.Message += "\nO link não existe, tente novamente.";

        return response;
    }

    private async Task SendEmailConfirm(string name, string email)
    {
        var title = "Email Confirmado.";
        var message = "Email confirmado com sucesso.";
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
        htmlEmail.AppendLine("      <p>© " + DateTime.Now.Year.ToString() + " - Empresa</p>");
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