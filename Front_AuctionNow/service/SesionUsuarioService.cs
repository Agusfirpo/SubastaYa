using System.Text.Json;
using Aplicacion.DTOs.Response;
using Microsoft.JSInterop;

namespace Front_SubastaYa.Services;

public class SesionUsuarioService
{
    private readonly IJSRuntime _js;

    public LoginResponse? Usuario { get; private set; }
    public bool EstaLogueado => Usuario != null;

    public SesionUsuarioService(IJSRuntime js)
        => _js = js;

    public async Task IniciarSesion(LoginResponse usuario)
    {
        Usuario = usuario;

        await _js.InvokeVoidAsync(
            "sessionStorage.setItem",
            "usuario",
            JsonSerializer.Serialize(usuario));
    }

    public async Task CargarSesion()
    {
        var json = await _js.InvokeAsync<string?>(
            "sessionStorage.getItem",
            "usuario");

        Usuario = string.IsNullOrWhiteSpace(json)
            ? null
            : JsonSerializer.Deserialize<LoginResponse>(json);
    }

    public async Task CerrarSesion()
    {
        Usuario = null;

        await _js.InvokeVoidAsync(
            "sessionStorage.removeItem",
            "usuario");
    }
}