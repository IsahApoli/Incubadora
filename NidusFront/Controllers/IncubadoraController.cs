using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using NidusFront.Models;
using NidusFront.DAOs;

namespace NidusFront.Controllers
{
    public class IncubadoraController : Controller
    {
        private readonly IncubadoraDAO _dao;
        private readonly AnimalDAO _animalDao;
        private readonly UsuarioDAO _usuarioDao;

        public IncubadoraController()
        {
            _dao = new IncubadoraDAO();
            _animalDao = new AnimalDAO();
            _usuarioDao = new UsuarioDAO();
        }

        private void CarregarViewBags()
        {
            ViewBag.Animais = _animalDao.List();
            ViewBag.Usuarios = _usuarioDao.List();
        }

        // Pega o idFazenda da sessão com segurança
        private int GetFazendaAtual()
        {
            return Convert.ToInt32(HttpContext.Session.GetString("FazendaAtual") ?? "0");
        }

        // 1. TELA DE LISTAGEM
        public IActionResult Incubadoras(int idFazenda)
        {
            try
            {
                if (HttpContext.Session.GetString("UsuarioLogado") == null)
                    return RedirectToAction("Index", "Home");

                // Salva na sessão para usar nos redirects
                if (idFazenda > 0)
                    HttpContext.Session.SetString("FazendaAtual", idFazenda.ToString());
                else
                    idFazenda = GetFazendaAtual();

                var lista = _dao.ListPorFazenda(idFazenda);
                return View("Incubadoras", lista);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // 2. TELA DE CRIAR
        [HttpGet]
        public IActionResult CriarIncubadora()
        {
            CarregarViewBags();
            return View("CriarIncubadora", new IncubadoraViewModel());
        }

        // 3. TELA DE EDITAR
        [HttpGet]
        public IActionResult EditarIncubadora(int id)
        {
            try
            {
                var incubadora = _dao.Select(id);
                if (incubadora == null || incubadora.Id == 0)
                    return RedirectToAction("Incubadoras", new { idFazenda = GetFazendaAtual() });

                CarregarViewBags();
                return View("EditarIncubadora", incubadora);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // 4. AÇÃO DE SALVAR
        [HttpPost]
        public IActionResult SalvarIncubadora(IncubadoraViewModel model)
        {
            try
            {
                if (model.QuantidadeOvos < 0 || model.QuantidadeOvos > 5)
                    ModelState.AddModelError("QuantidadeOvos", "A capacidade máxima é de 5 ovos.");

                if (!ModelState.IsValid)
                {
                    CarregarViewBags();
                    if (model.Id == 0) return View("CriarIncubadora", model);
                    else return View("EditarIncubadora", model);
                }

                if (model.Id == 0)
                {
                    // Vincula à fazenda atual da sessão
                    model.IdFazenda = GetFazendaAtual();
                    // Vincula ao usuário logado
                    model.IdUsuario = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId") ?? "0");
                    _dao.Insert(model);
                }
                else
                {
                    // Preserva IdFazenda original para não perder o vínculo
                    var original = _dao.Select(model.Id);
                    model.IdFazenda = original.IdFazenda;
                    _dao.Update(model);
                }

                return RedirectToAction("Incubadoras", new { idFazenda = GetFazendaAtual() });
            }
            catch (Exception ex)
            {
                ViewBag.Erro = "Erro ao salvar: " + ex.Message;
                CarregarViewBags();
                return View(model.Id == 0 ? "CriarIncubadora" : "EditarIncubadora", model);
            }
        }

        // 5. AÇÃO DE EXCLUIR
        public IActionResult ExcluirIncubadora(int id)
        {
            try
            {
                _dao.Delete(id);
                return RedirectToAction("Incubadoras", new { idFazenda = GetFazendaAtual() });
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}