using System.ComponentModel.DataAnnotations;

namespace DevShop.Identity.Application.Models.Requesties;

public class UserRemoveRequest
{
    [Required(ErrorMessage = "O campo {PropertyName} é obrigatório.")]
    public string  UserId { get; set; }
       
    [Required(ErrorMessage = "O campo {PropertyName} é obrigatório.")]
    [MaxLength(50, ErrorMessage = "O {PropertyName} deve ter até 50 caracteres")]
    public string Email { get; set; }
       
}