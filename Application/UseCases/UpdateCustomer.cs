using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases
{
    public class UpdateCustomer
    {
        private readonly ICustomerRepository _customerRepository;

        public UpdateCustomer(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task ExecuteAsync(CustomerDto customerDto)
        {
            var existingCustomer = await _customerRepository.GetByCpfAsync(customerDto.Cpf);
            if (existingCustomer == null || existingCustomer.Id != customerDto.Id)
            {
                throw new Exception("Cliente não encontrado ou CPF não pode ser alterado.");
            }

            var customer = new Customer
            {
                Id = customerDto.Id,
                Name = customerDto.Name,
                Cpf = customerDto.Cpf,
                Email = customerDto.Email
            };

            await _customerRepository.UpdateAsync(customer);
        }
    }
}
