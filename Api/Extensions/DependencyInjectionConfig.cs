using Application.Interfaces;
using Application.UseCases;
using Infrastructure.Repositories;

namespace Api.Extensions
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddResolveDependencies(this WebApplicationBuilder builder)
        {
            IServiceCollection services = builder.Services;

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
