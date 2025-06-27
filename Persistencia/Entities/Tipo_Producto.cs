using System.ComponentModel.DataAnnotations;
namespace Persistencia.Entities;


public class Tipo_Producto
{
    [Key]
    public int id_TipoProducto { get; set; } 

    [Required(ErrorMessage = "El Campo Descripcion es Requerido")]
    public string? Descripcion_Tipo { get; set; }
} 

