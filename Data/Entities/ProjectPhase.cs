using System;

namespace Data.Entities
{
    public class ProjectPhase
    {
        public int ProjectPhaseId { get; set; }
        public string PhaseName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    }
}
