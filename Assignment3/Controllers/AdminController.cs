using Assignment3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Assignment3.Controllers
{
    public class AdminController : Controller
    {

        private HospitalEntity db = new HospitalEntity();


        // GET: Admin Home Page
        public ActionResult Index()
        {
            ViewModel model = new ViewModel();
            model.Doctors = db.Users.Where(x => x.Role == 1).ToList();
            model.Patients = db.Patients.ToList();
            return validate(View(model));
        }


        // GET: Admin/AddDoctor Page
        public ActionResult AddDoctor()
        {
            return validate(View());
        }


        // POST: Admin/AddDoctor
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDoctor([Bind(Include = "Username,Password,Name,Email,Office,Speciality")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Role = 1;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return validate(View(user));
        }


        // GET: Admin/AddPatient Page
        public ActionResult AddPatient()
        {
            return validate(View());
        }

        // POST: Admin/AddPatient
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPatient([Bind(Include = "Id,Name,Birthdate,Phone,Address")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return validate(View(patient));
        }


        // GET: Admin/EditDoctor/ani
        public ActionResult EditDoctor(string username)
        {
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(username);
            if (user == null)
            {
                return HttpNotFound();
            }
            return validate(View(user));
        }

        // POST: Admin/EditDoctor/ani
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDoctor([Bind(Include = "Username,Password,Name,Email,Office,Speciality")] User user)
        {
            
            if (ModelState.IsValid)
            {
                user.Role = 1;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return validate(View(user));
        }


        // GET: Admin/EditPatient/1
        public ActionResult EditPatient(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return validate(View(patient));
        }


        // POST: Admin/EditPatient/1
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPatient([Bind(Include = "Id,Name,Birthdate,Phone,Address")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return validate(View(patient));
        }


        // GET: Admin/DoctorDetails/ani
        public ActionResult DoctorDetails(string username)
        {
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(username);
            if (user == null)
            {
                return HttpNotFound();
            }
            return validate(View(user));
        }


        // GET: Admin/PatientDetails/5
        public ActionResult PatientDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return validate(View(patient));
        }


        // GET: Admin/DeleteDoctor/ani
        public ActionResult DeleteDoctor(string username)
        {
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(username);
            if (user == null)
            {
                return HttpNotFound();
            }
            return validate(View(user));
        }


        // POST: Admin/DeleteDoctor/ani
        [HttpPost, ActionName("DeleteDoctor")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string username)
        {
            User user = db.Users.Find(username);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Admin/DeletePatient/5
        public ActionResult DeletePatient(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return validate(View(patient));
        }


        // POST: Admin/DeletePatient/5
        [HttpPost, ActionName("DeletePatient")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult AdmitPatient(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewBag.patient = patient;
            List<SelectListItem> doctors = new List<SelectListItem>();
            foreach (var d in db.Users.Where(x => x.Role == 1).ToList())
            {
                doctors.Add(new SelectListItem { Text = d.Name,  Value = d.Username });
            }
            ViewBag.doctors = doctors;
            return validate(View());
        }


        [HttpPost, ActionName("AdmitPatient")]
        [ValidateAntiForgeryToken]
        public ActionResult AdmitConfirm([Bind(Include = "Patient_Id,Doctor_Id,Admission_date")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return validate(View());
        }


        public ActionResult ViewAppointments()
        {
            return validate(View(db.Appointments.ToList()));
        }


        public ActionResult DischargeToday(int id)
        {
            Appointment appointment = db.Appointments.Where(x => x.Id == id).First();
            if (ModelState.IsValid)
            {
                appointment.Discharge_date = DateTime.Today;
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("ViewAppointments");
        }


        public ActionResult Discharge(int id)
        {
            return validate(View(db.Appointments.Where(x => x.Id == id).First()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Discharge([Bind(Include = "Id,Patient_Id,Doctor_Id,Admission_date,Discharge_date")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("ViewAppointments");
        }

        // CLEAR: clears the db connection for other places to use.
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        protected ActionResult validate(ViewResult vr)
        {
            User u = (User)Session["user"];
            if (Session["user"] == null)
            {
                return Redirect("/Home/Index");
            }
            else
            {

                if (u.Role == 1)
                {
                    return Redirect("/User/Index");
                }
            }
            return vr;
        }


    }
}