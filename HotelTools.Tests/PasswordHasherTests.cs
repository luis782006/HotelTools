using HotelTools.Seguridad;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace HotelTools.Tests;

/// <summary>
/// Cubre la ventana de migración del pepper: verificación dual en
/// PasswordHasher (specs/security/secret-management).
/// </summary>
public class PasswordHasherTests
{
    private const string PepperNuevo = "pepper-nuevo-para-tests-2026";
    private const string PepperViejo = "pepper-viejo-comprometido";
    private const string Password = "Secreta123!";

    private static IConfiguration Config(string? pepperAnterior = null)
    {
        var datos = new Dictionary<string, string?>
        {
            ["Util:ClaveSecreta"] = PepperNuevo
        };
        if (pepperAnterior != null)
            datos["Util:ClaveSecretaAnterior"] = pepperAnterior;

        return new ConfigurationBuilder().AddInMemoryCollection(datos).Build();
    }

    [Fact]
    public void Login_ConPepperNuevo_Verifica_SinMigracion()
    {
        var config = Config();
        var hash = PasswordHasher.HashPassword(Password, config);

        var ok = PasswordHasher.VerifyPassword(Password, hash, config, out bool migrar);

        Assert.True(ok);
        Assert.False(migrar);
    }

    [Fact]
    public void Login_ConPepperViejo_Y_VentanaAbierta_Verifica_Y_MarcaMigracion()
    {
        var configVieja = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["Util:ClaveSecreta"] = PepperViejo })
            .Build();
        var hashViejo = PasswordHasher.HashPassword(Password, configVieja);

        var configNueva = Config(PepperViejo);
        var ok = PasswordHasher.VerifyPassword(Password, hashViejo, configNueva, out bool migrar);

        Assert.True(ok);
        Assert.True(migrar);
    }

    [Fact]
    public void Migracion_RehasheoConPepperNuevo_LuegoVerifica_SinUsarPepperViejo()
    {
        var configVieja = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["Util:ClaveSecreta"] = PepperViejo })
            .Build();
        var hashViejo = PasswordHasher.HashPassword(Password, configVieja);

        var configNueva = Config(PepperViejo);
        Assert.True(PasswordHasher.VerifyPassword(Password, hashViejo, configNueva, out bool migrar));
        Assert.True(migrar);

        // Re-hasheo como lo hace SeguridadSesion en el login exitoso
        var hashMigrado = PasswordHasher.HashPassword(Password, configNueva);

        // Ya migrado: verifica con el pepper nuevo sin necesidad del anterior
        var soloNuevo = Config();
        Assert.True(PasswordHasher.VerifyPassword(Password, hashMigrado, soloNuevo, out bool migrar2));
        Assert.False(migrar2);
    }

    [Fact]
    public void Login_ConPepperViejo_Y_VentanaCerrada_EsRechazado()
    {
        var configVieja = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["Util:ClaveSecreta"] = PepperViejo })
            .Build();
        var hashViejo = PasswordHasher.HashPassword(Password, configVieja);

        // Sin Util:ClaveSecretaAnterior => la ventana está cerrada
        var configNueva = Config();
        var ok = PasswordHasher.VerifyPassword(Password, hashViejo, configNueva, out bool migrar);

        Assert.False(ok);
        Assert.False(migrar);
    }

    [Fact]
    public void PasswordIncorrecta_NoVerifica_Y_NoMarcaMigracion()
    {
        var configVieja = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["Util:ClaveSecreta"] = PepperViejo })
            .Build();
        var hashViejo = PasswordHasher.HashPassword(Password, configVieja);

        var configNueva = Config(PepperViejo);
        var ok = PasswordHasher.VerifyPassword("OtraClaveDistinta", hashViejo, configNueva, out bool migrar);

        Assert.False(ok);
        Assert.False(migrar);
    }

    [Fact]
    public void HashPassword_SinPepper_Definido_LanzaError()
    {
        var configVacia = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["Util:ClaveSecreta"] = "" })
            .Build();

        Assert.Throws<InvalidOperationException>(() =>
            PasswordHasher.HashPassword(Password, configVacia));
    }
}
