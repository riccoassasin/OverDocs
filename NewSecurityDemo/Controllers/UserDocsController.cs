using Common.Enum.DBLookupEnum;
using Common.Files;
using Microsoft.AspNet.Identity;
using NewSecurityDemo.Models;
using OverDocsModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewSecurityDemo.Controllers
{
    public class UserDocsController : Controller
    {
        private WebDocsEntities db = new WebDocsEntities();
        // GET: UserDocs
        [Authorize]
        public ActionResult ShowUserDocs()
        {
            ObjectResult<View_UserDocs_AllUserCreatedDocs> dd = db.UserDocs_R_GetAllUserCreatedDocs(User.Identity.GetUserId());

            List<View_UserDocs_AllUserCreatedDocs> AllUserFiles = dd.ToList<View_UserDocs_AllUserCreatedDocs>();

            foreach (View_UserDocs_AllUserCreatedDocs f in AllUserFiles)
            {
                f.FileType = FileExtensionHelper.GetFileType(f.FileExtension);
            }
            EmailSetting ES = db.EmailSettings.FirstOrDefault();
            //Common.Email.EmailHelper.sendMessageAsync(
            //    _ToAddress: "Brendanw@mweb.co.za",
            //    _FromAddress: "Brendanw@mweb.co.za",
            //    _FromName: "Brednan Wood",
            //    _ToName: "Ricco",
            //    _Subject: "Test Message",
            //    _Message: "This Shows that the System can can send email messages",
            //     _Credentials_UserName: "Brendanw@mweb.co.za",
            //     _Credentials_Password: "speedie3",
            //      _SMTP_HOST: "smtp.mweb.co.za",
            //      _SMTP_PORT: 25,
            //      _IsSsl: false);

            return View(AllUserFiles);

        }

        [HttpPost]
        public ActionResult UploadUserFile(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    byte[] uploadedFile = new byte[file.InputStream.Length];
                    file.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

                    int length = file.FileName.Length;
                    string[] Name = file.FileName.Split('.');

                    File newfile = new File();

                    File std = new File()
                    {
                        ContentType = file.ContentType,
                        CurrentVersionNumber = 1,
                        DateCreated = DateTime.Now,
                        FileImage = uploadedFile,
                        FileName = Name[0],
                        FileSize = file.ContentLength,
                        ParentFileID = 0,
                        UserIDOfFileOwner = User.Identity.GetUserId(),
                        UserIDOfLastUploaded = User.Identity.GetUserId(),
                        FileLookupStatusID = (int)FileViewStatus.FileIsAvailable,
                        FileShareStatusID = (int)FileSharedStatus.Private,
                        FileExtension = Name[1]


                    };
                    db.Files.Add(std);
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



                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }

            }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            //if (file != null && file.ContentLength > 0)
            //    try
            //    {
            //        string path = Path.Combine(Server.MapPath("~/Images"),
            //                                   Path.GetFileName(file.FileName));
            //        file.SaveAs(path);
            //        ViewBag.Message = "File uploaded successfully";
            //    }
            //    catch (Exception ex)
            //    {
            //        ViewBag.Message = "ERROR:" + ex.Message.ToString();
            //    }
            //else
            //{
            //    ViewBag.Message = "You have not specified a file.";
            //}
            return RedirectToAction("ShowUserDocs");
        }

    }
}