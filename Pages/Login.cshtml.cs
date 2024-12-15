using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;
using System.Security.Cryptography;
using System.Text;

namespace pruebadesarrollo.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; } = "";

        private string connectionString = "Server=.;Database=desarrollocrud;Trusted_Connection=True;TrustServerCertificate=True";

        public void OnGet()
        {
            // Página vacía para mostrar el formulario de login
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Por favor ingrese ambos campos (Usuario y Contraseña).";
                return Page();
            }

            try
            {
                string role = string.Empty;
                if (AuthenticateUser(Username, Password, out role))
                {
                    // Almacenar el rol en la sesión
                    HttpContext.Session.SetString("UsuarioRol", role);

                    // Redirigir al Index (Inicio) después de iniciar sesión correctamente
                    return RedirectToPage("/Index");
                }
                else
                {
                    ErrorMessage = "Usuario o contraseña incorrectos.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Ocurrió un error: " + ex.Message;
                return Page();
            }
        }

        private bool AuthenticateUser(string username, string password, out string role)
        {
            role = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Consultar el nombre de usuario y obtener el hash de la contraseña y el rol
                string sql = "SELECT PasswordHash, Role FROM Usuarios WHERE Username = @username";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedHash = reader.GetString(0);
                            role = reader.GetString(1);

                            // Verificar si el hash de la contraseña ingresada coincide
                            return VerifyPasswordHash(password, storedHash);
                        }
                        else
                        {
                            return false;  // Usuario no encontrado
                        }
                    }
                }
            }
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedPasswordBytes = sha256.ComputeHash(passwordBytes);
                string hashedPassword = Convert.ToBase64String(hashedPasswordBytes);

                return hashedPassword == storedHash;  // Verifica si el hash coincide
            }
        }
    }
}
