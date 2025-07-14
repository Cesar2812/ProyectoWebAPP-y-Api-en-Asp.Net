using Microsoft.AspNetCore.Mvc;
using Persistencia.Entities;
using System.Text.Json;

namespace WebApp_Inventory.Controllers;

public class FormaVentaController : Controller
{
    private static string? _baseUrl;

    private JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,//agregado la sintaxis camelcase al los JSON que se devuelen de la API
    };

    public FormaVentaController()
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();//configurando la variable de conreuccion para que cuando se cree el controler obtenga el archivo de cofiguracion
        _baseUrl = builder.GetSection("ApiSetting:baseUrl").Value;//obteniendo el endpoint de la API desde el archivo de configuracion 
    }

    public IActionResult Listar()
    {
        List<Forma_Venta> lista = new List<Forma_Venta>();


        var client = new HttpClient();//cliente http para consumir la API
        client.BaseAddress = new Uri(_baseUrl);//endpoint como parametro
        var response = client.GetAsync("/Forma_Venta");
        if (response.Result.IsSuccessStatusCode)
        {
            var json_respuesta = response.Result.Content.ReadAsStringAsync().Result;
            var resultado = JsonSerializer.Deserialize<List<Forma_Venta>>(json_respuesta, options);
            lista = resultado ?? new List<Forma_Venta>();
        }

        return View(lista);
    }

    //crear una Nueva Forma de Venta
    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(Forma_Venta objForma)
    {
       
        var client = new HttpClient();
        client.BaseAddress = new Uri(_baseUrl);

        var response = client.PostAsJsonAsync("/Forma_Venta", objForma);

        if (response.Result.IsSuccessStatusCode)
        {
            ViewBag.Exito = true; // se guardo correctamente
            ModelState.Clear();   //se limpia el modelo para que no se repita el mensaje de exito
            return View();
        }
        else
        {
            return View();
        }
    }

    //editar una Forma de Venta
    public IActionResult Editar(int id)
    {
        Forma_Venta ob = new Forma_Venta();

        var client = new HttpClient();
        client.BaseAddress = new Uri(_baseUrl);
        var response = client.GetAsync($"/Forma_Venta/{id}");
        if (response.Result.IsSuccessStatusCode)
        {
            var json_respuesta = response.Result.Content.ReadAsStringAsync().Result;
            //Deserializando el objeto JSON a un objeto Tipo_Producto
            var objForma = JsonSerializer.Deserialize<Forma_Venta>(json_respuesta, options);
            ob = objForma ?? new Forma_Venta();
        }
        return View(ob);
    }

    [HttpPost]
    public IActionResult Editar(Forma_Venta objForma)
    {
       
        var client = new HttpClient();
        client.BaseAddress = new Uri(_baseUrl);
        var response = client.PutAsJsonAsync($"/Forma_Venta/{objForma.id_FormaVenta}", objForma);
        if (response.Result.IsSuccessStatusCode)
        {
            ViewBag.Exito = true; // se guardo correctamente
            ModelState.Clear();   //se limpia el modelo para que no se repita el mensaje de exito
            return View();
        }
        else
        {
            return View();
        }
      
    }

    //eliminar una Froma de Venta
    public IActionResult Eliminar(int id)
    {
        Forma_Venta ob = new Forma_Venta();

        var client = new HttpClient();
        client.BaseAddress = new Uri(_baseUrl);
        var response = client.GetAsync($"/Forma_Venta/{id}");
        if (response.Result.IsSuccessStatusCode)
        {
            var json_respuesta = response.Result.Content.ReadAsStringAsync().Result;
            //Deserializando el objeto JSON a un objeto Tipo_Producto
            var objForma = JsonSerializer.Deserialize<Forma_Venta>(json_respuesta, options);
            ob = objForma ?? new Forma_Venta();
        }
        return View(ob);
    }

    [HttpPost]
    public IActionResult Eliminar(Forma_Venta objForma)
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri(_baseUrl);
        var response = client.DeleteAsync($"/Forma_Venta/{objForma.id_FormaVenta}");
        if (response.Result.IsSuccessStatusCode)
        {
            ViewBag.Exito = true; // se guardo correctamente
            ModelState.Clear();   //se limpia el modelo para que no se repita el mensaje de exito
            return View();
        }
        else
        {
            var mensajeError = response.Result.Content.ReadAsStringAsync().Result;
            ViewBag.MensajeError = mensajeError;
            
        }
        return View();
    }
}
