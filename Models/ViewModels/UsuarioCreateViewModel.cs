using Microsoft.AspNetCore.Mvc.Rendering;

namespace Layout.Models.ViewModels
{
    public class UsuarioCreateViewModel
    {
        public string NombreCompleto { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        // Rol del sistema
        public string Rol { get; set; }

        // Tipo de firma del usuario
        public int? TipoFirmaId { get; set; }

        // Áreas asignadas al usuario
        public List<int> AreasSeleccionadas { get; set; }
            = new();

        // Combos
        public List<SelectListItem>? Areas { get; set; }

        public List<SelectListItem>? TiposFirma { get; set; }

        public List<SelectListItem>? Roles { get; set; }
    }
}