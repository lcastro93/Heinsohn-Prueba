namespace WebPruebaHeinsohn.Models
{
    public class TareaEstadoViewModel
    {
        public int Id { get; set; }
        public int IdTarea { get; set; }
        public int IdEstado { get; set; }
        public string? NombreTarea { get; set; }
        public string? NombreEstado { get; set; }
        public DateTime Fecha { get; set; }
    }
}
