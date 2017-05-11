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
        //method to load home page
        public ActionResult Index()
        {
            
            User u = (User)Session["user"]; //get user type from session
            if (u != null) {
                if (u.Role == 1) //if user is doctor
                {
                    return Redirect("/User"); //redirect to user view
                }
                else if (u.Role == 0) //if user is admin
                {
                    return Redirect("/Admin"); //redirect to admin home view
                }
            }

            return View();
        }

        // POST: Users/Create
        // this method will validate username and password combinations
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Username,Password")] Authenticate user)
        {
            if (ModelState.IsValid)
            {
                List<User> list = db.Users.Where(x => x.Username.ToUpper().Equals(user.Username) && x.Password.Equals(user.Password)).ToList();
                if(list.Count == 1)
                {
                    Session["user"] = list[0]; //set user into sessions
                    return list[0].Role == 0 ? Redirect("/Admin/Index") : Redirect("/User/Index"); //redirect to corrosponding views
                }
                else
                {
                    return RedirectToAction("Index"); //if username and password combination do not find out in database then redirect to home view
                }
            }
            return View(user);
        }




        // GET: Home/Logout
        // Logs out the user and redirects index.
        public ActionResult Logout()
        {
            Session.Clear();//clear the sessions
            return RedirectToAction("Index");
        }

    }
}