using Assignment3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment3.Controllers
{
    public class UserController : Controller
    {
        private HospitalEntity db = new HospitalEntity();

        // GET: User
        public ActionResult Index()
        {
            User u = (User)Session["user"];
            List<Appointment> app = db.Appointments.Where(x => x.Doctor_Id.Equals(u.Username)).ToList();
            ViewBag.AllAppointments = app;
            HashSet<int> map = new HashSet<int>();
            List<Patient> patients = new List<Patient>();
            foreach (var item in app)
            {
                if(!map.Contains(item.Patient_Id))
                {
                    map.Add(item.Patient_Id);
                    patients.Add(item.Patient);
                }
            }
            return validate(View(patients));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchByName([Bind(Include = "Name")] Patient p)
        {
            if(p == null || db.Patients.Where(k => k.Name == p.Name).FirstOrDefault() == null)
            {
                return RedirectToAction("SearchByName");
            }

            User doctor = (User)Session["user"];
           
            List<Appointment> appointments = db.Appointments.Where(x => x.Patient_Id == db.Patients.Where(k => k.Name == p.Name).FirstOrDefault().Id && x.Doctor_Id == doctor.Username).ToList();


            return validate(View("ViewAppointments",appointments ));

        }

        public ActionResult SearchByName()
        {
                return validate(View());
        }

        public ActionResult SearchByAdmitDate()
        {
            return validate(View());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchByAdmitDate([Bind(Include = "Admission_date")] Appointment appointment)
        {
            if (appointment.Admission_date == null || appointment.Admission_date == Convert.ToDateTime("0001-01-01"))
            {
                return validate(View());
            }
            User doctor = (User)Session["user"];
            return validate(View("ViewAppointments", db.Appointments.Where(x => x.Admission_date == appointment.Admission_date).Where(x => x.Doctor_Id == doctor.Username).ToList()));
        }
        public ActionResult SearchByDischargeDate()
        {
            return validate(View());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchByDischargeDate([Bind(Include = "Discharge_date")] Appointment appointment)
        {
            if (appointment.Discharge_date == null || appointment.Discharge_date == Convert.ToDateTime("0001-01-01"))
            {
                return validate(View());
            }
            User doctor = (User)Session["user"];

            return validate(View("ViewAppointments", db.Appointments.Where(x => x.Discharge_date == appointment.Discharge_date).Where(x =>x.Doctor_Id == doctor.Username).ToList()));
        }



        public ActionResult LookupAppointments(int id)
        {
            return validate(View("ViewAppointments",db.Appointments.Where(x => x.Patient_Id == id).ToList()));
        }


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
                return Redirect("Home/Index");
            }
            else
            {

                if (u.Role == 0)
                {
                    return Redirect("/Admin/Index");
                } 
            }
            return vr;
        }


    }
}