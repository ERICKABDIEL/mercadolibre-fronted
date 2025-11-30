using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

public class RegistroController : Controller
{
    private readonly AuthClientService auth;

    public RegistroController(AuthClientService auth)
    {
        this.auth = auth;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(UsuarioPwd modelo)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await auth.RegistrarAsync(modelo);
                TempData["msg"] = "Cuenta creada correctamente. Ya puede iniciar sesión.";
                return RedirectToAction("Index", "Auth");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }

        return View(modelo);
    }
}
