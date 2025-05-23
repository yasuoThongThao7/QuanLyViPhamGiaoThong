using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WpfApp2.Model
{
    public class AppDbContext : DbContext
    {
        private static readonly string? connectionString = "server=localhost;port=3306;database=quanlygiaothong;user id=root;password=liemlol0934;sslmode=Preferred";

        public DbSet<Account>? Account { get; set; }
        public DbSet<Person>? Person { get; set; }
        public DbSet<Police>? Police { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<Violation> Violation { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<ReportInfo> ReportInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(connectionString!);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReportInfo>()
                .HasKey(ri => new { ri.ReportId, ri.ViolationId });

            modelBuilder.Entity<ReportInfo>()
                .HasOne(ri => ri.Report)
                .WithMany(r => r.Violations)
                .HasForeignKey(ri => ri.ReportId);

            modelBuilder.Entity<ReportInfo>()
                .HasOne(ri => ri.Violation)
                .WithMany()
                .HasForeignKey(ri => ri.ViolationId);
        }
    }
}
