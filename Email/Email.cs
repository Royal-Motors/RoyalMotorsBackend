using System.Net.Mail;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.IO.Pipes;

namespace CarWebsiteBackend.Email
{
    public static class Email
    {
        public static void sendEmail(string recipient, string subject, string body, Stream fileStream = null)
        {
            // create the email message
            var fromAddress = new MailAddress("royal.motors.send@gmail.com", "Royal Motors");
            var toAddress = new MailAddress(recipient);
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                IsBodyHtml = true
            };

            // attach the file
            if (fileStream != null)
            {
                Attachment attachment = new Attachment(fileStream, "image");
                attachment.ContentId = "image1";
                attachment.ContentDisposition.Inline = true;
                message.Attachments.Add(attachment);

            }

            // set the body of the email
            message.Body = body;

            // configure the SMTP client
            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("royal.motors.send@gmail.com", "pclaknmhhmvarvrm"),
                EnableSsl = true
            };

            // send the email message
            smtpClient.Send(message);
        }
    }
}