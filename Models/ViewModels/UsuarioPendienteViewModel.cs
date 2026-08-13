namespace Layout.Models.ViewModels
{
    public class UsuarioPendienteViewModel
    {
        public string Id { get; set; }

        public string NombreCompleto { get; set; }

        public string Email { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public string Estatus { get; set; }
    }
}