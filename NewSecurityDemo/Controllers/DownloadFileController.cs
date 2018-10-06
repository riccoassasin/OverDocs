using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewSecurityDemo.Controllers
{
    public class DownloadFileController : Controller
    {
        private WebDocsEntities db = new WebDocsEntities();
        // GET: DownloadFile


        public ActionResult DownLoadSelectedFile(int FileID)
        {
            Models.File ObjFile = GetFileToDownload(FileID);
            if (ObjFile != null)
            {
                return File(ObjFile.FileImage, ObjFile.ContentType, ObjFile.FileName + "." + ObjFile.FileExtension);
            }
            else
            {

                return null;
            }


        }

        private Models.File GetFileToDownload(int _FileID)
        {

            Models.File fileToDownload;

            fileToDownload = db.Files.Where(a => a.FileID == _FileID).FirstOrDefault<File>();
            if (fileToDownload != null)
            {
                return fileToDownload;
            }
            else
            {
                return null;
            }

        }
    }
}