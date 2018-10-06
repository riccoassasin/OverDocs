using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewSecurityDemo.Models.ModelExtensions
{
    public partial class WebDocsEntities
    {
        
        public static void ObjectContext_ObjectMaterialized(object sender, System.Data.ObjectMaterializedEventArgs e)
        {
            if (e.Entity == null)
                return;
            //if (e.Entity.GetType() == typeof(Student))
            //{
            //    Student student = e.Entity as Student;
            //    // There is no problem in using Navigation Properties here
            //    // If they haven't been materialized yet, it will be done here, but only once.
            //    // Don't worry, there will be no stack overflow.

            //    Teacher teacher = student.Teacher;
            //    Course firstCourse = student.Courses.First();

            //    // Do whatever you want here with your entities.
            //}
        }
    }
}