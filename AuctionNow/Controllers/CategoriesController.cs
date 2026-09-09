using Application.DTOs.Response;
using Application.UseCases.Categoria.Handler;
using Application.UseCases.Categoria.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api_SubastaYa.Controllers
{
    [ApiController]
    [Route("api/v1/categorias")]
    public class CategoriesController : ControllerBase
    {
        private readonly GetCategoriesHandler _listarCategoriasHandler;
        public CategoriesController(GetCategoriesHandler listarCategoriasHandler)
        {
            _listarCategoriasHandler = listarCategoriasHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IList<CategoryResponse>>> ObtenerTodas()
        {
            var query = new GetCategoriesQuery();

            var resultado = await _listarCategoriasHandler.Handle(query);

            return Ok(resultado);
        }
    }
}
