using API.Contracts.Requests;
using FluentValidation;

namespace API.Validation;

public class CustomerRequestValidator : AbstractValidator<CustomerRequest>
{
    public CustomerRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.GitHubUsername).NotEmpty();
        RuleFor(x => x.DateOfBirth).NotEmpty();
    }
}
