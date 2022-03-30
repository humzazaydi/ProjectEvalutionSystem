namespace ProjectEvalutionSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Student
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string FullName { get; set; }

        [StringLength(50)]
        public string EmailAddress { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        public int? UserRole { get; set; }

        public int? TeacherID { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreationTimStamp { get; set; }

        public DateTime? ModificationTimeStamp { get; set; }
    }
}
