using HotelTools.Components;
using HotelTools.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Serilog;
using Serilog.Events;
using MudBlazor.Charts;
using HotelTools.Seguridad;
using HotelTools.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

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

// VALIDACION DE PEPPER (clave secreta para hashes de contraseñas).
// Debe venir del entorno (Util__ClaveSecreta); si falta, la app NO arranca
// para evitar operar con un pepper vacío o por defecto.
var claveSecreta = configuration["Util:ClaveSecreta"];
if (string.IsNullOrWhiteSpace(claveSecreta))
{
    throw new InvalidOperationException(
        "Falta la variable de configuración 'Util:ClaveSecreta' (definir la variable de entorno 'Util__ClaveSecreta' antes de iniciar la aplicación).");
}
//===============================================================

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
builder.Services.AddAuthentication("HotelToolsAuth")
    .AddScheme<AuthenticationSchemeOptions, HotelToolsAuthHandler>("HotelToolsAuth", null);
// Denegación por defecto: cualquier endpoint/página sin política propia exige sesión autenticada.
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddScoped<AuthServices>();
builder.Services.AddScoped<SeguridadSesion>();
builder.Services.AddSingleton<SeguridadGlobal>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();
builder.Services.AddScoped<BrowserJS>();
builder.Services.AddScoped<ProductoEstadoService>();
builder.Services.AddScoped<PrestamoService>();
builder.Services.AddScoped<QuejaService>();

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

    if (path.StartsWith("/css") || path.StartsWith("/js") || path.StartsWith("/img") ||
        path.StartsWith("/_content") || path.StartsWith("/_framework") ||
        path.StartsWith("/_blazor"))
    {
        await next();
        return;
    }

    // El token se valida contra la BD en HotelToolsAuthHandler (existe, activo y
    // no expirado) antes de llegar aquí: una cookie forjada queda como anónima.
    var autenticado = context.User.Identity?.IsAuthenticated == true;

    if (path == "/")
    {
        if (autenticado)
        {
            context.Response.Redirect("/home");
            return;
        }
        context.Response.Redirect("/login");
        return;
    }

    if (path == "/login" || path == "/logout")
    {
        await next();
        return;
    }

    if (!autenticado)
    {
        context.Response.Redirect("/login");
        return;
    }

    await next();
});


app.MapRazorComponents<App>()        
    .AddInteractiveServerRenderMode();

app.Run();
