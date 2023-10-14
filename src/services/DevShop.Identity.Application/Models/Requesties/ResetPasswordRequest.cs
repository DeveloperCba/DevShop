using System.ComponentModel.DataAnnotations;

namespace DevShop.Identity.Application.Models.Requesties;

public class ResetPasswordRequest
{
    [Required(ErrorMessage = "O campo {PropertyName} é obrigatório.")]
    [MaxLength(50, ErrorMessage = "O {PropertyName} deve ter até 50 caracteres")]
    public string Email { get; set; }
}