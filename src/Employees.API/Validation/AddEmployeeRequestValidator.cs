using Employees.Shared.Model;
using Employees.Shared.Requests;
using FluentValidation;

namespace Employees.API.Validation;

public class AddEmployeeRequestValidator : AbstractValidator<AddEmployeeRequest>
{
    public AddEmployeeRequestValidator(IValidator<EmployeeDto> employeeValidator)
    {
        RuleFor(x => x.Employee)
            .NotNull()
            .WithMessage("Employee can't be empty");

        RuleFor(x => x.Employee)
            .SetValidator(employeeValidator);
    }
}