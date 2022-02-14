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
               "INNER JOIN brokers ON brokers.idBroker = appointments.idBroker " +
               "INNER JOIN customers ON customers.idCustomer = appointments.idCustomer" */

            // On aurait même pu faire "from a in _db.Appointments" directement au lieu d’utiliser les IEnumerable

            /*
            string connectionString;
            SqlConnection cnn;

            connectionString = @"Server=DESKTOP-6IV8GIO\\SQLEXPRESS;Database=agendaLogin;Trusted_Connection=True;MultipleActiveResultSets=true";

            cnn = new SqlConnection(connectionString);
            cnn.Open();

            SqlCommand command;
            SqlDataReader dataReader;
            string sql, Output = "";

            sql = "SELECT appointments.idAppointment, appointments.dateHour, appointments.subject, brokers.firstname, " +
               "brokers.lastname, customers.firstname, customers.lastname FROM appointments " +
               "INNER JOIN brokers ON brokers.idBroker = appointments.idBroker " +
               "INNER JOIN customers ON customers.idCustomer = appointments.idCustomer";


            command = new SqlCommand(sql);

            dataReader = command.ExecuteReader();

            List<string> AppointmentViewModel = new List<string>();

            while (dataReader.Read())
            {
                AppointmentViewModel.Add(dataReader.GetValue(0) + "");
                AppointmentViewModel.Add(dataReader.GetValue(1) + "");
                AppointmentViewModel.Add(dataReader.GetValue(2) + "");
                AppointmentViewModel.Add(dataReader.GetValue(3) + "");
                AppointmentViewModel.Add(dataReader.GetValue(4) + "");
                AppointmentViewModel.Add(dataReader.GetValue(5) + "");
                AppointmentViewModel.Add(dataReader.GetValue(6) + "");
            }


            cnn.Close();
            */

            IEnumerable<BrokerCustomerAppointment> AppointmentViewModel = from a in Appointments.ToList()
                                                                          join c in Customers.ToList() on a.IdCustomer equals c.IdCustomer
                                                                          join b in Brokers.ToList() on a.IdBroker equals b.IdBroker
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

            // return View();
        }

        // code ci-dessous pas encore mis en place ----------------------------------
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            var appoint = _db.Appointments.Find(id);

            if (appoint == null)
            {
                return NotFound();
            }

            /*
            IEnumerable<BrokerCustomerAppointment> AppointmentViewModel = from a in _db.Appointments
                      join c in _db.Customers on a.IdCustomer equals c.IdCustomer
                      join b in _db.Brokers on a.IdBroker equals b.IdBroker
                      where a.IdAppointment == id
                      select new BrokerCustomerAppointment { AppointmentVm = a, CustomerVm = c, BrokerVm = b };
            */

            // select and from clause in C#
            // ViewBag.Brokers = from b in _db.Brokers select b.Lastname;
            // ViewBag.Customers = from c in _db.Customers select c.Lastname; 

            // Where method in C#
            // var result = _db.Customers.Where(customer => customer.Firstname.StartsWith(id) );

            // ViewBag.DateHour = from a in _db.Appointments where a.IdAppointment == id select a.DateHour;
            // ViewBag.Subject = from a in _db.Appointments where a.IdAppointment == id select a.Subject;
            /* ViewData["subject"] = from a in _db.Appointments where a.IdAppointment == id select a.Subject;
             ViewData["datehour"] = from a in _db.Appointments where a.IdAppointment == id select a.DateHour; */

            ViewBag.Brokers = new SelectList(_db.Brokers, "IdBroker", "Lastname");
            ViewBag.Customers = new SelectList(_db.Customers, "IdCustomer", "Lastname");

            return View(appoint);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        // on peut aussi mettre "int Id" dans les parametres
        // et ca récupère l'id depuis le form sans avoir besoin d'ajouter un hidden dans le form
        public IActionResult Edit(Appointment appoint)
        {

            //if (ModelState.IsValid)
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
            }
            else
            {
                _db.Appointments.Update(appoint);
                _db.SaveChanges();

                TempData["success"] = "Le rendez-vous a bien été modifié";

                return RedirectToAction("Index");
            }

            
            //}


            // return View();

        }

        // pour aller sur la view Delete 
        public IActionResult Delete(int? id)
        {
            // si j'ai pas d'id je retourne a la liste des articles
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            var appoint = _db.Appointments.Find(id);

            if (appoint == null)
            {
                return NotFound();
            }
            /*
            TempData["brokers"] = from b in _db.Brokers where appoint.IdBroker == b.IdBroker select b.Lastname;
            TempData["customers"] = from c in _db.Customers where appoint.IdCustomer == c.IdCustomer select c.Lastname;*/
            ViewBag.Brokers = _db.Brokers.Find(appoint.IdBroker);
            ViewBag.Customers = _db.Customers.Find(appoint.IdCustomer);

            return View(appoint);
        }

        [HttpPost]
        public IActionResult Delete(Appointment appoint)
        {
            _db.Appointments.Remove(appoint);
            _db.SaveChanges();

            TempData["success"] = "Le rendez-vous a bien été supprimé";

            return RedirectToAction("Index");
        }
    }
}
