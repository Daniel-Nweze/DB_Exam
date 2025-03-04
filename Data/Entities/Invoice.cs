namespace Data.Entities
{
    /// <summary>
    /// Representerar en faktura kopplad till ett projekt.
    /// </summary>
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    }
}
