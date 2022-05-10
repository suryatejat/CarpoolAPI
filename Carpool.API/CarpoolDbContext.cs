using Carpool.API.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Carpool.API
{
    public class CarpoolDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Ride> Rides { get; set; }

        public CarpoolDbContext(DbContextOptions<CarpoolDbContext> options)
            : base(options)
        {
            
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Carpool;Integrated Security=True");
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");
                entity.Property(m => m.CustomerId);
                entity.Property(m => m.EmailId);
                entity.Property(m => m.Password);
            });

            modelBuilder.Entity<Ride>(entity =>
            {
                entity.ToTable("Rides");
                entity.Property(m => m.RideId);
                entity.Property(m => m.Source);
                entity.Property(m => m.Destination);
                entity.Property(m => m.Date);
                entity.Property(m => m.Time);
                entity.Property(m => m.Price);
                entity.Property(m => m.SeatsAvailable);
            });
        }
    }
}