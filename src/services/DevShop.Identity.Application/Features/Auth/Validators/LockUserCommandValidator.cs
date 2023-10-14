using DevShop.Identity.Application.Features.User.Commands;
using FluentValidation;
namespace DevShop.Identity.Application.Features.Auth.Validators;

public class LockUserCommandValidator : AbstractValidator<LockUserCommand>
{
    public LockUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
            .NotEmpty().WithMessage("O campo {PropertyName} não pode ser vazio.")
            .WithName("Código do Usuário");
    }
}