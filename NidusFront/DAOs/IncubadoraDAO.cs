using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using NidusFront.Models;

namespace NidusFront.DAOs
{
    public class IncubadoraDAO : PadraoDAO<IncubadoraViewModel>
    {
        protected override SqlParameter[] CreateParameters(IncubadoraViewModel model)
        {
            return new SqlParameter[]
            {
                new SqlParameter("Id", model.Id),
                new SqlParameter("NomeIncubadora", string.IsNullOrEmpty(model.NomeIncubadora) ? DBNull.Value : (object)model.NomeIncubadora),
                new SqlParameter("QuantidadeOvos", model.QuantidadeOvos),
                new SqlParameter("Status", string.IsNullOrEmpty(model.Status) ? DBNull.Value : (object)model.Status),
                new SqlParameter("IdAnimal", model.IdAnimal),
                new SqlParameter("IdUsuario", model.IdUsuario),
                new SqlParameter("IdFazenda", model.IdFazenda)
            };
        }

        public override void Insert(IncubadoraViewModel model)
        {
            string sql = "INSERT INTO Incubadoras (NomeIncubadora, QuantidadeOvos, Status, IdAnimal, IdUsuario, IdFazenda) " +
                         "VALUES (@NomeIncubadora, @QuantidadeOvos, @Status, @IdAnimal, @IdUsuario, @IdFazenda)";
            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddRange(CreateParameters(model));
                comando.ExecuteNonQuery();
            }
        }

        public override void Update(IncubadoraViewModel model)
        {
            string sql = "UPDATE Incubadoras SET NomeIncubadora = @NomeIncubadora, QuantidadeOvos = @QuantidadeOvos, " +
                         "Status = @Status, IdAnimal = @IdAnimal, IdUsuario = @IdUsuario, IdFazenda = @IdFazenda " +
                         "WHERE IdIncubadora = @Id";
            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddRange(CreateParameters(model));
                comando.ExecuteNonQuery();
            }
        }

        public override void Delete(int id)
        {
            string sql = "DELETE FROM Incubadoras WHERE IdIncubadora = @Id";
            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("Id", id);
                comando.ExecuteNonQuery();
            }
        }

        // Agora lê IdFazenda também para não perder o vínculo no Update
        public override IncubadoraViewModel Select(int id)
        {
            string sql = "SELECT * FROM Incubadoras WHERE IdIncubadora = @Id";
            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("Id", id);
                using (SqlDataReader registro = comando.ExecuteReader())
                {
                    if (registro.Read())
                    {
                        return new IncubadoraViewModel
                        {
                            Id = Convert.ToInt32(registro["IdIncubadora"]),
                            NomeIncubadora = registro["NomeIncubadora"].ToString(),
                            QuantidadeOvos = Convert.ToInt32(registro["QuantidadeOvos"]),
                            Status = registro["Status"].ToString(),
                            IdAnimal = Convert.ToInt32(registro["IdAnimal"]),
                            IdUsuario = Convert.ToInt32(registro["IdUsuario"]),
                            IdFazenda = registro["IdFazenda"] != DBNull.Value ? Convert.ToInt32(registro["IdFazenda"]) : 0
                        };
                    }
                }
            }
            return new IncubadoraViewModel();
        }

        public override List<IncubadoraViewModel> List()
        {
            List<IncubadoraViewModel> lista = new List<IncubadoraViewModel>();
            string sql = "SELECT * FROM Vw_ResumoIncubadoras";
            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                using (SqlDataReader registro = comando.ExecuteReader())
                {
                    while (registro.Read())
                        lista.Add(MontarResumo(registro));
                }
            }
            return lista;
        }

        public List<IncubadoraViewModel> ListPorFazenda(int idFazenda)
        {
            List<IncubadoraViewModel> lista = new List<IncubadoraViewModel>();
            string sql = @"SELECT I.IdIncubadora, I.NomeIncubadora, I.QuantidadeOvos, I.IdFazenda,
                          I.Status AS StatusIncubadora,
                          A.NomeEspecie AS AnimalVinculado,
                          A.Foto AS FotoAnimal,
                          U.Nome AS Responsavel
                   FROM Incubadoras I
                   INNER JOIN Animais A ON I.IdAnimal = A.IdAnimal
                   INNER JOIN Usuarios U ON I.IdUsuario = U.IdUsuario
                   WHERE I.IdFazenda = @IdFazenda";

            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("@IdFazenda", idFazenda);
                using (SqlDataReader registro = comando.ExecuteReader())
                {
                    while (registro.Read())
                    {
                        byte[] fotoBytes = registro["FotoAnimal"] != DBNull.Value ? (byte[])registro["FotoAnimal"] : null;
                        lista.Add(new IncubadoraViewModel
                        {
                            Id = Convert.ToInt32(registro["IdIncubadora"]),
                            NomeIncubadora = registro["NomeIncubadora"].ToString(),
                            QuantidadeOvos = Convert.ToInt32(registro["QuantidadeOvos"]),
                            Status = registro["StatusIncubadora"].ToString(),
                            AnimalVinculado = registro["AnimalVinculado"].ToString(),
                            Responsavel = registro["Responsavel"].ToString(),
                            FotoAnimalBase64 = fotoBytes != null ? Convert.ToBase64String(fotoBytes) : null
                        });
                    }
                }
            }
            return lista;
        }

        private IncubadoraViewModel MontarResumo(SqlDataReader registro)
        {
            return new IncubadoraViewModel
            {
                Id = Convert.ToInt32(registro["IdIncubadora"]),
                NomeIncubadora = registro["NomeIncubadora"].ToString(),
                QuantidadeOvos = Convert.ToInt32(registro["QuantidadeOvos"]),
                Status = registro["StatusIncubadora"].ToString(),
                AnimalVinculado = registro["AnimalVinculado"].ToString(),
                Responsavel = registro["Responsavel"].ToString()
            };
        }
    }
}