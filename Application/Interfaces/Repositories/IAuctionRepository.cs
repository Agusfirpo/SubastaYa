using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IAuctionRepository
    {
        Task<IList<Auction>> ObtenerTodasAsync(
            CancellationToken cancellationToken);

        Task<Auction?> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken);

        Task AgregarAsync(
            Auction subasta,
            CancellationToken cancellationToken);

        Task<IList<Auction>> ObtenerPorVendedorIdAsync(
            int vendedorId,
            CancellationToken cancellationToken);

        Task<Auction?> ObtenerPorIdParaActualizarAsync(
            int id,
            CancellationToken cancellationToken);

        Task<IList<Auction>> ObtenerVencidasParaActualizarAsync(
            DateTime fechaActual,
            CancellationToken cancellationToken);

        Task<(IList<Auction> Items, int TotalItems)> ObtenerTodasAsync(
            string? estado,
            int? categoriaId,
            decimal? precioMinimo,
            decimal? precioMaximo,
            string? orden,
            int pagina,
            int tamanioPagina,
            string? busqueda,
            CancellationToken cancellationToken);

        Task<IList<Auction>> ObtenerProgramadasParaProcesarAsync(
            DateTime ahora,
            CancellationToken cancellationToken);
    }
}
