using Aplicacion.DTOs.Response;
using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Categoria.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Mappers;

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

            return categorias.Select(CategoriaMappers.ToResponse).ToList();
        }
    }
}
