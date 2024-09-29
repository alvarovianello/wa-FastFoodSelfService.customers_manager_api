using Application.DTOs;
using Application.Interfaces;

namespace Application.UseCases
{
    public class GetCustomerById
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerById(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerDto?> ExecuteAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) return null;

            return new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Cpf = customer.Cpf,
                Email = customer.Email
            };
        }
    }
}
