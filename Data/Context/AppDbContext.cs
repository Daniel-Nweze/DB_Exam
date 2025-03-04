using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    /* Klassen hanterar anslutningen till databasen och skapar databastabeller av entiteterna som jag har kodat.
       Använder Entity Framework Core för att utföra CRUD-operationer mot databasen.
     */
    public class AppDbContext : DbContext
    {
        // Konstruktor som tar in konfigurationsalternativ t ex connection string.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Sätter varje entitet som ska lagras i databasen med DbSet
        public DbSet<Project> Projects { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ProjectManager> ProjectManagers { get; set; }
        public DbSet<Service> Services { get; set; }

        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<TimeLog> TimeLogs { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ProjectPhase> ProjectPhases { get; set; }
        public DbSet<Risk> Risks { get; set; }
        public DbSet<ProjectEmployee> ProjectEmployees { get; set; }

        // Konfiguration av modellen för specifika entiteter som sker innan programmet körs
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konvertera ProjectStatus från enum till string så att strängen visas i SSMS - Chatgpt genererad
            modelBuilder.Entity<Project>()
                .Property(p => p.Status)
                .HasConversion<string>();

            // Kopplade primära nycklar för en många-till-många relation.. en anställd kan vara kopplad till flera projekt och vice versa - Chatgpt genererad
            modelBuilder.Entity<ProjectEmployee>()
                .HasKey(pe => new { pe.ProjectId, pe.EmployeeId });

            // Employee - Department relation.. en department kan ha flera employees, men en employee har en department, alltså en-till-många relation - Chatgpt genererad
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId);

            // Projektansvariga kan inte ha samma e-post.. varje e-post är unik.
            modelBuilder.Entity<ProjectManager>()
                .HasIndex(pm => pm.Email)
                .IsUnique();

        }
    }
}
