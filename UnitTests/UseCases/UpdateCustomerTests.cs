using Application.DTOs;
using Application.Interfaces;
using Application.UseCases;
using Domain.Entities;
using Moq;

namespace UnitTests.UseCases
{
    public class UpdateCustomerTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly UpdateCustomer _updateCustomer;

        public UpdateCustomerTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _updateCustomer = new UpdateCustomer(_customerRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldUpdateCustomer_WhenCustomerExistsAndCpfMatches()
        {
            // Arrange
            var customerDto = new CustomerDto
            {
                Id = 1,
                Name = "John Updated",
                Cpf = "12345678901",
                Email = "john.updated@example.com"
            };

            var existingCustomer = new Customer
            {
                Id = 1,
                Name = "John Doe",
                Cpf = "12345678901",
                Email = "john.doe@example.com"
            };

            _customerRepositoryMock
                .Setup(repo => repo.GetByCpfAsync(customerDto.Cpf))
                .ReturnsAsync(existingCustomer);

            // Act
            await _updateCustomer.ExecuteAsync(customerDto);

            // Assert
            _customerRepositoryMock.Verify(repo => repo.GetByCpfAsync(customerDto.Cpf), Times.Once);
            _customerRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Customer>(c =>
                c.Id == customerDto.Id &&
                c.Name == customerDto.Name &&
                c.Cpf == customerDto.Cpf &&
                c.Email == customerDto.Email
            )), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowException_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerDto = new CustomerDto
            {
                Id = 1,
                Name = "Jane Doe",
                Cpf = "98765432109",
                Email = "jane.doe@example.com"
            };

            _customerRepositoryMock
                .Setup(repo => repo.GetByCpfAsync(customerDto.Cpf))
                .ReturnsAsync((Customer?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _updateCustomer.ExecuteAsync(customerDto));
            Assert.Equal("Cliente não encontrado ou CPF não pode ser alterado.", exception.Message);

            _customerRepositoryMock.Verify(repo => repo.GetByCpfAsync(customerDto.Cpf), Times.Once);
            _customerRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Customer>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowException_WhenCpfBelongsToAnotherCustomer()
        {
            // Arrange
            var customerDto = new CustomerDto
            {
                Id = 2,
                Name = "Jane Doe",
                Cpf = "12345678901",
                Email = "jane.doe@example.com"
            };

            var existingCustomer = new Customer
            {
                Id = 1,
                Name = "John Doe",
                Cpf = "12345678901",
                Email = "john.doe@example.com"
            };

            _customerRepositoryMock
                .Setup(repo => repo.GetByCpfAsync(customerDto.Cpf))
                .ReturnsAsync(existingCustomer);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _updateCustomer.ExecuteAsync(customerDto));
            Assert.Equal("Cliente não encontrado ou CPF não pode ser alterado.", exception.Message);

            _customerRepositoryMock.Verify(repo => repo.GetByCpfAsync(customerDto.Cpf), Times.Once);
            _customerRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Customer>()), Times.Never);
        }
    }
}
