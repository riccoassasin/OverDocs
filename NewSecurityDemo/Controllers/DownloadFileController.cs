using NewSecurityDemo.Models;
using OverDocsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NewSecurityDemo.Controllers
{
    public class DownloadFileController : Controller
    {
        private WebDocsEntities db = new WebDocsEntities();
        // GET: DownloadFile

        [HttpPost]
        public async Task<ActionResult> DownLoadSelectedFile(int FileID, string UserIDOfPersonThatDownloadedTheFile)
        {
            File ObjFile = GetFileToDownload(FileID);

            string errorMessage = "";
            AspNetUser FileOwner;
            AspNetUser PersonThatDownLoadedTheFile;

            FileOwner = db.AspNetUsers.Where(a => a.Id == ObjFile.UserIDOfFileOwner).FirstOrDefault();
            PersonThatDownLoadedTheFile = db.AspNetUsers.Where(a => a.Id == UserIDOfPersonThatDownloadedTheFile).FirstOrDefault();

            EmailSetting es = (from a in db.EmailSettings select a).FirstOrDefault<EmailSetting>();

            //Check To verify that the file has not been downloaded and locked 
            //If it has been locked it means that it is iether being downloaded by the same user again or is a download from the file history.
            if ((from a in db.UserThatDownloadedFiles where a.FileID == FileID select a).Count() == 0)
            {
                ObjFile.FileLookupStatusID = (int)Common.Enum.DBLookupEnum.FileViewStatus.FileIsLocked;
                ObjFile.UserThatDownloadedFile = new UserThatDownloadedFile
                {
                    FileID = FileID,
                    UserIDThatDownloadedFIle = UserIDOfPersonThatDownloadedTheFile,
                    DateDownloaded = DateTime.Now
                };

                string ownerFullName = FileOwner.FirstName + " " + FileOwner.LastName;
                string PersonThatDownloadedTheFile_FullName = PersonThatDownLoadedTheFile.FirstName + "  " + PersonThatDownLoadedTheFile.LastName;
                //send message to the file owner informing the owner that the file has been down loaded by user.

                await Common.Email.EmailHelper.sendMessageAsync(
                               _ToAddress: FileOwner.Email,
                               _FromAddress: FileOwner.Email,
                               _FromName: ownerFullName,
                               _ToName: ownerFullName,
                               _Subject: "Over Docs System - File (" + ObjFile.FileID + " - " + ObjFile.FileName + "." + ObjFile.FileExtension + ") has been downloaded.",
                               _Message: "Good day " + ownerFullName + ". <br/><br/>One of your files have been downloaded by :<br/>" + PersonThatDownloadedTheFile_FullName + " and been locked.< br />< br /> Regards Over Docs.",
                                _Credentials_UserName: es.UserName,
                                _Credentials_Password: es.Password,
                                 _SMTP_HOST: es.SmtpHost,
                                 _SMTP_PORT: es.SmtpPort,
                                 _IsSsl: es.SslEnabled);

                //if the person that downloaded tehe file is a doofferent person to the owner send that person a message informing them that the file can be downloaded by them
                if (!(UserIDOfPersonThatDownloadedTheFile.Equals(FileOwner.Id)))
                {
                    await Common.Email.EmailHelper.sendMessageAsync(
                                  _ToAddress: PersonThatDownLoadedTheFile.Email,
                                  _FromAddress: FileOwner.Email,
                                  _FromName: ownerFullName,
                                  _ToName: PersonThatDownloadedTheFile_FullName,
                                  _Subject: "Over Docs System - File (" + ObjFile.FileID + " - " + ObjFile.FileName + "." + ObjFile.FileExtension + ") has been downloaded.",
                                  _Message: "Good day " + PersonThatDownloadedTheFile_FullName + ". <br/><br/>You successfull downloadded the following File::<br/>(" + ObjFile.FileName + "." + ObjFile.FileExtension + ") and been locked for you, ONLY you may now edit the file.<br/>The file will remain locked for other users untill you upload it again!< br />< br /> Regards Over Docs.",
                                   _Credentials_UserName: es.UserName,
                                   _Credentials_Password: es.Password,
                                    _SMTP_HOST: es.SmtpHost,
                                    _SMTP_PORT: es.SmtpPort,
                                    _IsSsl: es.SslEnabled);
                }
               
            }

            try
            {
                await db.SaveChangesAsync();
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