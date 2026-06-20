namespace Layout.Models
{
    public class Area
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        // Relaciones
        public ICollection<SolicitudMovimiento> Solicitudes { get; set; }
    }
}