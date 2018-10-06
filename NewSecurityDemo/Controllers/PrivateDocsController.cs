using Common.Files;
using Microsoft.AspNet.Identity;
using NewSecurityDemo.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewSecurityDemo.Controllers
{
    public class PrivateDocsController : Controller
    {

        private WebDocsEntities db = new WebDocsEntities();
        // GET: PrivateDocs
        [Authorize]
        public ActionResult PrivateDocDisplay()
        {
            //IQueryable<View_PrivateDocView_AllSharedPrivateFiles> File_Query = from f in db.View_PrivateDocView_AllSharedPrivateFiles
            //select f;

            var dd = db.PrivateDocs_R_GetAllPrivateSharedUserFiles(User.Identity.GetUserId());

            ////Generates a list of all files returned from the database.
            List<View_PrivateDocView_AllSharedPrivateFiles> AllPrivateFiles = dd.ToList<View_PrivateDocView_AllSharedPrivateFiles>();

            foreach (View_PrivateDocView_AllSharedPrivateFiles f in AllPrivateFiles)
            {
                f.FileType = FileExtensionHelper.GetFileType(f.FileExtension);
            }
            return View(AllPrivateFiles);
        }
    }
}