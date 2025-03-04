namespace Data.Entities
{
    /*                  
        Entitetsklass med modellering för en företagskund. 
        Klassen innehåller egenskaper för kundens id, namn och kontaktinformation.
     */
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;

        // En samling av projekt som kunden är kopplad till.
        // Varje kund kan vara kopplad till flera projekt, och denna samling innehåller alla dessa kopplingar.
        public ICollection<Project> Projects { get; set; } = [];
    }
}