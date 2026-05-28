using Microsoft.Data.SqlClient;

namespace NidusFront.DAOs
{
    public static class HelperDAO
    {
        // String de conexão ajustada para .\SQLEXPRESS e autenticação SQL Server (sa / 1234)
        private static readonly string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=NIDUS_DB;User ID=sa;Password=1234;TrustServerCertificate=True;";

        /// <summary>
        /// Factory Method que cria e retorna uma conexão ativa com o SQL Server.
        /// </summary>
        public static SqlConnection GetConnection()
        {
            SqlConnection conexao = new SqlConnection(ConnectionString);
            conexao.Open();
            return conexao;
        }
    }
}