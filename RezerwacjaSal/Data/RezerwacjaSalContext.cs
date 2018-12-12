using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// potrzebne EF oraz nazwa.Models
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace RezerwacjaSal.Data
{
    public class RezerwacjaSalContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public RezerwacjaSalContext(DbContextOptions<RezerwacjaSalContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<ApplicationUser> AppUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   // fluent API

            modelBuilder.Entity<Message>().ToTable("Message");
            modelBuilder.Entity<Department>()
                .ToTable("Department")
                .HasMany(r => r.AppUsers)
                .WithOne(r => r.Department)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Building>()
                .ToTable("Building")
                .HasOne(r => r.Department)
                .WithMany(r => r.Buildings)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Room>()
                .ToTable("Room")
                .HasOne(r => r.Building)
                .WithMany(r => r.Rooms)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Reservation>()
                .ToTable("Reservation")
                .HasOne(r => r.Room)                
                .WithMany(b => b.Reservations)      
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("ApplicationUser")
                .HasMany(b => b.Reservations)
                .WithOne(r => r.ApplicationUser)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("ApplicationUser")
                .HasMany(b => b.Messages)
                .WithOne(r => r.ApplicationUser)
                .OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(modelBuilder);
        }
    }
}
