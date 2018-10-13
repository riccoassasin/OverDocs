using NewSecurityDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using OverDocsModels;

using System.Data.Entity.Validation;
using Microsoft.AspNet.Identity;
using Common.Enum.DBLookupEnum;
using System.Data.Entity.Core.Objects;

namespace NewSecurityDemo.Controllers
{
    public class UploadFileController : Controller
    {
        private WebDocsEntities db = new WebDocsEntities();
        // GET: UploadFile
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost] // Fetches the userfile from the database that the user uloaded and passes it through the controller
        public JsonResult UserFileUpload(int _FileShareStatusID)
        {
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        try
                        {
                            byte[] uploadedFile = new byte[fileContent.InputStream.Length];
                            fileContent.InputStream.Read(uploadedFile, 0, uploadedFile.Length);
                            string xddd = Path.GetFileName(fileContent.FileName);
                            int length = fileContent.FileName.Length;
                            string[] Name = fileContent.FileName.Split('.');

                            Models.File newfile = new Models.File();

                            Models.File std = new Models.File()
                            {
                                ContentType = fileContent.ContentType,
                                CurrentVersionNumber = 1,
                                DateCreated = DateTime.Now,
                                FileImage = uploadedFile,
                                FileName = Path.GetFileNameWithoutExtension(fileContent.FileName),
                                FileSize = fileContent.ContentLength,
                                ParentFileID = 0, //default value
                                UserIDOfFileOwner = User.Identity.GetUserId(),
                                UserIDOfLastUploaded = User.Identity.GetUserId(),
                                FileLookupStatusID = (int)FileViewStatus.FileIsAvailable,
                                FileShareStatusID = _FileShareStatusID,
                                FileExtension = Path.GetExtension(fileContent.FileName).Replace(".", "")
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
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }

            return Json("File uploaded successfully");
        }


        [HttpPost]
        public JsonResult ReturnDocsFileUpload(int OldFileID)
        {
            File_R_AllFileDetailsWithoutFioleImage_Result OldFile = db.File_R_AllFileDetailsWithoutFioleImage(OldFileID).FirstOrDefault<File_R_AllFileDetailsWithoutFioleImage_Result>();
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        try
                        {
                            byte[] uploadedFile = new byte[fileContent.InputStream.Length];
                            fileContent.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

                            int length = fileContent.FileName.Length;
                            string[] Name = fileContent.FileName.Split('.');

                            Models.File newfile = new Models.File();

                            try
                            {
                                Models.File NewFileObjc = db.Files_I_NewFile(OldFile.FileID,
                                OldFile.UserIDOfFileOwner,
                                User.Identity.GetUserId(),
                                (int)FileViewStatus.FileIsAvailable,
                                OldFile.FileShareStatusID,
                                fileContent.ContentType,
                                Path.GetFileNameWithoutExtension(fileContent.FileName),
                                Path.GetExtension(fileContent.FileName).Replace(".", ""),
                                uploadedFile,
                                fileContent.ContentLength,
                                OldFile.CurrentVersionNumber + 1).FirstOrDefault<Models.File>();

                                List<FileSharedWithUser> FSWU = (from a in db.FileSharedWithUsers
                                                                 where a.FileID == OldFileID
                                                                 select a).ToList<FileSharedWithUser>();

                                List<FileSharedWithUser> NewFSWUList = new List<FileSharedWithUser>();
                                foreach (FileSharedWithUser CurrentFSWU in FSWU)
                                {
                                    NewFSWUList.Add(new FileSharedWithUser()
                                    {
                                        UserIDOfSharedDocs = CurrentFSWU.UserIDOfSharedDocs,
                                        FileID = NewFileObjc.FileID,
                                        DateShared = DateTime.Now
                                    });
                                }
                                db.FileSharedWithUsers.AddRange(NewFSWUList);

                                UserThatDownloadedFile UTDF = db.UserThatDownloadedFiles.Where(a => a.FileID == OldFileID).FirstOrDefault();

                                UTDF.HasFileBeenReturned = true;


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
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }

            return Json("File uploaded successfully");
        }
    }
}