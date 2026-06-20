using System;

namespace Layout.Models
{
    public class SolicitudHistorial
    {
        public int Id { get; set; }

        public int SolicitudMovimientoId { get; set; }
        public SolicitudMovimiento SolicitudMovimiento { get; set; }

        public string UsuarioId { get; set; }
        public ApplicationUser Usuario { get; set; }

        public string EstatusAnterior { get; set; }
        public string EstatusNuevo { get; set; }

        public string Comentario { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}