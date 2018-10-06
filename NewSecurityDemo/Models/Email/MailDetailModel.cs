using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewSecurityDemo.Models.Email
{
    public class MailDetailModel
    {
        public MailDetailModel()
        {
             
            using (WebDocsEntities db = new WebDocsEntities())
            {
                CurrentEmailSettings = db.EmailSettings.FirstOrDefault<EmailSetting>();
            };
        }
        public string fromName { get; set; }
        public string toName { get; set; }
        public MailAddress toAddress { get; set; }
        public MailAddress fromAddress { get; set; }
        public string subject { get; set; }
        public string message { get; set; }

        public EmailSetting CurrentEmailSettings { get; set; }
    }
}