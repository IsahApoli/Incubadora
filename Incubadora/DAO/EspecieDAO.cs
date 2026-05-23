using Incubadora.DAO;
using Incubadora.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Emit;
using System.Collections.Generic;

namespace Incubadora.DAO
{
    public class EspecieDAO : PadraoDAO<EspecieViewModel>
    {
        protected override void SetTabela()
        {
            Tabela = "Especies";
        }

        protected override SqlParameter[] CriaParametros(EspecieViewModel model)
        {
            // Tratamento de valor nulo para a imagem [6, 7]
            object imgByte = model.ImagemEmByte ?? (object)DBNull.Value;

            return new SqlParameter[] {
            new SqlParameter("id", model.Id),
            new SqlParameter("nomeEspecie", model.NomeEspecie),
            new SqlParameter("tempMin", model.TemperaturaMin),
            new SqlParameter("tempMax", model.TemperaturaMax),
            new SqlParameter("umidMin", model.UmidadeMin),
            new SqlParameter("umidMax", model.UmidadeMax),
            new SqlParameter("luzIdeal", model.LuminosidadeIdeal),
            new SqlParameter("tempo", model.TempoIncubacao),
            new SqlParameter("imagem", imgByte)
        };
        }

        protected override EspecieViewModel MontaModel(DataRow registro)
        {
            EspecieViewModel e = new EspecieViewModel
            {
                Id = Convert.ToInt32(registro["id"]),
                NomeEspecie = registro["nomeEspecie"].ToString(),
                TemperaturaMin = Convert.ToDecimal(registro["temperaturaMin"]),
                TemperaturaMax = Convert.ToDecimal(registro["temperaturaMax"]),
                UmidadeMin = Convert.ToDecimal(registro["umidadeMin"]),
                UmidadeMax = Convert.ToDecimal(registro["umidadeMax"]),
                LuminosidadeIdeal = Convert.ToDecimal(registro["luminosidadeIdeal"]),
                TempoIncubacao = Convert.ToInt32(registro["tempoIncubacao"])
            };

            // Recupera a imagem do banco se ela não for nula [6]
            if (registro["imagemAnimal"] != DBNull.Value)
                e.ImagemEmByte = registro["imagemAnimal"] as byte[];

            return e;
        }
    }
}