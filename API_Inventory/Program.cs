using Microsoft.EntityFrameworkCore;
using Persistencia.Contexto_DataBase;
using Persistencia.Entities;

var builder = WebApplication.CreateBuilder(args);//creacion de el constructor de la API


//cadena de conexion a la base de datos 
var conexionString = builder.Configuration.GetConnectionString("CadenaSql");//leyendo la cadena de conexion desde el appsettings.json


//inyeccion de dependencias para el contexto de la base de datos usando cofiguarada para usar SQL Server
builder.Services.AddDbContext<Inventory_Context>(options =>
{
    options.UseSqlServer(conexionString);
});


var app = builder.Build();//constrccion de la API con los servicios definidos anteriormente

//Creacion de endpoints de la API

#region Tipo_Producto
//POST para Tipo_Producto
app.MapPost("/Tipo_Producto", (Tipo_Producto objTipo, Inventory_Context db) =>
{
        db.Tipo_Producto.Add(objTipo);//agregando el objeto tipo producto a la base de datos
        db.SaveChanges();//guardando los cambios en la base de datos
        return Results.Created($"/Tipo_Producto/{objTipo.id_TipoProducto}", objTipo);//retornando el objeto creado

});

//GET para Tipo_Producto
app.MapGet("/Tipo_Producto", (Inventory_Context db) =>
{
    var lst=db.Tipo_Producto.ToList();//obteniendo la lista de tipo productos desde la base de datos
    return Results.Ok(lst);//retornando la lista de tipo productos
});

//GET para Tipo_Producto por id
app.MapGet("/Tipo_Producto/{id}", (int id, Inventory_Context db) =>
{
    //obj objeto devuelto
    var obj = db.Tipo_Producto.FirstOrDefault(t=>t.id_TipoProducto==id);//buscando el tipo producto por id
    if (obj == null)
    {
        return Results.NotFound();//retornando 404 si no se encuentra el objeto
    }
    return Results.Ok(obj);//retornando el objeto encontrado
});

//PUT para Tipo_Producto
app.MapPut("/Tipo_Producto/{id}", (int id, Tipo_Producto objTipo, Inventory_Context db) =>
{
    var obj = db.Tipo_Producto.FirstOrDefault(t => t.id_TipoProducto == id);//buscando el tipo producto por id
    if (obj == null)
    {
        return Results.NotFound();//retornando 404 si no se encuentra el objeto
    }
    obj.Descripcion_Tipo = objTipo.Descripcion_Tipo;//actualizando la descripcion del tipo producto
    db.SaveChanges();//guardando los cambios en la base de datos
    return Results.Ok(obj);//retornando el objeto actualizado
});


//DELETE para Tipo_Producto
app.MapDelete("/Tipo_Producto/{id}", (int id, Inventory_Context db) =>
{
    var obj = db.Tipo_Producto.FirstOrDefault(t => t.id_TipoProducto == id);//buscando el tipo producto por id
    if (obj == null)
    {
        return Results.NotFound();//retornando 404 si no se encuentra el objeto
    }
    db.Tipo_Producto.Remove(obj);//eliminando el objeto de la base de datos
    db.SaveChanges();//guardando los cambios en la base de datos
    return Results.NoContent();//retornando 204 No Content
});
#endregion


//EndPoints Forma_Venta
#region Forma_Venta
//POST para Forma_Venta
app.MapPost("/Forma_Venta", (Forma_Venta objForma, Inventory_Context db) =>
{
    db.Forma_Venta.Add(objForma);//agregando el objeto forma venta a la base de datos
    db.SaveChanges();//guardando los cambios en la base de datos
    return Results.Created($"/Forma_Venta/{objForma.id_FormaVenta}", objForma);//retornando el objeto creado
});


//GET para Forma_Venta
app.MapGet("/Forma_Venta", (Inventory_Context db) =>
{
    var lst = db.Forma_Venta.ToList();//obteniendo la lista de forma venta desde la base de datos
    return Results.Ok(lst);//retornando la lista de forma venta
});

//GET para Forma_Venta por id
app.MapGet("/Forma_Venta/{id}", (int id, Inventory_Context db) =>
{
    var obj = db.Forma_Venta.FirstOrDefault(f => f.id_FormaVenta == id);//buscando la forma venta por id
    if (obj == null)
    {
        return Results.NotFound();//retornando 404 si no se encuentra el objeto
    }
    return Results.Ok(obj);//retornando el objeto encontrado
});

//PUT para Forma_Venta
app.MapPut("/Forma_Venta/{id}", (int id, Forma_Venta objForma, Inventory_Context db) =>
{
    var obj = db.Forma_Venta.FirstOrDefault(f => f.id_FormaVenta == id);//buscando la forma venta por id
    if (obj == null)
    {
        return Results.NotFound();//retornando 404 si no se encuentra el objeto
    }
    obj.Descripcion_FormaVenta = objForma.Descripcion_FormaVenta;//actualizando la descripcion de la forma venta
    obj.Simbolo = objForma.Simbolo;//actualizando el simbolo de la forma venta
    db.SaveChanges();//guardando los cambios en la base de datos
    return Results.Ok(obj);//retornando el objeto actualizado
});

//DELETE para Forma_Venta
app.MapDelete("/Forma_Venta/{id}", (int id, Inventory_Context db) =>
{
    var obj = db.Forma_Venta.FirstOrDefault(f => f.id_FormaVenta == id);//buscando la forma venta por id
    if (obj == null)
    {
        return Results.NotFound();//retornando 404 si no se encuentra el objeto
    }
    db.Forma_Venta.Remove(obj);//eliminando el objeto de la base de datos
    db.SaveChanges();//guardando los cambios en la base de datos
    return Results.NoContent();//retornando 204 No Content
});
#endregion 


app.Run();


