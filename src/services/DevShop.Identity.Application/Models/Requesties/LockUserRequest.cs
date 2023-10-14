using System.ComponentModel.DataAnnotations;

namespace DevShop.Identity.Application.Models.Requesties;

public class LockUserRequest
{
    [Required(ErrorMessage = "O campo {PropertyName} é obrigatório.")]
    public string UserId { get; set; }
}