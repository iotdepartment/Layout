namespace Layout.Models
{
    public class SolicitudMovimientosTecnicos
    {
        public int Id { get; set; }

        public int SolicitudId { get; set; }
        public SolicitudMovimiento Solicitud { get; set; }

        public bool MovimientoITIoT { get; set; }
        public bool MovimientoProgramacion { get; set; }
        public bool MovimientoElectrico { get; set; }
        public bool MovimientoEHS { get; set; }
        public bool CambioNomenclatura { get; set; }

        public bool RequierePCR { get; set; }
        public string? NumeroPCR { get; set; }


        public string? ImagenAntes { get; set; }
        public string? ImagenDespues { get; set; }

    }
}