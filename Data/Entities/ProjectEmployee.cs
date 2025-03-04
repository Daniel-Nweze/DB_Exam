namespace Data.Entities
{
    // Kopplingstabell mellan Project och Employee (många-till-många).

    public class ProjectEmployee
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
    }
}
