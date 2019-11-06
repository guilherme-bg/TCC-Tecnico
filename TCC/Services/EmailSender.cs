using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TCC.Services {
    public class EmailSender : IEmailSender {
        public Task SendEmailAsync(string email, string subject, string htmlMessage) {
            var client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("patinhas.douradas.adocoes@gmail.com", "rabodecoelho");
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("patinhas.douradas.adocoes@gmail.com");
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = htmlMessage;
            return client.SendMailAsync(mailMessage);
        }

      
    }
}
