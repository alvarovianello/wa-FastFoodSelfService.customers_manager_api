using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task<Customer?> GetByCpfAsync(string cpf);
        Task<Customer?> GetByIdAsync(int id);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task RemoveAsync(int id);
    }
}
