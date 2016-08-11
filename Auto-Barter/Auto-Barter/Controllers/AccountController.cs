using Auto_Barter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auto_Barter.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            using (OurDbContext db = new OurDbContext())
            {
                return View(db.UserAccount.ToList());
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserAccount account)
        {
            if (ModelState.IsValid)
            {
                using (OurDbContext db = new OurDbContext())
                {
                    db.UserAccount.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = $"{account.FirstName} {account.LastName} successfully registered";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserAccount account)
        {
            using (OurDbContext db = new OurDbContext())
            {
                var user = db.UserAccount.FirstOrDefault(x => x.EmailAddress == account.EmailAddress && x.Password == account.Password);
                if (user != null)
                {
                    Session["UserId"] = user.UserId.ToString();
                    Session["EmailAddress"] = user.EmailAddress;
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "EmailAddress or Password is incorrect");
                }

            }
            return View();
        }

        [HttpGet]
        public ActionResult LoggedIn()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            if (Session["UserId"] != null)
            {
                Session.Abandon();
            }
            return RedirectToAction("Index", "Default", null);
        }

        [HttpGet]
        public ActionResult UserDetails()
        {
            if (Session["UserId"] != null)
            {
                var UserDetails = new UserDetails();
                var db = new OurDbContext();
                var id = int.Parse(Session["UserId"].ToString());
                UserDetails = db.UserDetails.FirstOrDefault(x => x.UserAccount.UserId == id);

                return View(UserDetails);
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult UserDetails(UserDetails details)
        {
            if (Session["UserId"] != null)
            {
                var id = int.Parse(Session["UserId"].ToString());
                using (OurDbContext db = new OurDbContext())
                {
                    var UserDetails = db.UserDetails.FirstOrDefault(x => x.UserAccount.UserId == id);
                    if (UserDetails != null)
                    {
                        UserDetails = details;
                    }
                    else
                    {
                        db.UserDetails.Add(details);
                    }
                    db.SaveChanges();

                    ViewBag.Message = "Successfully updated UserDetails";
                    return View(UserDetails);
                }
            }

            ViewBag.Message = "An error occured updated UserDetails";
            return View(details);
        }
    }
}