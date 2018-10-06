using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewSecurityDemo.Common.Email
{
    public class EmailHelper
    {
        public async void sendMessage(MailDetailModel EmailMessage)
        {
            var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var message = new MailMessage();
            message.To.Add(EmailMessage.toAddress);  // replace with valid value 
            message.From = new MailAddress("sender@outlook.com");  // replace with valid value
            message.Subject = "Your email subject";
            message.Body = string.Format(body, EmailMessage.fromName, EmailMessage.fromAddress.Address, EmailMessage.message);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {

                    UserName = EmailMessage.CurrentEmailSettings.UserName,  // replace with valid value
                    Password = EmailMessage.CurrentEmailSettings.Password  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = EmailMessage.CurrentEmailSettings.SmtpHost;
                smtp.Port = EmailMessage.CurrentEmailSettings.SmtpPort;
                smtp.EnableSsl = EmailMessage.CurrentEmailSettings.SslEnabled;
                await smtp.SendMailAsync(message);

            }
        }
    }
}