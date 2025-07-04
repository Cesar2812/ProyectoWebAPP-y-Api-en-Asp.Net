using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Persistencia.Contexto_DataBase;
using Persistencia.Entities;
using System.Text.Json;
using WebApp_Inventory.Models.ViewModels;


namespace WebApp_Inventory.Controllers;

public class ProductoController : Controller
{
    private readonly Inventory_Context _db;//declarando el contexto ya configurado para SQL Server

   
    public ProductoController(Inventory_Context db) //inyectando contexto a la base de datos
    {
        _db = db;
    }

    //listar prodcutos en la Tabla
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
        //obteniendo en un ViwBag la lista de tipos de producto y formas de ventas
        ViewBag.TiposProducto = new SelectList(_db.Tipo_Producto, "id_TipoProducto", "Descripcion_Tipo");
        ViewBag.FormasVenta = new SelectList(_db.Forma_Venta, "id_FormaVenta", "Descripcion_FormaVenta");


        // Carga el diccionario de formas de venta para mostrarlos en la tabla  dinamica
        var formasVentaDict = _db.Forma_Venta
            .ToDictionary(f => f.id_FormaVenta, f => f.Descripcion_FormaVenta);

        ViewBag.FormasVentaDict = formasVentaDict;//ViewBag que se va a pasar a la Tabla


        // Obtener detalles actuales en sesión
        var json = HttpContext.Session.GetString("ListaDetalle");

        List<ProductoStockDetalleViewModel> detalles = new List<ProductoStockDetalleViewModel>();//Creando Lista de detalle del modelo

        if (json != null)
        {
            detalles = JsonSerializer.Deserialize<List<ProductoStockDetalleViewModel>>(json) ?? new List<ProductoStockDetalleViewModel>();
        }

        ViewBag.Detalles = detalles;//pasando la data de detalles insertados a la vista
        return View();
    }

  

    [HttpPost]
    public IActionResult AgregarDetalle(ProductoStockDetalleViewModel detalle)
    {
        if (string.IsNullOrWhiteSpace(detalle.Codigo_Producto) || detalle.id_formaventa == 0 || detalle.Stock <= 0)
        {
            TempData["Error"] = "Complete todos los campos del detalle.";
            return RedirectToAction("Crear");
        }

        var json = HttpContext.Session.GetString("ListaDetalle");//variable de sesion que mantendra la data en la tabla

        List<ProductoStockDetalleViewModel> lista = new();

        if (json != null)
        {
            lista = JsonSerializer.Deserialize<List<ProductoStockDetalleViewModel>>(json) ?? new List<ProductoStockDetalleViewModel>();
        }


        // Verificar duplicado por código
        if (lista.Any(d => d.Codigo_Producto == detalle.Codigo_Producto))//validando con Any si exste ese codigo en la lista osea que si esta el codigo cualquiera de los codigos pasados en el objeto, en la lista
        {
            TempData["Error"] = "Ya existe este Codigo en el resgistro.";
            return RedirectToAction("Crear");
        }

        lista.Add(detalle);
        json = JsonSerializer.Serialize(lista);

        HttpContext.Session.SetString("ListaDetalle", json);
        return RedirectToAction("Crear");
    }


    [HttpPost]
    public IActionResult GuardarTodo(ProductoViewModel maestro)
    {
        // Validar maestro
        if (string.IsNullOrWhiteSpace(maestro.Nombre) || maestro.id_tipoProducto == 0)
        {
            TempData["Error"] = "Debe ingresar nombre y tipo de producto.";
            return RedirectToAction("Crear");
        }

        // Leer detalles
        var jsonDetalles = HttpContext.Session.GetString("ListaDetalle");

        //inicialializando lista
        List<ProductoStockDetalleViewModel> detalles = new List<ProductoStockDetalleViewModel>();

        if (jsonDetalles != null)
        {
            detalles = JsonSerializer.Deserialize<List<ProductoStockDetalleViewModel>>(jsonDetalles) ?? new List<ProductoStockDetalleViewModel>();
        }
           

        if (!detalles.Any())//osea la lista esta vacia
        {
            TempData["Error"] = "Debe agregar al menos un detalle.";
            return RedirectToAction("Crear");
        }

        // Validar que no exista otro producto con el mismo nombre
        bool existe = _db.Producto.Any(p => p.Nombre == maestro.Nombre);
        if (existe)
        {
            TempData["Error"] = "Ya existe este producto en el sistema .";
            return RedirectToAction("Crear");
        }

        // Validar códigos duplicados en base de datos
        foreach (var d in detalles)
        {
            bool existeCodigo = _db.Producto_Stock.Any(ps => ps.Codigo_Producto == d.Codigo_Producto);
            if (existeCodigo)
            {
                TempData["Error"] = $"El código '{d.Codigo_Producto}' ya existe en la base de datos.";
                return RedirectToAction("Crear");
            }
        }


        //creando el transact a la base de datos
        using (var transaction = _db.Database.BeginTransaction())
        {

            try
            {
                var producto = new Producto
                {
                    Nombre = maestro.Nombre,
                    id_tipoProducto = maestro.id_tipoProducto
                };

                _db.Producto.Add(producto);
                _db.SaveChanges();

                foreach (var d in detalles)
                {
                    var detalleEntity = new Producto_Stock
                    {
                        Codigo_Producto = d.Codigo_Producto,
                        id_producto = producto.id_Producto,
                        id_formaventa = d.id_formaventa,
                        Stock = d.Stock,
                        Estado = true
                    };
                    _db.Producto_Stock.Add(detalleEntity);
                }

                _db.SaveChanges();
                transaction.Commit();

                // Limpiar sesiones
                HttpContext.Session.Remove("ListaDetalle");

                TempData["Guardado"] = "Producto registrado correctamente.";
                return RedirectToAction("Crear");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                TempData["Error"] = "Error al guardar: " + ex.Message;
                return RedirectToAction("Crear");
            }

        }
          
    }

    public IActionResult DarDeBaja(string codigo)
    {
        // Traemos el producto con la relación al Productocon su nombre y tipo de venta
        var productoStock = _db.Producto_Stock
            .Include(ps => ps.Producto)
            .Include(ps=>ps.FormaVenta)
            .FirstOrDefault(p => p.Codigo_Producto == codigo);

        if (productoStock == null)//si no existe el producto que estamos trayendo con reespecto al codigo 
        {
            return RedirectToAction("Listar", "Producto");
        }

        return View(productoStock);
    }


    [HttpPost]
    public IActionResult DarDeBajaConfirmado(string Codigo_Producto)
    {
        var productoStock = _db.Producto_Stock
            .FirstOrDefault(p => p.Codigo_Producto == Codigo_Producto);

        if (productoStock == null)
        {
            return RedirectToAction("Listar", "Producto");
        }
        else
        {
            productoStock.Estado = false; //cambiando a no disponible

            _db.SaveChanges();

           
            ViewBag.Exito = true;

            // Cargar la relación de nuevo
            _db.Entry(productoStock).Reference(p => p.Producto).Load();
            _db.Entry(productoStock).Reference(p => p.FormaVenta).Load();

        }
        return View("DarDeBaja", productoStock);
    }
}