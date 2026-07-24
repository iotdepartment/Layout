namespace Layout.Models
{
    public class Area
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int? AreaOrganizacionalId { get; set; }

        public AreaOrganizacional? AreaOrganizacional { get; set; }

        // Relaciones
        public ICollection<SolicitudMovimiento> Solicitudes { get; set; }

        public ICollection<UsuarioArea> Usuarios { get; set; }
            = new List<UsuarioArea>();
    }
}