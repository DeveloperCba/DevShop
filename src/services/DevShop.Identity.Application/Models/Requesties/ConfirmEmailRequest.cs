using System.ComponentModel.DataAnnotations;

namespace DevShop.Identity.Application.Models.Requesties;

public class ConfirmEmailRequest
{
    [Required(ErrorMessage = "O campo {PropertyName} é obrigatório.")]
    public string UserId { get; set; }
    [Required(ErrorMessage = "O campo {PropertyName} é obrigatório.")]
    public string Token { get; set; }
}