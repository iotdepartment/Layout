using System.ComponentModel.DataAnnotations;

namespace Layout.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirmar Contraseña")]
        public string ConfirmPassword { get; set; }
    }
}