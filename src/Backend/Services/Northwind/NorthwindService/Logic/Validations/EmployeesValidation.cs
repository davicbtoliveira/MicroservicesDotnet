using Common.Extensions.Logic;
using FluentValidation;
using Northwind.Data.Northwind.Entity;

namespace NorthwindService.Logic.Validations
{
    public class EmployeesValidation : AbstractValidator<Employees>
    {
        public EmployeesValidation()
        {
            RuleFor(c => c.LastName)
               .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
               .MaximumLength(20).WithMessage("O campo {PropertyName} precisa ter no máximo 20 caracteres")
               .Must(StringValidation.ValidString).WithMessage("O campo {PropertyName} precisa ser fornecido");

            RuleFor(c => c.FirstName)
               .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
               .MaximumLength(10).WithMessage("O campo {PropertyName} precisa ter no máximo 10 caracteres")
               .Must(StringValidation.ValidString).WithMessage("O campo {PropertyName} precisa ser fornecido");
        }
    }
}
