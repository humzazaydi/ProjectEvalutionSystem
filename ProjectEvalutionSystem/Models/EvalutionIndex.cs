namespace ProjectEvalutionSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EvalutionIndex")]
    public partial class EvalutionIndex
    {
        public int ID { get; set; }

        public DateTime? SubmissionDate { get; set; }

        public DateTime? EvalutionDate { get; set; }

        public string Remarks { get; set; }

        public string Comments { get; set; }

        public int? AssignmentID { get; set; }

        public bool? IsCompleted { get; set; }

        [StringLength(50)]
        public string PlagCount { get; set; }

        [StringLength(50)]
        public string UniqueCount { get; set; }

        public string MatchesUrls { get; set; }

        public virtual Assignment Assignment { get; set; }
    }
}
