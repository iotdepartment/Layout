namespace Layout.Models.ViewModels
{
    public class MisFirmasViewModel
    {
        public List<PendienteFirmaAgrupadaViewModel>
            PendientesAgrupadas
        { get; set; }
            = new();

        public List<FirmaRealizadaAgrupadaViewModel>
RealizadasAgrupadas
        { get; set; }
= new();

        public List<SolicitudFirma>
            Realizadas
        { get; set; }
            = new();
    }
}