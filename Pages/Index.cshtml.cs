using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace pruebadesarrollo.Pages
{
    public class IndexModel : PageModel
    {
        public string UsuarioRol { get; set; }

        public void OnGet()
        {
            // Obtener el valor de la sesión y asignarlo a la propiedad
            UsuarioRol = HttpContext.Session.GetString("UsuarioRol");
        }
    }
}
