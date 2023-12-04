using Employees.Shared.Enums;
using Employees.Shared.Model;
using FluentValidation;

namespace Employees.API.Validation;

public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
{
    public EmployeeDtoValidator()
    {
        RuleFor(x => x.Title)
            .IsEnumName(typeof(EmployeeTitle), caseSensitive: false)
            .WithMessage("Employee title is not valid");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email is not valid");

        RuleFor(x => x.CompanyIds)
            .NotEmpty();
    }
}