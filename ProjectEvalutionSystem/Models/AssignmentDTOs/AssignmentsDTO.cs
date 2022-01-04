using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectEvalutionSystem.Models.AssignmentDTOs
{
    public class AssignmentsDTO
    {
        public int ID { get; set; }
        public int? TeacherStudentID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public HttpPostedFileBase Files { get; set; }
    }
}