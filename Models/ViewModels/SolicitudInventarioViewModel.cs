namespace Layout.Models.ViewModels
{
    public class SolicitudInventarioViewModel
    {
        public int SolicitudId { get; set; }

        // 🟦 INVENTARIO

        public bool AplicaValidacion { get; set; }
        public string? NumeroValidacion { get; set; }

        public bool AplicaResponsable { get; set; }
        public string? ResponsableInventario { get; set; }

        public bool AplicaMandril { get; set; }
        public string? MandrilKanbanNP { get; set; }

        public bool AplicaPallets { get; set; }
        public int? NumeroPallets { get; set; }

        public bool AplicaRazon { get; set; }
        public string? RazonInventario { get; set; }

        public bool AplicaFecha { get; set; }
        public DateTime? FechaCompromiso { get; set; }

        // 🟨 MOVIMIENTOS TÉCNICOS (estos están bien ✅)

        public bool MovimientoRed { get; set; }
        public bool MovimientoIoT { get; set; }
        public bool MovimientoProgramacion { get; set; }
        public bool MovimientoElectrico { get; set; }
        public bool MovimientoEHS { get; set; }
        public bool CambioNomenclatura { get; set; }
        public bool RequierePCR { get; set; }
    }
}
