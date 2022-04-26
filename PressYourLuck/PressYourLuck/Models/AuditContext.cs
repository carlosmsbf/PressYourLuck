using Microsoft.EntityFrameworkCore;
using System;

namespace PressYourLuck.Models
{
    public class AuditContext: DbContext
    {
        public AuditContext(DbContextOptions options)
           : base(options) { }

        public DbSet<AuditType> AuditTypes { get; set; }

        public DbSet<Audit> Audits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditType>().HasData(
                new AuditType()
                {
                    AuditTypeId = 1,
                    Name = "Cash In"
                },
                new AuditType()
                {
                    AuditTypeId = 2,
                    Name = "Cash Out"
                },
                new AuditType()
                {
                    AuditTypeId = 3,
                    Name = "Win"
                },
                new AuditType()
                {
                    AuditTypeId = 4,
                    Name = "Lose"
                });


            modelBuilder.Entity<Audit>().HasData(
                new Audit
                {
                    AuditId = 1,
                    Name = "test",
                    CreatedDate = DateTime.Now,
                    Amount=100.10,
                    AuditTypeId=1
                }
                );

                }


    }
}
