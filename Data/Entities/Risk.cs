using System;

namespace Data.Entities
{
   
    public class Risk
    {
        public int RiskId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string MitigationPlan { get; set; } = string.Empty;
        public DateTime IdentifiedDate { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    }
}
