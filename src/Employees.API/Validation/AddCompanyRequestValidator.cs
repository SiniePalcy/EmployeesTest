using Employees.Shared.Requests;
using FluentValidation;

namespace Employees.API.Validation;

public class AddCompanyRequestValidator : AbstractValidator<AddCompanyRequest>
{
    public AddCompanyRequestValidator()
    {
        RuleFor(x => x.Company)
            .NotNull()
            .WithMessage("Company can't be empty")
            .DependentRules(() =>
            {
                RuleFor(x => x.Company.Name)
                    .NotEmpty()
                    .WithMessage("Company name can't be empty");
            });
    }
}