using Layout.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Layout.Models.ViewModels
{
    public class SolicitudCreateViewModel
    {
        [Required]
        public int AreaId { get; set; }

        public List<SelectListItem>? Areas { get; set; }

        [Required]
        public TipoMovimiento TipoMovimiento { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string Razon { get; set; }

        public IFormFile? Imagen { get; set; }
    }
}