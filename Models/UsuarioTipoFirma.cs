namespace Layout.Models
{
    public class UsuarioTipoFirma
    {
        public int Id { get; set; }

        public string UsuarioId { get; set; }

        public ApplicationUser Usuario { get; set; }

        public int TipoFirmaId { get; set; }

        public TipoFirma TipoFirma { get; set; }
    }
}