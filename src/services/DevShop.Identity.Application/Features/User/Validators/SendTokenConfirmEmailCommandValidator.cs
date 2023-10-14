using DevShop.Identity.Application.Features.User.Commands;
using FluentValidation;
namespace DevShop.Identity.Application.Features.User.Validators;

public class SendTokenConfirmEmailCommandValidator : AbstractValidator<SendTokenConfirmEmailCommand>
{
    public SendTokenConfirmEmailCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
            .NotEmpty().WithMessage("O campo {PropertyName} não pode ser vazio.")
            .MinimumLength(10).WithMessage("O campo {PropertyName} deve ter mais de {TotalLength} caracteres.")
            .EmailAddress().WithMessage("O campo {PropertyName} não é um válido.")
            .WithName("Email");

        RuleFor(x => x.UserId)
            .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
            .NotEmpty().WithMessage("O campo {PropertyName} não pode ser vazio.")
            .WithName("Id do Usuário");
    }
}