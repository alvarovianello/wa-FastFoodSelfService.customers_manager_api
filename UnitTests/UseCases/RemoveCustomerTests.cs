using Application.Interfaces;
using Application.UseCases;
using Domain.Entities;
using Moq;

namespace UnitTests.UseCases
{
    public class RemoveCustomerTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly RemoveCustomer _removeCustomer;

        public RemoveCustomerTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _removeCustomer = new RemoveCustomer(_customerRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldRemoveCustomer_WhenCustomerExists()
        {
            // Arrange
            var customerId = 1;
            var existingCustomer = new Customer
            {
                Id = customerId,
                Name = "John Doe",
                Cpf = "12345678901",
                Email = "john.doe@example.com"
            };

            _customerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(customerId))
                .ReturnsAsync(existingCustomer);

            // Act
            await _removeCustomer.ExecuteAsync(customerId);

            // Assert
            _customerRepositoryMock.Verify(repo => repo.GetByIdAsync(customerId), Times.Once);
            _customerRepositoryMock.Verify(repo => repo.RemoveAsync(customerId), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowException_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = 2;

            _customerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(customerId))
                .ReturnsAsync((Customer?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _removeCustomer.ExecuteAsync(customerId));
            Assert.Equal("Cliente não encontrado.", exception.Message);

            _customerRepositoryMock.Verify(repo => repo.GetByIdAsync(customerId), Times.Once);
            _customerRepositoryMock.Verify(repo => repo.RemoveAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
