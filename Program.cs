using HotelTools.Components;
//using HotelTools.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Serilog;
using Serilog.Events;
using MudBlazor.Charts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//MudBlazor Services
builder.Services.AddMudServices();

// CONFIGURACION DE LECTURA DE VARIABLES DE ENTORNO
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile(
        $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
        optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();
builder.Services.AddSingleton(configuration);
//==============================================================

// CONFIGURACION DE LOGS
Directory.CreateDirectory("Logs"); // CREO LA CARPETA SINO EXISTE

//Ademas de los Paquetes Serilog y Serilog.Sinks.File, se necesita el paquete Serilog.Settings.Configuration
//dotnet add package Serilog.Settings.Configuration. 
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration) // Leer configuraci�n desde appsettings.json
    .Enrich.WithProperty("Application", "BlazorApp")
    .Enrich.FromLogContext()
    .WriteTo.File(
        Path.Combine("Logs", "log-.txt"),
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level}] {Message}{NewLine}{Exception}"
    )
    .CreateLogger();
//===============================================================


//Agrego servicio de cadena de conexion
//builder.Services.AddDbContext<Hotel_ToolsContext>(options =>
// options.UseSqlServer(builder.Configuration.GetConnectionString("Hotel_Tools")));

//Agregar Servicio de Autenticaci�n y Autorizaci�n
//builder.Services.AddIdentity<Empleado,Rol>()
//  .AddDefaultTokenProviders()
//  .AddEntityFrameworkStores<Hotel_ToolsContext>();

//Configura Cookie Authentication
//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.Cookie.HttpOnly = true;
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
//    options.LoginPath = "/Account/Login";
//    options.AccessDeniedPath = "/Account/AccessDenied";
//    options.SlidingExpiration = true;
//});

////Politicas a
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("RequireEmpleadoRole", policy => policy.RequireRole("Empleado"));
//});


builder.Services.AddRazorComponents();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
