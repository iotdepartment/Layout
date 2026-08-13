using System.ComponentModel.DataAnnotations;

namespace Layout.Models.ViewModels
{
    public class RechazarUsuarioViewModel
    {
        public string UsuarioId { get; set; }

        [Required]
        public string Motivo { get; set; }
    }
}