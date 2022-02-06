using Microsoft.AspNetCore.Mvc;
using agendaEFD.Data;
using agendaEFD.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace agendaEFD.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly agendaContext _db;

        public AppointmentController(agendaContext db)
        {
            _db = db;
        }
    
        public IActionResult Index()
        {
            IEnumerable<Appointment> Appointments = _db.Appointments;
            IEnumerable<Broker> Brokers = _db.Brokers;
            IEnumerable<Customer> Customers = _db.Customers;

            /*
           "SELECT appointments.idAppointment, appointments.dateHour, appointments.subject, brokers.firstname, " +
               "brokers.lastname, customers.firstname, customers.lastname FROM appointments " +
               "INNER JOIN brokers ON brokers.idBroker = appointments.idAppointment " +
               "INNER JOIN customers ON customers.idCustomer = appointments.idAppointment" */

            // On aurait même pu faire "from a in _db.Appointments" directement au lieu d’utiliser les IEnumerable

            IEnumerable<BrokerCustomerAppointment> AppointmentViewModel = from a in Appointments.ToList()
                                                                          join c in Customers.ToList() on a.IdAppointment equals c.IdCustomer
                                                                          join b in Brokers.ToList() on a.IdAppointment equals b.IdBroker
                                                                          select new BrokerCustomerAppointment { AppointmentVm = a, CustomerVm = c, BrokerVm = b };

            return View(AppointmentViewModel);
        }



        public IActionResult Add(int? id)
        {
            if (id == null || id == 0)
            {
                ViewBag.Brokers = new SelectList(_db.Brokers, "IdBroker", "Lastname");
                ViewBag.Customers = new SelectList(_db.Customers, "IdCustomer", "Lastname");
                return View();
            }

            var broker = _db.Brokers.Find(id);

            if (broker == null)
            {
                return NotFound();
            }

            List<Broker> brokers = new List<Broker>();
            brokers.Add(broker);

            ViewBag.Brokers = new SelectList(brokers, "IdBroker", "Lastname");
            ViewBag.Customers = new SelectList(_db.Customers, "IdCustomer", "Lastname");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Appointment appoint)
        {

            //if (ModelState.IsValid)  On ne peut pas utiliser ModelState ici, pour verifier si tous les champs sont bien remplis
            //{
            
            if (appoint.IdBroker == 0)
            {
                TempData["success"] = "Erreur! Vous n'avez pas séléctionné un courtier !";

                return RedirectToAction("Index");
            }
            else if (appoint.IdCustomer == 0)
            {
                TempData["success"] = "Erreur! Vous n'avez pas séléctionné un client !";

                return RedirectToAction("Index");
            } else
            {
                _db.Appointments.Add(appoint);
                _db.SaveChanges();

                TempData["success"] = "Le rendez-vous a bien été ajouté";

                return RedirectToAction("Index");
            }

            
            //}

            return View();
        }
    }
}
