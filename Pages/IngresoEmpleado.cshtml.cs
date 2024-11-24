using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace pruebadesarrollo.Pages
{
    public class IngresoEmpleadoModel : PageModel
    {
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

        public void OnPost()
        {
            // Validar el modelo antes de continuar
            if (!ModelState.IsValid)
            {
                return;
            }

            try
            {
                // Cadena de conexión a la base de datos (base de datos 'desarrollocrud')
                string connectionString = "Server=.;Database=desarrollocrud;Trusted_Connection=True;TrustServerCertificate=True";

                // Abriendo la conexión a la base de datos
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta SQL para insertar el nuevo empleado
                    string sql = "INSERT INTO [dbo].[Empleados] ([Nombre], [Apellido], [Cargo], [FechaIngreso], [Salario]) " +
                                 "VALUES (@nombre, @apellido, @cargo, @fechaIngreso, @salario);";

                    // Ejecutando la consulta
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Agregar los parámetros antes de ejecutar el comando
                        command.Parameters.AddWithValue("@nombre", nombre);
                        command.Parameters.AddWithValue("@apellido", apellido);
                        command.Parameters.AddWithValue("@cargo", cargo);
                        command.Parameters.AddWithValue("@fechaIngreso", fechaIngreso);
                        command.Parameters.AddWithValue("@salario", salario);

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

            // Redirigir a la página principal de empleados después de insertar
            Response.Redirect("/Empleados");
        }
    }
}
