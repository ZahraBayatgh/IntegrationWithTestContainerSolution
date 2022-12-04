﻿using API.Contracts.Data;
using API.Domain;

namespace API.Mapping;

public static class DomainToDtoMapper
{
    public static CustomerDto ToCustomerDto(this Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id.Value,
            Email = customer.Email.Value,
            GitHubUsername = customer.GitHubUsername.Value,
            FullName = customer.FullName.Value,
            DateOfBirth = customer.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue)
        };
    }
}
