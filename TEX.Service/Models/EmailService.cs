using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace TEX.Service.Controllers
{
    public class EmailService
    {
        public void SendWelcomeEmail(string emailAddress)
        {
            //SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            //SmtpClient client = new SmtpClient("rachael.aserv.co.za", 465);
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = false;
            //client.EnableSsl = true;
            //client.Credentials = new NetworkCredential("ndavhe89@gmail.com", "Tshivha@@1");

            string emailUserName = ConfigurationManager.AppSettings["EmailUserName"];
            string emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
            string emailHost = ConfigurationManager.AppSettings["EmailHost"];
            int emailPot = Convert.ToInt16(ConfigurationManager.AppSettings["EmailPot"]);

            SmtpClient client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = emailHost,
                Timeout = 100000,
                Port = emailPot,
                Credentials = new NetworkCredential(emailUserName, emailPassword),
                EnableSsl = false
            };

            MailMessage mail = new MailMessage(emailUserName, emailAddress);
            mail.Subject = "Welcome to TEX Online shop.";
            string Body = "Thank you for registering on our TEX online store. We look forward to gearing you up  with our latest  gear. Happy Shopping!";
            mail.Body = Body;

            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SendOrderPurchaseEmail(string email, string orderNumber, string invoicePath
            , string firstName, string lastName, string body)
        {
            string emailUserName = ConfigurationManager.AppSettings["EmailUserName"];
            string emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
            string emailHost = ConfigurationManager.AppSettings["EmailHost"];
            int emailPot = Convert.ToInt16(ConfigurationManager.AppSettings["EmailPot"]);

            SmtpClient client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = emailHost,
                Timeout = 100000,
                Port = emailPot,
                Credentials = new NetworkCredential(emailUserName, emailPassword),
                EnableSsl = false
            };

            MailMessage mail = new MailMessage(emailUserName, email);
            mail.Subject = "Thank you for placing an Order with us.";
            string Body = body;

            Body = Body.Replace("{FirstName}", firstName);
            Body = Body.Replace("{LastName}", lastName);
            Body = Body.Replace("{ReferenceNumber}", orderNumber);

            mail.Body = Body;
            mail.IsBodyHtml = true;
            mail.Attachments.Add(new Attachment(invoicePath));

            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SendResertPasswordEmail(string email, string decriptedPassword)
        {
            string emailUserName = ConfigurationManager.AppSettings["EmailUserName"];
            string emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
            string emailHost = ConfigurationManager.AppSettings["EmailHost"];
            int emailPot = Convert.ToInt16(ConfigurationManager.AppSettings["EmailPot"]);

            SmtpClient client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = emailHost,
                Timeout = 100000,
                Port = emailPot,
                Credentials = new NetworkCredential(emailUserName, emailPassword),
                EnableSsl = false
            };

            MailMessage mail = new MailMessage(emailUserName, email);
            mail.Subject = "Password Resert : Team Expansion Gear.";
            string Body = string.Format("Your password: {0}", decriptedPassword);
            mail.Body = Body;

            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
