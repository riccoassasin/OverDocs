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
    
    public partial class PublicDocs_R_GetSelectedFileHistory_Result
    {
        public Nullable<int> FileID { get; set; }
        public Nullable<int> ParentFileID { get; set; }
        public string UserIDOfFileOwner { get; set; }
        public string UserIDOfLastUploaded { get; set; }
        public string CurrentFileStatus { get; set; }
        public Nullable<int> FileLookupStatusID { get; set; }
        public string CurrentFileShareStatus { get; set; }
        public Nullable<int> FileShareStatusID { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FullFileName { get; set; }
        public Nullable<int> FileSize { get; set; }
        public Nullable<int> CurrentVersionNumber { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string NameOfFileOwner { get; set; }
        public string NameOfUserThatLastUpdatedFile { get; set; }
        public string ListOfUserIDThatTheFileISsharedWith { get; set; }
        public string IdOfUserThatDownloadedTheFile { get; set; }
        public Nullable<int> ComponentLevel { get; set; }
    }
}
