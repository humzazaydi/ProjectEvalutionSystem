namespace ProjectEvalutionSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Assignment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Assignment()
        {
            EvalutionIndexes = new HashSet<EvalutionIndex>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(120)]
        public string Description { get; set; }

        public string Path { get; set; }

        public int? CourseID { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime CreationTimeStamp { get; set; }

        public int? StudentID { get; set; }

        public virtual Cours Cours { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EvalutionIndex> EvalutionIndexes { get; set; }
    }
}
