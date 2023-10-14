using DevShop.Identity.Application.Features.User.Queries;
using FluentValidation;
namespace DevShop.Identity.Application.Features.User.Validators;

public class GetUserByUserNameQueryValidator : AbstractValidator<GetUserByUserNameQuery>
{
    public GetUserByUserNameQueryValidator()
    {
        RuleFor(x => x.UserName)
            .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
            .NotEmpty().WithMessage("O campo {PropertyName} não pode ser vazio.")
            .WithName("Código do Usuário");
    }
}