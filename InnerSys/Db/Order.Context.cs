﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace InnerSys.Db
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OrderEntities : DbContext
    {
        public OrderEntities()
            : base("name=OrderEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeePremission> EmployeePremission { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Menus> Menus { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<OrderNum> OrderNum { get; set; }
        public virtual DbSet<RolesToMenus> RolesToMenus { get; set; }
    }
}
