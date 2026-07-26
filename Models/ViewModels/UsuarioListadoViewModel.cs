namespace Layout.Models.ViewModels
{
    public class UsuarioListadoViewModel
    {
        public string Id { get; set; }

        public string NombreCompleto { get; set; }

        public string Email { get; set; }

        public bool Activo { get; set; }

        public string Rol { get; set; }

        public List<string> Areas { get; set; }
    = new();

        public List<string> TiposFirma { get; set; }
            = new();
    }
}