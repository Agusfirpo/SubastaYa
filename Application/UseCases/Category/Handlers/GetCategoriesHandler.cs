using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Application.UseCases.Categoria.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Mappers;

namespace Application.UseCases.Categoria.Handler
{
    public class GetCategoriesHandler
    {
        private readonly ICategoryRepository _categoriasRepository;

        public GetCategoriesHandler(ICategoryRepository categoriasRepository)
        {
            _categoriasRepository = categoriasRepository;
        }

        public async Task<IList<CategoryResponse>> Handle(GetCategoriesQuery query)
        {
            var categorias = await _categoriasRepository.ObtenerTodasAsync();

            return categorias.Select(CategoryMapper.ToResponse).ToList();
        }
    }
}
