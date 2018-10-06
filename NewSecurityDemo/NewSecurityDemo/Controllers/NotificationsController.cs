using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewSecurityDemo.Controllers
{
    public class NotificationsController : Controller
    {
        // GET: Notifications
        public ActionResult ShowNotifications()
        {
            return View();
        }
    }
}