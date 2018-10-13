using NewSecurityDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using Newtonsoft.Json;

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


        #region File Request Notification Actions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NotificationID"></param>
        /// <param name="FileID"></param>
        /// <param name="IDOfUserThatRequestedTheFileToBeShared"></param>
        /// <param name="IDOfTheFileOwner"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AcceptFileRequest(int FileID, int NotificationID, string IDOfUserThatRequestedTheFileToBeShared, string IDOfTheFileOwner)
        {
            AspNetUser UserThatSentRequest; //IDOfUserThatRequestedTheFileToBeShared 
            AspNetUser FileOwner;           //IDOfTheFileOwner (Person Currently Lofgged on)


            string GlobalErrorMessage = "";
            string rtnSuccessMessage = "";

            Boolean UpdatedCorrectly = false;

            using (db)
            {
                try
                {

                    //updates the Notification acknolegemant to indicate that the notification has been responeded to.
                    Notification Noti = db.Notifications.Where(a => a.NotificationID == NotificationID).FirstOrDefault<Notification>();
                    if (!(Noti.UserHasAcknowledgement))
                    {
                        FileOwner = (from a in db.AspNetUsers
                                     where a.Id == IDOfTheFileOwner
                                     select a).FirstOrDefault<AspNetUser>();

                        UserThatSentRequest = (from a in db.AspNetUsers
                                               where a.Id == IDOfUserThatRequestedTheFileToBeShared
                                               select a).FirstOrDefault<AspNetUser>();

                        EmailSetting es = (from a in db.EmailSettings select a).FirstOrDefault<EmailSetting>();
                        var PartialFileObject = db.Files.Where(a => a.FileID == FileID).Select(a => new { a.FileID, a.FileName, a.FileExtension }).FirstOrDefault();


                        //db.Files_U_SetFileStatus(FileID, (int)Common.Enum.DBLookupEnum.FileViewStatus.FileIsLocked);


                        FileSharedWithUser FSWU = new FileSharedWithUser()
                        {
                            UserIDOfSharedDocs = IDOfUserThatRequestedTheFileToBeShared,
                            FileID = FileID,
                            DateShared = DateTime.Now

                        };

                        Noti.UserHasAcknowledgement = true;

                        db.FileSharedWithUsers.Add(FSWU);

                        db.SaveChanges();

                        string FromName = FileOwner.FirstName + " " + FileOwner.LastName;
                        string ToName = UserThatSentRequest.FirstName + " " + UserThatSentRequest.LastName;

                        //Email Message To the file owner. Notifying him/her of the that the Private File Was successfull shared  With the person that requested it. 
                        await Common.Email.EmailHelper.sendMessageAsync(
                           _ToAddress: FileOwner.Email,
                           _FromAddress: UserThatSentRequest.Email,
                           _FromName: FromName,
                           _ToName: ToName,
                           _Subject: "Over Docs System - File (" + PartialFileObject.FileID + " - " + PartialFileObject.FileName + "." + PartialFileObject.FileExtension + ") has been shared.",
                           _Message: "Good day " + FromName + ". <br/><br/>A file shared notifiction has been sent to you from the system to notify you that the following file has bee successfully Shared with " + ToName + " .<br/> File Name: " + PartialFileObject.FileName + "." + PartialFileObject.FileExtension + ".<br/>File Ref# " + FileID + ".<br/><br/> Regards Over Docs.",
                            _Credentials_UserName: es.UserName,
                            _Credentials_Password: es.Password,
                             _SMTP_HOST: es.SmtpHost,
                             _SMTP_PORT: es.SmtpPort,
                             _IsSsl: es.SslEnabled);

                        //Email Message To the Person that requested the file. To Confirm that the file has been shared out with user and can now download if so desired. 
                        await Common.Email.EmailHelper.sendMessageAsync(
                           _ToAddress: UserThatSentRequest.Email,
                           _FromAddress: FileOwner.Email,
                           _FromName: ToName,
                           _ToName: FromName,
                           _Subject: "Over Docs System - Notification (" + PartialFileObject.FileID + " - " + PartialFileObject.FileName + "." + PartialFileObject.FileExtension + ") has been shared!",
                           _Message: "Good day " + ToName + ". <br/><br/>" + FromName + " has responded to your request to share the following file.<br/> (" + PartialFileObject.FileID + " - " + PartialFileObject.FileName + "." + PartialFileObject.FileExtension + ").<br/> The file has been successfully shared with you and is now viewable under you private documents.<br/>The file can now be download if you require it.<br/><br/> Regards Over Docs.",
                            _Credentials_UserName: es.UserName,
                            _Credentials_Password: es.Password,
                             _SMTP_HOST: es.SmtpHost,
                             _SMTP_PORT: es.SmtpPort,
                             _IsSsl: es.SslEnabled);

                        rtnSuccessMessage += "Notification has been successfully recorded and Email Notification Sent to the person that request it in forming them that the file is avaiable!";

                    }
                    else
                    {
                        rtnSuccessMessage += "Notification has been already successfully been sent both the owner and the person that request the file to be shared! please check you email.";
                    }

                }
                catch (DbEntityValidationException dbEx)
                {

                    foreach (DbEntityValidationResult entityErr in dbEx.EntityValidationErrors)
                    {
                        foreach (DbValidationError error in entityErr.ValidationErrors)
                        {
                            GlobalErrorMessage = "Error Property Name " + error.PropertyName + " : Error Message: " + error.ErrorMessage;
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobalErrorMessage = "Error : " + ex.Message.ToString();
                }


                return Json(rtnSuccessMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DenyFileRequest()
        {



            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> SendRequestNotification(int FileID, int NotificationTypeID, string IDOFPersonLoggedOn, string IDOfTheFileOwner)
        {
            AspNetUser CurrentUser;
            AspNetUser FileOwner;


            string GlobalErrorMessage = "";
            string rtnSuccessMessage = "";

            Notification hasNotificationAlreadyBeenSent;

            using (db)
            {

                try
                {
                    hasNotificationAlreadyBeenSent = (from a in db.Notifications
                                                      where
                                                              a.FileID == FileID &&
                                                              a.NotificationTypeID == (int)Common.Enum.DBLookupEnum.NotificationTypes.FileRequest &&
                                                              a.UserIDOfNotificationSender == IDOFPersonLoggedOn &&
                                                              a.UserIDOfNotificationRecipient == IDOfTheFileOwner
                                                      select a
                                                              ).FirstOrDefault<Notification>();

                    CurrentUser = (from a in db.AspNetUsers
                                   where a.Id == IDOFPersonLoggedOn
                                   select a).FirstOrDefault<AspNetUser>();
                    FileOwner = (from a in db.AspNetUsers
                                 where a.Id == IDOfTheFileOwner
                                 select a).FirstOrDefault<AspNetUser>();

                    var PartialFileObject = db.Files.Where(a => a.FileID == FileID).Select(a => new { a.FileID, a.FileName, a.FileExtension }).FirstOrDefault();


                    (from a in db.Files
                     where a.FileID == FileID
                     select a).FirstOrDefault<File>();

                    EmailSetting es = (from a in db.EmailSettings
                                       select a).FirstOrDefault<EmailSetting>();

                    string FromName = CurrentUser.FirstName + " " + CurrentUser.LastName;
                    string ToName = FileOwner.FirstName + " " + FileOwner.LastName;

                    //test to see if the Request notification has been sent for this user.
                    if (hasNotificationAlreadyBeenSent is null)
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

                        db.SaveChanges();
                        rtnSuccessMessage += "Notification has been successfully recorded and Email Notification Sent to the file owner for approval.";

                        //determine if the notification has already been sent?

                        //Email Message To the file owner. Notifying him/her of the that a user wants to gain access to a private file. 
                        await Common.Email.EmailHelper.sendMessageAsync(
                           _ToAddress: FileOwner.Email,
                           _FromAddress: CurrentUser.Email,
                           _FromName: FromName,
                           _ToName: ToName,
                           _Subject: "Over Docs System - Request to share private file (" + PartialFileObject.FileID + " - " + PartialFileObject.FileName + "." + PartialFileObject.FileExtension + ")",
                           _Message: "Good day " + FileOwner.FirstName + " " + FileOwner.LastName + ". <br/><br/>A request notifiction has been sent to you from: " + CurrentUser.FirstName + " " + CurrentUser.LastName + " to please share one of your documents.<br/> REquest for the following file:<br/>File Name: " + PartialFileObject.FileName + "." + PartialFileObject.FileExtension + ".<br/>File Ref# " + FileID + ".<br/><br/> Regards Over Docs.",
                            _Credentials_UserName: es.UserName,
                            _Credentials_Password: es.Password,
                             _SMTP_HOST: es.SmtpHost,
                             _SMTP_PORT: es.SmtpPort,
                             _IsSsl: es.SslEnabled);

                        //Email Message To the Current user logged. To Confirm that a notification was sent. 
                        await Common.Email.EmailHelper.sendMessageAsync(
                           _ToAddress: CurrentUser.Email,
                           _FromAddress: FileOwner.Email,
                           _FromName: ToName,
                           _ToName: FromName,
                           _Subject: "Over Docs System - Notification sent to the owner of  (" + PartialFileObject.FileID + " - " + PartialFileObject.FileName + "." + PartialFileObject.FileExtension + ")",
                           _Message: "Good day " + CurrentUser.FirstName + " " + CurrentUser.LastName + ". <br/><br/>A REQUEST notifiction has been sent to the owner: " + FileOwner.FirstName + " " + FileOwner.LastName + ".<br/> For the following file:<br/>File Name: " + PartialFileObject.FileName + "." + PartialFileObject.FileExtension + ".<br/>File Ref# " + FileID + ".<br/><br/> Regards Over Docs.",
                            _Credentials_UserName: es.UserName,
                            _Credentials_Password: es.Password,
                             _SMTP_HOST: es.SmtpHost,
                             _SMTP_PORT: es.SmtpPort,
                             _IsSsl: es.SslEnabled);
                        rtnSuccessMessage += "Notification has been successfully recorded and posted to the file owner, as well Email Notification Sent to the file owner for approval.";
                    }
                    else
                    {
                        //Email Message To the file owner. Notifying him/her of the that a user wants to gain access to a private file. 
                        await Common.Email.EmailHelper.sendMessageAsync(
                           _ToAddress: FileOwner.Email,
                           _FromAddress: CurrentUser.Email,
                           _FromName: FromName,
                           _ToName: ToName,
                           _Subject: "Over Docs System - Reminder - Request to share private file (" + PartialFileObject.FileID + " - " + PartialFileObject.FileName + "." + PartialFileObject.FileExtension + ")",
                           _Message: "Good day " + FileOwner.FirstName + " " + FileOwner.LastName + ". <br/><br/>This is a friendly reminder that a request notifiction has been sent to you from: " + CurrentUser.FirstName + " " + CurrentUser.LastName + " to please share one of your documents.<br/> Request for the following file:<br/>File Name: " + PartialFileObject.FileName + "." + PartialFileObject.FileExtension + "<br/>File Ref# " + FileID + ".<br/><br/> Regards Over Docs.",
                            _Credentials_UserName: es.UserName,
                            _Credentials_Password: es.Password,
                             _SMTP_HOST: es.SmtpHost,
                             _SMTP_PORT: es.SmtpPort,
                             _IsSsl: es.SslEnabled);

                        //Email Message To the Current user logged. To Confirm that a notification was sent. 
                        await Common.Email.EmailHelper.sendMessageAsync(
                           _ToAddress: CurrentUser.Email,
                           _FromAddress: FileOwner.Email,
                           _FromName: ToName,
                           _ToName: FromName,
                           _Subject: "Over Docs System - Notification sent to the owner of  (" + PartialFileObject.FileID + " - " + PartialFileObject.FileName + "." + PartialFileObject.FileExtension + ")",
                           _Message: "Good day " + CurrentUser.FirstName + " " + CurrentUser.LastName + ". <br/><br/>A REQUEST notifiction has been re-sent to the owner: " + FileOwner.FirstName + " " + FileOwner.LastName + ", reminding him/her to review your notification.<br/> For the following file:<br/>File Name: " + PartialFileObject.FileName + "." + PartialFileObject.FileExtension + ".<br/>File Ref# " + FileID + ".<br/><br/> Regards Over Docs.",
                            _Credentials_UserName: es.UserName,
                            _Credentials_Password: es.Password,
                             _SMTP_HOST: es.SmtpHost,
                             _SMTP_PORT: es.SmtpPort,
                             _IsSsl: es.SslEnabled);

                        rtnSuccessMessage += "Notification has been successfully resent to the file owner and Email Notification Sent to the file owner for prompting to process your request.";
                    }
                }
                catch (DbEntityValidationException dbEx)
                {

                    foreach (DbEntityValidationResult entityErr in dbEx.EntityValidationErrors)
                    {
                        foreach (DbValidationError error in entityErr.ValidationErrors)
                        {
                            GlobalErrorMessage = "Error Property Name " + error.PropertyName + " : Error Message: " + error.ErrorMessage;
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobalErrorMessage = "Error : " + ex.Message.ToString();
                }
            };

            return Json(rtnSuccessMessage, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}

