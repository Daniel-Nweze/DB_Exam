using Data.Entities;
using Data.Interfaces;
using Data.Repositories;

namespace Data.Services
{
    public class ProjectManagerService(IBaseRepository<ProjectManager> projectManagerRepository) : IProjectManagerService
    {
        private readonly IBaseRepository<ProjectManager> _projectManagerRepository = projectManagerRepository;

        public async Task<IEnumerable<ProjectManager>> GetAllProjectManagersAsync()
        {
            return await _projectManagerRepository.GetAllAsync();
        }

        public async Task<ProjectManager?> GetProjectManagerByIdAsync(int id)   
        {
            return await _projectManagerRepository.GetByIdAsync(id);
        }

        public async Task CreateProjectManagerAsync(ProjectManager projectManager)
        {
            await _projectManagerRepository.AddAsync(projectManager);
        }

        public async Task UpdateProjectManagerAsync(ProjectManager projectManager)
        {
            await _projectManagerRepository.UpdateAsync(projectManager);
        }

        public async Task DeleteProjectManagerAsync(int id)
        {
            var projectManager = await _projectManagerRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Projektansvarig med ID {id} hittades inte.");

            await _projectManagerRepository.DeleteAsync(projectManager);
        }
    }
}
