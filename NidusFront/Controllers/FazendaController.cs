using Microsoft.AspNetCore.Mvc;
using System;
using NidusFront.Models;
using NidusFront.DAOs;

namespace NidusFront.Controllers
{
    public class FazendaController : Controller
    {
        private readonly FazendaDAO _dao;

        // Construtor: Prepara o DAO para conversar com o banco
        public FazendaController()
        {
            _dao = new FazendaDAO();
        }

        // 1. TELA DE LISTAGEM (Abre a tabela de fazendas)
        public IActionResult Fazendas()
        {
            try
            {
                string perfil = HttpContext.Session.GetString("UsuarioPerfil") ?? "";
                List<FazendaViewModel> lista;

                if (perfil == "Admin")
                {
                    // Admin vê todas as fazendas
                    lista = _dao.List();
                }
                else
                {
                    // Cliente vê apenas as fazendas do seu CNPJ
                    UsuarioDAO usuarioDao = new UsuarioDAO();
                    int idUsuario = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));
                    var usuario = usuarioDao.Select(idUsuario);
                    lista = _dao.ListPorCnpj(usuario.Cnpj ?? "");
                }

                return View("Fazendas", lista);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // 2. TELA DE CRIAR (Abre o formulário em branco)
        [HttpGet]
        public IActionResult CriarFazenda()
        {
            return View("CriarFazenda", new FazendaViewModel());
        }

        // 3. TELA DE EDITAR (Busca os dados da fazenda e preenche o formulário)
        [HttpGet]
        public IActionResult EditarFazenda(int id)
        {
            try
            {
                var fazenda = _dao.Select(id);
                if (fazenda == null)
                    return RedirectToAction("Fazendas");

                return View("EditarFazenda", fazenda);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // 4. AÇÃO DE SALVAR (Executa o Insert ou o Update no SQL Server)
        [HttpPost]
        public IActionResult SalvarFazenda(FazendaViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    if (model.Id == 0) return View("CriarFazenda", model);
                    else return View("EditarFazenda", model);
                }

                // Limpa formatação do CNPJ para salvar apenas os números no banco
                if (!string.IsNullOrEmpty(model.Cnpj))
                {
                    model.Cnpj = model.Cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
                }

                // Se o Id for 0 cria uma nova, se for maior atualiza a existente
                if (model.Id == 0)
                {
                    _dao.Insert(model);
                }
                else
                {
                    _dao.Update(model);
                }

                return RedirectToAction("Fazendas");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // 5. AÇÃO DE EXCLUIR
        public IActionResult ExcluirFazenda(int id)
        {
            try
            {
                _dao.Delete(id);
                return RedirectToAction("Fazendas");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
