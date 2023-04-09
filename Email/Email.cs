using System.Net.Mail;
using System.Net;

namespace CarWebsiteBackend.Email
{
    public static class Email
    {
        public static void sendEmail(string recipient, string subject, string body)
        {

            // create the email message
            var fromAddress = new MailAddress("royal.motors.send@gmail.com", "Royal Motors");
            var toAddress = new MailAddress(recipient);
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            // configure the SMTP client
            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("royal.motors.send@gmail.com", "pass"),
                EnableSsl = true
            };

            // send the email message
            smtpClient.Send(message);
            return;

        }
    }
}
