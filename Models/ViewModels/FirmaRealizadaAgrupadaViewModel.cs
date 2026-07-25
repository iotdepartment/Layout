namespace Layout.Models.ViewModels
{
    public class FirmaRealizadaAgrupadaViewModel
    {
        public int FirmaIdReferencia { get; set; }

        public int SolicitudId { get; set; }

        public string Folio { get; set; }

        public string Area { get; set; }

        public DateTime? FechaFirma { get; set; }

        public List<string> TiposFirma { get; set; }
            = new();
    }
}
