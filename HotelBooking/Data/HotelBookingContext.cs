using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HotelBooking.Models;

namespace HotelBooking.Data
{
    public class HotelBookingContext : IdentityDbContext<CustomUser, CustomRole, int>
    {
        public HotelBookingContext(DbContextOptions<HotelBookingContext> options) : base(options)
        {
        }

        // Identity tables are inherited from IdentityDbContext
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<RoomAmenity> RoomAmenities { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationGuest> ReservationGuests { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentBatch> PaymentBatches { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<RefundMethod> RefundMethods { get; set; }
        public DbSet<Cancellation> Cancellations { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships with NO ACTION to avoid cascade conflicts
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            // Fix CustomUser-Role relationship
            modelBuilder.Entity<CustomUser>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RoomAmenity>()
                .HasKey(ra => new { ra.RoomID, ra.AmenityID });

            modelBuilder.Entity<RoomAmenity>()
                .HasOne(ra => ra.Room)
                .WithMany(r => r.RoomAmenities)
                .HasForeignKey(ra => ra.RoomID);

            modelBuilder.Entity<RoomAmenity>()
                .HasOne(ra => ra.Amenity)
                .WithMany(a => a.RoomAmenities)
                .HasForeignKey(ra => ra.AmenityID);

            modelBuilder.Entity<ReservationGuest>()
                .HasKey(rg => new { rg.ReservationID, rg.GuestID });

            modelBuilder.Entity<ReservationGuest>()
                .HasOne(rg => rg.Reservation)
                .WithMany(r => r.ReservationGuests)
                .HasForeignKey(rg => rg.ReservationID);

            modelBuilder.Entity<ReservationGuest>()
                .HasOne(rg => rg.Guest)
                .WithMany(g => g.ReservationGuests)
                .HasForeignKey(rg => rg.GuestID);

            // Configure Room relationships
            modelBuilder.Entity<Room>()
                .HasOne(r => r.RoomType)
                .WithMany(rt => rt.Rooms)
                .HasForeignKey(r => r.RoomTypeID);

            // Configure Reservation relationships
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserID);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany(rm => rm.Reservations)
                .HasForeignKey(r => r.RoomID);

            // Configure Payment relationships
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Reservation)
                .WithMany(r => r.Payments)
                .HasForeignKey(p => p.ReservationID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentBatch>()
                .HasOne(pb => pb.User)
                .WithMany()
                .HasForeignKey(pb => pb.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure State relationships
            modelBuilder.Entity<State>()
                .HasOne(s => s.Country)
                .WithMany(c => c.States)
                .HasForeignKey(s => s.CountryID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Guest relationships to avoid cascade conflicts
            modelBuilder.Entity<Guest>()
                .HasOne(g => g.User)
                .WithMany()
                .HasForeignKey(g => g.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Guest>()
                .HasOne(g => g.Country)
                .WithMany()
                .HasForeignKey(g => g.CountryID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Guest>()
                .HasOne(g => g.State)
                .WithMany()
                .HasForeignKey(g => g.StateID)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Countries
            modelBuilder.Entity<Country>().HasData(
                new Country { CountryID = 1, CountryName = "Vietnam", CountryCode = "VN", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Country { CountryID = 2, CountryName = "United States", CountryCode = "US", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now }
            );

            // Seed States
            modelBuilder.Entity<State>().HasData(
                new State { StateID = 1, StateName = "Ho Chi Minh City", StateCode = "HCM", CountryID = 1, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new State { StateID = 2, StateName = "Hanoi", StateCode = "HN", CountryID = 1, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now }
            );

            // Seed Roles
            modelBuilder.Entity<CustomRole>().HasData(
                new CustomRole { Id = 1, Name = "Admin", RoleID = 1, RoleName = "Admin", Description = "Administrator", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new CustomRole { Id = 2, Name = "Customer", RoleID = 2, RoleName = "Customer", Description = "Customer", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new CustomRole { Id = 3, Name = "Staff", RoleID = 3, RoleName = "Staff", Description = "Hotel Staff", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now }
            );

            // Seed Room Types
            modelBuilder.Entity<RoomType>().HasData(
                new RoomType { RoomTypeID = 1, TypeName = "Standard", Description = "Standard Room", MaxOccupancy = 2, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new RoomType { RoomTypeID = 2, TypeName = "Deluxe", Description = "Deluxe Room", MaxOccupancy = 3, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new RoomType { RoomTypeID = 3, TypeName = "Suite", Description = "Suite Room", MaxOccupancy = 4, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now }
            );

            // Seed Amenities
            modelBuilder.Entity<Amenity>().HasData(
                new Amenity { AmenityID = 1, AmenityName = "WiFi", Description = "Free WiFi", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Amenity { AmenityID = 2, AmenityName = "Air Conditioning", Description = "Air Conditioning", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Amenity { AmenityID = 3, AmenityName = "TV", Description = "Television", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Amenity { AmenityID = 4, AmenityName = "Mini Bar", Description = "Mini Bar", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now }
            );

            // Seed Rooms
            modelBuilder.Entity<Room>().HasData(
                new Room { RoomID = 1, RoomNumber = "101", RoomTypeID = 1, Price = 100.00m, BedType = "Single", ViewType = "City", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Room { RoomID = 2, RoomNumber = "102", RoomTypeID = 1, Price = 100.00m, BedType = "Double", ViewType = "City", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Room { RoomID = 3, RoomNumber = "201", RoomTypeID = 2, Price = 150.00m, BedType = "Queen", ViewType = "Ocean", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Room { RoomID = 4, RoomNumber = "301", RoomTypeID = 3, Price = 250.00m, BedType = "King", ViewType = "Ocean", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now }
            );
        }
    }
}
