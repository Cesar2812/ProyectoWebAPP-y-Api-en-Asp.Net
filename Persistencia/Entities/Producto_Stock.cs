
using System.ComponentModel.DataAnnotations;

namespace Persistencia.Entities;

public class Producto_Stock
{
    [Key]
    public string? Codigo_Producto { get; set; }

    [Required]
    public int Stock { get; set; }

    [Required]
    public bool Estado { get; set; }
    public DateTime FechaRegistro { get; set; }
}
