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
        Task<IList<Auction>> ObtenerTodasAsync();

        Task<Auction?> ObtenerPorIdAsync(int id);

        Task AgregarAsync(Auction subasta);

        Task<IList<Auction>> ObtenerPorVendedorIdAsync(int vendedorId);

        Task<Auction?> ObtenerPorIdParaActualizarAsync(int id);

        Task<IList<Auction>> ObtenerVencidasParaActualizarAsync(DateTime fechaActual);

        Task<(IList<Auction> Items, int TotalItems)> ObtenerTodasAsync(string? estado, int? categoriaId, decimal? precioMinimo, decimal? precioMaximo, string? orden, int pagina, int tamanioPagina, string? busqueda);

        Task<IList<Auction>> ObtenerProgramadasParaProcesarAsync(DateTime ahora);
    }
}
