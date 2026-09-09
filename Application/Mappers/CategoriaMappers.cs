using Aplicacion.DTOs.Response;
using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Mappers
{
    public static class CategoriaMappers
    {
        public static CategoriaResponse ToResponse(Categoria categoria) =>
        new()
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            UrlIcono = categoria.UrlIcono
        };
    }
}
