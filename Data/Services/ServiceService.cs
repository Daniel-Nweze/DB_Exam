using Data.Entities;
using Data.Interfaces;
using Data.Repositories;

namespace Data.Services
{
    public class ServiceService(IBaseRepository<Service> serviceRepository) : IServiceService
    {
        private readonly IBaseRepository<Service> _serviceRepository = serviceRepository;

        public async Task<IEnumerable<Service>> GetAllServicesAsync()
        {
            return await _serviceRepository.GetAllAsync();
        }

        public async Task<Service?> GetServiceByIdAsync(int id)
        {
            return await _serviceRepository.GetByIdAsync(id);
        }

        public async Task CreateServiceAsync(Service service)
        {
            await _serviceRepository.AddAsync(service);
        }

        public async Task UpdateServiceAsync(Service service)
        {
            await _serviceRepository.UpdateAsync(service);
        }

        public async Task DeleteServiceAsync(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Tjänst med ID {id} hittades inte.");

            await _serviceRepository.DeleteAsync(service);
        }
    }
}
    