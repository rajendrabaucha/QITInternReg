using QITInterReg.DAO;
using QITInterReg.DAOImpl;
using QITInterReg.Models;
using QITInterReg.Services;
using QITInterReg.ServicesImpl;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Security;

namespace QITInterReg.Controllers
{
    public class UserController : Controller
    {
        private RegistrationService registrationService;
        private EmailService emailService;
        private AuthenticationService authenticationService;

        public UserController()
        {
            this.registrationService = new RegistrationServiceImpl();
            this.emailService = new EmailServiceImpl();
            this.authenticationService = new AuthenticationServiceImpl();

        }
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
                var isExist = registrationService.isEmailExist(user.EmailID);

                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }

                user = registrationService.registerUserInDB(user);
                if (user != null)
                {
                    message = "Registration successfully done. ";
                    //Send Email to User
                    message = message + SendVerificationEmail(user.EmailID, user.ActivationCode.ToString());
                    isVerified = true;
                }
                else
                {
                    message = "Failed to register. Contact administrator";
                    isVerified = false;
                }

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

            isVerified = registrationService.verifyAccount(id);
            if (!isVerified)
            {
                ViewBag.message = "Invalid Request";
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


            var cookie = authenticationService.authenticateUser(login);
                if(cookie != null)
                {
                    Response.Cookies.Add(cookie);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Customer");
                        
                    }
                   
                }
                else
                {
                Debug.WriteLine("Invalid user credentials.");
                    message ="Invalid user credentials.";
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
        public string SendVerificationEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            if (emailService.SendVerificationEmail(emailID, activationCode, link))
            {
                return "Account activation link has been" +
                    "sent to you to your email address " + emailID;
            }

            return "But failed to send activation link to your email address.";
        }
    }
}