using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace pruebadesarrollo.Pages
{
    public class EditarEmpleadoModel : PageModel
    {
        [BindProperty, Required(ErrorMessage = "El ID es requerido")]
        public int ID { get; set; }

        [BindProperty, Required(ErrorMessage = "El nombre es requerido")]
        public string nombre { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "El apellido es requerido")]
        public string apellido { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "El cargo es requerido")]
        public string cargo { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "La fecha de ingreso es requerida")]
        public DateTime fechaIngreso { get; set; }

        [BindProperty, Required(ErrorMessage = "El salario es requerido")]
        public double salario { get; set; }

        public string ErrorMessage { get; set; } = "";

        // Método para cargar los datos del empleado por ID
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

                    // Definir la consulta SQL para obtener los datos del empleado
                    string sql = "SELECT EmpleadoID, Nombre, Apellido, Cargo, FechaIngreso, Salario FROM Empleados WHERE EmpleadoID=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Agregar el parámetro de id para la consulta
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Si existe el empleado
                            if (reader.Read())
                            {
                                // Asignar valores de la base de datos a las propiedades de la clase
                                ID = reader.GetInt32(0);      // EmpleadoID
                                nombre = reader.GetString(1);  // Nombre
                                apellido = reader.GetString(2); // Apellido
                                cargo = reader.GetString(3);   // Cargo
                                fechaIngreso = reader.GetDateTime(4); // FechaIngreso
                                salario = reader.GetDouble(5); // Salario
                            }
                            else
                            {
                                // Si no se encuentra el empleado, asignar un mensaje de error
                                ErrorMessage = "Empleado no encontrado";
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Capturar errores y asignar el mensaje al ErrorMessage
                ErrorMessage = "Error al obtener el empleado: " + ex.Message;
            }
        }

        // Método para guardar los cambios al empleado
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

                    // Consulta SQL para actualizar el empleado
                    string sql = "UPDATE [dbo].[Empleados] SET [Nombre] = @nombre, [Apellido] = @apellido, [Cargo] = @cargo, [FechaIngreso] = @fechaIngreso, [Salario] = @salario WHERE [EmpleadoID] = @id;";

                    // Ejecutar la consulta SQL
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Agregar los parámetros para la actualización
                        command.Parameters.AddWithValue("@id", ID);
                        command.Parameters.AddWithValue("@nombre", nombre);
                        command.Parameters.AddWithValue("@apellido", apellido);
                        command.Parameters.AddWithValue("@cargo", cargo);
                        command.Parameters.AddWithValue("@fechaIngreso", fechaIngreso);
                        command.Parameters.AddWithValue("@salario", salario);

                        // Ejecutar la consulta sin esperar resultados
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // En caso de error, asignar el mensaje al ErrorMessage
                ErrorMessage = "Error al actualizar el empleado: " + ex.Message;
                return;
            }

            // Redirigir a la lista de empleados después de editar
            Response.Redirect("/Empleados");
        }
    }
}
