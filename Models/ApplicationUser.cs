using Microsoft.AspNetCore.Identity;

namespace Layout.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; }
        public bool Activo { get; set; } = true;
    }
}