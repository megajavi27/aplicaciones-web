using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebClientes.Models;

namespace WebClientes.Controllers
{
    public class ClienteController : Controller
    {
        List<ClienteModel> _lista_Clientes = new List<ClienteModel>() {
            new ClienteModel{
                id = 1,
                Nombres_Cliente="Luis Antonio",
                Apellidos = "Llerena Ocaña",
                Direccion = "Ambato",
                Telefono = "0987654321",
                Correo = "lleroc1@gmail.com"
            },
            new ClienteModel{
                id = 2,
                Nombres_Cliente="Otro Luis",
                Apellidos = "Otro Llerena",
                Direccion = "Quero",
                Telefono = "0999999999",
                Correo = "otro@gmail.com"
            }
        };

        // GET: ClienteController
        public ActionResult Index()
        {
            return View(_lista_Clientes);
        }

        // GET: ClienteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClienteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClienteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClienteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
