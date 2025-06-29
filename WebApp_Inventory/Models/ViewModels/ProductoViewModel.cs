namespace WebApp_Inventory.Models.ViewModels
{
    public class ProductoViewModel
    {
        public string? Nombre { get; set; }
        public int id_tipoProducto { get; set; }
        public List<ProductoStockDetalleViewModel>? Detalles { get; set; }
    }
}
