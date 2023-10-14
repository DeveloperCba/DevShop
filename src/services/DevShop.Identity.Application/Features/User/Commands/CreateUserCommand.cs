using DevShop.Identity.Application.Features.User.Dtos;
using MediatR;
namespace DevShop.Identity.Application.Features.User.Commands;

public class CreateUserCommand : IRequest<ConfirmEmailDto>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Document { get; set; }
    public string Street { get; set; }
    public string Complement { get; set; }
    public string Number { get; set; }
    public string ZipCode { get; set; }
    public string Neighborhood { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PhoneNumber { get; set; }
}