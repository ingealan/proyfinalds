using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace pruebadesarrollo.Pages
{
    public class BorrarProd : PageModel
    {
        // Propiedad para almacenar el ID del producto
        [BindProperty(SupportsGet = true)]
        public int ID { get; set; }

        // Método que se ejecuta cuando se realiza una petición GET
        public void OnGet()
        {
            // En este caso no es necesario cargar nada en OnGet si solo se muestra la vista
            // El ID se pasa en la URL como parámetro.
        }

        // Método que se ejecuta cuando se hace un POST
        public void OnPost(int id)
        {
            borrarprodu(id);
            Response.Redirect("/Produ");  // Redirige al listado de productos después de borrar
        }

        // Método para borrar el producto en la base de datos
        private void borrarprodu(int id)
        {
            try
            {
                string connectionString = "Server=.;Database=desarrollocrud;Trusted_Connection=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM Productos WHERE ProductoID=@id";  // Cambié "Produ" por "Productos" y ProductoID
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // En caso de error, mostrar un mensaje en la consola
                Console.WriteLine("No se puede borrar el producto: " + ex.Message);
            }
        }
    }
}
