using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entities;

namespace Aplicacion.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorEmailAsync(string email);
}