using NewSecurityDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;
using System.Threading.Tasks;

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
                                           .Include("LookupTableNotificationType")
                                           .Include("AspNetUser")
                                           .Include("AspNetUser1")
                                           .Include("File")
                                           where a.UserIDOfNotificationRecipient == UserIDOfPersonCurrentlyLogged
                                           select a).ToList<Notification>();
            }
            return View(new Models.Notifications.NotificationsModel()
            {

                CurrentTabIndex = TabIndex,
                UserNotifications = ListOfUserNotifications
            });
        }

        public ActionResult FileRequestNontification_ConfirmationView(int id)
        {
            //Write your logic here 
            return PartialView("_FileRequestNontification_ConfirmationView");
        }

        [HttpPost]
        public async Task<ActionResult> SendRequestNotification(int FileID, int NotificationTypeID, string IDOFPersonLoggedOn, string IDOfTheFileOwner)
        {
            AspNetUser CurrentUser;
            AspNetUser FileOwner;
            Models.File f;

            using (db)
            {
                Notification n = new Notification()
                {
                    FileID = FileID,
                    DateCreated = DateTime.Now,
                    NotificationTypeID = (int)Common.Enum.DBLookupEnum.NotificationTypes.FileRequest,
                    UserIDOfNotificationRecipient = IDOfTheFileOwner,
                    UserIDOfNotificationSender = IDOFPersonLoggedOn,
                    UserHasAcknowledgement = false
                };
                db.Notifications.Add(n);

                CurrentUser = (from a in db.AspNetUsers
                               where a.Id == IDOFPersonLoggedOn
                               select a).FirstOrDefault<AspNetUser>();
                FileOwner = (from a in db.AspNetUsers
                             where a.Id == IDOfTheFileOwner
                             select a).FirstOrDefault<AspNetUser>();

                f = (from a in db.Files
                     where a.FileID == FileID
                     select a).FirstOrDefault<File>();

                EmailSetting es = (from a in db.EmailSettings
                                   select a).FirstOrDefault<EmailSetting>();
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (DbEntityValidationResult entityErr in dbEx.EntityValidationErrors)
                    {
                        foreach (DbValidationError error in entityErr.ValidationErrors)
                        {
                            Console.WriteLine("Error Property Name {0} : Error Message: {1}",
                                                error.PropertyName, error.ErrorMessage);
                        }
                    }
                }

                string FromName = CurrentUser.FirstName + " " + CurrentUser.LastName;
                string ToName = FileOwner.FirstName + " " + FileOwner.LastName;

                //Email Message To the file owner. Notifying him/her of the that a user wants to gain access to a private file. 
                await Common.Email.EmailHelper.sendMessageAsync(
                   _ToAddress: FileOwner.Email,
                   _FromAddress: CurrentUser.Email,
                   _FromName: FromName,
                   _ToName: ToName,
                   _Subject: "Over Docs System - Request to share private file (" + f.FileID + " - " + f.FileName + ")",
                   _Message: "Good day " + FileOwner.FirstName + " " + FileOwner.LastName + ". <br/><br/>A request notifiction has been sent to you from: " + CurrentUser.FirstName + " " + CurrentUser.LastName + " to please share one of your documents.<br/> REquest for the following file:<br/>File Name: " + f.FileName + ".<br/>File Ref# " + FileID + ".<br/><br/> Regards Over Docs.",
                    _Credentials_UserName: es.UserName,
                    _Credentials_Password: es.Password,
                     _SMTP_HOST: es.SmtpHost,
                     _SMTP_PORT: es.SmtpPort,
                     _IsSsl: es.SslEnabled);

                //Email Message To the Current. To Confirm that a notification was sent. 
                await Common.Email.EmailHelper.sendMessageAsync(
                   _ToAddress: CurrentUser.Email,
                   _FromAddress: FileOwner.Email,
                   _FromName: ToName,
                   _ToName: FromName,
                   _Subject: "Over Docs System - Notification sent to the owner of  (" + f.FileID + " - " + f.FileName + ")",
                   _Message: "Good day " + CurrentUser.FirstName + " " + CurrentUser.LastName + ". <br/><br/>A REQUEST notifiction has been sent to the owner: " + FileOwner.FirstName + " " + FileOwner.LastName + ".<br/> For the following file:<br/>File Name: " + f.FileName + ".<br/>File Ref# " + FileID + ".<br/><br/> Regards Over Docs.",
                    _Credentials_UserName: es.UserName,
                    _Credentials_Password: es.Password,
                     _SMTP_HOST: es.SmtpHost,
                     _SMTP_PORT: es.SmtpPort,
                     _IsSsl: es.SslEnabled);

            };

            return Json("Notification Sent", JsonRequestBehavior.AllowGet);
        }

    }
}

