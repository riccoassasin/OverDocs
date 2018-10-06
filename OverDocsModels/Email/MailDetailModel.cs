using System.Linq;
using System.Net.Mail;

namespace OverDocsModels.Email
{
    public class MailDetailModel
    {
        public MailDetailModel(string _ToAddress, string _FromAddress, string _FromName, string _ToName)
        {
            this.toAddress = new MailAddress(_ToAddress);
            this.fromAddress = new MailAddress(_FromAddress);
            this.fromName = _FromName;
            this.toName = _ToName;
            //using (WebDocsEntities db = new WebDocsEntities())
            //{
            //    CurrentEmailSettings = db.EmailSettings.FirstOrDefault<EmailSetting>();
            //};
        }
        public string fromName { get; set; }
        public string toName { get; set; }
        public MailAddress toAddress { get; set; }
        public MailAddress fromAddress { get; set; }
        public string subject { get; set; }
        public string message { get; set; }

       // public EmailSetting CurrentEmailSettings { get; set; }
    }
}