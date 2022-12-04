using System.Net;
using System.Net.Http.Json;
using Bogus;
using API.Contracts.Requests;
using API.Contracts.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace API.Tests.Integration.ControllerTest;

public class CreateControllerTestTests : IClassFixture<CustomFactory>
{
    private readonly HttpClient _client;

    private readonly Faker<CustomerRequest> _customerGenerator = new Faker<CustomerRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        .RuleFor(x => x.GitHubUsername, "ZahraBayatgh")
        .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date);

    public CreateControllerTestTests(CustomFactory apiFactory)
    {
        _client = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Create_CreatesUser_WhenDataIsValid()
    {
        // Arrange
        var customer = _customerGenerator.Generate();

        // Act
        var response = await _client.PostAsJsonAsync("customers", customer);

        // Assert
        var customerResponse = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        customerResponse.Should().BeEquivalentTo(customer);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location!.ToString().Should()
            .Be($"http://localhost/customers/{customerResponse!.Id}");
    }

    [Fact]
    public async Task Create_ReturnsValidationError_WhenEmailIsInvalid()
    {
        // Arrange
        const string invalidEmail = "ZahraBayat";
        var customer = _customerGenerator.Clone()
            .RuleFor(x => x.Email, invalidEmail).Generate();

        // Act
        var response = await _client.PostAsJsonAsync("customers", customer);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error.Title.Should().Be("One or more validation errors occurred.");
        error.Errors["Email"][0].Should().Be($"{invalidEmail} is not a valid email address");
    }
}
