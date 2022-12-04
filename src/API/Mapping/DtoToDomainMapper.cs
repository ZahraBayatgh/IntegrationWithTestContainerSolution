using API.Contracts.Data;
using API.Domain;
using API.Domain.Common;

namespace API.Mapping;

public static class DtoToDomainMapper
{
    public static Customer ToCustomer(this CustomerDto customerDto)
    {
        return new Customer
        {
            Id = CustomerId.From(customerDto.Id),
            Email = Email.From(customerDto.Email),
            GitHubUsername = GitHubUsername.From(customerDto.GitHubUsername),
            FullName = FullName.From(customerDto.FullName),
            DateOfBirth = DateOfBirth.From(DateOnly.FromDateTime(customerDto.DateOfBirth))
        };
    }
}
