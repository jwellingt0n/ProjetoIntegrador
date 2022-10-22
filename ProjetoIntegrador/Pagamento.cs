
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjetoIntegrador
{
    internal class Pagamento
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public Pagamento(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }

        // METODO PARA LISTAR AS FORMAS DE PAGAMENTO
        public static List<Pagamento> ListarFormasPagamento(ConexaoBD bd)
        {
            string sql = "SELECT * FROM Pagamento";

            SqlCommand comando = new SqlCommand(sql, bd.conn);

            int id;
            string forma;
            List<Pagamento> pagamentos = new List<Pagamento>();

            using (var reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    id = reader.GetInt32(reader.GetOrdinal("id"));
                    forma = reader.GetString(reader.GetOrdinal("forma"));

                    pagamentos.Add(new Pagamento(id, forma));
                }
            }

            return pagamentos;
        }
    }
}
