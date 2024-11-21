using HotelTools.Autenticacion;
using HotelTools.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

//using HotelTools.Models;

//using HotelTools.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//MudBlazor Services
builder.Services.AddMudServices();

//Agregar Servicio de Configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
builder.Services.AddSingleton(configuration);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/login"; // Ruta a tu página de inicio de sesión
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);        
        options.AccessDeniedPath = "/noexiste";
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthenticationStateProvider,EstadoAuthProveedor>();
// Test Servicio de cuentas de usuarios Hardcodeada
builder.Services.AddSingleton<ServicioCuentaUsuario>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireEmpleadoRole", policy => policy.RequireRole("User"));

});


builder.Services.AddCascadingAuthenticationState();



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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
