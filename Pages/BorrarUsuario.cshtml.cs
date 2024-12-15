using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace pruebadesarrollo.Pages
{
    public class BorrarUsuarioModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int ID { get; set; }

        // Propiedades para almacenar los mensajes de error y éxito
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
            // El ID ya se pasa por la URL, no es necesario cargar nada adicional
        }

        public IActionResult OnPost(int id)
        {
            // Intentar borrar el usuario
            if (BorrarUsuario(id))
            {
                // Si el borrado fue exitoso, mostramos un mensaje de éxito
                SuccessMessage = "El usuario se ha borrado correctamente.";
                return RedirectToPage("/Usuarios"); // Redirige a la lista de usuarios
            }

            // Si ocurre un error al borrar, mostramos un mensaje de error
            ErrorMessage = "No se pudo borrar el usuario.";
            return Page(); // Retorna la misma página para mostrar el mensaje de error
        }

        // Método que realiza el borrado del usuario en la base de datos
        private bool BorrarUsuario(int id)
        {
            try
            {
                // Cadena de conexión a la base de datos
                string connectionString = "Server=.;Database=desarrollocrud;Trusted_Connection=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM Usuarios WHERE UserID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Parámetro para evitar SQL Injection
                        command.Parameters.AddWithValue("@id", id);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0; // Si se afectaron filas, el borrado fue exitoso
                    }
                }
            }
            catch (Exception ex)
            {
                // Si ocurre un error, se logea y se retorna false
                Console.WriteLine("Error al borrar el usuario: " + ex.Message);
                return false;
            }
        }
    }
}
