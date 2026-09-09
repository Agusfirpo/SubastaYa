using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace Api_SubastaYa.Middlewares
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
            catch (ValidationException ex)
            {
                await Respond(
                    context,
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
            catch (NotFoundException ex)
            {
                await Respond(
                    context,
                    HttpStatusCode.NotFound,
                    ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                await Respond(
                    context,
                    HttpStatusCode.Conflict,
                    "La información fue modificada por otro usuario.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled internal server error.");

                await Respond(
                    context,
                    HttpStatusCode.InternalServerError,
                    "Ocurrió un error interno.");
            }
        }

        private static async Task Respond(
            HttpContext context,
            HttpStatusCode status,
            string message)
        {
            context.Response.StatusCode = (int)status;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(new
                {
                    mensaje = message
                }));
        }
    }
}