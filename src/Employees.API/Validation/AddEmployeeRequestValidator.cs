using Employees.Shared.Enums;
using Employees.Shared.Requests;
using FluentValidation;

namespace Employees.API.Validation;

public class AddEmployeeRequestValidator : AbstractValidator<AddEmployeeRequest>
{
    public AddEmployeeRequestValidator()
    {
        RuleFor(x => x.Employee)
            .NotNull()
            .WithMessage("Employee can't be empty")
            .DependentRules(() =>
            {
                RuleFor(x => x.Employee.Title)
                    .IsEnumName(typeof(EmployeeTitle), caseSensitive: false)
                    .WithMessage("Employee title is not valid");

                RuleFor(x => x.Employee.Email)
                    .EmailAddress()
                    .WithMessage("Email is not valid");

                RuleFor(x => x.Employee.CompanyIds)
                    .NotEmpty();
            });
    }
}