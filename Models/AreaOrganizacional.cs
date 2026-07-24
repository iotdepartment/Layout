namespace Layout.Models
{
    public class AreaOrganizacional
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string? Descripcion { get; set; }

        public ICollection<Area> Areas { get; set; }
            = new List<Area>();
    }
}