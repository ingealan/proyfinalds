using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace pruebadesarrollo.Pages
{
    public class CrearUsuarioModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string Role { get; set; }

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        private string connectionString = "Server=.;Database=desarrollocrud;Trusted_Connection=True;TrustServerCertificate=True";

        public void OnGet()
        {
            // Página vacía para crear un usuario
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Role))
            {
                ErrorMessage = "Todos los campos son requeridos.";
                return Page();
            }

            try
            {
                string hashedPassword = HashPassword(Password);
                if (InsertarUsuario(Username, hashedPassword, Role))
                {
                    SuccessMessage = "Usuario creado exitosamente.";
                    return RedirectToPage("/Index");  // Redirigir a la lista de usuarios
                }
                else
                {
                    ErrorMessage = "Error al crear el usuario.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Ocurrió un error: " + ex.Message;
            }

            return Page();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedPasswordBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedPasswordBytes);
            }
        }

        private bool InsertarUsuario(string username, string hashedPassword, string role)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Consulta SQL para insertar el nuevo usuario
                string sql = "INSERT INTO Usuarios (Username, PasswordHash, Role) VALUES (@username, @passwordHash, @role)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@passwordHash", hashedPassword);
                    command.Parameters.AddWithValue("@role", role);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; // Si se insertó al menos un usuario, devuelve true
                }
            }
        }
    }
}
