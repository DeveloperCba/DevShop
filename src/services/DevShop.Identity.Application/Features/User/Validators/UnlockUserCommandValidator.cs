using DevShop.Identity.Application.Features.User.Commands;
using FluentValidation;
namespace DevShop.Identity.Application.Features.User.Validators;

public class UnlockUserCommandValidator : AbstractValidator<UnlockUserCommand>
{
    public UnlockUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
            .NotEmpty().WithMessage("O campo {PropertyName} não pode ser vazio.")
            .WithName("Código do Usuário");
    }
}