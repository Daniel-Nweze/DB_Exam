using Data.Entities;
using Data.Interfaces;
using Data.Repositories;

namespace Data.Services
{
    public class CustomerService(IRepository<Customer> customerRepository) : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository = customerRepository;

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task CreateCustomerAsync(Customer customer)
        {
            await _customerRepository.AddAsync(customer);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            await _customerRepository.UpdateAsync(customer);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id) 
                ?? throw new KeyNotFoundException($"Kund med id {id} hittades inte.");
            await _customerRepository.DeleteAsync(customer);
        }
    }
}

