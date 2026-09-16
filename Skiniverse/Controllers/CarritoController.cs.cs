using Microsoft.AspNetCore.Mvc;
using Skiniverse.Models;

namespace Skiniverse.Controllers
{
    public class CarritoController : Controller
    {
        // Muestra la vista del carrito
        public IActionResult Index()
        {
            return View();
        }
    }
}