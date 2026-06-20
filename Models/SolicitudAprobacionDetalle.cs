using Layout.Models.Enums;

namespace Layout.Models
{
    public class SolicitudAprobacionDetalle
    {
        public int Id { get; set; }

        public int SolicitudMovimientoId { get; set; }
        public SolicitudMovimiento SolicitudMovimiento { get; set; }

        public string UsuarioEjecutorId { get; set; }
        public ApplicationUser UsuarioEjecutor { get; set; }

        public DateTime? FechaInicioProgramada { get; set; }
        public DateTime? FechaFinProgramada { get; set; }

        public DateTime? FechaInicioReal { get; set; }
        public DateTime? FechaFinReal { get; set; }

        public string RecursosNecesarios { get; set; }
        public string Observaciones { get; set; }

        public EstatusSolicitud EstatusEjecucion { get; set; }
    }
}