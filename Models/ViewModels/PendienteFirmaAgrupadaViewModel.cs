namespace Layout.Models.ViewModels
{
    public class PendienteFirmaAgrupadaViewModel
    {
        public int FirmaIdReferencia { get; set; }

        public int SolicitudId { get; set; }

        public string Folio { get; set; }

        public string Area { get; set; }

        public DateTime FechaCreacion { get; set; }

        public List<string> TiposFirma { get; set; }
            = new();
    }
}