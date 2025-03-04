using Common.Extensions.Logic;
using FluentValidation;
using Northwind.Data.Northwind.Entity;

namespace NorthwindService.Logic.Validations
{
    public class CategoriesValidation : AbstractValidator<Categories>
    {
        public CategoriesValidation()
        {
            RuleFor(c => c.CategoryName)
               .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
               .MaximumLength(15).WithMessage("O campo {PropertyName} precisa ter no máximo 15 caracteres")
               .Must(StringValidation.ValidString).WithMessage("O campo {PropertyName} precisa ser fornecido");

        }
    }
}
