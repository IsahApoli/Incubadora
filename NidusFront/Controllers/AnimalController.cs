using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using NidusFront.Models;
using NidusFront.DAOs;

namespace NidusFront.Controllers
{
    public class AnimalController : Controller
    {
        private readonly AnimalDAO _dao;

        public AnimalController()
        {
            _dao = new AnimalDAO();
        }

        // 1. TELA DE LISTAGEM
        public IActionResult Animais()
        {
            try
            {
                var lista = _dao.List();
                return View("Animais", lista);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // 2. TELA DE CRIAR
        [HttpGet]
        public IActionResult CriarAnimal()
        {
            return View("CriarAnimal", new AnimalViewModel());
        }

        // 3. TELA DE EDITAR — bloqueia cliente de editar animal que não é dele
        [HttpGet]
        public IActionResult EditarAnimal(int id)
        {
            try
            {
                var animal = _dao.Select(id);
                if (animal == null)
                    return RedirectToAction("Animais");

                string perfil = HttpContext.Session.GetString("UsuarioPerfil") ?? "";
                int idUsuario = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));

                // Cliente só pode editar animais que ele mesmo criou
                if (perfil != "Admin" && animal.IdUsuario != idUsuario)
                    return RedirectToAction("Animais");

                return View("EditarAnimal", animal);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // 4. AÇÃO DE SALVAR
        [HttpPost]
        public IActionResult SalvarAnimal(AnimalViewModel model)
        {
            try
            {
                if (Request.Form.ContainsKey("TempMin"))
                {
                    string rawTempMin = Request.Form["TempMin"].ToString();
                    if (!string.IsNullOrEmpty(rawTempMin))
                        model.TempMin = Convert.ToDecimal(rawTempMin.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                }

                if (Request.Form.ContainsKey("TempMax"))
                {
                    string rawTempMax = Request.Form["TempMax"].ToString();
                    if (!string.IsNullOrEmpty(rawTempMax))
                        model.TempMax = Convert.ToDecimal(rawTempMax.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                }

                ModelState.Remove("TempMin");
                ModelState.Remove("TempMax");

                if (!ModelState.IsValid)
                {
                    if (model.Id == 0) return View("CriarAnimal", model);
                    else return View("EditarAnimal", model);
                }

                if (model.FotoUpload != null && model.FotoUpload.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        model.FotoUpload.CopyTo(ms);
                        model.FotoEmBytes = ms.ToArray();
                    }
                }
                else if (model.Id > 0)
                {
                    var animalAntigo = _dao.Select(model.Id);
                    model.FotoEmBytes = animalAntigo?.FotoEmBytes;
                }

                string perfil = HttpContext.Session.GetString("UsuarioPerfil") ?? "";
                int idUsuario = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));

                if (model.Id == 0)
                {
                    // Novo animal: se for cliente, vincula ao usuário e marca como Customizado
                    if (perfil != "Admin")
                    {
                        model.IdUsuario = idUsuario;
                        model.Tipo = "Customizado";
                    }
                    _dao.Insert(model);
                }
                else
                {
                    // Edição: bloqueia cliente de editar animal de outro usuário
                    if (perfil != "Admin")
                    {
                        model.IdUsuario = idUsuario;
                    }
                    _dao.Update(model);
                }

                return RedirectToAction("Animais");
            }
            catch (Exception ex)
            {
                return Content("ERRO: " + ex.Message);
            }
        }

        // 5. AÇÃO DE EXCLUIR — bloqueia cliente de excluir animal que não é dele
        public IActionResult ExcluirAnimal(int id)
        {
            try
            {
                string perfil = HttpContext.Session.GetString("UsuarioPerfil") ?? "";
                int idUsuario = Convert.ToInt32(HttpContext.Session.GetString("UsuarioId"));

                if (perfil != "Admin")
                {
                    var animal = _dao.Select(id);
                    if (animal == null || animal.IdUsuario != idUsuario)
                        return RedirectToAction("Animais");
                }

                _dao.Delete(id);
                return RedirectToAction("Animais");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}