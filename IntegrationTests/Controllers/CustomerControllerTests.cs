using Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Controllers
{
    public class CustomerControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public CustomerControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
        }

        [Fact]
        public async Task GetAllCustomers_ShouldReturnOkAndCustomerList()
        {
            // Arrange
            var expectedCustomer = new CustomerDto
            {
                Id = 1,
                Name = "Álvaro Oliveira",
                Cpf = "40851368875",
                Email = "alvaro@email.com"
            };

            // Act
            var response = await _client.GetAsync("/api/customer/all");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var customers = await response.Content.ReadFromJsonAsync<List<CustomerDto>>();
            Assert.NotNull(customers);
            Assert.Contains(customers, c => c.Cpf == expectedCustomer.Cpf);
        }
    }
}
