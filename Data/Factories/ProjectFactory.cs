using Data.Entities;
using Data.Enums;

namespace Data.Factories
{
    /* Klassen skapar instanser av Project klassen med alla nödvändiga värden.
     * Factories används för att: 
     * - Centralisera koden, 
     * - Förenkla testning, 
     * - Förenkla ändringar, 
     * - Undviker upprepning av instansiering 
     * - Minskar felrisk t ex om en ny parameter läggs till i Project-klassen.
     */
    public class ProjectFactory
    {
        public static Project CreateProject(
            string projectName,
            DateTime startDate,
            DateTime endDate,
            decimal totalPrice,
            Customer customer,
            ProjectManager projectManager,
            Service service,
            ProjectStatus status 
            )
        {
            return new Project
            {
                ProjectName = projectName,
                StartDate = startDate,
                EndDate = endDate,
                Status = status,
                TotalPrice = totalPrice,
                Customer = customer,
                ProjectManager = projectManager,
                Service = service,


            };
        }

    }
}
