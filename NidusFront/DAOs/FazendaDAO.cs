using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using NidusFront.Models;

namespace NidusFront.DAOs
{
    public class FazendaDAO : PadraoDAO<FazendaViewModel>
    {
        // 1. MAPEAMENTO DE PARÂMETROS
        protected override SqlParameter[] CreateParameters(FazendaViewModel model)
        {
            object nomeValor = string.IsNullOrEmpty(model.NomeFantasia) ? DBNull.Value : (object)model.NomeFantasia;
            object cnpjValor = string.IsNullOrEmpty(model.Cnpj) ? DBNull.Value : (object)model.Cnpj;

            return new SqlParameter[]
            {
                new SqlParameter("Id", model.Id),
                new SqlParameter("NomeFantasia", nomeValor),
                new SqlParameter("Cnpj", cnpjValor)
            };
        }

        // 2. INSERIR
        public override void Insert(FazendaViewModel model)
        {
            string sql = "INSERT INTO Fazendas (NomeFantasia, Cnpj) VALUES (@NomeFantasia, @Cnpj)";
            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddRange(CreateParameters(model));
                comando.ExecuteNonQuery();
            }
        }

        // 3. ATUALIZAR
        public override void Update(FazendaViewModel model)
        {
            string sql = "UPDATE Fazendas SET NomeFantasia = @NomeFantasia, Cnpj = @Cnpj WHERE IdFazenda = @Id";
            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddRange(CreateParameters(model));
                comando.ExecuteNonQuery();
            }
        }

        // 4. EXCLUIR
        public override void Delete(int id)
        {
            string sql = "DELETE FROM Fazendas WHERE IdFazenda = @Id";
            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("Id", id);
                comando.ExecuteNonQuery();
            }
        }

        // 5. CONSULTAR UM ESPECÍFICO (Para a tela de Editar)
        public override FazendaViewModel Select(int id)
        {
            string sql = "SELECT * FROM Fazendas WHERE IdFazenda = @Id";
            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("Id", id);
                using (SqlDataReader registro = comando.ExecuteReader())
                {
                    if (registro.Read())
                    {
                        return MontarFazenda(registro);
                    }
                }
            }
            return null;
        }

        // 6. LISTAR TODAS
        public override List<FazendaViewModel> List()
        {
            List<FazendaViewModel> lista = new List<FazendaViewModel>();
            string sql = "SELECT * FROM Fazendas";
            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                using (SqlDataReader registro = comando.ExecuteReader())
                {
                    while (registro.Read())
                    {
                        lista.Add(MontarFazenda(registro));
                    }
                }
            }
            return lista;
        }

        public List<FazendaViewModel> ListPorCnpj(string cnpj)
        {
            List<FazendaViewModel> lista = new List<FazendaViewModel>();
            string sql = "SELECT * FROM Fazendas WHERE Cnpj = @Cnpj";
            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("@Cnpj", cnpj);
                using (SqlDataReader registro = comando.ExecuteReader())
                {
                    while (registro.Read())
                        lista.Add(MontarFazenda(registro));
                }
            }
            return lista;
        }

        // MÉTODO AUXILIAR
        private FazendaViewModel MontarFazenda(SqlDataReader registro)
        {
            return new FazendaViewModel
            {
                Id = Convert.ToInt32(registro["IdFazenda"]),
                NomeFantasia = registro["NomeFantasia"].ToString(),
                Cnpj = registro["Cnpj"].ToString()
            };
        }
    }
}