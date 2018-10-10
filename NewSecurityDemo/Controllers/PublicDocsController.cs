


using Common.Files;
using NewSecurityDemo.Models;
using OverDocsModels;
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
        public ActionResult PublicDocDisplay()
        {
            var dd = db.PublicDocs_R_GetMostRecentFileVersion();
            //Generates a list of all files returned from the database.
            List<View_PublicDocView_AllFilesWithOwnerAndUserThatLastUpdatedFile> AllPublicFiles = dd.ToList<View_PublicDocView_AllFilesWithOwnerAndUserThatLastUpdatedFile>();

            foreach (View_PublicDocView_AllFilesWithOwnerAndUserThatLastUpdatedFile f in AllPublicFiles)
            {
                f.FileType = FileExtensionHelper.GetFileType(f.FileExtension);
            }

            //Pass the list to the view so that it can display the data in the web page.
            return View(AllPublicFiles);
        }

        // GET: PublicDocs/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PublicDocs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PublicDocs/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PublicDocs/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PublicDocs/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PublicDocs/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PublicDocs/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
