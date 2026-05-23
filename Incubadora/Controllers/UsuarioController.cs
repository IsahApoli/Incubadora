using Incubadora.DAO;
using Incubadora.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Incubadora.Controllers
{
    public class UsuarioController : PadraoController<UsuarioViewModel>
    {
        public UsuarioController()
        {
            DAO = new UsuarioDAO();
            GeraProximoId = true;
        }

        /// <summary>
        /// Prepara a lista de fazendas para carregar a Combo Box no formulário
        /// </summary>
        private void PreparaListaFazendasParaCombo()
        {
            FazendaDAO fazendaDao = new FazendaDAO();
            var fazendas = fazendaDao.Listagem();
            List<SelectListItem> listaFazendas = new List<SelectListItem>();

            listaFazendas.Add(new SelectListItem("Selecione uma fazenda...", "0"));

            foreach (var f in fazendas)
            {
                SelectListItem item = new SelectListItem(f.NomeFazenda, f.Id.ToString());
                listaFazendas.Add(item);
            }

            ViewBag.Fazendas = listaFazendas;

            List<SelectListItem> tipos = new List<SelectListItem>
            {
                new SelectListItem("Selecione o nível...", "0"),
                new SelectListItem("Administrador Master", "ADMIN_EMPRESA"),
                new SelectListItem("Administrador de Fazenda", "ADMIN_FAZENDA"),
                new SelectListItem("Funcionário Operacional", "FUNCIONARIO")
            };
            ViewBag.TiposUsuario = tipos;
        }

        /// <summary>
        /// Sobrescreve o método ancestral para injetar as listas das combos na View [5]
        /// </summary>
        protected override void PreencheDadosParaView(string Operacao, UsuarioViewModel model)
        {
            base.PreencheDadosParaView(Operacao, model);
            PreparaListaFazendasParaCombo();
        }

        /// <summary>
        /// Implementa as validações de regras de negócio específicas para Usuários [6, 7]
        /// </summary>
        protected override void ValidaDados(UsuarioViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao); // Mantém a validação de ID básica [2]

            if (string.IsNullOrEmpty(model.Nome))
                ModelState.AddModelError("Nome", "O nome é obrigatório.");

            if (string.IsNullOrEmpty(model.Email))
                ModelState.AddModelError("Email", "O e-mail é obrigatório.");

            if (string.IsNullOrEmpty(model.Senha))
                ModelState.AddModelError("Senha", "A senha é obrigatória.");

            if (model.TipoUsuario == "0")
                ModelState.AddModelError("TipoUsuario", "Selecione um nível de permissão.");

            // Regra de Negócio Crucial: Usuários ADMIN_FAZENDA e FUNCIONARIO precisam de uma FazendaId [4, 8]
            if (model.TipoUsuario != "ADMIN_EMPRESA" && (model.FazendaId == null || model.FazendaId == 0))
            {
                ModelState.AddModelError("FazendaId", "Este tipo de usuário exige a vinculação com uma fazenda.");
            }
        }
    }

}
