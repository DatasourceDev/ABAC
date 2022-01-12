using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABAC.Models;
using Microsoft.EntityFrameworkCore;

namespace ABAC.DAL
{
    public class SpuContext : DbContext
    {
        public SpuContext(DbContextOptions options) : base(options) { }

        public SpuContext()
        {

        }
        public DbSet<setup>table_setup { get; set; }
        public DbSet<cms> table_cms { get; set; }
        public DbSet<landing_page> table_landing_page { get; set; }

        public DbSet<User_VIP> User_VIP { get; set; }
        public DbSet<User_Office> User_Office { get; set; }
        public DbSet<User_Bulk> User_Bulk { get; set; }
        public DbSet<User_Bulk_Import> User_Bulk_Import { get; set; }
        public DbSet<temp_import> table_temp_import { get; set; }
        public DbSet<user_role> table_user_role { get; set; }
        public DbSet<activate_code> table_activate_code { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
