namespace Layout.Models.ViewModels
{
    public class SolicitudAccesoViewModel
    {
        public int Id { get; set; }

        public string UsuarioId { get; set; }

        public string NombreCompleto { get; set; }

        public string Email { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public bool Aprobada { get; set; }

        public string? UsuarioAprobador { get; set; }

        public DateTime? FechaAprobacion { get; set; }

        public string? Comentarios { get; set; }
    }
}