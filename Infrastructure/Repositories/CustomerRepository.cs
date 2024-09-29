using Microsoft.Extensions.Configuration;
using Npgsql;
using Dapper;
using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IConfiguration _configuration;

        public CustomerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task AddAsync(Customer customer)
        {
            var query = "INSERT INTO dbo.Customer (name, cpf, email) VALUES (@Name, @Cpf, @Email)";
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(query, new { customer.Name, customer.Cpf, customer.Email });
            }
        }

        public async Task UpdateAsync(Customer customer)
        {
            var query = "UPDATE dbo.Customer SET name = @Name, cpf = @Cpf, email = @Email WHERE id = @Id";
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(query, new { customer.Name, customer.Cpf, customer.Email, customer.Id });
            }
        }

        public async Task<Customer?> GetByCpfAsync(string cpf)
        {
            var query = "SELECT * FROM dbo.Customer WHERE cpf = @Cpf";
            using (var connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { Cpf = cpf });
            }
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM dbo.Customer WHERE id = @Id";
            using (var connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { Id = id });
            }
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var query = "SELECT * FROM dbo.Customer";
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<Customer>(query);
            }
        }

        public async Task RemoveAsync(int id)
        {
            var query = "DELETE FROM dbo.Customer WHERE id = @Id";
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
