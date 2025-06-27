using Microsoft.AspNetCore.Mvc;
using Persistencia.Entities;
using System.Text.Json;

namespace WebApp_Inventory.Controllers;

public class TipoProductoController : Controller
{
    private static string? _baseUrl;

    private JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
    public TipoProductoController()
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        _baseUrl = builder.GetSection("ApiSetting:baseUrl").Value;
    }

    //listar tipos de Productos desde la API
    public IActionResult Listar()
    {
        List<Tipo_Producto> lista= new List<Tipo_Producto>();


        var client = new HttpClient();
        client.BaseAddress = new Uri(_baseUrl);
        var response = client.GetAsync("/Tipo_Producto");
        if (response.Result.IsSuccessStatusCode)
        { 
            var json_respuesta= response.Result.Content.ReadAsStringAsync().Result;
            var resultado= JsonSerializer.Deserialize<List<Tipo_Producto>>(json_respuesta,options);
            lista = resultado ?? new List<Tipo_Producto>();
        }

        return View(lista);
    }

    //crear un nuevo tipo de producto
    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(Tipo_Producto objTipo)
    {
        bool? respuesta = true;

        var client = new HttpClient();
        client.BaseAddress = new Uri(_baseUrl);

        var response = client.PostAsJsonAsync("/Tipo_Producto", objTipo);

        if (response.Result.IsSuccessStatusCode)
        {
            respuesta=true;
        }
        return RedirectToAction("Listar");
    }

    //editando un tipo de producto
    public IActionResult Editar(int id)
    { 
        Tipo_Producto ob = new Tipo_Producto();

        var client = new HttpClient();
        client.BaseAddress = new Uri(_baseUrl);
        var response = client.GetAsync($"/Tipo_Producto/{id}");
        if (response.Result.IsSuccessStatusCode)
        {
            var json_respuesta = response.Result.Content.ReadAsStringAsync().Result;
            //Deserializando el objeto JSON a un objeto Tipo_Producto
            var objTipo = JsonSerializer.Deserialize<Tipo_Producto>(json_respuesta,options);
            ob= objTipo ?? new Tipo_Producto();
        }
        return View(ob);
    }

    [HttpPost]
    public IActionResult Editar(Tipo_Producto objTipo)
    {
        bool? respuesta = true;
        var client = new HttpClient();
        client.BaseAddress = new Uri(_baseUrl);
        var response = client.PutAsJsonAsync($"/Tipo_Producto/{objTipo.id_TipoProducto}", objTipo);
        if (response.Result.IsSuccessStatusCode)
        {
            respuesta = true;
        }
        return RedirectToAction("Listar");
    }

    //eliminar un tipo de producto
    public IActionResult Eliminar(int id)
    {
        Tipo_Producto ob = new Tipo_Producto();

        var client = new HttpClient();
        client.BaseAddress = new Uri(_baseUrl);
        var response = client.GetAsync($"/Tipo_Producto/{id}");
        if (response.Result.IsSuccessStatusCode)
        {
            var json_respuesta = response.Result.Content.ReadAsStringAsync().Result;
            //Deserializando el objeto JSON a un objeto Tipo_Producto
            var objTipo = JsonSerializer.Deserialize<Tipo_Producto>(json_respuesta, options);
            ob = objTipo ?? new Tipo_Producto();
        }
        return View(ob);
    }

    [HttpPost]
    public IActionResult Eliminar(Tipo_Producto objTipo)
    {
        bool? respuesta = true;
        var client = new HttpClient();
        client.BaseAddress = new Uri(_baseUrl);
        var response = client.DeleteAsync($"/Tipo_Producto/{objTipo.id_TipoProducto}");
        if (response.Result.IsSuccessStatusCode)
        {
            respuesta = true;
        }
        return RedirectToAction("Listar");
    }
}