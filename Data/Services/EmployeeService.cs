//using Data.Entities;
//using Data.Interfaces;
//using Data.Repositories;

//namespace Data.Services
//{
//    public class EmployeeService(IRepository<Employee> employeeRepository) : IEmployeeService
//    {
//        private readonly IRepository<Employee> _employeeRepository = employeeRepository;

//        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
//        {
//            return await _employeeRepository.GetAllAsync();
//        }

//        public async Task<Employee?> GetEmployeeByIdAsync(int id)
//        {
//            return await _employeeRepository.GetByIdAsync(id);
//        }

//        public async Task CreateEmployeeAsync(Employee employee)
//        {
//            await _employeeRepository.AddAsync(employee);
//        }

//        public async Task UpdateEmployeeAsync(Employee employee)
//        {
//            await _employeeRepository.UpdateAsync(employee);
//        }

//        public async Task DeleteEmployeeAsync(int id)
//        {
//            var employee = await _employeeRepository.GetByIdAsync(id)
//                ?? throw new KeyNotFoundException($"Anställd med ID {id} hittades inte.");

//            await _employeeRepository.DeleteAsync(employee);
//        }
//    }
//}
