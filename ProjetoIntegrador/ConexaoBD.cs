using System.Data.SqlClient;

namespace ProjetoIntegrador
{
    internal class ConexaoBD
    {
        public SqlConnection conn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;User ID=josewellington;Initial Catalog=SenacImpressoes;Data Source=DESKTOP-RHD5FD3\\SQLEXPRESS");

        public void Conectar()
        {
            conn.Open();
        }

        public void Desconectar()
        {
            conn.Close();
        }
    }
}
