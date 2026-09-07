using System.Net;
using System.Text.Json;
using Aplicacion.Exceptions;
using Dominio.Exceptions;

namespace SubastaYa.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                await Responder(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (RecursoNoEncontradoException ex)
            {
                await Responder(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (ConcurrenciaException ex)
            {
                await Responder(context, HttpStatusCode.Conflict, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error interno no controlado.");

                await Responder(
                    context,
                    HttpStatusCode.InternalServerError,
                    "Ocurrió un error interno.");
            }
        }

        private static async Task Responder(
            HttpContext context,
            HttpStatusCode status,
            string mensaje)
        {
            context.Response.StatusCode = (int)status;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(new { mensaje }));
        }
    }
}