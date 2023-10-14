using System.ComponentModel.DataAnnotations;

namespace DevShop.Identity.Application.Models.Requesties;

public class ResetPasswordConfirmationRequest
{
    [Required(ErrorMessage = "O campo {PropertyName} é obrigatório.")]
    public string UserId { get; set; }

    [Required(ErrorMessage = "O campo {PropertyName} é obrigatório.")]
    public string Token { get; set; }

    [Required(ErrorMessage = "O campo {PropertyName} é obrigatório.")]
    public string Password { get; set; }
}