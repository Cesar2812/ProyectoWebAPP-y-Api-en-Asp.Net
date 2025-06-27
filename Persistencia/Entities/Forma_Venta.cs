using System.ComponentModel.DataAnnotations;

namespace Persistencia.Entities
{
    public class Forma_Venta
    {
        [Key]
        public int id_FormaVenta { get; set; }

        [Required(ErrorMessage = "El Campo Descripcion es Requerido")]
        public string? Descripcion_FormaVenta { get; set; }

        [Required(ErrorMessage = "El Campo Simbolo es Requerido")]
        public string? Simbolo{ get; set; }
    }
}
