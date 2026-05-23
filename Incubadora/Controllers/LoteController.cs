using Incubadora.DAO;
using Incubadora.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Incubadora.Controllers
{
    public class LoteController : Controller
    {
        public IActionResult Index()
        {
            LoteDAO dao = new LoteDAO();
            var lista = dao.Listagem();

            return View(lista);
        }

        public IActionResult Create()
        {
            LoteViewModel lote = new LoteViewModel();
            lote.DataEntrada = DateTime.Now;
            lote.StatusLote = "INCUBANDO";

            return View("Form", lote);
        }

        public IActionResult Edit(int id)
        {
            LoteDAO dao = new LoteDAO();
            LoteViewModel lote = dao.Consulta(id);

            return View("Form", lote);
        }

        public IActionResult Delete(int id)
        {
            LoteDAO dao = new LoteDAO();
            dao.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Save(LoteViewModel lote)
        {
            LoteDAO dao = new LoteDAO();

            if (lote.Id == 0)
                dao.Insert(lote);
            else
                dao.Update(lote);

            return RedirectToAction("Index");
        }
    }
}
