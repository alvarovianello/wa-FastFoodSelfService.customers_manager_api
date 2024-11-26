using Api.Extensions;
using Application.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.TestFixtures
{
    public class CustomerRepositoryFixture : IDisposable
    {
        public IDbConnectionFactory ConnectionFactory { get; }
        public ICustomerRepository CustomerRepository { get; }

        public CustomerRepositoryFixture()
        {
            SQLitePCL.Batteries.Init();

            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string?>("DatabaseType", "SQLite"),
                    new KeyValuePair<string, string?>("ConnectionStrings:DefaultConnection", "DataSource=./mydatabase.db")
                })
                .Build();

            services.AddSingleton<IConfiguration>(configuration);
            services.AddResolveDependencies(configuration);

            var serviceProvider = services.BuildServiceProvider();
            ConnectionFactory = serviceProvider.GetRequiredService<IDbConnectionFactory>();
            CustomerRepository = serviceProvider.GetRequiredService<ICustomerRepository>();

            // Criar a tabela no banco de dados em memória
            using (var connection = ConnectionFactory.CreateConnection())
            {
                var query = @"
                        CREATE TABLE IF NOT EXISTS Customer (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name VARCHAR(100) NOT NULL,
                            Cpf VARCHAR(11) NOT NULL,
                            Email VARCHAR(100)
                        );
                    ";
                connection.ExecuteAsync(query).Wait();
            }
        }

        public void Dispose()
        {
            // Limpeza de recursos, se necessário
        }
    }
}
