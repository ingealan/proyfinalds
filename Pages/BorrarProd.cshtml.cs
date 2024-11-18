using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
namespace pruebadesarrollo.Pages
{
    public class BorrarProd : PageModel
    {
        public void OnGet()
        {
        }
        public void OnPost(int id)
        {
            borrarprodu(id);
            Response.Redirect("/Produ");
        }

        private void borrarprodu(int id)
        {
            try
            {
                string connectionString = "Server=.;Database=NEGOCIO;Trusted_Connection=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString)){
                    connection.Open();
                    string sql = "DELETE FROM Produ WHERE ID=@id";
                    using (SqlCommand command = new SqlCommand(sql,connection)){
                        command.Parameters.AddWithValue("@id",id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("No se puede borrar a la persona "+ex.Message);
            }
        }

    }
}