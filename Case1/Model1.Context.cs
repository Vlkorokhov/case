﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Case1
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WWWEntities3 : DbContext
    {
        public WWWEntities3()
            : base("name=WWWEntities3")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Case> Case { get; set; }
        public virtual DbSet<city> city { get; set; }
        public virtual DbSet<Class_serv> Class_serv { get; set; }
        public virtual DbSet<Know> Know { get; set; }
        public virtual DbSet<know2> know2 { get; set; }
        public virtual DbSet<Table> Table { get; set; }
        public virtual DbSet<Table2> Table2 { get; set; }
    }
}
