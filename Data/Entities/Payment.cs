namespace Data.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }

        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;
    }
}
