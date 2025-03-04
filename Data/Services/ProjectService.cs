using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    public class ProjectService(IRepository<Project> projectRepository) : IProjectService
    {
        private readonly IRepository<Project> _projectRepository = projectRepository;

        public async Task CreateProjectAsync(Project project)
        {
            await _projectRepository.AddAsync(project);
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _projectRepository.GetAllAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _projectRepository.GetByIdAsync(id);
        }

        public async Task UpdateProjectAsync(Project project)
        {
            await _projectRepository.UpdateAsync(project);
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Projekt med ID {id} hittades inte.");
            await _projectRepository.DeleteAsync(project);
        }
    }
}

