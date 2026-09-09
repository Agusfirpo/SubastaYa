using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.Response;
using Aplicacion.Interfaces.Repositories;
using Aplicacion.UseCases.Usuario.Command;

namespace Aplicacion.UseCases.Usuario.Handler;

public class LoginHandler
{
    private readonly IUsuarioRepository _usuarioRepository;

    public LoginHandler(IUsuarioRepository usuarioRepository)
        => _usuarioRepository = usuarioRepository;

    public async Task<LoginResponse?> Handle(LoginCommand command)
    {
        var usuario = await _usuarioRepository
            .ObtenerPorEmailAsync(command.Email);

        if (usuario == null ||
            !BCrypt.Net.BCrypt.Verify(command.Password, usuario.PasswordHash))
            return null;

        return new LoginResponse
        {
            Id = usuario.Id,
            Email = usuario.Email,
            Nombre = usuario.Nombre
        };
    }
}