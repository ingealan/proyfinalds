using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace pruebadesarrollo.Pages;

public class PrivacyModel : PageModel
{

    public void OnGet()
    {
        try{
            string connectionString="Server=.;Database=NEGOCIO;Trusted_Connection=True;TrustServerCertificate=True";
            using(SqlConnection connection = new SqlConnection(connectionString)){
                connection.Open();
                string sql= "SELECT * FROM Clientes";

                using(SqlCommand command = new SqlCommand(sql,connection)){
                    using(SqlDataReader reader = command.ExecuteReader()){
                        while(reader.Read()){
                            ClienteInfo clienteInfo = new ClienteInfo();
                            clienteInfo.cedulaCliente = reader.GetString(0);
                            clienteInfo.nombreCliente = reader.GetString(1);
                            clienteInfo.apellidoCliente = reader.GetString(2);
                            clienteInfo.correo = reader.GetString(3);
                            clienteInfo.direccion = reader.GetString(4);
                            ClienteList.Add(clienteInfo);

                        }
                    }
                }
            }
        }
        catch(Exception ex){
            Console.WriteLine("Existe un error: "+ex.Message);
        }
        
    }

    public class ClienteInfo{
        public String cedulaCliente {get;set;}
        public String nombreCliente {get;set;}="";
        public String apellidoCliente {get;set;}="";
        public String correo {get;set;}="";
        public String direccion {get;set;}="";

    }
    public List<ClienteInfo> ClienteList {get;set;} = new List<ClienteInfo>();
}

