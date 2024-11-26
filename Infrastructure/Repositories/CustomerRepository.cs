using Application.Interfaces;
using Dapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly string _tablePrefix;

        public CustomerRepository(IDbConnectionFactory connectionFactory, IConfiguration configuration)
        {
            _connectionFactory = connectionFactory;
            var dbType = configuration["DatabaseType"];
            _tablePrefix = dbType == "Postgres" ? "dbo." : "";
        }

        public async Task AddAsync(Customer customer)
        {
            var query = $"INSERT INTO {_tablePrefix}Customer (name, cpf, email) VALUES (@Name, @Cpf, @Email)";
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { customer.Name, customer.Cpf, customer.Email });
            }
        }

        public async Task UpdateAsync(Customer customer)
        {
            var query = $"UPDATE {_tablePrefix}Customer SET name = @Name, cpf = @Cpf, email = @Email WHERE id = @Id";
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { customer.Name, customer.Cpf, customer.Email, customer.Id });
            }
        }

        public async Task<Customer?> GetByCpfAsync(string cpf)
        {
            var query = $"SELECT * FROM {_tablePrefix}Customer WHERE cpf = @Cpf";
            using (var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { Cpf = cpf });
            }
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            var query = $"SELECT * FROM {_tablePrefix}Customer WHERE id = @Id";
            using (var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { Id = id });
            }
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var query = $"SELECT * FROM {_tablePrefix}Customer";
            using (var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Customer>(query);
            }
        }

        public async Task RemoveAsync(int id)
        {
            var query = $"DELETE FROM {_tablePrefix}Customer WHERE id = @Id";
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
