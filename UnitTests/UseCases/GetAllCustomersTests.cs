using Application.Interfaces;
using Application.UseCases;
using Domain.Entities;
using Moq;

namespace UnitTests.UseCases
{
    public class GetAllCustomersTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly GetAllCustomers _getAllCustomers;

        public GetAllCustomersTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _getAllCustomers = new GetAllCustomers(_customerRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnAllCustomers_WhenCustomersExist()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "John Doe", Cpf = "12345678901", Email = "john@example.com" },
                new Customer { Id = 2, Name = "Jane Doe", Cpf = "09876543210", Email = "jane@example.com" }
            };

            _customerRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(customers);

            // Act
            var result = await _getAllCustomers.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Collection(result,
                c =>
                {
                    Assert.Equal("John Doe", c.Name);
                    Assert.Equal("12345678901", c.Cpf);
                },
                c =>
                {
                    Assert.Equal("Jane Doe", c.Name);
                    Assert.Equal("09876543210", c.Cpf);
                });
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnEmptyList_WhenNoCustomersExist()
        {
            // Arrange
            _customerRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Customer>());

            // Act
            var result = await _getAllCustomers.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}