using Layout.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace Layout.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; }

        public bool Activo { get; set; }

        public EstatusUsuario EstatusUsuario { get; set; }

        public DateTime FechaSolicitudAcceso { get; set; }
        public ICollection<UsuarioArea> Areas
        { get; set; } = new List<UsuarioArea>();

        public ICollection<UsuarioTipoFirma> TiposFirma
        { get; set; } = new List<UsuarioTipoFirma>();
    }
}