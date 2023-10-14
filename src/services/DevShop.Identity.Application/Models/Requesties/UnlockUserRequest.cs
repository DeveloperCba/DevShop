using System.ComponentModel.DataAnnotations;

namespace DevShop.Identity.Application.Models.Requesties;

public class UnlockUserRequest
{
    [Required(ErrorMessage = "O campo {PropertyName} é obrigatório.")]
    public string UserId { get; set; }
}