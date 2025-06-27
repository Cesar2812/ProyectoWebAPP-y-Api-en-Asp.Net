using Microsoft.EntityFrameworkCore;
using Persistencia.Contexto_DataBase;

var builder = WebApplication.CreateBuilder(args);


//cadena de conexion a la base de datos 
var conexionString = builder.Configuration.GetConnectionString("CadenaSql");//leyendo la cadena de conexion desde el appsettings.json


//inyeccion de dependencias para el contexto de la base de datos usando cofiguarada para usar SQL Server
builder.Services.AddDbContext<Inventory_Context>(options =>
{
    options.UseSqlServer(conexionString);
});



// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
