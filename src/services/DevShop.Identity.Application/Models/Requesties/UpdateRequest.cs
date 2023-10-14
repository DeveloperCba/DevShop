using System.ComponentModel.DataAnnotations;

namespace DevShop.Identity.Application.Models.Requesties;

public class UpdateRequest
{
    [Required(ErrorMessage = "O campo {PropertyName} é obrigatório.")]
    public string UserId { get; set; }

    [Required(ErrorMessage = "O campo {PropertyName} é obrigatório.")]
    [MaxLength(50, ErrorMessage = "O {PropertyName} deve ter até 50 caracteres")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo {PropertyName} é obrigatório.")]
    [MinLength(6, ErrorMessage = "A {PropertyName} deve ter pelo menos 6 caracteres")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O campo {PropertyName} é obrigatório.")]
    public string Document { get; set; }

    public string Street { get; set; }

    public string Complement { get; set; }
    public string Number { get; set; }
    public string ZipCode { get; set; }
    public string Neighborhood { get; set; }
    public string City { get; set; }
    public string State { get; set; }

    [Required(ErrorMessage = "O campo {PropertyName} é obrigatório.")]
    public string PhoneNumber { get; set; }
}