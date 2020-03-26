using QITInterReg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QITInterReg.Services
{
    public interface AuthenticationService
    {
        HttpCookie authenticateUser(UserLogin login);
    }
}