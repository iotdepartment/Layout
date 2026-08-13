using Microsoft.AspNetCore.Mvc.Rendering;

namespace Layout.Models.ViewModels
{
    public class AprobarUsuarioViewModel
    {
        public string UsuarioId { get; set; }

        public string NombreCompleto { get; set; }

        public string Email { get; set; }

        public string RolSeleccionado { get; set; }

        public List<SelectListItem> Roles { get; set; }
            = new();

        public List<SelectListItem> Areas { get; set; }
            = new();

        public List<SelectListItem> TiposFirma { get; set; }
            = new();

        public List<int> AreasSeleccionadas { get; set; }
            = new();

        public List<int> TiposFirmaSeleccionados { get; set; }
            = new();
    }
}