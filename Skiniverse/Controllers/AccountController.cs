using Microsoft.AspNetCore.Mvc;

namespace Skiniverse.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string correo, string contrasena)
        {
            // Aquí validaremos con ADO.NET contra tu base de datos
            // Por ahora, si escribe cualquier cosa, lo mandamos al Test de Piel:
            return RedirectToAction("TestPiel", "Productos");
        }
    }
}