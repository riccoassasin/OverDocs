using NewSecurityDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace NewSecurityDemo.Controllers
{
    public class NotificationsController : Controller
    {
        private WebDocsEntities db = new WebDocsEntities();
        // GET: Notifications

        [Authorize]
        public ActionResult ShowNotifications(int TabIndex = 0)
        {
            List<Notification> ListOfUserNotifications = new List<Notification>();
            using (db)
            {
                string UserIDOfPersonCurrentlyLogged = User.Identity.GetUserId();
                ListOfUserNotifications = (from a in db.Notifications
                                           where a.UserIDOfNotificationRecipient == UserIDOfPersonCurrentlyLogged
                                           select a).ToList<Notification>();
            }
            return View(ListOfUserNotifications);
        }

        public ActionResult Show()
        {
            return View();
        }
    }


}