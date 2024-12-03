using Application.Interfaces;
using Application.UseCases;
using Domain.Entities;
using Moq;

namespace UnitTests.UseCases
{
    public class GetCustomerByIdTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly GetCustomerById _getCustomerById;

        public GetCustomerByIdTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _getCustomerById = new GetCustomerById(_customerRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnCustomerDto_WhenCustomerExists()
        {
            // Arrange
            var customerId = 1;
            var expectedCustomer = new Customer
            {
                Id = customerId,
                Name = "Jane Doe",
                Cpf = "12345678901",
                Email = "jane.doe@example.com"
            };

            _customerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(customerId))
                .ReturnsAsync(expectedCustomer);

            // Act
            var result = await _getCustomerById.ExecuteAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCustomer.Id, result.Id);
            Assert.Equal(expectedCustomer.Name, result.Name);
            Assert.Equal(expectedCustomer.Cpf, result.Cpf);
            Assert.Equal(expectedCustomer.Email, result.Email);

            _customerRepositoryMock.Verify(repo => repo.GetByIdAsync(customerId), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = 999;
            _customerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(customerId))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _getCustomerById.ExecuteAsync(customerId);

            // Assert
            Assert.Null(result);

            _customerRepositoryMock.Verify(repo => repo.GetByIdAsync(customerId), Times.Once);
        }
    }
}