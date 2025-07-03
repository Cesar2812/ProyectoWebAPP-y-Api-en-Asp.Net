using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistencia.Entities;

public  class Producto_Stock
{
    [Key]
    public string? Codigo_Producto { get; set; }
    [Required]
    [ForeignKey("Producto")]
    public int id_producto { get; set; }

    [Required]
    [ForeignKey("FormaVenta")]
    public int id_formaventa { get; set; }

    [Required]
    public int Stock { get; set; }

    public bool Estado { get; set; } = true;


    // Relación con Producto
    public Producto? Producto { get; set; }

    // Relación con Forma_Venta
    public Forma_Venta? FormaVenta { get; set; }
}
