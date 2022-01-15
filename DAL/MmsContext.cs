using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABAC.Models;
using Microsoft.EntityFrameworkCore;

namespace ABAC.DAL
{
    public class MmsContext : DbContext
    {
        public MmsContext(DbContextOptions<MmsContext> options) : base(options) { }

        public MmsContext()
        {

        }
        public DbSet<m_step_history> mms_step_history { get; set; }
        public DbSet<m_run_history> mms_run_history { get; set; }
        public DbSet<m_step_object_details> mms_step_object_details { get; set; }


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
