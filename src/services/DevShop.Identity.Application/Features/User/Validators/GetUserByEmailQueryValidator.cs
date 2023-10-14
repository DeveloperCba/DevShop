using DevShop.Identity.Application.Features.User.Queries;
using FluentValidation;
namespace DevShop.Identity.Application.Features.User.Validators;

public class GetUserByEmailQueryValidator : AbstractValidator<GetUserByEmailQuery>
{
    public GetUserByEmailQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
            .NotEmpty().WithMessage("O campo {PropertyName} não pode ser vazio.")
            .MinimumLength(10).WithMessage("O campo {PropertyName} deve ter mais de {TotalLength} caracteres.")
            .EmailAddress().WithMessage("O campo {PropertyName} não é um válido.")
            .WithName("Email");
    }
}