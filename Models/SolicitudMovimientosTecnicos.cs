namespace Layout.Models
{
    public class SolicitudMovimientosTecnicos
    {
        public int Id { get; set; }

        public int SolicitudId { get; set; }
        public SolicitudMovimiento Solicitud { get; set; }

        public bool MovimientoRed { get; set; }          // IT
        public bool MovimientoIoT { get; set; }          // IoT
        public bool MovimientoProgramacion { get; set; } // PLC
        public bool MovimientoElectrico { get; set; }    // energía
        public bool MovimientoEHS { get; set; }          // seguridad
        public bool CambioNomenclatura { get; set; }          // seguridad
        public bool RequierePCR { get; set; }            // proceso
    }
}
