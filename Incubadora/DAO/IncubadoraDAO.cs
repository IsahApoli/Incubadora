using Incubadora.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Incubadora.DAO
{
    public class IncubadoraDAO : PadraoDAO<IncubadoraViewModel>
    {
        protected override void SetTabela()
        {
            Tabela = "Incubadoras";
        }

        protected override SqlParameter[] CriaParametros(IncubadoraViewModel model)
        {
            SqlParameter[] parametros = new SqlParameter[8];

            parametros[0] = new SqlParameter("Id", model.Id);
            parametros[1] = new SqlParameter("NomeIncubadora", model.NomeIncubadora);
            parametros[2] = new SqlParameter("Modelo", model.Modelo);
            parametros[3] = new SqlParameter("Localizacao", model.Localizacao);
            parametros[4] = new SqlParameter("Status", model.Status);
            parametros[5] = new SqlParameter("CapacidadeMaxima", model.CapacidadeMaxima);
            parametros[6] = new SqlParameter("DataInstalacao", model.DataInstalacao);
            parametros[7] = new SqlParameter("FazendaId", model.FazendaId);

            return parametros;
        }

        protected override IncubadoraViewModel MontaModel(DataRow registro)
        {
            IncubadoraViewModel incubadora = new IncubadoraViewModel();

            incubadora.Id = Convert.ToInt32(registro["Id"]);
            incubadora.NomeIncubadora = registro["NomeIncubadora"].ToString();
            incubadora.Modelo = registro["Modelo"].ToString();
            incubadora.Localizacao = registro["Localizacao"].ToString();
            incubadora.Status = registro["Status"].ToString();
            incubadora.CapacidadeMaxima = Convert.ToInt32(registro["CapacidadeMaxima"]);
            incubadora.DataInstalacao = Convert.ToDateTime(registro["DataInstalacao"]);
            incubadora.FazendaId = Convert.ToInt32(registro["FazendaId"]);

            return incubadora;
        }
    }
}