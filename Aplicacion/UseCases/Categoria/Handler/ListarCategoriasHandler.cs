using Aplicacion.DTOs.Response;
using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Categoria.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.UseCases.Categoria.Handler
{
    public class ListarCategoriasHandler
    {
        private readonly ICategoriaRepository _categoriasRepository;

        public ListarCategoriasHandler(ICategoriaRepository categoriasRepository)
        {
            _categoriasRepository = categoriasRepository;
        }

        public async Task<IList<CategoriaResponse>> Handle(ListarCategoriasQuery query)
        {
            var categorias = await _categoriasRepository.ObtenerTodasAsync();

            return categorias.Select(c => new CategoriaResponse
            {
                Id = c.Id,
                Nombre = c.Nombre,
                UrlIcono = c.UrlIcono,
            })
                .ToList();
        }
    }
}
