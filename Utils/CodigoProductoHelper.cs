using System.Numerics;
using System.Security.Cryptography;
using HotelTools.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelTools.Utils
{
    public static class CodigoProductoHelper
    {
        private const string Caracteres = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public static string GenerarIdCorto()
        {
            var bytes = new byte[8];
            RandomNumberGenerator.Fill(bytes);
            var valor = new BigInteger(bytes, isUnsigned: true, isBigEndian: true);

            var chars = new char[11];
            for (int i = 10; i >= 0; i--)
            {
                valor = BigInteger.DivRem(valor, 62, out var rem);
                chars[i] = Caracteres[(int)rem];
            }

            return new string(chars);
        }

        public static async Task<string> GenerarCodigoUnico(HotelContext context, string prefijo, string iniciales)
        {
            while (true)
            {
                var codigo = $"{prefijo}-{GenerarIdCorto()}-{iniciales}";
                var existe = await context.Productos.AsNoTracking()
                    .AnyAsync(p => p.Codigo == codigo);
                if (!existe)
                    return codigo;
            }
        }
    }
}
