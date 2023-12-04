using Employees.Shared.Model;
using Employees.Shared.Requests;
using FluentValidation;

namespace Employees.API.Validation;

public class AddCompanyRequestValidator : AbstractValidator<AddCompanyRequest>
{
    public AddCompanyRequestValidator(IValidator<CompanyDto> companyValidator)
    {
        RuleFor(x => x.Company)
            .NotNull()
            .WithMessage("Company can't be empty");

        RuleFor(x => x.Company)
            .SetValidator(companyValidator);
    }
}