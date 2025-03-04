namespace Data.Entities
{
    /*
       Entitetsklass som representerar en projektansvarig.
       Klassen innehåller egenskaper för en projektansvarigs id, fullständiga namn och e-postadress.
     */
    public class ProjectManager
    {
        public int ProjectManagerId { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; } = string.Empty;

        // En samling av projekt som en projektansvarig ansvarar för.
        // En projektansvarig kan ansvara för flera projekt, och denna samling innehåller alla dessa relationer.
        public ICollection<Project> Projects { get; set; } = [];
    }
}
