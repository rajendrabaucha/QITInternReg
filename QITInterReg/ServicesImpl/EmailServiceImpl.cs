using QITInterReg.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace QITInterReg.ServicesImpl
{
    public class EmailServiceImpl : EmailService
    {
        public bool SendVerificationEmail(string emailID, string activationCode, string link)
        {
            try
            {
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
                return true;
            }
            catch (Exception)
            {

                return false;
                
            }
            
        }
    }
}