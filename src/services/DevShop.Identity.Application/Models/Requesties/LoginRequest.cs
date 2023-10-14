using System.ComponentModel.DataAnnotations;

namespace DevShop.Identity.Application.Models.Requesties;

public class LoginRequest
{
    [Required(ErrorMessage = "Informe o Login de Acesso")]
    [MaxLength(50, ErrorMessage = "O login deve ter até 50 caracteres")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Informe a senha")]
    [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres")]
    public string Password { get; set; }
}