using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Entities
{
    public  class Producto
    {
        [Key]
        public int id_Producto { get; set; }

        [Required]
        [StringLength(20)]
        public string? Nombre { get; set; }

        [Required]
        [ForeignKey("TipoProducto")]
        public int id_tipoProducto { get; set; }

        // Relación con Tipo_Producto
        public Tipo_Producto? TipoProducto { get; set; }

        // Relación con Producto_Stock
        public ICollection<Producto_Stock>? ProductosStock { get; set; }
    }
}
