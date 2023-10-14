using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Microsoft.Extensions.Options;

namespace DevShop.Core.Validations.Services.Email;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(
        IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task<EmailResponse> Send(EmailRequest emailRequest)
    {
        var emailResponse = new EmailResponse();
        try
        {
            var recipients = emailRequest.Recipients?.GroupBy(c => c?.Trim()).Select(c => c.Key?.Trim()).ToList() ?? new List<string>();
            var withCopy = emailRequest.WithCopy?.GroupBy(x => x?.Trim()).Select(x => x.Key?.Trim()).ToList() ?? new List<string>();
            var withBlindCopy = emailRequest.WithBlindCopy.GroupBy(w => w?.Trim()).Select(w => w.Key?.Trim()).ToList() ?? new List<string>();

            if (string.IsNullOrEmpty(emailRequest.Title))
                return null;

            var mailMessage = new System.Net.Mail.MailMessage
            {
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true,
                Sender = new MailAddress(_emailSettings.SenderAddress, emailRequest.Title),
                From = new MailAddress(_emailSettings.SenderAddress, emailRequest.Title)
            };

            if (recipients is not null && recipients.Count > 0)
                foreach (var recipient in recipients)
                    mailMessage.To.Add(recipient);

            if (withCopy is not null && withCopy.Count > 0)
                foreach (var withcopy in withCopy)
                    mailMessage.CC.Add(withcopy);

            if (withBlindCopy is not null && withBlindCopy.Count > 0)
                foreach (var withBlindcopy in withBlindCopy)
                    mailMessage.Bcc.Add(withBlindcopy);

            if (mailMessage.To.Count == 0)
                mailMessage.To.Add(_emailSettings.SenderAddress);

            var htmlText = new StringBuilder();
            if (emailRequest.SendImage)
            {
                try
                {
                    var path = $"{AppDomain.CurrentDomain.BaseDirectory}logo.png";
                    var inlineLogo = new Attachment(path)
                    {
                        ContentId = "Image"
                    };
                    inlineLogo.ContentDisposition.Inline = true;
                    inlineLogo.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

                    mailMessage.Attachments.Add(inlineLogo);
                }
                catch   { }
            }

            if (emailRequest.Attachments is not null && emailRequest.Attachments.Count > 0)
                foreach (var attachment in emailRequest.Attachments)
                    mailMessage.Attachments.Add(attachment);


            mailMessage.Body = emailRequest.Message;
            mailMessage.Subject = emailRequest.Subject;
            mailMessage.Priority = MailPriority.High;

            using var client = new SmtpClient();
            client.Host = _emailSettings.Host;
            client.Port = Convert.ToInt32(_emailSettings.Port);
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(_emailSettings.SenderAddress, _emailSettings.Password);
            client.EnableSsl = _emailSettings.EnableSsl;
            await Task.Run(() => client.Send(mailMessage));
        }
        catch (Exception ex)
        {
            var errors = ex.Message;
            return null;
        }

        return emailResponse;
    }
}