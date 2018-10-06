using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewSecurityDemo.Models.FileUpload
{
    public class UserFileUploadModel
    {
        [Required(ErrorMessage = "Please Select File.")]
        [Display(Name ="Select File")]
        public HttpPostedFileBase[] files { get; set; }
    }
}