using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using NidusFront.Models;

namespace NidusFront.DAOs
{
    public class AnimalDAO : PadraoDAO<AnimalViewModel>
    {
        // 1. MAPEAMENTO DE PARÂMETROS
        protected override SqlParameter[] CreateParameters(AnimalViewModel model)
        {
            return new SqlParameter[]
            {
                new SqlParameter("Id", model.Id),
                new SqlParameter("NomeEspecie", string.IsNullOrEmpty(model.NomeEspecie) ? DBNull.Value : (object)model.NomeEspecie),
                new SqlParameter("TempMin", model.TempMin),
                new SqlParameter("TempMax", model.TempMax),
                new SqlParameter("UmidMin", model.UmidMin),
                new SqlParameter("UmidMax", model.UmidMax),
                new SqlParameter("LuzMin", model.LuzMin),
                new SqlParameter("LuzMax", model.LuzMax),
                new SqlParameter("Tipo", string.IsNullOrEmpty(model.Tipo) ? DBNull.Value : (object)model.Tipo),
                new SqlParameter("Foto", System.Data.SqlDbType.VarBinary) { Value = model.FotoEmBytes ?? (object)DBNull.Value },
                // Dono do animal: null = padrão do sistema, preenchido = pertence ao cliente
                new SqlParameter("IdUsuario", model.IdUsuario.HasValue ? (object)model.IdUsuario.Value : DBNull.Value)
            };
        }

        // 2. INSERIR
        public override void Insert(AnimalViewModel model)
        {
            string sql = "INSERT INTO Animais (NomeEspecie, TempMin, TempMax, UmidMin, UmidMax, LuzMin, LuzMax, Tipo, Foto, IdUsuario) " +
                         "VALUES (@NomeEspecie, @TempMin, @TempMax, @UmidMin, @UmidMax, @LuzMin, @LuzMax, @Tipo, @Foto, @IdUsuario)";
            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddRange(CreateParameters(model));
                comando.ExecuteNonQuery();
            }
        }

        // 3. ATUALIZAR
        public override void Update(AnimalViewModel model)
        {
            string sql = "UPDATE Animais SET NomeEspecie = @NomeEspecie, TempMin = @TempMin, TempMax = @TempMax, " +
                         "UmidMin = @UmidMin, UmidMax = @UmidMax, LuzMin = @LuzMin, LuzMax = @LuzMax, Tipo = @Tipo, Foto = @Foto " +
                         "WHERE IdAnimal = @Id";
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
            string sql = "DELETE FROM Animais WHERE IdAnimal = @Id";
            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("Id", id);
                comando.ExecuteNonQuery();
            }
        }

        // 5. CONSULTAR UM ESPECÍFICO
        public override AnimalViewModel Select(int id)
        {
            string sql = "SELECT * FROM Animais WHERE IdAnimal = @Id";
            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("Id", id);
                using (SqlDataReader registro = comando.ExecuteReader())
                {
                    if (registro.Read())
                        return MontarAnimal(registro);
                }
            }
            return null;
        }

        // 6. LISTAR TODOS
        public override List<AnimalViewModel> List()
        {
            List<AnimalViewModel> lista = new List<AnimalViewModel>();
            string sql = "SELECT * FROM Animais";
            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                using (SqlDataReader registro = comando.ExecuteReader())
                {
                    while (registro.Read())
                        lista.Add(MontarAnimal(registro));
                }
            }
            return lista;
        }

        // MÉTODO AUXILIAR PARA MONTAR O OBJETO
        private AnimalViewModel MontarAnimal(SqlDataReader registro)
        {
            return new AnimalViewModel
            {
                Id = Convert.ToInt32(registro["IdAnimal"]),
                NomeEspecie = registro["NomeEspecie"].ToString(),
                TempMin = Convert.ToDecimal(registro["TempMin"]),
                TempMax = Convert.ToDecimal(registro["TempMax"]),
                UmidMin = Convert.ToInt32(registro["UmidMin"]),
                UmidMax = Convert.ToInt32(registro["UmidMax"]),
                LuzMin = Convert.ToInt32(registro["LuzMin"]),
                LuzMax = Convert.ToInt32(registro["LuzMax"]),
                Tipo = registro["Tipo"].ToString(),
                FotoEmBytes = registro["Foto"] != DBNull.Value ? (byte[])registro["Foto"] : null,
                // Lê o dono do animal (null = padrão do sistema)
                IdUsuario = registro["IdUsuario"] != DBNull.Value ? Convert.ToInt32(registro["IdUsuario"]) : (int?)null
            };
        }
    }
}