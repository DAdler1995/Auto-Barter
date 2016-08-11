using Auto_Barter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

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
                    account.UserRole = UserRole.DEFAULT;
                    db.UserAccount.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                return RedirectToAction("Login");
            }

            ViewBag.Message = "An error occured creating your account. Please contact support.";
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
                    Session["FullName"] = user.FullName;
                    Session["UserRole"] = user.UserRole;
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
                using (var bd = new OurDbContext())
                {
                    var UserDetails = new UserDetails();
                    var db = new OurDbContext();
                    var id = int.Parse(Session["UserId"].ToString());
                    UserDetails = db.UserDetails.Include(x => x.Address).Include(x => x.UserAccount).FirstOrDefault(x => x.UserAccount.UserId == id);

                    return View(UserDetails);
                }
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
                    if (details.UserAccount == null)
                    {
                        details.UserAccount = db.UserAccount.FirstOrDefault(x => x.UserId == id);
                    }
                    else
                    {
                        var UserDetails = db.UserDetails.Include(x => x.Address).Include(x => x.UserAccount).FirstOrDefault(x => x.UserAccount.UserId == id);
                        if (UserDetails != null)
                        {
                            UserDetails = details;

                            // Updating Database
                            db.Entry(UserDetails).State = EntityState.Modified;
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
            }

            ViewBag.Message = "An error occured updated UserDetails";
            return View(details);
        }
    }
}