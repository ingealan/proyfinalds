using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace pruebadesarrollo.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Eliminar la sesión para hacer logout
            HttpContext.Session.Remove("UsuarioRol");  // Eliminar el rol de la sesión
            HttpContext.Session.Remove("Usuario");    // Eliminar el usuario de la sesión

            // Redirigir al usuario a la página de inicio
            return RedirectToPage("/Index");
        }
    }
}
