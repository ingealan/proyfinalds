using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace pruebadesarrollo.Pages
{
    public class UsuariosModel : PageModel
    {
        public List<UsuarioInfo> UsuariosList { get; set; } = new List<UsuarioInfo>();

        private readonly string connectionString = "Server=.;Database=desarrollocrud;Trusted_Connection=True;TrustServerCertificate=True";

        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT UserID, Username, Role FROM Usuarios";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UsuarioInfo usuario = new UsuarioInfo
                                {
                                    UserID = reader.GetInt32(0),
                                    Username = reader.GetString(1),
                                    Role = reader.GetString(2)
                                };

                                UsuariosList.Add(usuario);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public class UsuarioInfo
        {
            public int UserID { get; set; }
            public string Username { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
        }
    }
}
