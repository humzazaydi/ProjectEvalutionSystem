using System;
using System.Collections.Generic;

namespace ProjectEvalutionSystem.Models.EvalutionIndexDTOs
{
    public class EvalutionIndexDTO
    {
        public int Id { get; set; }
        public DateTime? EvalutionDate { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public string Remarks { get; set; }
        public string Comments { get; set; }
        public int? StudentID { get; set; }
        public int? TeacherID { get; set; }
        public int? AssignmentID { get; set; }

        public string AssignmentName { get; set; }
        public string StudentName { get; set; }
        public string TeacherName { get; set; }

        public static EvalutionIndexDTO EvalutionIndexConverter(EvalutionIndex input)
        {
            return new EvalutionIndexDTO()
            {
                Id = input.ID,
                EvalutionDate = input.EvalutionDate,
                SubmissionDate = input.SubmissionDate,
                Remarks = input.Remarks,
                Comments = input.Comments,
                StudentID = input.StudentID,
                TeacherID = input.TeacherID,
                AssignmentID = input.AssignmentID,
                AssignmentName = input.Assignment.Name,
                StudentName = input.Student.FullName,
                TeacherName = input.Teacher.FullName
            };
        }
    }

    
}