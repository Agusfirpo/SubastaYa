using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class VolumeTestSeeder
    {
        public static async Task SeedAsync(
            AppDbContext context,
            CancellationToken cancellationToken = default)
        {
            // Evita cargar dos veces los datos de volumen
            if (await context.Subastas.AnyAsync(
                s => s.Titulo.StartsWith("[VOLUMEN]"),
                cancellationToken))
            {
                return;
            }

            var categorias = await context.Categorias
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);

            var usuarios = await context.Usuarios
                .Select(u => u.Id)
                .ToListAsync(cancellationToken);

            if (categorias.Count == 0 || usuarios.Count < 3)
                throw new Exception(
                    "Se necesitan categorías y al menos 3 usuarios para la prueba.");

            var vendedorId = usuarios[0];

            var compradores = usuarios
                .Where(id => id != vendedorId)
                .ToList();

            var ahora = DateTime.UtcNow;

            // =========================
            // 500 SUBASTAS
            // =========================

            var subastas = new List<Auction>();

            for (var i = 1; i <= 500; i++)
            {
                subastas.Add(new Auction
                {
                    VendedorId = vendedorId,

                    CategoriaId =
                        categorias[(i - 1) % categorias.Count],

                    Titulo = $"[VOLUMEN] Subasta {i}",

                    Descripcion =
                        $"Subasta generada para prueba de volumen #{i}",

                    UrlImagen =
                        $"https://picsum.photos/seed/auction{i}/800/600",

                    PrecioBase = 1000 + i,

                    IncrementoMinimo = 100,

                    FechaInicio = ahora.AddHours(-1),

                    FechaFin = ahora.AddDays(7),

                    Estado = AuctionStatus.Activa
                });
            }

            await context.Subastas.AddRangeAsync(
                subastas,
                cancellationToken);

            // Necesitamos guardar para obtener los IDs
            await context.SaveChangesAsync(cancellationToken);

            // =========================
            // 5.000 PUJAS
            // 10 POR SUBASTA
            // =========================

            var pujas = new List<Bid>();

            foreach (var subasta in subastas)
            {
                for (var j = 1; j <= 10; j++)
                {
                    var compradorId =
                        compradores[(j - 1) % compradores.Count];

                    pujas.Add(new Bid
                    {
                        SubastaId = subasta.Id,
                        CompradorId = compradorId,

                        Monto =
                            subasta.PrecioBase +
                            (j * subasta.IncrementoMinimo),

                        FechaPuja =
                            ahora.AddMinutes(-10 + j)
                    });
                }
            }

            await context.Pujas.AddRangeAsync(
                pujas,
                cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}