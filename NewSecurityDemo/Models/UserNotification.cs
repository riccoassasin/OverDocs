//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NewSecurityDemo.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserNotification
    {
        public int UserNotificationID { get; set; }
        public int NotificationID { get; set; }
        public int UserID { get; set; }
        public int SenderUserID { get; set; }
    }
}