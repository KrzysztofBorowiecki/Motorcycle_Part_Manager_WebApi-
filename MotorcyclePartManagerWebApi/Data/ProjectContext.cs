using Microsoft.EntityFrameworkCore;
using MotorcyclePartManagerWebApi.Entities;
using System;

namespace MotorcyclePartManagerWebApi.Data
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Part>()
                .HasOne<Motorcycle>(p => p.Motorcycle)
                .WithMany(m => m.Parts)
                .HasForeignKey(p => p.MotorcycleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Motorcycle>()
                .HasOne<User>(m => m.User)
                .WithMany(u => u.Motorcycles)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired();
        }

        internal void AddFailure(string v1, string v2)
        {
            throw new NotImplementedException();
        }

        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

    }
}
