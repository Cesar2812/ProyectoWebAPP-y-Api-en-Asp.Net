using Microsoft.EntityFrameworkCore;
using Persistencia.Entities;
namespace Persistencia.Contexto_DataBase
{
    public class Inventory_Context(DbContextOptions<Inventory_Context>opc):DbContext(opc)//configurando el contexto para que acepte culaquier gestor de base de datos relacional heredando de DBcontext
    { 
        public DbSet<Producto> Producto=>Set<Producto>();
        public DbSet<Tipo_Producto> Tipo_Producto=>Set<Tipo_Producto>();
        public DbSet<Producto_Stock> Producto_Stock => Set<Producto_Stock>();
        public DbSet<Forma_Venta> Forma_Venta=>Set<Forma_Venta>();
    }
}
