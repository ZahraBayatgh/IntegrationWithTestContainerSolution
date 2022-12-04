﻿using API.Contracts.Responses;
using API.Domain;

namespace API.Mapping;

public static class DomainToApiContractMapper
{
    public static CustomerResponse ToCustomerResponse(this Customer customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id.Value,
            Email = customer.Email.Value,
            GitHubUsername = customer.GitHubUsername.Value,
            FullName = customer.FullName.Value,
            DateOfBirth = customer.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue)
        };
    }

    public static GetAllCustomersResponse ToCustomersResponse(this IEnumerable<Customer> customers)
    {
        return new GetAllCustomersResponse
        {
            Customers = customers.Select(x => new CustomerResponse
            {
                Id = x.Id.Value,
                Email = x.Email.Value,
                GitHubUsername = x.GitHubUsername.Value,
                FullName = x.FullName.Value,
                DateOfBirth = x.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue)
            })
        };
    }
}
