using frontendnet.Models;  // ← IMPORTANTE
namespace frontendnet.Models;

public class CarritoItem
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }

    // Producto siempre puede existir después de cargarlo desde la API
    public Producto Producto { get; set; } = null!;
}
