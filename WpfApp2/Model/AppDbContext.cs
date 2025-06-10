using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WpfApp2.Model
{
    public class AppDbContext : DbContext
    {
        private static readonly string? connectionString;

        static AppDbContext()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (!File.Exists(path))
            {
                MessageBox.Show("File appsettings.json không tồn tại ở: " + path);
            }

            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            connectionString = config.GetConnectionString("DefaultConnection");
        }


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
