using Application.Interfaces;
using Dapper;
using Infrastructure.Data.Initializer.Helpers;

namespace Infrastructure.Data.Initializer
{
    public class DatabaseInitializer
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DatabaseInitializer(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void InitializeDatabase()
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {

                        // Verifica se o esquema 'dbo' existe, e o cria caso não exista
                        var createSchemaSql = "CREATE SCHEMA IF NOT EXISTS dbo;";
                        connection.Execute(createSchemaSql, transaction: transaction);

                        // Verifica se a tabela existe
                        var tableExists = DatabaseHelper.TableExists(connection, "customer", transaction);

                        if (!tableExists)
                        {
                            // Comando SQL para criar a tabela
                            var createTableSql = @"
                        CREATE TABLE dbo.Customer (
                            id SERIAL PRIMARY KEY,
                            name VARCHAR(100),
                            email VARCHAR(100) UNIQUE,
                            cpf VARCHAR(11) UNIQUE
                        );";

                            // Executa o comando de criação da tabela
                            connection.Execute(createTableSql, transaction: transaction);

                            // Comando SQL para inserir dados iniciais
                            var seedDataSql = @"
                        INSERT INTO dbo.Customer (name, email, cpf) VALUES
                        ('Cliente Anônimo', 'anonimo@email.com', '12345678909'),
                        ('Álvaro da Silva Oliveira', 'alvaro@email.com', '86112631032'),
                        ('William Alves Marques', 'william@email.com', '91576385000');";

                            // Executa o comando de inserção de dados
                            connection.Execute(seedDataSql, transaction: transaction);
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

    }
}
