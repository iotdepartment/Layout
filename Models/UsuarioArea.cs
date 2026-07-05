namespace Layout.Models
{
    public class UsuarioArea
    {
        public string UsuarioId { get; set; }
        public ApplicationUser Usuario { get; set; }

        public int AreaId { get; set; }
        public Area Area { get; set; }
    }   
}