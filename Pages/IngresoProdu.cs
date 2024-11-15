using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace pruebadesarrollo.Pages
{
    public class CreateModel : PageModel
{
    [BindProperty, Required(ErrorMessage = "El nombre es requerido")]
    public string nombre { get; set; } = "";

    [BindProperty, Required(ErrorMessage = "La categoria es requerida")]
    public string categoria { get; set; } = "";

    [BindProperty, Required(ErrorMessage = "El stock es requerido")]
    public int stock { get; set; }

    [BindProperty, Required(ErrorMessage = "El precio es requerido")]
    public decimal precio { get; set; }

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
            // Cadena de conexión a la base de datos
            string connectionString = "Server=.;Database=NEGOCIO;Trusted_Connection=True;TrustServerCertificate=True";

            // Abriendo la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Consulta SQL para insertar el nuevo producto
                string sql = "INSERT INTO [dbo].[Produ] ([nombre], [categoria], [stock], [precio]) " +
                             "VALUES (@nombre, @categoria, @stock, @precio);";

                // Ejecutando la consulta
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // Agregar los parámetros antes de ejecutar el comando
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

        // Redirigir a la página principal después de insertar
        Response.Redirect("/Produ");
    }
}

}
