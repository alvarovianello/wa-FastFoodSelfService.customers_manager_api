using Application.Interfaces;
using Application.UseCases;
using Infrastructure.Configurations.Database;
using Infrastructure.Data.Initializer;
using Infrastructure.Repositories;

namespace Api.Extensions
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddResolveDependencies(this WebApplicationBuilder builder)
        {
            return AddResolveDependencies(builder.Services, builder.Configuration);
        }

        public static IServiceCollection AddResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {          
            var dbType = configuration["DatabaseType"];

            if (dbType == "SQLite")
            {
                services.AddSingleton<IDbConnectionFactory, SQLiteConnectionFactory>();
            }
            else
            {
                services.AddSingleton<IDbConnectionFactory, PostgreSqlConnectionFactory>();
            }

            // Inicializador do banco de dados
            services.AddSingleton<DatabaseInitializer>();

            //Customer
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<RegisterCustomer>();
            services.AddScoped<UpdateCustomer>();
            services.AddScoped<GetAllCustomers>();
            services.AddScoped<GetCustomerByCpf>();
            services.AddScoped<GetCustomerById>();
            services.AddScoped<RemoveCustomer>();

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddHttpClient();

            return services;
        }
    }
}
