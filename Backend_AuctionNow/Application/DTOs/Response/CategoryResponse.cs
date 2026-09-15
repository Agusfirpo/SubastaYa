using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class CategoryResponse
    {   
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? UrlIcono { get; set; }

    }
}
