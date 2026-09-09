using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Application.UseCases.Usuario.Command;

namespace Application.UseCases.Usuario.Handler;

public class LoginHandler
{
    private readonly IUserRepository _usuarioRepository;

    public LoginHandler(IUserRepository usuarioRepository)
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