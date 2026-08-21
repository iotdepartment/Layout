namespace Layout.Models.ViewModels
{
    public class MisFirmasViewModel
    {
        public List<PendienteFirmaAgrupadaViewModel> PendientesAgrupadas { get; set; } = new();

        public List<FirmaRealizadaAgrupadaViewModel> RealizadasAgrupadas{ get; set; } = new();

        public List<SolicitudFirma> Realizadas{ get; set; } = new();

        public List<FirmaPAViewModel> FirmasPA { get; set; } = new();

        public bool PuedeAsignarPA { get; set; }

        public List<FirmaPAViewModel> SoyPADe { get; set; } = new();

        public List<FirmaPAViewModel> PendientesPA { get; set; }
    = new();

    }
}