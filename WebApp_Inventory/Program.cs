using Persistencia.Contexto_DataBase;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


//cadena de conexion a la base de datos 
var conexionString = builder.Configuration.GetConnectionString("DefaultConnection");//leyendo la cadena de conexion desde el appsettings.json


//intyectando el uso de sql server a la clase conttext
builder.Services.AddDbContext<Inventory_Context>(options =>
{
    options.UseSqlServer(conexionString);
});


//serivicios de sesion
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(int.Parse(builder.Configuration["TiempoExipiracion"]!)); // Tiempo de expiración de la sesión
    options.Cookie.IsEssential = true; // La cookie es esencial para el funcionamiento de la aplicación
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseSession(); // Habilitar el uso de sesiones

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
