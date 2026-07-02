using Layout.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Layout.Models.ViewModels
{

    //ESTE VIEWMODEL ES EL QUE CONTIENE O RECIBE, MALEA LA INFORMACIÓN QUE EL USUARIO REQUISITOR LLENA PARA CREAR LA SOLICITUD DE MOVIMIENTO
    public class SolicitudCreateViewModel
    {
        // ========================
        // DATOS BASE
        // ========================

        [Required]
        public int AreaId { get; set; }

        public List<SelectListItem>? Areas { get; set; }

        [Required]
        public TipoMovimiento TipoMovimiento { get; set; }

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public string Razon { get; set; } = string.Empty;

        public DateTime? FechaInicioMovimiento { get; set; }

        public DateTime? FechaFinMovimiento { get; set; }

        public IFormFile? Imagen { get; set; }

        // ========================
        // 🟦 INVENTARIO (MOVIDO AQUÍ ✅)
        // ========================

        public bool AplicaValidacion { get; set; }
        public string? NumeroValidacion { get; set; }

        public bool AplicaResponsable { get; set; }
        public string? ResponsableInventario { get; set; }

        public bool AplicaMandril { get; set; }
        public string? MandrilKanbanNP { get; set; }

        public bool AplicaPallets { get; set; }
        public int? NumeroPallets { get; set; }

        public bool AplicaRazonInventario { get; set; }
        public string? RazonInventario { get; set; }

      
    }
}