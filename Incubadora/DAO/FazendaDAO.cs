using Incubadora.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Incubadora.DAO
{
    public class FazendaDAO : PadraoDAO<FazendaViewModel>
    {
        protected override void SetTabela()
        {
            Tabela = "Fazenda";
        }

        protected override SqlParameter[] CriaParametros(FazendaViewModel model)
        {
            SqlParameter[] parametros = new SqlParameter[7];

            parametros[0] = new SqlParameter("IdFazenda", model.Id);
            parametros[1] = new SqlParameter("NomeFazenda", model.NomeFazenda);
            parametros[2] = new SqlParameter("CEP", model.CEP);
            parametros[3] = new SqlParameter("RuaFazenda", model.RuaFazenda);
            parametros[4] = new SqlParameter("BairroFazenda", model.BairroFazenda);
            parametros[5] = new SqlParameter("CidadeFazenda", model.CidadeFazenda);
            parametros[6] = new SqlParameter("EstadoFazenda", model.EstadoFazenda);

            return parametros;
        }

        protected override FazendaViewModel MontaModel(DataRow registro)
        {
            FazendaViewModel fazenda = new FazendaViewModel();

            fazenda.Id = Convert.ToInt32(registro["IdFazenda"]);
            fazenda.NomeFazenda = registro["NomeFazenda"].ToString();
            fazenda.CEP = registro["CEP"].ToString();
            fazenda.RuaFazenda = registro["RuaFazenda"].ToString();
            fazenda.BairroFazenda = registro["BairroFazenda"].ToString();
            fazenda.CidadeFazenda = registro["CidadeFazenda"].ToString();
            fazenda.EstadoFazenda = registro["EstadoFazenda"].ToString();

            return fazenda;
        }
    }
}
