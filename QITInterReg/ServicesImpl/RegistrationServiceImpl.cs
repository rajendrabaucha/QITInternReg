using QITInterReg.DAO;
using QITInterReg.DAOImpl;
using QITInterReg.Models;
using QITInterReg.Services;
using QITInterReg.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace QITInterReg.ServicesImpl
{
    public class RegistrationServiceImpl : RegistrationService
    {

        private UserDAO userDAO;

        public RegistrationServiceImpl()
        {
            this.userDAO = new UserDAOImpl(new QUIRegDBEntities());
        }

        public bool isEmailExist(String emailID)
        {
            return userDAO.getUserByEmail(emailID) != null;
        }

        public User registerUserInDB(User user)
        {
            try
            {
                #region Generate activation code

                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password Hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion
                user.IsEmailVerified = false;

                #region Save to DB
                userDAO.insertUser(user);

                #endregion

                return user;
            }
            catch (Exception e)
            {
                Debug.WriteLine("exception :" + e.Message);
                return null;
            }
            
        }

        public bool verifyAccount(string id)
        {
            User user = userDAO.getUserOnActivationSent(id);

            if (user != null)
            {
                user.IsEmailVerified = true;
                userDAO.updateUser(user);
                return true;
            }
            return false;
           
        }
    }
}