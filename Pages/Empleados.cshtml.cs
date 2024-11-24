using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace pruebadesarrollo.Pages
{
    public class EmpleadosModel : PageModel
    {
        public List<EmpleadoInfo> EmpleadosList { get; set; } = new List<EmpleadoInfo>(); // Lista para almacenar los empleados

        public void OnGet()
        {
            try
            { 
                // Cadena de conexión a la base de datos
                string connectionString = "Server=.;Database=desarrollocrud;Trusted_Connection=True;TrustServerCertificate=True";

                // Abriendo la conexión a la base de datos
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta SQL para la tabla Empleados
                    string sql = "SELECT EmpleadoID, Nombre, Apellido, Cargo, FechaIngreso, Salario FROM Empleados";

                    // Ejecutando la consulta
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Leyendo los datos
                            while (reader.Read())
                            {
                                EmpleadoInfo empleadoInfo = new EmpleadoInfo();
                                empleadoInfo.id = reader.GetInt32(0); // EmpleadoID
                                empleadoInfo.nombre = reader.GetString(1); // Nombre del empleado
                                empleadoInfo.apellido = reader.GetString(2); // Apellido del empleado
                                empleadoInfo.cargo = reader.GetString(3); // Cargo del empleado
                                empleadoInfo.fechaIngreso = reader.GetDateTime(4); // Fecha de ingreso
                                empleadoInfo.salario = reader.GetDouble(5); // Salario del empleado

                                // Agregando a la lista
                                EmpleadosList.Add(empleadoInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Aquí se podría registrar el error de manera más eficiente
                Console.WriteLine("Existe un error: " + ex.Message);
            }
        }

        // Clase para contener la información del empleado
        public class EmpleadoInfo
        {
            public int id { get; set; }
            public string nombre { get; set; } = "";
            public string apellido { get; set; } = "";
            public string cargo { get; set; } = "";
            public DateTime fechaIngreso { get; set; }
            public double salario { get; set; }  // Usando double para salario
        }
    }
}
