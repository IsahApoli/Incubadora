using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using NidusFront.Models;
using NidusFront.DAOs;
using System.Collections.Generic;

namespace NidusFront.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioDAO _dao;

        public UsuarioController()
        {
            _dao = new UsuarioDAO();
        }

        public IActionResult Logins()
        {
            if (HttpContext.Session.GetString("UsuarioLogado") == null)
                return RedirectToAction("Index", "Home");
            if (HttpContext.Session.GetString("UsuarioPerfil") != "Admin")
                return RedirectToAction("Fazendas", "Fazenda");
            var lista = _dao.List();
            return View("Logins", lista);
        }

        [HttpGet]
        public IActionResult CriarLogin()
        {
            return View("CriarLogin", new UsuarioViewModel());
        }

        [HttpGet]
        public IActionResult EditarLogin(int id)
        {
            var usuario = _dao.Select(id);
            if (usuario == null) return RedirectToAction("Logins");
            return View("EditarLogin", usuario);
        }

        [HttpPost]
        public IActionResult SalvarLogin(UsuarioViewModel model)
        {
            if (!ModelState.IsValid)
                return model.Id == 0 ? View("CriarLogin", model) : View("EditarLogin", model);

            if (!string.IsNullOrEmpty(model.Celular))
                model.Celular = model.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

            if (model.Id == 0) _dao.Insert(model);
            else _dao.Update(model);

            return RedirectToAction("Logins");
        }

        public IActionResult ExcluirLogin(int id)
        {
            _dao.Delete(id);
            return RedirectToAction("Logins");
        }

        [HttpGet]
        public IActionResult EditarPerfil()
        {
            string? idSessao = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(idSessao)) return RedirectToAction("Index", "Home");
            var usuario = _dao.Select(Convert.ToInt32(idSessao));
            return View("EditarPerfil", usuario);
        }

        [HttpPost]
        public IActionResult SalvarPerfil(UsuarioViewModel model, IFormFile? FotoFile)
        {
            string? idSessao = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(idSessao)) return RedirectToAction("Index", "Home");

            var usuario = _dao.Select(Convert.ToInt32(idSessao));
            usuario.Nome = model.Nome;
            usuario.Celular = (model.Celular ?? "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

            if (FotoFile != null && FotoFile.Length > 0)
            {
                using (var stream = new System.IO.MemoryStream())
                {
                    FotoFile.CopyTo(stream);
                    usuario.Foto = stream.ToArray();
                }
            }

            _dao.Update(usuario);

            // Atualiza a sessão para refletir imediatamente na sidebar
            HttpContext.Session.SetString("UsuarioNome", usuario.Nome ?? "");
            if (usuario.Foto != null && usuario.Foto.Length > 0)
                HttpContext.Session.SetString("UsuarioFoto", Convert.ToBase64String(usuario.Foto));
            else
                HttpContext.Session.Remove("UsuarioFoto");

            return RedirectToAction("Fazendas", "Fazenda");
        }
    }
}