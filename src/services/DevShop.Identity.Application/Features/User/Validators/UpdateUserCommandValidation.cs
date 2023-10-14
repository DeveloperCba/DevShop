using DevShop.Core.Extensions;
using DevShop.Core.Validations;
using DevShop.Identity.Application.Features.User.Commands;
using FluentValidation;
namespace DevShop.Identity.Application.Features.User.Validators;

public class UpdateUserCommandValidation : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidation()
    {
        RuleFor(x => x.Email)
            .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
            .NotEmpty().WithMessage("O campo {PropertyName} não pode ser vazio.")
            .MinimumLength(10).WithMessage("O campo {PropertyName} deve ter mais de {TotalLength} caracteres.")
            .EmailAddress().WithMessage("O campo {PropertyName} não é um válido.")
            .WithName("Email");

        RuleFor(x => x.Name)
            .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
            .NotEmpty().WithMessage("O campo {PropertyName} não pode ser vazio.")
            .MinimumLength(6).WithMessage("O campo {PropertyName} deve ter no minimo {MinLength} caracteres e foi informado {TotalLength} caracteres.")
            .MaximumLength(150).WithMessage("O campo {PropertyName} deve ter no máximo {MaxLength} caracteres e foi informado {TotalLength} caracteres")
            .WithName("Nome");

        RuleFor(x => x.Document)
            .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
            .NotEmpty().WithMessage("O campo {PropertyName} não pode ser vazio.")
            .WithName(x => x.Document.Length == 11 ? "CPF" : "CNPJ");


        When(x => x.Document.OnlyNumber().ToString().Length == 11, () =>
        {
            RuleFor(f => Convert.ToInt64(f.Document.OnlyNumber()))
                .NotEqual(CpfValidation.SizeCPF)
                .WithMessage("O campo {PropertyName} precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.")
                .WithName("CPF");

            RuleFor(f => CpfValidation.Validate(f.Document)).Equal(true)
                .WithMessage("O {PropertyName} fornecido é inválido.")
                .WithName("CPF");
        });


        When(x => x.Document.OnlyNumber().ToString().Length == 14, () =>
        {
            RuleFor(f => Convert.ToInt64(f.Document.OnlyNumber()))
                .NotEqual(CnpjValidation.SizeCNPJ).WithMessage("O campo {PropertyName} precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.")
                .WithName("CNPJ");

            RuleFor(f => CnpjValidation.Validate(f.Document)).Equal(true)
                .WithMessage("O {PropertyName} fornecido é inválido.")
                .WithName("CNPJ");
        });
    }
}