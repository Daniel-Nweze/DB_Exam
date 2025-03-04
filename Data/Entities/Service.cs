namespace Data.Entities
{
    /* 
      Entitetsklass som representerar tjänsten kopplad till ett projekt. 
      Klassen innehåller egenskaper för tjänstens id, beskrivning och pris.
    */
    public class Service
    {
        public int ServiceId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // En samling av projekt där tjänsten i fråga används.
        // Varje tjänst kan alltså vara kopplad till flera projekt, och denna samling innehåller alla dessa kopplingar.
        public ICollection<Project> Projects { get; set; } = [];
    }
}
