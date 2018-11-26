using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// potrzebne EF oraz nazwa.Models
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RezerwacjaSal.Data
{
    // RezerwacjaSalContext precyzuje które encje są włączone do modelu danych 
    public class RezerwacjaSalContext : IdentityDbContext
    {
        public RezerwacjaSalContext(DbContextOptions<RezerwacjaSalContext> options) : base(options)
        {
        }
        // tworzy tabele
        public DbSet<Employment> Employments { get; set; }
        public DbSet<Pearson> People { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<ApplicationUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   // fluent API
            modelBuilder.Entity<Employment>().ToTable("Employment"); // nazwy tabeli w bazie danych, ale nie jest to konieczne bo 
            modelBuilder.Entity<Pearson>().ToTable("Pearson");       // domyślnie EF nazywa tabele tak samo jak property DbSet
            modelBuilder.Entity<Message>().ToTable("Message");
            modelBuilder.Entity<Department>().ToTable("Department")
                .ToTable("Department")
                .HasMany(r => r.Employments)
                .WithOne(r => r.Department)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Building>().ToTable("Building")
                .ToTable("Building")
                .HasOne(r => r.Department)
                .WithMany(r => r.Buildings)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Room>().ToTable("Room")
                .ToTable("Room")
                .HasOne(r => r.Building)
                .WithMany(r => r.Rooms)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Reservation>()
                .ToTable("Reservation")
                .HasOne(r => r.Room)                // ponieważ występuje kaskada buildingID -> roomID to trzeba takie coś aby przy usuwaniu się nie wykrzaczało 
                .WithMany(b => b.Reservations)      // jeden pokój może posiadać wiele rezerwacji. Najwyraźniej trzeba to tu określić 
                .OnDelete(DeleteBehavior.Cascade); // modyfikacja domyślnego zachowania przy usuwaniu 
            base.OnModelCreating(modelBuilder);
        }
    }
}
