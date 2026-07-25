namespace Layout.Models
{
    public class TipoFirma
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public ICollection<UsuarioTipoFirma> Usuarios
        { get; set; } = new List<UsuarioTipoFirma>();
    }
}