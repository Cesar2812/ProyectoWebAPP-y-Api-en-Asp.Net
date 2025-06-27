using System.ComponentModel.DataAnnotations;

namespace Persistencia.Entities
{
    public class Forma_Venta
    {
        [Key]
        public int id_FormaVenta { get; set; }

        [Required]
        public string? Descripcion_FormaVenta { get; set; }

        [Required]
        public string? Simbolo{ get; set; }
    }
}
