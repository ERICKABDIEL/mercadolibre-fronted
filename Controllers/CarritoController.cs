using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

[Authorize(Roles = "Usuario")]
public class CarritoController : Controller
{
    private readonly ProductosClientService productosService;

    public CarritoController(ProductosClientService productosService)
    {
        this.productosService = productosService;
    }

    public async Task<IActionResult> Index()
    {
        var carrito = HttpContext.Session.GetObject<List<CarritoItem>>("carrito")
                    ?? new List<CarritoItem>();

        foreach (var item in carrito)
        {
            item.Producto = await productosService.ObtenerProductoAsync(item.ProductoId);
        }

        return View(carrito);
    }


    [HttpPost]
    public IActionResult Agregar(int productoId)
    {
        var carrito = HttpContext.Session.GetObject<List<CarritoItem>>("carrito")
                       ?? new List<CarritoItem>();

        var item = carrito.FirstOrDefault(x => x.ProductoId == productoId);

        if (item != null)
        {
            item.Cantidad++;
        }
        else
        {
            carrito.Add(new CarritoItem
            {
                ProductoId = productoId,
                Cantidad = 1
            });
        }

        HttpContext.Session.SetObject("carrito", carrito);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Eliminar(int id)
    {
        var carrito = HttpContext.Session.GetObject<List<CarritoItem>>("carrito")
                       ?? new List<CarritoItem>();

        var item = carrito.FirstOrDefault(x => x.ProductoId == id);
        if (item != null)
            carrito.Remove(item);

        HttpContext.Session.SetObject("carrito", carrito);

        return RedirectToAction("Index");
    }
}
