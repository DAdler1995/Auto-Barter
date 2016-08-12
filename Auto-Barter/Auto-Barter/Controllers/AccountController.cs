using Auto_Barter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net.Mail;

namespace Auto_Barter.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["UserRole"].ToString() == "ADMIN")
            {
                using (OurDbContext db = new OurDbContext())
                {
                    return View(db.UserAccount.ToList());
                }
            }
            return RedirectToAction("Index", "Default", null);
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
                    bool AccountExists = db.UserAccount.FirstOrDefault(x => x.EmailAddress == account.EmailAddress) != null;
                    if (AccountExists)
                    {
                        ViewBag.Message = "This email address is already in use.";
                        account.EmailAddress = null;
                        return View(account);
                    }

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
                    return RedirectToAction("UserDetails");
                }
                else
                {
                    ModelState.AddModelError("", "EmailAddress or Password is incorrect");
                }

            }
            return View();
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
                using (var db = new OurDbContext())
                {
                    var UserDetails = new UserDetails();
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

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string emailAddress)
        {
            // TODO: Hook this up once it's hosted on a server
            //if (string.IsNullOrEmpty(emailAddress))
            //{
            //    ViewBag.Message = "Please enter a valid Email Address";
            //    return View();
            //}

            //using (OurDbContext db = new OurDbContext())
            //{
            //    var ExistingAccount = db.UserAccount.FirstOrDefault(x => x.EmailAddress == emailAddress);

            //    if (ExistingAccount != null)
            //    {
            //        MailMessage message = new MailMessage();
            //        SmtpClient client = new SmtpClient();
            //        client.Port = 587;
            //        client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //        client.UseDefaultCredentials = false;
            //        client.Host = "smtp.gmail.com";
            //        client.EnableSsl = true;

            //        message.To.Add(new MailAddress("Zeketiki@gmail.com"/*ExistingAccount.UserAccount.EmailAddress*/));
            //        message.Subject = "Auto-Barter.com | Account Recovery";
            //        message.From = new MailAddress("EmaiRecovery@Auto-Barter.com");
            //        message.Body = "Email Recovery.";

            //        client.Send(message);
            //    }
            //}

            return View();
        }
    }
}