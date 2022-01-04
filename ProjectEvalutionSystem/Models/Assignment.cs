//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectEvalutionSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Assignment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Assignment()
        {
            this.EvalutionIndexes = new HashSet<EvalutionIndex>();
        }
    
        public int ID { get; set; }
        public Nullable<int> TeacherStudent { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreationTimeStamp { get; set; }
    
        public virtual StudentTeacher StudentTeacher { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EvalutionIndex> EvalutionIndexes { get; set; }
    }
}
