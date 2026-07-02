namespace Layout.Models
{
    public class SolicitudInventarioTemporal
    {
        public int Id { get; set; }

        public int SolicitudId { get; set; }
        public SolicitudMovimiento Solicitud { get; set; }

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
    }
}
