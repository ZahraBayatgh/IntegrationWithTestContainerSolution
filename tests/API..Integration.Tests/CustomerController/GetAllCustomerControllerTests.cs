using System.Net;
using System.Net.Http.Json;
using Bogus;
using API.Contracts.Requests;
using API.Contracts.Responses;
using FluentAssertions;
using Xunit;

namespace API.Tests.Integration.ControllerTest;

public class GetAllControllerTestTests : IClassFixture<CustomerApiFactory>
{
    private readonly HttpClient _client;

    private readonly Faker<CustomerRequest> _customerGenerator = new Faker<CustomerRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        .RuleFor(x => x.GitHubUsername, "ZahraBayatgh")
        .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date);

    public GetAllControllerTestTests(CustomerApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsAllCustomers_WhenCustomersExist()
    {
        // Arrange
        var customer = _customerGenerator.Generate();
        var createdResponse = await _client.PostAsJsonAsync("customers", customer);
        var createdCustomer = await createdResponse.Content.ReadFromJsonAsync<CustomerResponse>();

        // Act
        var response = await _client.GetAsync("customers");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var customersResponse = await response.Content.ReadFromJsonAsync<GetAllCustomersResponse>();
        customersResponse!.Customers.Single().Should().BeEquivalentTo(createdCustomer);
        
        // Cleanup
        await _client.DeleteAsync($"customers/{createdCustomer!.Id}");
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyResult_WhenNoCustomersExist()
    {
        // Act
        var response = await _client.GetAsync("customers");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var customersResponse = await response.Content.ReadFromJsonAsync<GetAllCustomersResponse>();
        customersResponse!.Customers.Should().BeEmpty();
    }
}
