using System.ComponentModel.DataAnnotations;

namespace Layout.Models.Enums
{
    public enum TipoMovimiento
    {
        [Display(Name = "Movimiento Temporal")]
        Temporal = 1,

        [Display(Name = "Movimiento Definitivo")]
        Definitivo = 2
    }
}