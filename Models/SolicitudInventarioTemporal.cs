namespace Layout.Models
{
    public class SolicitudInventarioTemporal
    {
        public int Id { get; set; }

        public int SolicitudId { get; set; }
        public SolicitudMovimiento Solicitud { get; set; }

        // ✅ 1. Número de validación
        public bool AplicaValidacion { get; set; }
        public string? NumeroValidacion { get; set; }

        // ✅ 2. Responsable inventario
        public bool AplicaResponsable { get; set; }
        public string? ResponsableInventario { get; set; }

        // ✅ 3. Mandril / Kanban / NP
        public bool AplicaMandril { get; set; }
        public string? MandrilKanbanNP { get; set; }

        // ✅ 4. Número de pallets
        public bool AplicaPallets { get; set; }
        public int? NumeroPallets { get; set; }

        // ✅ 5. Razón inventario
        public bool AplicaRazon { get; set; }
        public string? RazonInventario { get; set; }

        // ✅ 6. Fecha compromiso
        public bool AplicaFecha { get; set; }
        public DateTime? FechaCompromiso { get; set; }
    }
}
