using Microsoft.AspNetCore.Identity;

namespace Layout.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; }

        public bool Activo { get; set; } = true;
        public ICollection<UsuarioArea> Areas { get; set; }
    = new List<UsuarioArea>();      

        // Tipo de firma que puede realizar
        public int? TipoFirmaId { get; set; }
        public TipoFirma? TipoFirma { get; set; }
    }
}