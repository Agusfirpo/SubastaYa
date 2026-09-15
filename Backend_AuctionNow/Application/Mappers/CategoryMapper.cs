using Application.DTOs.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryResponse ToResponse(Category categoria) =>
        new()
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            UrlIcono = categoria.UrlIcono
        };
    }
}
