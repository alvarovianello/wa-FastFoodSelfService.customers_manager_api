using Application.Interfaces;
using Domain.Entities;
using IntegrationTests.TestFixtures;

namespace IntegrationTests.Repositories
{
    public class CustomerRepositoryTests : IClassFixture<CustomerRepositoryFixture> 
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDbConnectionFactory _connectionFactory;

        public CustomerRepositoryTests(CustomerRepositoryFixture fixture)
        {
            _connectionFactory = fixture.ConnectionFactory;
            _customerRepository = fixture.CustomerRepository;
        }

        [Fact]
        public async Task AddAsync_ShouldAddCustomer()
        {
            // Criação de um cliente
            var customer = new Customer { Name = "John Doe", Cpf = "12345678901", Email = "johndoe@example.com" };

            // Adiciona o cliente
            await _customerRepository.AddAsync(customer);

            // Verifica se o cliente foi adicionado corretamente
            var addedCustomer = await _customerRepository.GetByCpfAsync("12345678901");
            Assert.NotNull(addedCustomer);
            Assert.Equal("John Doe", addedCustomer?.Name);
            Assert.Equal("12345678901", addedCustomer?.Cpf);
            Assert.Equal("johndoe@example.com", addedCustomer?.Email);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCustomer()
        {
            // Cria um cliente para testes
            var customer = new Customer { Name = "Jane Doe", Cpf = "98765432100", Email = "janedoe@example.com" };
            await _customerRepository.AddAsync(customer);

            // Recupera o cliente pelo Cpf
            var retrievedCustomer = await _customerRepository.GetByCpfAsync("98765432100");

            Assert.NotNull(retrievedCustomer);
            Assert.Equal(customer.Email, retrievedCustomer?.Email);
            Assert.Equal(customer.Name, retrievedCustomer?.Name);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCustomers()
        {
            // Cria e adiciona múltiplos clientes
            var customer1 = new Customer { Name = "Alice", Cpf = "11111111111", Email = "alice@example.com" };
            var customer2 = new Customer { Name = "Bob", Cpf = "22222222222", Email = "bob@example.com" };

            await _customerRepository.AddAsync(customer1);
            await _customerRepository.AddAsync(customer2);

            // Recupera todos os clientes
            var customers = await _customerRepository.GetAllAsync();

            Assert.Contains(customers, c => c.Name == "Alice");
            Assert.Contains(customers, c => c.Name == "Bob");
        }

        [Fact]
        public async Task RemoveAsync_ShouldRemoveCustomer()
        {
            // Cria um cliente
            var customer = new Customer { Name = "Charlie", Cpf = "33333333333", Email = "charlie@example.com" };
            await _customerRepository.AddAsync(customer);

            // Remove o cliente
            await _customerRepository.RemoveAsync(customer.Id);

            // Verifica se o cliente foi removido
            var removedCustomer = await _customerRepository.GetByIdAsync(customer.Id);
            Assert.Null(removedCustomer);
        }
    }
}
