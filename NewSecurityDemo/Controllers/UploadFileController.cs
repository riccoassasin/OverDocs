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

        [HttpPost]
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
    }
}