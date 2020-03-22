using QITInterReg.Models;
using QITInterReg.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QITInterReg.Controllers
{
    public class UserController : Controller
    {

        //Registration action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        //Registration post action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")] User user)
        {
            bool isVerified = false;
            string message = "";

            // Validate Model
            if (ModelState.IsValid)
            {
                //Check Email already exist
                var isExist = isEmailExist(user.EmailID);

                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }

                #region Generate activation code

                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password Hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion
                user.IsEmailVerified = false; ;

                #region Save to DB
                using (QUIRegDBEntities dc = new QUIRegDBEntities())
                {
                    dc.Users.Add(user);
                    dc.SaveChanges();
                }

                #endregion


                //Send Email to User
                SendVerificationEmail(user.EmailID, user.ActivationCode.ToString());
                message = "Registration successfully done. Account activation link has been" +
                    "sent to you to your email address " + user.EmailID;
                isVerified = true;

            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.Message = message;
            ViewBag.isVerified = isVerified;
            return View(user);
        }

        //Verify Account
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool isVerified = false;
            using (QUIRegDBEntities dc = new QUIRegDBEntities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; // This helps to avoid confirm password doesnot match issue on save change

                var v = dc.Users.Where(u => u.ActivationCode == new Guid(id)).FirstOrDefault();

                if (v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                    isVerified = true;

                }
                else
                {
                    ViewBag.message = "Invalid Request";
                }
            }
            ViewBag.isVerified = isVerified;
            return View();
        }


        //Login 
        public ActionResult Login()
        {
            return View();
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(UserLogin login,string returnUrl="")
        {
            string message = "";
            using (QUIRegDBEntities dc = new QUIRegDBEntities())
            {
                var v = dc.Users.Where(u=> u.EmailID ==login.EmailID).FirstOrDefault();
                if(v != null)
                {
                    if(string.Compare(Crypto.Hash(login.Password),v.Password) == 0) {
                        int timeout = login.RememberMe ? 52500 : 60; //52500 min =1 year
                        var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        message = "Invalid user credentials.";
                    }
                }
                else
                {
                    message ="Invalid user credentials.";
                }
            }
            ViewBag.message = message;
            return View();
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        [NonAction]
        public bool isEmailExist(string emailID)
        {
            using(QUIRegDBEntities dc = new QUIRegDBEntities())
            {
                var v = dc.Users.Where(u => u.EmailID == emailID).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("rajendramanandhar7@gmail.com", "rajendra manandhar");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "sbqzufdatqfbnkdj";
            string subject = "Activation Account";

            string body = "<br/><br/>Your account is successfully created. Please " +
                "click below link to activate your account.<br/>" +
                "<a href='" + link + "'>" + link + "</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }
}