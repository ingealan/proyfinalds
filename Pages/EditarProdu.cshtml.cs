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

        [BindProperty, Required(ErrorMessage = "El precio es requerido")]
        public decimal precio { get; set; }

        public string ErrorMessage { get; set; } = "";

        public void OnGet(int id)
        {
            try
            {
                // Cadena de conexión a la base de datos
                string connectionString = "Server=.;Database=NEGOCIO;Trusted_Connection=True;TrustServerCertificate=True";

                // Abriendo la conexión a la base de datos
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Definir la consulta SQL
                    string sql = "SELECT ID, nombre, categoria, stock, precio FROM Produ WHERE ID=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Agregar el parámetro antes de ejecutar la consulta
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Si existe el registro
                            if (reader.Read())
                            {
                                // Asignar valores a las propiedades de la clase
                                ID = reader.GetInt32(0);      // ID
                                nombre = reader.GetString(1);  // nombre
                                categoria = reader.GetString(2); // categoria
                                stock = reader.GetInt32(3);   // stock
                                precio = reader.GetDecimal(4); // precio
                            }
                            else
                            {
                                // Si no encuentra el producto, redirige a la lista de productos
                                ErrorMessage = "Producto no encontrado";
                                return; // No redirigir aquí, sino mostrar mensaje de error
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Captura errores y muestra el mensaje
                ErrorMessage = ex.Message;
                return;
            }
        }

        public void OnPost()
        {
            if(!ModelState.IsValid){
                return;
            }
            try
        {
            // Cadena de conexión a la base de datos
            string connectionString = "Server=.;Database=NEGOCIO;Trusted_Connection=True;TrustServerCertificate=True";

            // Abriendo la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Consulta SQL para insertar el nuevo producto
                string sql = "UPDATE [dbo].[Produ] SET  [nombre] = @nombre, [categoria] = @categoria, [stock] = @stock, [precio] = @precio WHERE [ID] = @id;";


                // Ejecutando la consulta
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // Agregar los parámetros antes de ejecutar el comando
                   command.Parameters.AddWithValue("@id", ID);
                    command.Parameters.AddWithValue("@nombre", nombre);
                    command.Parameters.AddWithValue("@categoria", categoria);
                    command.Parameters.AddWithValue("@stock", stock);
                    command.Parameters.AddWithValue("@precio", precio);

                    // Ejecutar el comando sin esperar resultados
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            // En caso de error, asignar el mensaje al ErrorMessage
            ErrorMessage = ex.Message;
            return;
        }
        Response.Redirect("/Produ");
        }
        
        }
    }

