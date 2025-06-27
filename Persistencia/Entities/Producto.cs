using System.ComponentModel.DataAnnotations;

namespace Persistencia.Entities
{
    public  class Producto
    {
        [Key]
        public int id_Producto { get; set; }

        [Required]
        public string? Nombre { get; set; }
        [Required]
        public Tipo_Producto? TipoProducto { get; set; }
    }
}
