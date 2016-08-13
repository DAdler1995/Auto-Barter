using Auto_Barter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net.Mail;
using System.Security.Cryptography;
using System.IO;

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
                    
                    using (RijndaelManaged myRijndael = new RijndaelManaged())
                    {
                        myRijndael.GenerateKey();
                        myRijndael.GenerateIV();

                        Session["KEY"] = myRijndael.Key;
                        Session["IV"] = myRijndael.IV;
                    }

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
                    var id = int.Parse(Session["UserId"].ToString());
                    var UserDetails = db.UserDetails.Include(x => x.Address).Include(x => x.UserAccount).FirstOrDefault(x => x.UserAccount.UserId == id);

                    if (UserDetails == null)
                    {
                        UserDetails = new UserDetails();
                        UserDetails.UserAccount = db.UserAccount.FirstOrDefault(x => x.UserId == id);
                    }

                    byte[] EncryptPassword = EncryptStringToBytes(UserDetails.UserAccount.Password, (byte[])Session["KEY"], (byte[])Session["IV"]);
                    byte[] EncryptConfirmPassword = EncryptStringToBytes(UserDetails.UserAccount.Password, (byte[])Session["KEY"], (byte[])Session["IV"]);


                    UserDetails.UserAccount.Password = Convert.ToBase64String(EncryptPassword);
                    UserDetails.UserAccount.ConfirmPassword = Convert.ToBase64String(EncryptConfirmPassword);

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
                var EncryptedPassword = Convert.FromBase64String(details.UserAccount.Password);
                var EncryptedConfirmPassword = Convert.FromBase64String(details.UserAccount.ConfirmPassword);

                details.UserAccount.Password = DecryptStringFromBytes(EncryptedPassword, (byte[])Session["KEY"], (byte[])Session["IV"]);
                details.UserAccount.ConfirmPassword = DecryptStringFromBytes(EncryptedConfirmPassword, (byte[])Session["KEY"], (byte[])Session["IV"]);


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



        static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }
        static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }
    }
}