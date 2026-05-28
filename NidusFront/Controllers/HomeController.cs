using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NidusFront.Models;
using NidusFront.DAOs;
using System;

namespace NidusFront.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UsuarioLogado") != null)
                return RedirectToAction("Fazendas", "Fazenda");
            return View();
        }

        [HttpPost]
        public IActionResult FazLogin(string Email, string Senha)
        {
            try
            {
                UsuarioDAO dao = new UsuarioDAO();
                var usuario = dao.ValidarLogin(Email, Senha);
                if (usuario != null)
                {
                    HttpContext.Session.SetString("UsuarioLogado", "true");
                    HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
                    HttpContext.Session.SetString("UsuarioNome", usuario.Nome ?? "");
                    HttpContext.Session.SetString("UsuarioPerfil", usuario.Perfil ?? "");

                    // Salva a foto na sessão para exibir na sidebar
                    if (usuario.Foto != null && usuario.Foto.Length > 0)
                        HttpContext.Session.SetString("UsuarioFoto", Convert.ToBase64String(usuario.Foto));

                    return RedirectToAction("Fazendas", "Fazenda");
                }
                else
                {
                    ViewBag.Erro = "E-mail ou senha incorretos!";
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Index");
            }
        }

        public IActionResult Monitoramento()
        {
            if (HttpContext.Session.GetString("UsuarioLogado") == null)
                return RedirectToAction("Index");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}