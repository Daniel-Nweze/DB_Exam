using Data.Context;
using Data.Entities;
using Data.Enums;
using Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace TestProject.Repositories
{   
    /*
       Tester för Repository-metoder. 
       Testerna använder en InMemoryDatabase för att säkerställa att CRUD-operationerna fungerar korrekt mot databasen.
     */
    public class ProjectRepository_Tests
    {
        // Skapar en ny in-memory databas för varje test.
        private static AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }


        [Fact]
        public async Task AddProject__ShouldAddProject__To__InMemoryDataBase()
        {
            // Arrange 
            using var context = GetInMemoryDbContext();
            var repository = new BaseRepository<Project>(context);
            var project = new Project
            {
                ProjectName = "Test projekt",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Status = ProjectStatus.EjPåbörjad,
                CustomerId = 1,
                ProjectManagerId = 1,
                ServiceId = 1,
                TotalPrice = 5000M
            };

            await repository.AddAsync(project);

            // Act
            var projects = await repository.GetAllAsync();

            // Assert
            projects.Should().Contain(project).And.ContainSingle();
        }

        [Fact]
        public async Task GetProjectById__ShouldReturn__Correct__Project()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new BaseRepository<Project>(context);

            var project = new Project
            {
                ProjectName = "Test projekt",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Status = ProjectStatus.EjPåbörjad,
                CustomerId = 1,
                ProjectManagerId = 1,
                ServiceId = 1,
                TotalPrice = 5000M
            };

            await repository.AddAsync(project);
            // Act
            var result = await repository.GetByIdAsync(project.ProjectId);

            // Assert
            result?.ProjectName.Should().Be(project.ProjectName);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAllProjects_ShouldReturnListOfProjects()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new BaseRepository<Project>(context);

            var project1 = new Project
            {
                ProjectName = "Test projekt",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Status = ProjectStatus.EjPåbörjad,
                CustomerId = 1,
                ServiceId = 1,
                ProjectManagerId = 1,
                TotalPrice = 1000M
            };
            var project2 = new Project
            {
                ProjectName = "Test projekt 2",
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(10),
                Status = ProjectStatus.Avslutad,
                CustomerId = 1,
                ServiceId = 1,
                ProjectManagerId = 1,
                TotalPrice = 8000M
            };

            await repository.AddAsync(project1);
            await repository.AddAsync(project2);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
        }

        [Fact]
        public async Task UpdateProject__ShouldModifyExistingProject__Successfully()
        {
            // Arrange 
            using var context = GetInMemoryDbContext();
            var repository = new BaseRepository<Project>(context);

            var project = new Project
            {
                ProjectName = "Test projekt",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Status = ProjectStatus.EjPåbörjad,
                CustomerId = 1,
                ServiceId = 1,
                ProjectManagerId = 1,
                TotalPrice = 1000M
            };

            await repository.AddAsync(project);

            // Act
            project.ProjectName = "Uppdaterat namn";
            project.Status = ProjectStatus.Pågående;
            await repository.UpdateAsync(project);

            var updatedProject = await repository.GetByIdAsync(project.ProjectId);

            // Assert 
            updatedProject.Should().NotBeNull();
            updatedProject.ProjectName.Should().Be("Uppdaterat namn");
            updatedProject.Status.Should().Be(ProjectStatus.Pågående);
        }

        [Fact]
        public async Task DeleteProject__ShouldRemoveProject__Successfully()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new BaseRepository<Project>(context);

            var project = new Project
            {
                ProjectName = "Test projekt",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Status = ProjectStatus.EjPåbörjad,
                CustomerId = 1,
                ServiceId = 1,
                ProjectManagerId = 1,
                TotalPrice = 1000M
            };

            await repository.AddAsync(project);

            // Act          
            await repository.DeleteAsync(project);
            var projects = await repository.GetAllAsync();

            // Assert
            projects.Should().NotContain(p => p.ProjectId == project.ProjectId);

        }



    }

}
