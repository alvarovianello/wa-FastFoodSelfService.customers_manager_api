using Application.Interfaces;
using Application.UseCases;
using Domain.Entities;
using Moq;

namespace UnitTests.UseCases
{
    public class GetCustomerByCpfTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly GetCustomerByCpf _getCustomerByCpf;

        public GetCustomerByCpfTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _getCustomerByCpf = new GetCustomerByCpf(_customerRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnCustomer_WhenCpfExists()
        {
            // Arrange
            var cpf = "12345678901";
            var expectedCustomer = new Customer
            {
                Id = 1,
                Name = "John Doe",
                Cpf = cpf,
                Email = "john.doe@example.com"
            };

            _customerRepositoryMock
                .Setup(repo => repo.GetByCpfAsync(cpf))
                .ReturnsAsync(expectedCustomer);

            // Act
            var result = await _getCustomerByCpf.ExecuteAsync(cpf);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCustomer.Id, result.Id);
            Assert.Equal(expectedCustomer.Name, result.Name);
            Assert.Equal(expectedCustomer.Cpf, result.Cpf);
            Assert.Equal(expectedCustomer.Email, result.Email);

            _customerRepositoryMock.Verify(repo => repo.GetByCpfAsync(cpf), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnNull_WhenCpfDoesNotExist()
        {
            // Arrange
            var cpf = "98765432109";
            _customerRepositoryMock
                .Setup(repo => repo.GetByCpfAsync(cpf))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _getCustomerByCpf.ExecuteAsync(cpf);

            // Assert
            Assert.Null(result);

            _customerRepositoryMock.Verify(repo => repo.GetByCpfAsync(cpf), Times.Once);
        }
    }
}