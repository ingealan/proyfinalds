using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace pruebadesarrollo.Pages
{
    public class EditarProdu : PageModel
    {
        [BindProperty, Required(ErrorMessage = "El id es requerido")]
        public int ID { get; set; }

        [BindProperty, Required(ErrorMessage = "El nombre es requerido")]
        public string nombre { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "La categoria es requerida")]
        public string categoria { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "El stock es requerido")]
        public int stock { get; set; }

        // Cambié de 'decimal' a 'double' para que coincida con el tipo 'FLOAT' en la base de datos
        [BindProperty, Required(ErrorMessage = "El precio es requerido")]
        public double precio { get; set; } // Cambié aquí a 'double'

        public string ErrorMessage { get; set; } = "";

        public void OnGet(int id)
        {
            try
            {
                // Cadena de conexión a la base de datos
                string connectionString = "Server=.;Database=desarrollocrud;Trusted_Connection=True;TrustServerCertificate=True";

                // Abriendo la conexión a la base de datos
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Definir la consulta SQL para obtener los datos del producto
                    string sql = "SELECT ProductoID, Nombre, Categoria, Stock, Precio FROM Productos WHERE ProductoID=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Agregar el parámetro de id para la consulta
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Si existe el producto
                            if (reader.Read())
                            {
                                // Asignar valores de la base de datos a las propiedades de la clase
                                ID = reader.GetInt32(0);      // ProductoID
                                nombre = reader.GetString(1);  // Nombre
                                categoria = reader.GetString(2); // Categoria
                                stock = reader.GetInt32(3);   // Stock
                                precio = reader.GetDouble(4); // Precio (ahora es 'double')
                            }
                            else
                            {
                                // Si no se encuentra el producto, asignar un mensaje de error
                                ErrorMessage = "Producto no encontrado";
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Capturar errores y asignar el mensaje al ErrorMessage
                ErrorMessage = "Error al obtener el producto: " + ex.Message;
            }
        }

        public void OnPost()
        {
            // Validar los datos antes de continuar
            if (!ModelState.IsValid)
            {
                return;
            }

            try
            {
                // Cadena de conexión a la base de datos
                string connectionString = "Server=.;Database=desarrollocrud;Trusted_Connection=True;TrustServerCertificate=True";

                // Abriendo la conexión a la base de datos
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta SQL para actualizar el producto
                    string sql = "UPDATE [dbo].[Productos] SET [Nombre] = @nombre, [Categoria] = @categoria, [Stock] = @stock, [Precio] = @precio WHERE [ProductoID] = @id;";

                    // Ejecutar la consulta SQL
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Agregar los parámetros para la actualización
                        command.Parameters.AddWithValue("@id", ID);
                        command.Parameters.AddWithValue("@nombre", nombre);
                        command.Parameters.AddWithValue("@categoria", categoria);
                        command.Parameters.AddWithValue("@stock", stock);
                        command.Parameters.AddWithValue("@precio", precio);

                        // Ejecutar la consulta sin esperar resultados
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // En caso de error, asignar el mensaje al ErrorMessage
                ErrorMessage = "Error al actualizar el producto: " + ex.Message;
                return;
            }

            // Redirigir a la lista de productos después de editar
            Response.Redirect("/Produ");
        }
    }
}
