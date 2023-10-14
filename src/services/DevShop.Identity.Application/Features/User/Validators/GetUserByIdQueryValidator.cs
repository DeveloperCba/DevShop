using DevShop.Identity.Application.Features.User.Queries;
using FluentValidation;
namespace DevShop.Identity.Application.Features.User.Validators;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
            .NotEmpty().WithMessage("O campo {PropertyName} não pode ser vazio.")
            .WithName("Código do Usuário");
    }
}