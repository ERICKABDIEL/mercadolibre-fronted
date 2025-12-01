using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

[Authorize(Roles = "Usuario")]
public class CarritoController : Controller
{
    private readonly CarritoClientService _carrito;
    private readonly PedidosClientService _pedidos;
    private readonly IConfiguration _config;

    public CarritoController(CarritoClientService carrito,
                             PedidosClientService pedidos,
                             IConfiguration config)
    {
        _carrito = carrito;
        _pedidos = pedidos;
        _config = config;
    }

    public async Task<IActionResult> Index()
    {
        List<Producto>? lista = [];

        try
        {
            lista = await _carrito.ObtenerCarrito();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
        }

        // NECESARIO PARA QUE _CardPartial CARGUE LA IMAGEN
        ViewBag.Url = _config["UrlWebAPI"];

        return View(lista);
    }

    public IActionResult SeguirComprando()
    {
        return RedirectToAction("Index", "Comprar");
    }

    public async Task<IActionResult> LimpiarCarrito()
    {
        var carritoItems = await _carrito.ObtenerCarrito();
        if (carritoItems == null || carritoItems.Count == 0)
        {
            TempData["CarritoVacio"] = true;
            return RedirectToAction("Index", "Carrito");
        }

        try
        {
            await _carrito.LimpiarCarrito();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
        }

        return RedirectToAction("Index", "Carrito");
    }

    public async Task<IActionResult> FinalizarCompra()
    {
        var carritoItems = await _carrito.ObtenerCarrito();
        if (carritoItems == null || carritoItems.Count == 0)
        {
            TempData["CarritoVacio"] = true;
            return RedirectToAction("Index", "Carrito");
        }

        try
        {
            var userName = User.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Salir", "Auth");
            }
            
            await _pedidos.PostAsync(userName);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
        }

        return RedirectToAction("Index", "Comprar");
    }

    [HttpPost]
    public async Task<IActionResult> EliminarDelCarrito(int id)
    {
        try
        {
            await _carrito.EliminarDelCarrito(id);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
        }

        return RedirectToAction("Index", "Carrito");
    }
}
