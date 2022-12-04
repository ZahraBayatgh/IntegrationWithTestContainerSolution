using System.Net;
using System.Net.Http.Json;
using Bogus;
using API.Contracts.Requests;
using API.Contracts.Responses;
using FluentAssertions;
using Xunit;

namespace API.Tests.Integration.ControllerTest;

public class GetControllerTestTests : IClassFixture<CustomFactory>
{
    private readonly HttpClient _client;

    private readonly Faker<CustomerRequest> _customerGenerator = new Faker<CustomerRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        .RuleFor(x => x.GitHubUsername, "ZahraBayatgh")
        .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date);

    public GetControllerTestTests(CustomFactory apiFactory)
    {
        _client = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsCustomer_WhenCustomerExists()
    {
        // Arrange
        var customer = _customerGenerator.Generate();
        var createdResponse = await _client.PostAsJsonAsync("customers", customer);
        var createdCustomer = await createdResponse.Content.ReadFromJsonAsync<CustomerResponse>();

        // Act
        var response = await _client.GetAsync($"customers/{createdCustomer!.Id}");

        // Assert
        var retrievedCustomer = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        retrievedCustomer.Should().BeEquivalentTo(createdCustomer);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync($"customers/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
