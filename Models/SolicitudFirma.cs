namespace Layout.Models
{
    public class SolicitudFirma
    {
        public int Id { get; set; }

        public int SolicitudId { get; set; }
        public SolicitudMovimiento Solicitud { get; set; }

        // Usuario que debe firmar
        public string UsuarioRequeridoId { get; set; }
        public ApplicationUser UsuarioRequerido { get; set; }

        // Qué firma representa
        public int TipoFirmaId { get; set; }
        public TipoFirma TipoFirma { get; set; }

        public bool Firmada { get; set; }

        public string? UsuarioFirmanteId { get; set; }
        public ApplicationUser? UsuarioFirmante { get; set; }

        public DateTime? FechaFirma { get; set; }

        public string? Comentarios { get; set; }
    }
}
