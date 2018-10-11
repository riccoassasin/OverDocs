using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewSecurityDemo.Models.Notifications
{
    public class NotificationsModel
    {
        public List<Models.Notification> UserNotifications { get; set; }
        public int CurrentTabIndex { get; set; }
    }
}