using Aplicacion.DTOs.Response;
using Aplicacion.UseCases.Categoria.Handler;
using Aplicacion.UseCases.Categoria.Queries;
using Microsoft.AspNetCore.Mvc;

namespace SubastaYa.Controllers
{
    [ApiController]
    [Route("api/v1/categorias")]
    public class CategoriaController : ControllerBase
    {
        private readonly ListarCategoriasHandler _listarCategoriasHandler;
        public CategoriaController(ListarCategoriasHandler listarCategoriasHandler)
        {
            _listarCategoriasHandler = listarCategoriasHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IList<CategoriaResponse>>> ObtenerTodas()
        {
            var query = new ListarCategoriasQuery();

            var resultado = await _listarCategoriasHandler.Handle(query);

            return Ok(resultado);
        }
    }
}
