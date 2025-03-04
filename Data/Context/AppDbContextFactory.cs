using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Data.Context;
using System;
using Data.Entities;

namespace Data.Context
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                optionsBuilder.UseSqlServer("Data Source = localhost; Initial Catalog = dbExam; Integrated Security = True; Pooling = False; Encrypt = True; Trust Server Certificate = True");


                return new AppDbContext(optionsBuilder.Options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid skapande av DbContext: {ex.Message}");
                return null!;
            }
        }
    }
}
