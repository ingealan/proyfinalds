using Microsoft.AspNetCore.Mvc;

namespace pruebadesarrollo.Controllers
{
    public class SinSesionController : Controller
    {
        public IActionResult Index()
        {
            // Aquí se podría incluir la lógica adicional si es necesario
            return View();
        }
    }
}
