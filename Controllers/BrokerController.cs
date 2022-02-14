using Microsoft.AspNetCore.Mvc;
using agendaEFD.Data;
using agendaEFD.Models;

namespace agendaEFD.Controllers
{
    public class BrokerController : Controller
    {
        private readonly agendaContext _db;

        public BrokerController(agendaContext db)
        {
            _db = db;
        }
        /*
        public IActionResult Index()
        {
            IEnumerable<Broker> Brokers = _db.Brokers;
            return View(Brokers);
        } */

        public IActionResult Index(string? searchString)
        {
            IEnumerable<Broker> Brokers = _db.Brokers;

            if (!String.IsNullOrEmpty(searchString))
            {
                var brokers = from c in _db.Brokers select c;
                brokers = brokers.Where(s => s.Lastname!.Contains(searchString) || s.Firstname.Contains(searchString));
                return View(brokers.ToList());
            }

            return View(Brokers);
        }

        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Broker broker)
        {
            if (broker.Firstname == broker.Lastname)
            {
                ModelState.AddModelError("NotEquals", "le champs prénom doit etre différent du nom de famille");
            }

            if (ModelState.IsValid)
            {

                _db.Brokers.Add(broker);
                _db.SaveChanges();

                TempData["success"] = "Le courtier a bien été ajouté";

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
            var broker = _db.Brokers.Find(id);

            if (broker == null)
            {
                return NotFound();
            }

            return View(broker);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        // on peut aussi mettre "int Id" dans les parametres
        // et ca récupère l'id depuis le form sans avoir besoin d'ajouter un hidden dans le form
        public IActionResult Edit(Broker broker) 
        {
            if (broker.Firstname == broker.Lastname)
            {
                ModelState.AddModelError("NotEquals", "le champs prénom doit etre différent du nom de famille");
            }

            // broker.IdBroker = id;

            if (ModelState.IsValid)
            {


                _db.Brokers.Update(broker);
                _db.SaveChanges();

                TempData["success"] = "Le courtier a bien été modifié";

                return RedirectToAction("Index");
            }


            return View();

        }

        public IActionResult Profil(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            var broker = _db.Brokers.Find(id);

            if (broker == null)
            {
                return NotFound();
            }

            ViewBag.BrokerId = id;
            return View(broker);
        }

        // pour aller sur la view Delete 
        public IActionResult Delete(int? id)
        {
            // si j'ai pas d'id je retourne a la liste des articles
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            var broker = _db.Brokers.Find(id);

            if (broker == null)
            {
                return NotFound();
            }

            return View(broker);
        }

        [HttpPost]
        public IActionResult Delete(Broker broker)
        {
            foreach (var n in _db.Appointments.Where(brok => brok.IdBroker == broker.IdBroker).ToArray())
                _db.Appointments.Remove(n);
            /*
            var app = from a in _db.Appointments.ToList() where a.IdBroker == broker.IdBroker select a;
            if(app.Count() != 0) // ou app.Any()
            {
                _db.Appointments.RemoveRange(app);
            }
            */

            _db.Brokers.Remove(broker);
            
            _db.SaveChanges();

            TempData["success"] = "Le courtier a bien été supprimé";

            return RedirectToAction("Index");
        }
    }
}
