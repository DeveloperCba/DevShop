namespace DevShop.Core.Validations.Services.Email;

public interface IEmailService
{
    Task<EmailResponse> Send(EmailRequest emailRequest);
}