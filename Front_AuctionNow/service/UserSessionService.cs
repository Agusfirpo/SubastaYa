using System.Text.Json;
using Front_AuctionNow.Models.Responses;
using Microsoft.JSInterop;

namespace Front_AuctionNow.Services;

public class UserSessionService
{
    private readonly IJSRuntime _js;

    public LoginResponse? Usuario { get; private set; }
    public bool EstaLogueado => Usuario != null;

    public UserSessionService(IJSRuntime js)
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