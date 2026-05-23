using Incubadora.DAO;
using Incubadora.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Incubadora.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FazLogin(string email, string senha)
        {
            try
            {
                UsuarioDAO dao = new UsuarioDAO();

                var usuario = dao.Listagem()
                    .Find(u => u.Email == email && u.Senha == senha);

                if (usuario != null)
                {
                    // Sessão
                    HttpContext.Session.SetString("Logado", "true");
                    HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
                    HttpContext.Session.SetString("TipoUsuario", usuario.TipoUsuario);

                    // ADMIN_EMPRESA
                    if (usuario.TipoUsuario == "ADMIN_EMPRESA")
                    {
                        return RedirectToAction("DashboardEmpresa", "Home");
                    }

                    // ADMIN_FAZENDA ou FUNCIONARIO
                    else if (usuario.TipoUsuario == "ADMIN_FAZENDA" ||
                             usuario.TipoUsuario == "FUNCIONARIO")
                    {
                        if (usuario.FazendaId.HasValue)
                        {
                            HttpContext.Session.SetInt32(
                                "FazendaId",
                                usuario.FazendaId.Value
                            );

                            return RedirectToAction(
                                "DashboardFazenda",
                                "Home"
                            );
                        }
                        else
                        {
                            ViewBag.Erro = "Usuário sem fazenda vinculada!";
                            return View("Index");
                        }
                    }

                    return RedirectToAction("Index", "Home");
                }

                // LOGIN INVÁLIDO
                ViewBag.Erro = "Email ou senha inválidos!";
                return View("Index");
            }
            catch (Exception erro)
            {
                return View("Error",
                    new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}