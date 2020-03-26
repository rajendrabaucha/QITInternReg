using QITInterReg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QITInterReg.Services
{
    public interface RegistrationService
    {

        bool isEmailExist(String emailID);

        User registerUserInDB(User user);

        bool verifyAccount(String id);
    }
}