using System.Net;
using System.Net.Http.Json;
using Bogus;
using API.Contracts.Requests;
using API.Contracts.Responses;
using FluentAssertions;
using Xunit;

namespace API.Tests.Integration.ControllerTest;

public class DeleteControllerTestTests : IClassFixture<CustomerApiFactory>
{
    private readonly HttpClient _client;

    private readonly Faker<CustomerRequest> _customerGenerator = new Faker<CustomerRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        .RuleFor(x => x.GitHubUsername, "ZahraBayatgh")
        .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date);

    public DeleteControllerTestTests(CustomerApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenCustomerExists()
    {
        // Arrange
        var customer = _customerGenerator.Generate();
        var createdResponse = await _client.PostAsJsonAsync("customers", customer);
        var createdCustomer = await createdResponse.Content.ReadFromJsonAsync<CustomerResponse>();

        // Act
        var response = await _client.DeleteAsync($"customers/{createdCustomer!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        // Act
        var response = await _client.DeleteAsync($"customers/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
