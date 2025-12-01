using frontendnet.Models;

namespace frontendnet.Services;

public class ClientesClientService(HttpClient client)
{
    public async Task PostAsync(Client cliente)
    {
        var response = await client.PostAsJsonAsync("api/clientes", cliente);
        response.EnsureSuccessStatusCode();
    }
}
