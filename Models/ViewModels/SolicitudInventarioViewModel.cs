namespace Layout.Models.ViewModels
{
    //ESTA INFORMAICÓN LLENADA POR EL USUARIO APROBADOR 
    public class SolicitudInventarioViewModel
    {
        public int SolicitudId { get; set; }

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
