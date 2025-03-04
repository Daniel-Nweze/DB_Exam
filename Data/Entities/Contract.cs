namespace Data.Entities
{
    public class Contract
    {
        public int ContractId { get; set; }
        public string ContractNumber { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal ContractValue { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    }
}
