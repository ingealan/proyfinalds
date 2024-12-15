using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.IO;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace pruebadesarrollo.Pages
{
    public class EditarProdu : PageModel
    {
        [BindProperty, Required(ErrorMessage = "El id es requerido")]
        public int ID { get; set; }

        [BindProperty, Required(ErrorMessage = "El nombre es requerido")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
        public string nombre { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "La categoría es requerida")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "La categoría solo puede contener letras y espacios.")]
        public string categoria { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "El stock es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un número entero mayor o igual a 0.")]
        public int stock { get; set; }

        [BindProperty, Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un número positivo.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El precio debe tener hasta 2 decimales.")]
        public double precio { get; set; }

        [BindProperty]
        public IFormFile? imagen { get; set; }  // Campo de imagen opcional

        public string ErrorMessage { get; set; } = "";
        public string imagenExistente { get; set; } = "";

        public void OnGet(int id)
        {
            try
            {
                string connectionString = "Server=.;Database=desarrollocrud;Trusted_Connection=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT ProductoID, Nombre, Categoria, Stock, Precio, Imagen FROM Productos WHERE ProductoID=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ID = reader.GetInt32(0);
                                nombre = reader.GetString(1);
                                categoria = reader.GetString(2);
                                stock = reader.GetInt32(3);
                                precio = reader.GetDouble(4);
                                imagenExistente = reader.GetString(5); // Cargar la imagen actual
                            }
                            else
                            {
                                ErrorMessage = "Producto no encontrado";
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error al obtener el producto: " + ex.Message;
            }
        }

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                return;
            }

            try
            {
                string imagenNombre = imagenExistente; // Mantener la imagen existente

                if (imagen != null)
                {
                    // Generar un nuevo nombre para la imagen si se seleccionó una nueva
                    imagenNombre = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);

                    string imagenRuta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imagenNombre);

                    // Guardar la nueva imagen redimensionada
                    using (var stream = imagen.OpenReadStream())
                    {
                        using (var image = Image.Load(stream))
                        {
                            image.Mutate(x => x.Resize(200, 200));
                            image.Save(imagenRuta, new JpegEncoder());
                        }
                    }
                }

                string connectionString = "Server=.;Database=desarrollocrud;Trusted_Connection=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "UPDATE [dbo].[Productos] SET [Nombre] = @nombre, [Categoria] = @categoria, [Stock] = @stock, [Precio] = @precio, [Imagen] = @imagen WHERE [ProductoID] = @id;";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", ID);
                        command.Parameters.AddWithValue("@nombre", nombre);
                        command.Parameters.AddWithValue("@categoria", categoria);
                        command.Parameters.AddWithValue("@stock", stock);
                        command.Parameters.AddWithValue("@precio", precio);
                        command.Parameters.AddWithValue("@imagen", imagenNombre);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error al actualizar el producto: " + ex.Message;
                return;
            }

            Response.Redirect("/Produ");
        }
    }
}
