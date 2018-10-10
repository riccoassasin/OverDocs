using NewSecurityDemo.Models;
using OverDocsModels;
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

        [HttpPost]
        public ActionResult DownLoadSelectedFile(int FileID, string UserIDOfPersonThatDownloadedTheFile)
        {
            File ObjFile = GetFileToDownload(FileID);
            ObjFile.FileLookupStatusID = (int)Common.Enum.DBLookupEnum.FileViewStatus.FileIsLocked;
            ObjFile.UserThatDownloadedFile = new UserThatDownloadedFile
            {
                FileID = FileID,
                UserIDThatDownloadedFIle = UserIDOfPersonThatDownloadedTheFile,
                DateDownloaded = DateTime.Now
            };
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
            if (ObjFile != null)
            {
                return File(ObjFile.FileImage, ObjFile.ContentType, ObjFile.FileName + "." + ObjFile.FileExtension);
            }
            else
            {

                return null;
            }


        }


        private File GetFileToDownload(int _FileID)
        {

            File fileToDownload;

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