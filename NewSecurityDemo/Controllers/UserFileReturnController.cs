using NewSecurityDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Microsoft.AspNet.Identity;


namespace NewSecurityDemo.Controllers
{
    public class UserFileReturnController : Controller
    {
        private WebDocsEntities db = new WebDocsEntities();
        // GET: UserFileReturn
        public ActionResult UserFileReturnView(string sortOrder, string currentFilter, string searchString, int? page)
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

            string CurrentlyLogginUser = User.Identity.GetUserId();
            List<View_ReturnDocs_AllFilesThatHaveBeenDownloaded> AllPublicFiles = (from a in db.View_ReturnDocs_AllFilesThatHaveBeenDownloaded
                                                                                   where
                                                                                           a.HasFileBeenReturned == false &&
                                                                                           a.UserIDThatDownloadedFIle == CurrentlyLogginUser
                                                                                   select a).ToList<View_ReturnDocs_AllFilesThatHaveBeenDownloaded>();



            switch (sortOrder)
            {
                case "FullFileName_desc":
                    AllPublicFiles = AllPublicFiles.OrderByDescending(s => s.FullFileName).ToList();
                    break;

                //case "DateCreated":
                //    AllPublicFiles = AllPublicFiles.OrderBy(s => s.DateCreated).ToList();
                //    break;
                //case "DateCreated_desc":
                //    AllPublicFiles = AllPublicFiles.OrderByDescending(s => s.DateCreated).ToList();
                //    break;

                //case "NameOfFileOwner":
                //    AllPublicFiles = AllPublicFiles.OrderBy(s => s.NameOfFileOwner).ToList();
                //    break;
                //case "NameOfFileOwner_desc":
                //    AllPublicFiles = AllPublicFiles.OrderByDescending(s => s.NameOfFileOwner).ToList();
                //    break;

                //case "FileID":
                //    AllPublicFiles = AllPublicFiles.OrderBy(s => s.FileID).ToList();
                //    break;
                //case "FileID_desc":
                //    AllPublicFiles = AllPublicFiles.OrderByDescending(s => s.FileID).ToList();
                //    break;


                //case "FileSize":
                //    AllPublicFiles = AllPublicFiles.OrderBy(s => s.FileSize).ToList();
                //    break;
                //case "FileSize_desc":
                //    AllPublicFiles = AllPublicFiles.OrderByDescending(s => s.FileSize).ToList();
                //    break;

                //case "CurrentFileStatus":
                //    AllPublicFiles = AllPublicFiles.OrderBy(s => s.CurrentFileStatus).ToList();
                //    break;
                //case "CurrentFileStatus_desc":
                //    AllPublicFiles = AllPublicFiles.OrderByDescending(s => s.CurrentFileStatus).ToList();
                //    break;

                //case "CurrentFileShareStatus":
                //    AllPublicFiles = AllPublicFiles.OrderBy(s => s.CurrentFileShareStatus).ToList();
                //    break;
                //case "CurrentFileShareStatus_desc":
                //    AllPublicFiles = AllPublicFiles.OrderByDescending(s => s.CurrentFileShareStatus).ToList();
                //    break;

                //case "CurrentVersionNumber":
                //    AllPublicFiles = AllPublicFiles.OrderBy(s => s.CurrentVersionNumber).ToList();
                //    break;
                //case "CurrentVersionNumber_desc":
                //    AllPublicFiles = AllPublicFiles.OrderByDescending(s => s.CurrentVersionNumber).ToList();
                //    break;

                default:  // Name ascending 
                    AllPublicFiles = AllPublicFiles.OrderBy(s => s.FullFileName).ToList();
                    break;
            };


            //AllPublicFiles.ToPagedList(pageNumber, pageSize)
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(AllPublicFiles.ToPagedList(pageNumber, pageSize));
        }
    }
}