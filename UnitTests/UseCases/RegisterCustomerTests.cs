using Application.Interfaces;
using Application.UseCases;
using Application.DTOs;
using Domain.Entities;
using Moq;

namespace UnitTests.UseCases
{
    public class RegisterCustomerTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly RegisterCustomer _registerCustomer;

        public RegisterCustomerTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _registerCustomer = new RegisterCustomer(_customerRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldAddCustomer_WhenCustomerIsValid()
        {
            // Arrange
            var customerDto = new CustomerDto { Name = "John Doe", Cpf = "12345678901", Email = "john@example.com" };

            // Act
            await _registerCustomer.ExecuteAsync(customerDto);

            // Assert
            _customerRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Customer>(
                c => c.Name == customerDto.Name && c.Cpf == customerDto.Cpf && c.Email == customerDto.Email)), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowException_WhenCustomerCpfAlreadyExists()
        {
            // Arrange
            var existingCustomer = new Customer { Id = 1, Name = "John Doe", Cpf = "12345678901", Email = "john@example.com" };
            _customerRepositoryMock.Setup(repo => repo.GetByCpfAsync(existingCustomer.Cpf)).ReturnsAsync(existingCustomer);
            var customerDto = new CustomerDto { Name = "Jane Doe", Cpf = "12345678901", Email = "jane@example.com" };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _registerCustomer.ExecuteAsync(customerDto));
        }
    }
}