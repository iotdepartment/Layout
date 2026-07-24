namespace Layout.Models
{
    public class ResponsableFirma
    {
        public int Id { get; set; }

        public int TipoFirmaId { get; set; }

        public TipoFirma TipoFirma { get; set; }

        public int? AreaOrganizacionalId { get; set; }

        public AreaOrganizacional? AreaOrganizacional { get; set; }

        public string UsuarioId { get; set; }

        public ApplicationUser Usuario { get; set; }
    }
}
