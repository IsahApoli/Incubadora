using Incubadora.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Incubadora.DAO
{
    public class UsuarioDAO : PadraoDAO<UsuarioViewModel>
    {
        protected override void SetTabela()
        {
            Tabela = "Usuarios";
        }

        protected override SqlParameter[] CriaParametros(UsuarioViewModel model)
        {
            object fazendaIdValue = model.FazendaId ?? (object)DBNull.Value;

            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("id", model.Id),
                new SqlParameter("nome", model.Nome),
                new SqlParameter("email", model.Email),
                new SqlParameter("senha", model.Senha), // Recomenda-se salvar o Hash [6]
                new SqlParameter("tipoUsuario", model.TipoUsuario),
                new SqlParameter("fazendaId", fazendaIdValue)
            };

            return parametros;
        }

        protected override UsuarioViewModel MontaModel(DataRow registro)
        {
            UsuarioViewModel u = new UsuarioViewModel();
            u.Id = Convert.ToInt32(registro["id"]);
            u.Nome = registro["nome"].ToString();
            u.Email = registro["email"].ToString();
            u.Senha = registro["senha"].ToString();
            u.TipoUsuario = registro["tipoUsuario"].ToString();

            if (registro["fazendaId"] != DBNull.Value)
                u.FazendaId = Convert.ToInt32(registro["fazendaId"]);

            return u;
        }
    }
}
