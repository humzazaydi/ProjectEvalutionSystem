﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ProjectEvalutionSystemEntities : DbContext
    {
        public ProjectEvalutionSystemEntities()
            : base("name=ProjectEvalutionSystemEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Assignment> Assignments { get; set; }
        public virtual DbSet<EvalutionIndex> EvalutionIndexes { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentTeacher> StudentTeachers { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
    }
}
