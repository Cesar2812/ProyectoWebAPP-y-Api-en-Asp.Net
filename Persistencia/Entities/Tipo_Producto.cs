using System.ComponentModel.DataAnnotations;


namespace Persistencia.Entities;

public class Tipo_Producto
{
    [Key]
    public int id_TipoProducto { get; set; } 

    [Required]
    public string? Descripcion_TipoProducto { get; set; }

    public DateTime FechaRegistro { get; set; }
} 
