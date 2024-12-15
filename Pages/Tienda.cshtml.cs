using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace pruebadesarrollo.Pages
{
    public class TiendaModel : PageModel
    {
        public List<ProduInfo> ProduList { get; set; } = new List<ProduInfo>(); // Lista para almacenar los productos

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

                    // Consulta SQL actualizada para la tabla Productos
                    string sql = "SELECT ProductoID, Nombre, Categoria, Stock, Precio, Imagen FROM Productos"; // Imagen debe ser una columna en la base de datos

                    // Ejecutando la consulta
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Leyendo los datos
                            while (reader.Read())
                            {
                                ProduInfo produInfo = new ProduInfo();
                                produInfo.id = reader.GetInt32(0); // ID del producto (ProductoID)
                                produInfo.nombre = reader.GetString(1); // Nombre del producto
                                produInfo.categoria = reader.GetString(2); // Categoría del producto
                                produInfo.stock = reader.GetInt32(3); // Stock del producto
                                produInfo.precio = reader.GetDouble(4); // Precio del producto
                                produInfo.imagen = reader.IsDBNull(5) ? "default-image.jpg" : reader.GetString(5); // Ruta de la imagen del producto

                                // Agregando a la lista
                                ProduList.Add(produInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de error
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Clase para contener la información del producto
        public class ProduInfo
        {
            public int id { get; set; }
            public string nombre { get; set; } = "";
            public string categoria { get; set; } = "";
            public int stock { get; set; }
            public double precio { get; set; }
            public string imagen { get; set; } = ""; // Ruta de la imagen
        }
    }
}
