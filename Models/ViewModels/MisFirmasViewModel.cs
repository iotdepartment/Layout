namespace Layout.Models.ViewModels
{
    public class MisFirmasViewModel
    {
        public List<SolicitudFirma> Pendientes { get; set; } = new();

        public List<SolicitudFirma> Realizadas { get; set; } = new();
    }
}