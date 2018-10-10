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
    
    public partial class File
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public File()
        {
            this.FileCategories = new HashSet<FileCategory>();
            this.FileSharedWithUsers = new HashSet<FileSharedWithUser>();
            this.Notifications = new HashSet<Notification>();
        }
    
        public int FileID { get; set; }
        public int ParentFileID { get; set; }
        public string UserIDOfFileOwner { get; set; }
        public string UserIDOfLastUploaded { get; set; }
        public int FileLookupStatusID { get; set; }
        public int FileShareStatusID { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public byte[] FileImage { get; set; }
        public int FileSize { get; set; }
        public int CurrentVersionNumber { get; set; }
        public System.DateTime DateCreated { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FileCategory> FileCategories { get; set; }
        public virtual LookupTable_FileStatuses LookupTable_FileStatuses { get; set; }
        public virtual LookupTable_ShareStatues LookupTable_ShareStatues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FileSharedWithUser> FileSharedWithUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual UserThatDownloadedFile UserThatDownloadedFile { get; set; }
    }
}
