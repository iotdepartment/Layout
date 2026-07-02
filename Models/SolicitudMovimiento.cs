using Layout.Models.Enums;

namespace Layout.Models
{
    public class SolicitudMovimiento
    {
        public int Id { get; set; }
        public string Folio { get; set; }

        public int AreaId { get; set; }
        public Area Area { get; set; }

        public TipoMovimiento TipoMovimiento { get; set; }

        public string Descripcion { get; set; }
        public string Razon { get; set; }

        public string ImagenLayout { get; set; }

        public string UsuarioSolicitanteId { get; set; }
        public ApplicationUser UsuarioSolicitante { get; set; }

        public string? UsuarioAprobadorId { get; set; }
        public ApplicationUser? UsuarioAprobador { get; set; }

        public EstatusSolicitud Estatus { get; set; } = EstatusSolicitud.Pendiente;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaRevision { get; set; }

        public string? ComentariosRevision { get; set; }

        // ✅ NUEVO
        public DateTime? FechaInicioMovimiento { get; set; }

        // ✅ NUEVO
        public DateTime? FechaFinMovimiento { get; set; }

        public SolicitudAprobacionDetalle? DetalleAprobacion { get; set; }

        public SolicitudInventarioTemporal InventarioTemporal { get; set; }

        public SolicitudMovimientosTecnicos? MovimientosTecnicos { get; set; }

        public ICollection<SolicitudHistorial>? Historial { get; set; }
    }
}