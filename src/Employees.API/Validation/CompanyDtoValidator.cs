using Employees.Shared.Model;
using FluentValidation;

namespace Employees.API.Validation;

public class CompanyDtoValidator : AbstractValidator<CompanyDto>
{
    public CompanyDtoValidator(IValidator<ShortEmployeeDto> employeeDtoValidator)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Company name can't be empty");

        RuleForEach(x => x.Employees)
            .SetValidator(employeeDtoValidator)
            .When(x => x.Employees != null);
    }
}