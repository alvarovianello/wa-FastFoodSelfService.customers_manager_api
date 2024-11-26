using Application.DTOs;
using Application.Interfaces;
using Application.UseCases;
using Domain.Entities;
using Moq;
using TechTalk.SpecFlow;

namespace UnitTests.Steps.Customers
{
    [Binding]
    public class GetAllCustomersSteps
    {
        private readonly Mock<ICustomerRepository> _mockRepository;
        private readonly GetAllCustomers _useCase;
        private List<Customer> _customerList;
        private IEnumerable<CustomerDto> _result;

        public GetAllCustomersSteps()
        {
            _mockRepository = new Mock<ICustomerRepository>();
            _useCase = new GetAllCustomers(_mockRepository.Object);
        }

        [Given(@"que a API tem os seguintes clientes")]
        public void GivenTheAPIHasTheFollowingCustomers(Table table)
        {
            _customerList = table.Rows.Select(row => new Customer
            {
                Id = int.Parse(row["Id"]),
                Name = row["Name"],
                Cpf = row["Cpf"],
                Email = row["Email"]
            }).ToList();

            _mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(_customerList);
        }

        [When(@"solicito a todos os clientes")]
        public async Task WhenIRequestAllCustomers()
        {
            _result = await _useCase.ExecuteAsync();
        }

        [Then(@"a resposta deve conter (.*) clientes")]
        public void ThenTheResponseShouldContainCustomers(int count)
        {
            Assert.NotNull(_result);
            Assert.Equal(count, _result.Count());
        }

        [Then(@"a resposta deverá incluir um cliente com Cpf ""(.*)""")]
        public void ThenTheResponseShouldIncludeACustomerWithCpf(string cpf)
        {
            Assert.Contains(_result, customer => customer.Cpf == cpf);
        }
    }
}
