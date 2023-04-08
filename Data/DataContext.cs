using CarWebsiteBackend.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CarWebsiteBackend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<TestDrive> TestDrives { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.email)
                .IsUnique();
            modelBuilder.Entity<Car>()
                .HasIndex(a => a.name)
                .IsUnique();
            modelBuilder.Entity<Account>()
                .HasMany(a => a.TestDrives)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId);
            modelBuilder.Entity<Car>()
                .HasMany(a => a.TestDrives)
                .WithOne(t => t.Car)
                .HasForeignKey(t => t.CarId);
            modelBuilder.Entity<TestDrive>()
                .HasOne<Account>(t => t.Account)
                .WithMany(a => a.TestDrives)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TestDrive>()
                .HasOne<Car>(t => t.Car)
                .WithMany(a => a.TestDrives)
                .HasForeignKey(t => t.CarId)
                .OnDelete(DeleteBehavior.Cascade);
        }




    }
}