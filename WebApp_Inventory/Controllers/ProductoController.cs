using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Persistencia.Contexto_DataBase;
using Persistencia.Entities;
using WebApp_Inventory.Models.ViewModels;


namespace WebApp_Inventory.Controllers
{
    public class ProductoController : Controller
    {
        private readonly Inventory_Context _db;

        public ProductoController(Inventory_Context db)
        {
            _db = db;
        }


        public IActionResult Listar()
        {
            var lista = _db.Producto_Stock
            .Include(ps => ps.Producto)
                .ThenInclude(p => p.TipoProducto)
            .Include(ps => ps.FormaVenta)
            .ToList();
            return View(lista);
        }


        [HttpGet]
        public IActionResult Crear()
        {
            ViewBag.TiposProducto = new SelectList(_db.Tipo_Producto, "id_TipoProducto", "Descripcion_Tipo");
            ViewBag.FormasVenta = new SelectList(_db.Forma_Venta, "id_FormaVenta", "Descripcion_FormaVenta");
            return View();
        } 

        

        [HttpPost]
        public IActionResult Crear(ProductoViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Nombre))
            {
                TempData["Error"] = "Debe ingresar el nombre del producto.";
                return RedirectToAction("Crear");
            }

            if (model.id_tipoProducto == 0)
            {
                TempData["Error"] = "Debe seleccionar el tipo de producto.";
                return RedirectToAction("Crear");
            }

            // Validar que al menos un detalle sea ingresado
            if (model.Detalles == null || !model.Detalles.Any(d => !string.IsNullOrWhiteSpace(d.Codigo_Producto)))
            {
                TempData["Error"] = "Debe ingresar al menos un detalle de forma de venta.";
                return RedirectToAction("Crear");
            }

            // Validar duplicados por código o combinación producto-forma de venta
            var codigos = model.Detalles
                .Where(d => !string.IsNullOrWhiteSpace(d.Codigo_Producto))
                .Select(d => d.Codigo_Producto)
                .ToList();

            var formasVentaIds = model.Detalles
                .Where(d => d.id_formaventa != 0)
                .Select(d => d.id_formaventa)
                .ToList();

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // Verificar duplicados en BD por código
                    var codigosExistentes = _db.Producto_Stock
                        .Where(p => codigos.Contains(p.Codigo_Producto))
                        .Select(p => p.Codigo_Producto)
                        .ToList();

                    if (codigosExistentes.Any())
                    {
                        TempData["Error"] = $"El código '{codigosExistentes.First()}' ya existe.";
                        return RedirectToAction("Crear");
                    }

                    // Primero insertar el maestro
                    var producto = new Producto
                    {
                        Nombre = model.Nombre,
                        id_tipoProducto = model.id_tipoProducto,
                    };

                    _db.Producto.Add(producto);
                     _db.SaveChanges();

                    // Validar duplicados por combinación producto-forma de venta
                    var duplicadoFormaVenta = _db.Producto_Stock
                        .Where(p => p.id_producto == producto.id_Producto && formasVentaIds.Contains(p.id_formaventa))
                        .FirstOrDefault();

                    if (duplicadoFormaVenta != null)
                    {
                        TempData["Error"] = $"Ya existe un registro con esa forma de venta.";
                        transaction.Rollback();
                        return RedirectToAction("Crear");
                    }

                    // Insertar detalles
                    foreach (var detalle in model.Detalles)
                    {
                        if (string.IsNullOrWhiteSpace(detalle.Codigo_Producto))
                            continue;

                        var detalleEntity = new Producto_Stock
                        {
                            Codigo_Producto = detalle.Codigo_Producto,
                            id_producto = producto.id_Producto,
                            id_formaventa = detalle.id_formaventa,
                            Stock = detalle.Stock,
                            Estado = true,
                           
                        };

                        _db.Producto_Stock.Add(detalleEntity);
                    }

                     _db.SaveChanges();
                    transaction.Commit();

                    TempData["Exito"] = "Producto registrado correctamente.";
                    return RedirectToAction("Crear");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    TempData["Error"] = "Ocurrió un error al guardar: " + ex.Message;
                    return RedirectToAction("Crear");
                }
            }
        }
    }
}
