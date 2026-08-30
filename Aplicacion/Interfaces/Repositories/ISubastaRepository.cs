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
    }
    }
