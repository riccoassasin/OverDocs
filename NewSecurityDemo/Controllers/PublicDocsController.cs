﻿


using Common.Files;
using NewSecurityDemo.Models;
using OverDocsModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace MyOverDocs.Controllers
{
    public class PublicDocsController : Controller
    {

        private WebDocsEntities db = new WebDocsEntities();
        // GET: PublicDocs

        [Authorize]
        public ActionResult PublicDocDisplay(string sortOrder, string currentFilter, string searchString, int? page)
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


            List<View_PublicDocView_AllFilesWithOwnerAndUserThatLastUpdatedFile> AllPublicFiles = (from a in db.PublicDocs_R_GetMostRecentFileVersion()
                                  select a).ToList<View_PublicDocView_AllFilesWithOwnerAndUserThatLastUpdatedFile>();


            if (!String.IsNullOrEmpty(searchString))
            {
                AllPublicFiles = AllPublicFiles.Where(s => s.FullFileName.Contains(searchString)).ToList<View_PublicDocView_AllFilesWithOwnerAndUserThatLastUpdatedFile>();
            }

            var dd = db.PublicDocs_R_GetMostRecentFileVersion();

            //foreach (View_PublicDocView_AllFilesWithOwnerAndUserThatLastUpdatedFile f in AllPublicFiles)
            //{
            //    f.FileType = FileExtensionHelper.GetFileType(f.FileExtension);
            //}

            switch (sortOrder)
            {
                case "FullFileName_desc":
                    AllPublicFiles = AllPublicFiles.OrderByDescending(s => s.FullFileName).ToList();
                    break;

                case "DateCreated":
                    AllPublicFiles = AllPublicFiles.OrderBy(s => s.DateCreated).ToList();
                    break;
                case "DateCreated_desc":
                    AllPublicFiles = AllPublicFiles.OrderByDescending(s => s.DateCreated).ToList();
                    break;

                case "FileID":
                    AllPublicFiles = AllPublicFiles.OrderBy(s => s.FileID).ToList();
                    break;
                case "FileID_desc":
                    AllPublicFiles = AllPublicFiles.OrderByDescending(s => s.FileID).ToList();
                    break;


                case "FileSize":
                    AllPublicFiles = AllPublicFiles.OrderBy(s => s.FileSize).ToList();
                    break;
                case "FileSize_desc":
                    AllPublicFiles = AllPublicFiles.OrderByDescending(s => s.FileSize).ToList();
                    break;

                case "CurrentFileStatus":
                    AllPublicFiles = AllPublicFiles.OrderBy(s => s.CurrentFileStatus).ToList();
                    break;
                case "CurrentFileStatus_desc":
                    AllPublicFiles = AllPublicFiles.OrderByDescending(s => s.CurrentFileStatus).ToList();
                    break;

                case "CurrentFileShareStatus":
                    AllPublicFiles = AllPublicFiles.OrderBy(s => s.CurrentFileShareStatus).ToList();
                    break;
                case "CurrentFileShareStatus_desc":
                    AllPublicFiles = AllPublicFiles.OrderByDescending(s => s.CurrentFileShareStatus).ToList();
                    break;

                case "CurrentVersionNumber":
                    AllPublicFiles = AllPublicFiles.OrderBy(s => s.CurrentVersionNumber).ToList();
                    break;
                case "CurrentVersionNumber_desc":
                    AllPublicFiles = AllPublicFiles.OrderByDescending(s => s.CurrentVersionNumber).ToList();
                    break;
                default:  // Name ascending 
                    AllPublicFiles = AllPublicFiles.OrderBy(s => s.FullFileName).ToList();
                    break;
            };

            //ViewBag.CurrentFilter = "";
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            //Pass the list to the view so that it can display the data in the web page.
            return View(AllPublicFiles.ToPagedList(pageNumber, pageSize));
        }

        //public ActionResult PublicDocHistoryPartialView()
        //{
        //    List<Emp_Information> model = db..ToList();
        //    return PartialView("_EmpPartial", model);
        //}
    }
}
