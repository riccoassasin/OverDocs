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

            string errorMessage = "";

            //Check To verify that the file has not been downloaded and locked 
            //If it has been locked it means that it is iether being downloaded by the same user again or is a download from the file history.
            if ((from a in db.UserThatDownloadedFiles
                 where a.FileID == FileID
                 select a).Count() == 0)
            {
                ObjFile.FileLookupStatusID = (int)Common.Enum.DBLookupEnum.FileViewStatus.FileIsLocked;
                ObjFile.UserThatDownloadedFile = new UserThatDownloadedFile
                {
                    FileID = FileID,
                    UserIDThatDownloadedFIle = UserIDOfPersonThatDownloadedTheFile,
                    DateDownloaded = DateTime.Now
                };
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                throw;
                
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