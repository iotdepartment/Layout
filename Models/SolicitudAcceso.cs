namespace Layout.Models
{
    public class SolicitudAcceso
    {
        public int Id { get; set; }

        public string UsuarioId { get; set; }

        public ApplicationUser Usuario { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public bool Aprobada { get; set; }

        public string? UsuarioAprobadorId { get; set; }

        public DateTime? FechaAprobacion { get; set; }
    }
}
