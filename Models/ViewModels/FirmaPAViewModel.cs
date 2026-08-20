namespace Layout.Models.ViewModels
{
    public class FirmaPAViewModel
    {
        public int TipoFirmaId { get; set; }

        public string TipoFirma { get; set; }

        public string? UsuarioPAId { get; set; }

        public string? UsuarioPA { get; set; }

        public bool Activo { get; set; }
    }
}
