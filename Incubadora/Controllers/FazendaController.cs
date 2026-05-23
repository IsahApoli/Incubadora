using Incubadora.DAO;
using Incubadora.Models;
using Microsoft.AspNetCore.Mvc;

namespace Incubadora.Controllers
{
    public class FazendaController : Controller
    {
        public IActionResult Index()
        {
            FazendaDAO dao = new FazendaDAO();
            var lista = dao.Listagem();

            return View(lista);
        }

        public IActionResult Create()
        {
            FazendaViewModel fazenda = new FazendaViewModel();
            return View("Form", fazenda);
        }

        public IActionResult Edit(int id)
        {
            FazendaDAO dao = new FazendaDAO();
            FazendaViewModel fazenda = dao.Consulta(id);

            return View("Form", fazenda);
        }

        public IActionResult Delete(int id)
        {
            FazendaDAO dao = new FazendaDAO();
            dao.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Save(FazendaViewModel fazenda)
        {
            FazendaDAO dao = new FazendaDAO();

            if (fazenda.Id == 0)
                dao.Insert(fazenda);
            else
                dao.Update(fazenda);

            return RedirectToAction("Index");
        }
    }
}
