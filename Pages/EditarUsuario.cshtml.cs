using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.Security.Cryptography;
using System.Text;

namespace pruebadesarrollo.Pages
{
    public class EditarUsuarioModel : PageModel
    {
        [BindProperty]
        public int UserID { get; set; }  // Asegúrate de que UserID sea un campo vinculable
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string Role { get; set; }

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        private readonly string connectionString = "Server=.;Database=desarrollocrud;Trusted_Connection=True;TrustServerCertificate=True";

        // Método para cargar los datos del usuario por UserID
        public void OnGet(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT Username, PasswordHash, Role FROM Usuarios WHERE UserID = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Asignamos los valores al modelo
                                Username = reader.GetString(0);
                                Role = reader.GetString(2);
                                UserID = id;
                            }
                            else
                            {
                                ErrorMessage = "Usuario no encontrado";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error al cargar los datos del usuario: " + ex.Message;
            }
        }

        // Método que se ejecuta al enviar el formulario
        public IActionResult OnPost(int userId)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Role))
            {
                ErrorMessage = "El usuario y el rol son requeridos.";
                return Page();
            }

            try
            {
                string hashedPassword = string.IsNullOrEmpty(Password) ? null : HashPassword(Password);

                if (ActualizarUsuario(userId, hashedPassword, Role))  // Usamos el UserID aquí
                {
                    SuccessMessage = "Usuario actualizado exitosamente.";
                    return RedirectToPage("/Usuarios");  // Redirigir a la lista de usuarios
                }
                else
                {
                    ErrorMessage = "Error al actualizar el usuario.";
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

        private bool ActualizarUsuario(int userId, string hashedPassword, string role)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "UPDATE Usuarios SET Role = @role" + (hashedPassword != null ? ", PasswordHash = @passwordHash" : "") + " WHERE UserID = @userId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@role", role);

                    if (hashedPassword != null)
                    {
                        command.Parameters.AddWithValue("@passwordHash", hashedPassword);
                    }

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
