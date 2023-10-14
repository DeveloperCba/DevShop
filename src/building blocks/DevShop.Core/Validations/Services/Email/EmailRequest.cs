using System.Net.Mail;

namespace DevShop.Core.Validations.Services.Email;

public class EmailRequest
{
    /// <summary>
    /// Título
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Assunto
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Mensagem
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Destinatários
    /// </summary>
    public List<string> Recipients { get; set; }

    /// <summary>
    /// Com cópia
    /// </summary>
    public List<string> WithCopy { get; set; }

    /// <summary>
    /// Com cópia oculta
    /// </summary>
    public List<string> WithBlindCopy { get; set; }

    /// <summary>
    /// Anexos
    /// </summary>
    public List<Attachment> Attachments { get; set; }

    /// <summary>
    /// Enviar Logo
    /// </summary>
    public bool SendImage { get; set; }  


    public EmailRequest()
    {
        SendImage = true;
        WithCopy = new List<string>();
        WithBlindCopy = new List<string>();
        Recipients = new List<string>();
        Attachments = new List<Attachment>();
    }
}