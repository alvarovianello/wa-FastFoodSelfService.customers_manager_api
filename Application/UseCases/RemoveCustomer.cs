using Application.Interfaces;

namespace Application.UseCases
{
    public class RemoveCustomer
    {
        private readonly ICustomerRepository _customerRepository;

        public RemoveCustomer(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task ExecuteAsync(int id)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(id);
            if (existingCustomer == null)
            {
                throw new Exception("Cliente não encontrado.");
            }

            await _customerRepository.RemoveAsync(id);
        }
    }
}
