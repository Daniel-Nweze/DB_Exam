using Data.Entities;

namespace Data.Interfaces
{
    public interface IProjectManagerService
    {
        Task<IEnumerable<ProjectManager>> GetAllProjectManagersAsync();
        Task<ProjectManager?> GetProjectManagerByIdAsync(int id);
        Task CreateProjectManagerAsync(ProjectManager projectManager);
        Task UpdateProjectManagerAsync(ProjectManager projectManager);
        Task DeleteProjectManagerAsync(int id);
    }
}