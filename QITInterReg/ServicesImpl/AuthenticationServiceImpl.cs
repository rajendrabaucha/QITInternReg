using QITInterReg.DAO;
using QITInterReg.DAOImpl;
using QITInterReg.Models;
using QITInterReg.Services;
using QITInterReg.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace QITInterReg.ServicesImpl
{
    public class AuthenticationServiceImpl : AuthenticationService
    {
        private UserDAO userDAO;

        public AuthenticationServiceImpl()
        {
            this.userDAO = new UserDAOImpl(new QUIRegDBEntities());
        }

        public HttpCookie authenticateUser(UserLogin login)
        {
            string message = "";

            User user = userDAO.getUserByEmail(login.EmailID);
            if (user != null)
            {
                if (string.Compare(Crypto.Hash(login.Password), user.Password) == 0)
                {
                    int timeout = login.RememberMe ? 52500 : 60; //52500 min =1 year
                    var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
                    cookie.HttpOnly = true;

                    return cookie;
                }
                else
                {
                    return null;
                }
            }
            else
            {
               return null;
            }
        }
    }
}