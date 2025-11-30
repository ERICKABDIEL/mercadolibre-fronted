using frontendnet.Models;

namespace frontendnet.Services;

public class CarritoClientService(HttpClient client)
{
    public async Task<List<CarritoItem>?> GetAsync()
    {
        return await client.GetFromJsonAsync<List<CarritoItem>>("api/carrito");
    }

    public async Task AddAsync(string productoId)
    {
        var data = new { productoId };
        var response = await client.PostAsJsonAsync("api/carrito", data);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string id)
    {
        var response = await client.DeleteAsync($"api/carrito/{id}");
        response.EnsureSuccessStatusCode();
    }
}
