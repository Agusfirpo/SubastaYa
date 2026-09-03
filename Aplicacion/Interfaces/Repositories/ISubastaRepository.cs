using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entities;


    namespace Aplicacion.Interfaces.Repositories
    {
        public interface ISubastaRepository
        {
            Task<IList<Subasta>> ObtenerTodasAsync();

            Task<Subasta?> ObtenerPorIdAsync(int id);

            Task AgregarAsync(Subasta subasta);

            Task<IList<Subasta>> ObtenerPorVendedorIdAsync(int vendedorId);

            Task<Subasta?> ObtenerPorIdParaActualizarAsync(int id);

            Task<IList<Subasta>> ObtenerVencidasParaActualizarAsync(DateTime fechaActual);

            Task<(IList<Subasta> Items, int TotalItems)> ObtenerTodasAsync(string? estado,int? categoriaId,decimal? precioMinimo,decimal? precioMaximo,string? orden,int pagina,int tamanioPagina,string? busqueda);
            
            
        }
    }
