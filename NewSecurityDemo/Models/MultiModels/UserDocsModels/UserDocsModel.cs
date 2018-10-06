using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewSecurityDemo.Models.MultiModels.UserDocsModels
{
    public class UserDocsModel
    {
        public List<View_UserDocs_AllUserCreatedDocs> UserDocs { get; set; }
        public Models.FileUpload.UserFileUploadModel Files { get; set; }

    }
}