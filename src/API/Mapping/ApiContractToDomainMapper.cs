﻿using API.Contracts.Requests;
using API.Domain;
using API.Domain.Common;

namespace API.Mapping;

public static class ApiContractToDomainMapper
{
    public static Customer ToCustomer(this CustomerRequest request)
    {
        return new Customer
        {
            Id = CustomerId.From(Guid.NewGuid()),
            Email = Email.From(request.Email),
            GitHubUsername = GitHubUsername.From(request.GitHubUsername),
            FullName = FullName.From(request.FullName),
            DateOfBirth = DateOfBirth.From(DateOnly.FromDateTime(request.DateOfBirth))
        };
    }

    public static Customer ToCustomer(this UpdateCustomerRequest request)
    {
        return new Customer
        {
            Id = CustomerId.From(request.Id),
            Email = Email.From(request.Customer.Email),
            GitHubUsername = GitHubUsername.From(request.Customer.GitHubUsername),
            FullName = FullName.From(request.Customer.FullName),
            DateOfBirth = DateOfBirth.From(DateOnly.FromDateTime(request.Customer.DateOfBirth))
        };
    }
}
