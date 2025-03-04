namespace Data.Entities
{

    public class TimeLog
    {
        public int TimeLogId { get; set; }
        public DateTime LogDate { get; set; }
        public decimal Hours { get; set; }
        public string Description { get; set; } = string.Empty;

        
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    }
}
