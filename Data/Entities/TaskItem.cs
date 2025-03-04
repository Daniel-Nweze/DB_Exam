namespace Data.Entities
{
    public class TaskItem
    {
        public int TaskItemId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }

        
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

    }
}
