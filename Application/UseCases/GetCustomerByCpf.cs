using Application.DTOs;
using Application.Interfaces;

namespace Application.UseCases
{
    public class GetCustomerByCpf
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByCpf(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerDto?> ExecuteAsync(string cpf)
        {
            var customer = await _customerRepository.GetByCpfAsync(cpf);
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
