using Assignment3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment3.Controllers
{
    public class HomeController : Controller
    {
        private HospitalEntity db = new HospitalEntity();

        public ActionResult Index()
        {
            
            User u = (User)Session["user"];
            if (u != null) {
                if (u.Role == 1)
                {
                    return Redirect("/User");
                }
                else if (u.Role == 0)
                {
                    return Redirect("/Admin");
                }
            }

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Username,Password")] Authenticate user)
        {
            if (ModelState.IsValid)
            {
                List<User> list = db.Users.Where(x => x.Username.ToUpper().Equals(user.Username) && x.Password.Equals(user.Password)).ToList();
                if(list.Count == 1)
                {
                    Session["user"] = list[0];
                    return list[0].Role == 0 ? Redirect("/Admin/Index") : Redirect("/User/Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }




        // GET: Home/Logout
        // Logs out the user and redirects index.
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

    }
}