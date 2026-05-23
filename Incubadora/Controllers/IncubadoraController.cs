using Incubadora.DAO;
using Incubadora.Models;
using Microsoft.AspNetCore.Mvc;

namespace Incubadora.Controllers
{
    public class IncubadoraController : Controller
    {
        public IActionResult Index()
        {
            IncubadoraDAO dao = new IncubadoraDAO();
            var lista = dao.Listagem();

            return View(lista);
        }

        public IActionResult Create()
        {
            IncubadoraViewModel incubadora = new IncubadoraViewModel();
            incubadora.DataInstalacao = DateTime.Today;

            return View("Form", incubadora);
        }

        public IActionResult Edit(int id)
        {
            IncubadoraDAO dao = new IncubadoraDAO();
            IncubadoraViewModel incubadora = dao.Consulta(id);

            return View("Form", incubadora);
        }

        public IActionResult Delete(int id)
        {
            IncubadoraDAO dao = new IncubadoraDAO();
            dao.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Save(IncubadoraViewModel incubadora)
        {
            IncubadoraDAO dao = new IncubadoraDAO();

            if (incubadora.Id == 0)
                dao.Insert(incubadora);
            else
                dao.Update(incubadora);

            return RedirectToAction("Index");
        }
    }
}
