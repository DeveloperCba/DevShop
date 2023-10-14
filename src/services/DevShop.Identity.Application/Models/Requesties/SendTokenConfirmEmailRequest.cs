using System.ComponentModel.DataAnnotations;

namespace DevShop.Identity.Application.Models.Requesties;

public class SendTokenConfirmEmailRequest
{
    [Required(ErrorMessage = "O campo {PropertyName} é obrigatório.")]
    public string UserId { get; set; }
    [Required(ErrorMessage = "O campo {PropertyName} é obrigatório.")]
    public string Email { get; set; }
}