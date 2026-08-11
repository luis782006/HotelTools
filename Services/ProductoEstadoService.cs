using HotelTools.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelTools.Services;

public class ProductoEstadoService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private const decimal ID_CategoriaGenerica = 2;

    public int ProductosGenericos { get; private set; }
    public event Action? OnCambio;

    public ProductoEstadoService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task Actualizar()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HotelContext>();

        ProductosGenericos = await context.Productos
            .AsNoTracking()
            .CountAsync(p => p.ID_CategoriaProFK == ID_CategoriaGenerica);

        OnCambio?.Invoke();
    }
}
