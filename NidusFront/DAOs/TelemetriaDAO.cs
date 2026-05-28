using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using NidusFront.Models;

namespace NidusFront.DAOs
{
    public class TelemetriaDAO : PadraoDAO<TelemetriaViewModel>
    {
        protected override SqlParameter[] CreateParameters(TelemetriaViewModel model)
        {
            return new SqlParameter[]
            {
                new SqlParameter("Id", model.Id),
                new SqlParameter("IdIncubadora", model.IdIncubadora),
                // Se a placa não enviar a data, o C# pega o horário exato do servidor
                new SqlParameter("DataHora", model.DataHora == DateTime.MinValue ? DateTime.Now : model.DataHora),
                new SqlParameter("TemperaturaAtual", model.TemperaturaAtual),
                new SqlParameter("UmidadeAtual", model.UmidadeAtual),
                new SqlParameter("StatusGeral", string.IsNullOrEmpty(model.StatusGeral) ? "OK" : (object)model.StatusGeral)
            };
        }

        public override void Insert(TelemetriaViewModel model)
        {
            string sql = "INSERT INTO Telemetria (IdIncubadora, DataHora, TemperaturaAtual, UmidadeAtual, StatusGeral) " +
                         "VALUES (@IdIncubadora, @DataHora, @TemperaturaAtual, @UmidadeAtual, @StatusGeral)";
            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddRange(CreateParameters(model));
                comando.ExecuteNonQuery();
            }
        }

        // =========================================================================
        // MÉTODOS OBRIGATÓRIOS DO PADRÃO DAO (Ficam vazios pois não editamos logs)
        // =========================================================================
        public override void Update(TelemetriaViewModel model) { }
        public override void Delete(int id) { }
        public override TelemetriaViewModel Select(int id) { return new TelemetriaViewModel(); }
        public override List<TelemetriaViewModel> List() { return new List<TelemetriaViewModel>(); }

        // =========================================================================
        // MÉTODO EXCLUSIVO: Busca as últimas 20 leituras para desenhar o gráfico
        // =========================================================================
        public List<TelemetriaViewModel> ListarHistorico(int idIncubadora)
        {
            List<TelemetriaViewModel> lista = new List<TelemetriaViewModel>();
            // Ordena da leitura mais recente para a mais antiga (LIMITADO A 20 PARA O GRÁFICO NÃO TRAVAR)
            string sql = "SELECT TOP 20 * FROM Telemetria WHERE IdIncubadora = @IdIncubadora ORDER BY DataHora DESC";

            using (SqlConnection conexao = HelperDAO.GetConnection())
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.AddWithValue("IdIncubadora", idIncubadora);
                using (SqlDataReader registro = comando.ExecuteReader())
                {
                    while (registro.Read())
                    {
                        lista.Add(new TelemetriaViewModel
                        {
                            Id = Convert.ToInt32(registro["IdTelemetria"]),
                            IdIncubadora = Convert.ToInt32(registro["IdIncubadora"]),
                            DataHora = Convert.ToDateTime(registro["DataHora"]),
                            TemperaturaAtual = Convert.ToDecimal(registro["TemperaturaAtual"]),
                            UmidadeAtual = Convert.ToInt32(registro["UmidadeAtual"]),
                            StatusGeral = registro["StatusGeral"].ToString()
                        });
                    }
                }
            }
            // Inverte a lista para o gráfico desenhar da esquerda (antigo) para a direita (novo)
            lista.Reverse();
            return lista;
        }
    }
}