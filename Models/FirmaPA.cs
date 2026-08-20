namespace Layout.Models
{
    public class FirmaPA
    {
        public int Id { get; set; }

        // Dueño original de la firma
        public string UsuarioTitularId { get; set; }
        public ApplicationUser UsuarioTitular { get; set; }

        // Usuario autorizado como PA
        public string UsuarioPAId { get; set; }
        public ApplicationUser UsuarioPA { get; set; }

        // Tipo de firma delegada
        public int TipoFirmaId { get; set; }
        public TipoFirma TipoFirma { get; set; }

        public string MotivoAsignacion { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaAsignacion { get; set; } =
            DateTime.Now;
    }
}
