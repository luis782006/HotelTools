using HotelTools.Components;
using HotelTools.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Serilog;
using Serilog.Events;
using MudBlazor.Charts;
using HotelTools.Seguridad;
using Microsoft.AspNetCore.Components.Authorization;

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

//Adem�s de los Paquetes Serilog y Serilog.Sinks.File, se necesita el paquete Serilog.Settings.Configuration
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
builder.Services.AddDbContext<HotelContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Hotel_Tools")));
//================================================================

//Autenticaci�n y Autorizaci�n
builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddScoped<AuthServices>();
builder.Services.AddScoped<BrowserJS>();

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

app.UseAuthentication();
app.UseAuthorization();


app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower() ?? "";

    // Archivos estaticos e infraestructura Blazor - siempre permitir
    if (path.StartsWith("/css") || path.StartsWith("/js") || path.StartsWith("/img") ||
        path.StartsWith("/_content") || path.StartsWith("/_framework") ||
        path.StartsWith("/_blazor"))
    {
        await next();
        return;
    }

    var cookieName = configuration["Util:CookieName"];
    var tieneCookie = context.Request.Cookies.TryGetValue(cookieName, out var token) && !string.IsNullOrEmpty(token);

    // Ruta raiz: redirigir segun cookie
    if (path == "/")
    {
        if (tieneCookie)
        {
            using var scope = context.RequestServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<HotelContext>();
            var sesion = await db.SesionesActivas
                .Where(s => s.Token == token && s.EstadoSesion && s.FechaExpiracion > DateTime.Now)
                .FirstOrDefaultAsync();

            if (sesion != null)
            {
                sesion.FechaExpiracion = DateTime.Now.AddMinutes(30);
                await db.SaveChangesAsync();

                context.Response.Redirect("/home");
                return;
            }
        }
        context.Response.Redirect("/login");
        return;
    }

    // /login siempre permitir
    if (path == "/login")
    {
        await next();
        return;
    }

    // Rutas protegidas: validar cookie + sesion en BD
    if (!tieneCookie)
    {
        context.Response.Redirect("/login");
        return;
    }

    // Cookies existe, validar contra BD
    using var scopeProtected = context.RequestServices.CreateScope();
    var dbProtected = scopeProtected.ServiceProvider.GetRequiredService<HotelContext>();
    var sesionValida = await dbProtected.SesionesActivas
        .Where(s => s.Token == token && s.EstadoSesion && s.FechaExpiracion > DateTime.Now)
        .FirstOrDefaultAsync();

    if (sesionValida == null)
    {
        context.Response.Redirect("/login");
        return;
    }

    // Extender sesion por inactividad
    sesionValida.FechaExpiracion = DateTime.Now.AddMinutes(30);
    await dbProtected.SaveChangesAsync();

    await next();
});


app.MapRazorComponents<App>()        
    .AddInteractiveServerRenderMode();

app.Run();
