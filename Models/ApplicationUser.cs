using Microsoft.AspNetCore.Identity;

namespace Layout.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; }

        public bool Activo { get; set; } = true;

        // Área a la que pertenece
        public int? AreaId { get; set; }
        public Area? Area { get; set; }

        // Tipo de firma que puede realizar
        public int? TipoFirmaId { get; set; }
        public TipoFirma? TipoFirma { get; set; }
    }
}