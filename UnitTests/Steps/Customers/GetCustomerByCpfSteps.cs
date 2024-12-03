using Application.DTOs;
using Application.Interfaces;
using Application.UseCases;
using Domain.Entities;
using Moq;
using TechTalk.SpecFlow;

namespace UnitTests.Steps.Customers
{
    [Binding]
    public class GetCustomerByCpfSteps
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly GetCustomerByCpf _getCustomerByCpf;
        private CustomerDto? _result;
        private string _cpf;

        public GetCustomerByCpfSteps()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _getCustomerByCpf = new GetCustomerByCpf(_customerRepositoryMock.Object);
        }

        [Given(@"um cliente com CPF ""([^""]*)"" existe no sistema")]
        public void GivenUmClienteComCPFExisteNoSistema(string cpf)
        {
            _cpf = cpf;
            var customer = new Customer
            {
                Id = 1,
                Name = "Álvaro Oliveira",
                Cpf = cpf,
                Email = "alvaro@email.com"
            };

            _customerRepositoryMock
                .Setup(repo => repo.GetByCpfAsync(cpf))
                .ReturnsAsync(customer);
        }

        [Given(@"nenhum cliente com CPF ""([^""]*)"" existe no sistema")]
        public void GivenNenhumClienteComCPFExisteNoSistema(string cpf)
        {
            _cpf = cpf;

            _customerRepositoryMock
                .Setup(repo => repo.GetByCpfAsync(cpf))
                .ReturnsAsync((Customer?)null);
        }

        [When(@"eu buscar pelo CPF ""([^""]*)""")]
        public async Task WhenEuBuscarPeloCPF(string cpf)
        {
           _result = await _getCustomerByCpf.ExecuteAsync(cpf);
        }

        [Then(@"o cliente retornado deve ter o nome ""([^""]*)""")]
        public void ThenOClienteRetornadoDeveTerONome(string expectedName)
        {
            Assert.NotNull(_result);
            Assert.Equal(expectedName, _result!.Name);
        }

        [Then(@"nenhum cliente deve ser retornado")]
        public void ThenNenhumClienteDeveSerRetornado()
        {
            Assert.Null(_result);
        }
    }
}