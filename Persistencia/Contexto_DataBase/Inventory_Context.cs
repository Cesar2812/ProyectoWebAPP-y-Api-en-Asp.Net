using Microsoft.EntityFrameworkCore;
using Persistencia.Entities;
namespace Persistencia.Contexto_DataBase
{
    public class Inventory_Context(DbContextOptions<Inventory_Context>opc):DbContext(opc)
    { 
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Tipo_Producto> TipoProducto { get; set; }
        public DbSet<Producto_Stock> ProductoStock { get; set; }
        public DbSet<Forma_Venta> FormaVenta { get; set; }
    }
}
