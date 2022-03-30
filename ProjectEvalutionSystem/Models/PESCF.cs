using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ProjectEvalutionSystem.Models
{
    public partial class PESCF : DbContext
    {
        public PESCF()
            : base("name=PESCF")
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<AspNet_SqlCacheTablesForChangeNotification> AspNet_SqlCacheTablesForChangeNotification { get; set; }
        public virtual DbSet<Assignment> Assignments { get; set; }
        public virtual DbSet<Cours> Courses { get; set; }
        public virtual DbSet<EvalutionIndex> EvalutionIndexes { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cours>()
                .HasMany(e => e.Assignments)
                .WithOptional(e => e.Cours)
                .HasForeignKey(e => e.CourseID);
        }
    }
}
