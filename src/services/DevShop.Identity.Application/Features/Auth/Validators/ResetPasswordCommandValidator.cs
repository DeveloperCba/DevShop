using DevShop.Identity.Application.Features.Auth.Commands;
using FluentValidation;
namespace DevShop.Identity.Application.Features.Auth.Validators;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
            .NotEmpty().WithMessage("O campo {PropertyName} não pode ser vazio.")
            .MinimumLength(10).WithMessage("O campo {PropertyName} deve ter mais de {TotalLength} caracteres.")
            .EmailAddress().WithMessage("O campo {PropertyName} não é um válido.")
            .WithName("Email");
    }
}