using Microsoft.AspNetCore.Mvc;
using agendaEFD.Data;
using agendaEFD.Models;

namespace agenda.Controllers
{
    public class CustomerController : Controller
    {
        private readonly agendaContext _db;

        public CustomerController(agendaContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Customer> Customers = _db.Customers;
            return View(Customers);
        }

        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Customer customer)
        {
            if (customer.Firstname == customer.Lastname)
            {
                ModelState.AddModelError("NotEquals", "le champs prénom doit etre différent du nom de famille");
            }

            if (ModelState.IsValid)
            {

                _db.Customers.Add(customer);
                _db.SaveChanges();

                TempData["success"] = "Le client a bien été ajouté";

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            var customer = _db.Customers.Find(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Customer cust)
        {
            if (cust.Firstname == cust.Lastname)
            {
                ModelState.AddModelError("NotEquals", "le champs prénom doit etre différent du nom de famille");
            }



            if (ModelState.IsValid)
            {

                _db.Customers.Update(cust);
                _db.SaveChanges();

                TempData["success"] = "Le client a bien été modifié";

                return RedirectToAction("Index");
                // ViewData["dump"] = cust;

                // return View();
            }


            return View();

        }

        public IActionResult Profil(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            var customer = _db.Customers.Find(id);

            if (customer == null)
            {
                return NotFound();
            }

            ViewBag.CustomerId = id;
            return View(customer);
        }

        // pour aller sur la view Delete 
        public IActionResult Delete(int? id)
        {
            // si j'ai pas d'id je retourne a la liste des articles
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            var customer = _db.Customers.Find(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        public IActionResult Delete(Customer cust)
        {
            _db.Customers.Remove(cust);
            _db.SaveChanges();

            TempData["success"] = "Le client a bien été supprimé";

            return RedirectToAction("Index");
        }
    }
}

