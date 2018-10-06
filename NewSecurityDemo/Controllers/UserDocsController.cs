using Microsoft.AspNet.Identity;
using NewSecurityDemo.Models;
using NewSecurityDemo.Models.Common;
using NewSecurityDemo.Models.EnumClasses.DBLookupEnum;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewSecurityDemo.Controllers
{
    public class UserDocsController : Controller
    {
        private NewSecurityDemo.Models.WebDocsEntities db = new NewSecurityDemo.Models.WebDocsEntities();
        // GET: UserDocs
        [Authorize]
        public ActionResult ShowUserDocs()
        {
            System.Data.Entity.Core.Objects.ObjectResult<View_UserDocs_AllUserCreatedDocs> dd = db.UserDocs_R_GetAllUserCreatedDocs(User.Identity.GetUserId());

            //Generates a list of all files returned from the database.
            List<View_UserDocs_AllUserCreatedDocs> AllUserFiles = dd.ToList<View_UserDocs_AllUserCreatedDocs>();

            foreach (View_UserDocs_AllUserCreatedDocs f in AllUserFiles)
            {
                f.FileType = FileExtensionHelper.GetFileType(f.FileExtension);
            }

            return View(AllUserFiles);

        }

        [HttpPost]
        public ActionResult UploadUserFile(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    //string path = Path.Combine(Server.MapPath("~/Images"),
                    //                           Path.GetFileName(file.FileName));
                    //file.SaveAs(path);


                    byte[] uploadedFile = new byte[file.InputStream.Length];
                    file.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

                    int length = file.FileName.Length;
                    string[] Name = file.FileName.Split('.');
                    ///insert into db 
                    ///

                    Models.File newfile = new Models.File();

                    Models.File std = new Models.File()
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