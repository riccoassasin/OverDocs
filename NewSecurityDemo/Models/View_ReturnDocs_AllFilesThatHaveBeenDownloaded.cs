//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NewSecurityDemo.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class View_ReturnDocs_AllFilesThatHaveBeenDownloaded
    {
        public Nullable<int> FileID { get; set; }
        public Nullable<int> ParentFileID { get; set; }
        public string UserIDOfFileOwner { get; set; }
        public string UserIDOfLastUploaded { get; set; }
        public Nullable<int> CurrentVersionNumber { get; set; }
        public string UserIDThatDownloadedFIle { get; set; }
        public bool HasFileBeenReturned { get; set; }
        public string FullNameOfFileOwner { get; set; }
        public string FullNameOfThePersonThatLastUpdatedTheFile { get; set; }
        public string FullNameOfPersonThatDownloadedTheFile { get; set; }
        public Nullable<int> FileSize { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FullFileName { get; set; }
    }
}