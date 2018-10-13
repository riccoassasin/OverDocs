using Common.Enum.DBLookupEnum;
using Common.Files;
using Microsoft.AspNet.Identity;
using NewSecurityDemo.Models;
using OverDocsModels;
using PagedList;
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
        public ActionResult ShowUserDocs(string sortOrder, string currentFilter, string searchString, int? page)
        {


            ViewBag.CurrentSort = sortOrder;
            ViewBag.FileIDSortParm = sortOrder == "FileID" ? "FileID_desc" : "FileID";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "FullFileName_desc" : "";
            ViewBag.DateSortParm = sortOrder == "DateCreated" ? "DateCreated_desc" : "DateCreated";
            ViewBag.NameOfFileOwnerSortParm = sortOrder == "NameOfFileOwner" ? "NameOfFileOwner_desc" : "NameOfFileOwner";
            ViewBag.CurrentFileStatusSortParm = sortOrder == "CurrentFileStatus" ? "CurrentFileStatus_desc" : "CurrentFileStatus";
            ViewBag.CurrentFileShareStatusSortParm = sortOrder == "CurrentFileShareStatus" ? "CurrentFileShareStatus_desc" : "CurrentFileShareStatus";
            ViewBag.CurrentVersionNumberSortParm = sortOrder == "CurrentVersionNumber" ? "CurrentVersionNumber_desc" : "CurrentVersionNumber";
            ViewBag.FileSizeSortParm = sortOrder == "FileSize" ? "FileSize_desc" : "FileSize";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            ObjectResult<View_UserDocs_AllUserCreatedDocs> dd = db.UserDocs_R_GetAllUserCreatedDocs(User.Identity.GetUserId());

            List<View_UserDocs_AllUserCreatedDocs> AllUserFiles = dd.ToList<View_UserDocs_AllUserCreatedDocs>();

            foreach (View_UserDocs_AllUserCreatedDocs f in AllUserFiles)
            {
                f.FileType = FileExtensionHelper.GetFileType(f.FileExtension);
            }



            switch (sortOrder)
            {
                case "FullFileName_desc":
                    AllUserFiles = AllUserFiles.OrderByDescending(s => s.FullFileName).ToList();
                    break;

                case "DateCreated":
                    AllUserFiles = AllUserFiles.OrderBy(s => s.DateCreated).ToList();
                    break;
                case "DateCreated_desc":
                    AllUserFiles = AllUserFiles.OrderByDescending(s => s.DateCreated).ToList();
                    break;

                //case "NameOfFileOwner":
                //    AllUserFiles = AllUserFiles.OrderBy(s => s.NameOfFileOwner).ToList();
                //    break;
                //case "NameOfFileOwner_desc":
                //    AllUserFiles = AllUserFiles.OrderByDescending(s => s.NameOfFileOwner).ToList();
                //    break;

                case "FileID":
                    AllUserFiles = AllUserFiles.OrderBy(s => s.FileID).ToList();
                    break;
                case "FileID_desc":
                    AllUserFiles = AllUserFiles.OrderByDescending(s => s.FileID).ToList();
                    break;


                case "FileSize":
                    AllUserFiles = AllUserFiles.OrderBy(s => s.FileSize).ToList();
                    break;
                case "FileSize_desc":
                    AllUserFiles = AllUserFiles.OrderByDescending(s => s.FileSize).ToList();
                    break;

                //case "CurrentFileStatus":
                //    AllUserFiles = AllUserFiles.OrderBy(s => s.CurrentFileStatus).ToList();
                //    break;
                //case "CurrentFileStatus_desc":
                //    AllUserFiles = AllUserFiles.OrderByDescending(s => s.CurrentFileStatus).ToList();
                //    break;

                //case "CurrentFileShareStatus":
                //    AllUserFiles = AllUserFiles.OrderBy(s => s.CurrentFileShareStatus).ToList();
                //    break;
                //case "CurrentFileShareStatus_desc":
                //    AllUserFiles = AllUserFiles.OrderByDescending(s => s.CurrentFileShareStatus).ToList();
                //    break;

                case "CurrentVersionNumber":
                    AllUserFiles = AllUserFiles.OrderBy(s => s.CurrentVersionNumber).ToList();
                    break;
                case "CurrentVersionNumber_desc":
                    AllUserFiles = AllUserFiles.OrderByDescending(s => s.CurrentVersionNumber).ToList();
                    break;
                default:  // Name ascending 
                    AllUserFiles = AllUserFiles.OrderBy(s => s.FullFileName).ToList();
                    break;
            };

            //ViewBag.CurrentFilter = "";
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(AllUserFiles.ToPagedList(pageNumber, pageSize));

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
            
            return RedirectToAction("ShowUserDocs");
        }

    }
}