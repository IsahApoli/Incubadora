using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using NidusFront.Models;

namespace NidusFront.DAOs
{
    public class UsuarioDAO : PadraoDAO<UsuarioViewModel>
    {
        protected override SqlParameter[] CreateParameters(UsuarioViewModel model)
        {
            SqlParameter paramFoto = new SqlParameter("Foto", SqlDbType.VarBinary);
            paramFoto.Value = (object)model.Foto ?? DBNull.Value;

            return new SqlParameter[]
            {
                new SqlParameter("Id", model.Id),
                new SqlParameter("Nome", string.IsNullOrEmpty(model.Nome) ? DBNull.Value : (object)model.Nome),
                new SqlParameter("Email", string.IsNullOrEmpty(model.Email) ? DBNull.Value : (object)model.Email),
                new SqlParameter("Senha", string.IsNullOrEmpty(model.Senha) ? DBNull.Value : (object)model.Senha),
                new SqlParameter("Celular", string.IsNullOrEmpty(model.Celular) ? DBNull.Value : (object)model.Celular),
                new SqlParameter("Perfil", string.IsNullOrEmpty(model.Perfil) ? DBNull.Value : (object)model.Perfil),
                paramFoto
            };
        }

        public override void Insert(UsuarioViewModel model)
        {
            string sql = "INSERT INTO Usuarios (Nome, Email, Senha, Celular, Perfil, Foto) " +
                         "VALUES (@Nome, @Email, @Senha, @Celular, @Perfil, @Foto); " +
                         "SELECT SCOPE_IDENTITY();";

            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddRange(CreateParameters(model));
                int novoId = Convert.ToInt32(comando.ExecuteScalar());

                // Se for cliente e tiver CNPJ, insere na tabela de detalhes
                if (model.Perfil == "Cliente" && !string.IsNullOrEmpty(model.Cnpj))
                {
                    string sqlCnpj = "INSERT INTO Clientes_Detalhes (IdUsuario, CnpjVinculado) VALUES (@IdUsuario, @Cnpj)";
                    using (SqlCommand cmdCnpj = new SqlCommand(sqlCnpj, conexao))
                    {
                        cmdCnpj.Parameters.AddWithValue("@IdUsuario", novoId);
                        cmdCnpj.Parameters.AddWithValue("@Cnpj", model.Cnpj);
                        cmdCnpj.ExecuteNonQuery();
                    }
                }
            }
        }

        public override void Update(UsuarioViewModel model)
        {
            string sql = "UPDATE Usuarios SET Nome = @Nome, Email = @Email, Senha = @Senha, " +
                         "Celular = @Celular, Perfil = @Perfil, Foto = @Foto WHERE IdUsuario = @Id";

            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddRange(CreateParameters(model));
                comando.ExecuteNonQuery();

                // Atualiza ou insere o CNPJ na tabela de detalhes se for cliente
                if (model.Perfil == "Cliente" && !string.IsNullOrEmpty(model.Cnpj))
                {
                    string sqlCnpj = @"IF EXISTS (SELECT 1 FROM Clientes_Detalhes WHERE IdUsuario = @IdUsuario)
                                           UPDATE Clientes_Detalhes SET CnpjVinculado = @Cnpj WHERE IdUsuario = @IdUsuario
                                       ELSE
                                           INSERT INTO Clientes_Detalhes (IdUsuario, CnpjVinculado) VALUES (@IdUsuario, @Cnpj)";
                    using (SqlCommand cmdCnpj = new SqlCommand(sqlCnpj, conexao))
                    {
                        cmdCnpj.Parameters.AddWithValue("@IdUsuario", model.Id);
                        cmdCnpj.Parameters.AddWithValue("@Cnpj", model.Cnpj);
                        cmdCnpj.ExecuteNonQuery();
                    }
                }
            }
        }

        public override void Delete(int id)
        {
            // O CASCADE do banco já deleta o registro em Clientes_Detalhes automaticamente
            string sql = "DELETE FROM Usuarios WHERE IdUsuario = @Id";
            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("@Id", id);
                comando.ExecuteNonQuery();
            }
        }

        public override UsuarioViewModel Select(int id)
        {
            string sql = @"SELECT U.*, CD.CnpjVinculado
                           FROM Usuarios U
                           LEFT JOIN Clientes_Detalhes CD ON U.IdUsuario = CD.IdUsuario
                           WHERE U.IdUsuario = @Id";

            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("@Id", id);
                SqlDataReader registro = comando.ExecuteReader();
                if (registro.Read())
                    return MontarUsuario(registro);
                return null;
            }
        }

        public override List<UsuarioViewModel> List()
        {
            string sql = @"SELECT U.*, CD.CnpjVinculado
                           FROM Usuarios U
                           LEFT JOIN Clientes_Detalhes CD ON U.IdUsuario = CD.IdUsuario";

            List<UsuarioViewModel> lista = new List<UsuarioViewModel>();
            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                SqlDataReader registro = comando.ExecuteReader();
                while (registro.Read())
                    lista.Add(MontarUsuario(registro));
            }
            return lista;
        }

        public UsuarioViewModel ValidarLogin(string email, string senha)
        {
            string sql = @"SELECT U.*, CD.CnpjVinculado
                           FROM Usuarios U
                           LEFT JOIN Clientes_Detalhes CD ON U.IdUsuario = CD.IdUsuario
                           WHERE U.Email = @Email AND U.Senha = @Senha";

            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("@Email", email);
                comando.Parameters.AddWithValue("@Senha", senha);
                SqlDataReader registro = comando.ExecuteReader();
                if (registro.Read())
                    return MontarUsuario(registro);
                return null;
            }
        }

        private UsuarioViewModel MontarUsuario(SqlDataReader registro)
        {
            return new UsuarioViewModel
            {
                Id = Convert.ToInt32(registro["IdUsuario"]),
                Nome = registro["Nome"].ToString(),
                Email = registro["Email"].ToString(),
                Senha = registro["Senha"].ToString(),
                Celular = registro["Celular"].ToString(),
                Perfil = registro["Perfil"].ToString(),
                Foto = registro["Foto"] != DBNull.Value ? (byte[])registro["Foto"] : null,
                Cnpj = registro["CnpjVinculado"] != DBNull.Value ? registro["CnpjVinculado"].ToString() : null
            };
        }
    }
}