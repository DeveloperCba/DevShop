using DevShop.Core.Validations;
using DevShop.Identity.Application.Features.Auth.Commands;
using FluentValidation;
namespace DevShop.Identity.Application.Features.Auth.Validators;

public class ResetPasswordConfirmationValidator : AbstractValidator<ResetPasswordConfirmationCommand>
{
    public ResetPasswordConfirmationValidator()
    {
        RuleFor(x => x.Token)
            .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
            .NotEmpty().WithMessage("O campo {PropertyName} não pode ser vazio.")
            .WithName("Token");

        RuleFor(x => x.UserId)
            .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
            .NotEmpty().WithMessage("O campo {PropertyName} não pode ser vazio.")
            .WithName("Código do Usuário");

        RuleFor(x => x.Password)
            .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
            .NotEmpty().WithMessage("O campo {PropertyName} não pode ser vazio.")
            .Length(6, 20).WithMessage("O campo {PropertyName} deve ter entre {MinLength} e {MaxLength} caracteres.")
            .Must(senha => senha.ContainsUppercaseLettersAndContainsSpecialCharacters()).WithMessage("O campo {PropertyName} deve conter pelo menos uma letra minúscula, uma letra maiúscula, um número e um caractere especial..")
            .WithName("Senha");
    }
}