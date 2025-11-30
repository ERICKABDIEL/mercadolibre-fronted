using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json;
using frontendnet.Models;

namespace frontendnet.Services;

public class AuthClientService
{
    private readonly HttpClient client;
    private readonly IHttpContextAccessor http;

    public AuthClientService(HttpClient client, IHttpContextAccessor http)
    {
        this.client = client;
        this.http = http;
    }

    public async Task<AuthUser> ObtenTokenAsync(string email, string password)
    {
        var datos = new Login { Email = email, Password = password };
        var response = await client.PostAsJsonAsync("api/auth", datos);

        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<AuthUser>())!;
    }

    public async void IniciaSesionAsync(List<Claim> claims)
    {
        var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await http.HttpContext!.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identidad),
            new AuthenticationProperties()
        );
    }

    public async Task RegistrarAsync(UsuarioPwd usuario)
    {
        var response = await client.PostAsJsonAsync("api/auth/registro", usuario);

        if (response.IsSuccessStatusCode)
            return;

        var contenido = await response.Content.ReadAsStringAsync();

        try
        {
            var errorObj = JsonSerializer.Deserialize<ValidationErrorResponse>(contenido);

            if (errorObj?.errors != null)
            {
                string mensajes = string.Join("\n", errorObj.errors
                    .SelectMany(e => e.Value));

                throw new Exception(mensajes);
            }
        }
        catch
        {
            throw new Exception("Error al crear cuenta. Verifique los datos.");
        }
    }

    private class ValidationErrorResponse
    {
        public Dictionary<string, string[]>? errors { get; set; }
    }
}
