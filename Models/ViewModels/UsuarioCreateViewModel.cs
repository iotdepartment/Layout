
    using Microsoft.AspNetCore.Mvc.Rendering;

    namespace Layout.Models.ViewModels
    {
        public class UsuarioCreateViewModel
        {
            public string NombreCompleto { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }

            public string Rol { get; set; }
        public List<int> AreasSeleccionadas { get; set; }
            = new();
        public int? TipoFirmaId { get; set; }

            public List<SelectListItem>? Areas { get; set; }

            public List<SelectListItem>? TiposFirma { get; set; }

            public List<SelectListItem>? Roles { get; set; }

        }
    }

