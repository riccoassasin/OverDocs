using NewSecurityDemo.Models.Email;
using System;
using System.Net;
using System.Net.Mail;

namespace NewSecurityDemo.Common.Email
{
    public static class EmailHelper
    {
        public async static void sendMessage(MailDetailModel EmailMessage)
        {
            string body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            MailMessage message = new MailMessage();
            message.To.Add(EmailMessage.toAddress);  // replace with valid value 
            message.From = EmailMessage.fromAddress; // replace with valid value
            message.Subject = EmailMessage.subject;
            message.Body = string.Format(body, EmailMessage.fromName, EmailMessage.fromAddress.Address, EmailMessage.message);
            message.IsBodyHtml = true;

            using (SmtpClient smtp = new SmtpClient())
            {
                NetworkCredential credential = new NetworkCredential
                {

                    UserName = EmailMessage.CurrentEmailSettings.UserName,  // replace with valid value
                    Password = EmailMessage.CurrentEmailSettings.Password  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = EmailMessage.CurrentEmailSettings.SmtpHost; //smtp.mweb.co.za
                smtp.Port = Convert.ToInt16(EmailMessage.CurrentEmailSettings.SmtpPort); // default port numnber is 25
                smtp.EnableSsl = Convert.ToBoolean(EmailMessage.CurrentEmailSettings.SslEnabled);
                await smtp.SendMailAsync(message);

            }
        }
    }
}