using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewSecurityDemo.Models.Email
{
    public class EmailSetings 
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SmtpHost { get; set; }
        public string SmtpPort { get; set; }
        public string SslEnabled { get; set; }
    }
}