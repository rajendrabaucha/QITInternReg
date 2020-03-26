using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace QITInterReg.Services
{
    public interface EmailService
    {
        bool SendVerificationEmail(string emailID, string activationCode, string link);
    }
}