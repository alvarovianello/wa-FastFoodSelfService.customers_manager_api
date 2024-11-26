using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases
{
    public class RegisterCustomer
    {
        private readonly ICustomerRepository _customerRepository;

        public RegisterCustomer(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task ExecuteAsync(CustomerDto customerDto)
        {
            var existingCustomer = await _customerRepository.GetByCpfAsync(customerDto.Cpf);
            if (existingCustomer != null)
            {
                throw new InvalidOperationException("O CPF informado já possui cadastro");
            }

            var customer = new Customer
            {
                Name = customerDto.Name,
                Cpf = customerDto.Cpf,
                Email = customerDto.Email
            };

            await _customerRepository.AddAsync(customer);
        }
    }
}
