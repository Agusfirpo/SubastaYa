using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs.Response;

public class LoginResponse
{
    public int Id { get; set; }
    public string Email { get; set; } = "";
    public string Nombre { get; set; } = "";
}