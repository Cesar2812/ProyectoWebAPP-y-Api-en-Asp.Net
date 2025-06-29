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

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
