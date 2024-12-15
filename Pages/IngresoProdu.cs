using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient; // Asegúrate de tener esta directiva using
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;

namespace pruebadesarrollo.Pages
{
    public class IngresoProduModel : PageModel
    {
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

        [BindProperty, Required(ErrorMessage = "La imagen es requerida")]
        public IFormFile? imagen { get; set; }  // Permitir que sea null

        public string ErrorMessage { get; set; } = "";

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                return;
            }

            try
            {
                string imagenNombre = "";
                if (imagen != null)
                {
                    // Generar un nombre único para la imagen
                    imagenNombre = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);

                    // Ruta donde se guardará la imagen redimensionada
                    string imagenRuta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imagenNombre);

                    // Redimensionar y guardar la imagen
                    using (var stream = imagen.OpenReadStream())
                    {
                        using (var image = Image.Load(stream))
                        {
                            image.Mutate(x => x.Resize(200, 200)); // Redimensionar la imagen
                            image.Save(imagenRuta, new JpegEncoder()); // Guardar la imagen como JPEG
                        }
                    }
                }

                string connectionString = "Server=.;Database=desarrollocrud;Trusted_Connection=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "INSERT INTO [dbo].[Productos] ([Nombre], [Categoria], [Stock], [Precio], [FechaIngreso], [Imagen]) " +
                                 "VALUES (@nombre, @categoria, @stock, @precio, @fechaIngreso, @imagen);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", nombre);
                        command.Parameters.AddWithValue("@categoria", categoria);
                        command.Parameters.AddWithValue("@stock", stock);
                        command.Parameters.AddWithValue("@precio", precio);
                        command.Parameters.AddWithValue("@fechaIngreso", DateTime.Now);
                        command.Parameters.AddWithValue("@imagen", imagenNombre);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Produ"); // Redirigir a la página principal de productos
        }
    }
}
