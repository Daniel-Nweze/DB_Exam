using Data.Enums;

namespace Data.Entities
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public int ProjectManagerId { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public decimal TotalPrice { get; set; }
        public ProjectStatus Status { get; set; }

        public Customer Customer { get; set; } = null!;
        public ProjectManager ProjectManager { get; set; } = null!;
        public Service Service { get; set; } = null!;
    }
}
