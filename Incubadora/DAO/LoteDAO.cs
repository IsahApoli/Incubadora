using Incubadora.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Incubadora.DAO
{
    public class LoteDAO : PadraoDAO<LoteViewModel>
    {
        protected override void SetTabela()
        {
            Tabela = "Lotes";
        }

        protected override SqlParameter[] CriaParametros(LoteViewModel model)
        {
            SqlParameter[] parametros = new SqlParameter[7];

            parametros[0] = new SqlParameter("Id", model.Id);
            parametros[1] = new SqlParameter("CodigoLote", model.CodigoLote);
            parametros[2] = new SqlParameter("QuantidadeOvos", model.QuantidadeOvos);
            parametros[3] = new SqlParameter("DataEntrada", model.DataEntrada);
            parametros[4] = new SqlParameter("StatusLote", model.StatusLote);
            parametros[5] = new SqlParameter("EspecieId", model.EspecieId);
            parametros[6] = new SqlParameter("IncubadoraId", model.IncubadoraId);

            return parametros;
        }

        protected override LoteViewModel MontaModel(DataRow registro)
        {
            LoteViewModel lote = new LoteViewModel();

            lote.Id = Convert.ToInt32(registro["Id"]);
            lote.CodigoLote = registro["CodigoLote"].ToString();
            lote.QuantidadeOvos = Convert.ToInt32(registro["QuantidadeOvos"]);
            lote.DataEntrada = Convert.ToDateTime(registro["DataEntrada"]);
            lote.StatusLote = registro["StatusLote"].ToString();
            lote.EspecieId = Convert.ToInt32(registro["EspecieId"]);
            lote.IncubadoraId = Convert.ToInt32(registro["IncubadoraId"]);

            if (registro.Table.Columns.Contains("NomeEspecie"))
                lote.NomeEspecie = registro["NomeEspecie"].ToString();

            if (registro.Table.Columns.Contains("NomeIncubadora"))
                lote.NomeIncubadora = registro["NomeIncubadora"].ToString();

            return lote;
        }
    }
}
