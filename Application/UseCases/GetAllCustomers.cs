using Application.DTOs;
using Application.Interfaces;

namespace Application.UseCases
{
    public class GetAllCustomers
    {
        private readonly ICustomerRepository _customerRepository;

        public GetAllCustomers(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<CustomerDto>> ExecuteAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            return customers.Select(customer => new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Cpf = customer.Cpf,
                Email = customer.Email
            });
        }
    }
}
