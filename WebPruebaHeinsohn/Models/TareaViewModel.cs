using System.ComponentModel.DataAnnotations;

namespace WebPruebaHeinsohn.Models
{
    public class TareaViewModel
    {
        public int Id { get; set; }

        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string? Nombre { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una de los estados.")]
        public int IdEstado { get; set; }
        public string? NombreEstado { get; set; } = null;
    }
}
