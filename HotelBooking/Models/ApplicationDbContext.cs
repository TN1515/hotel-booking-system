using Microsoft.EntityFrameworkCore;
using HotelBooking.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace HotelManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<CustomUser, CustomRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet properties for each tableShirtable
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<RoomAmenity> RoomAmenities { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationGuest> ReservationGuests { get; set; }
        public DbSet<PaymentBatch> PaymentBatches { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Cancellation> Cancellations { get; set; }
        public DbSet<RefundMethod> RefundMethods { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);          

            // Map CustomUser to Users table
            modelBuilder.Entity<CustomUser>()
                .ToTable("Users")
                .Property(u => u.Id)
                .HasColumnName("UserID");
            modelBuilder.Entity<CustomUser>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<CustomUser>()
                .Property(u => u.IsActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<CustomUser>()
                .Property(u => u.CreatedDate)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<CustomUser>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleID)
                .OnDelete(DeleteBehavior.Restrict);

            // Map CustomRole to UserRoles table
            modelBuilder.Entity<CustomRole>()
                .ToTable("UserRoles")
                .Property(r => r.Id)
                .HasColumnName("RoleID");
            modelBuilder.Entity<CustomRole>()
                .Property(r => r.IsActive)
                .HasDefaultValue(true);

            // Ignore IdentityUserRole<int> to prevent mapping to any table
            //modelBuilder.Ignore<IdentityUserRole<int>>();

            // Countries
            modelBuilder.Entity<Country>()
                .Property(c => c.IsActive)
                .HasDefaultValue(true);

            // States
            modelBuilder.Entity<State>()
                .Property(s => s.IsActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<State>()
                .HasOne(s => s.Country)
                .WithMany()
                .HasForeignKey(s => s.CountryID)
                .OnDelete(DeleteBehavior.Restrict);

            // Guests
            modelBuilder.Entity<Guest>()
                .Property(g => g.AgeGroup)
                .HasConversion<string>()
                .IsRequired();
            modelBuilder.Entity<Guest>()
                .Property(g => g.CreatedDate)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Guest>()
                .ToTable(t => t.HasCheckConstraint("CK_Guests_AgeGroup",
                    "AgeGroup IN ('Adult', 'Child', 'Infant')"));
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

            // RoomTypes
            modelBuilder.Entity<RoomType>()
                .Property(rt => rt.IsActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<RoomType>()
                .Property(rt => rt.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            // Rooms
            modelBuilder.Entity<Room>()
                .Property(r => r.Price)
                .HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Room>()
                .Property(r => r.Status)
                .HasConversion<string>()
                .IsRequired();
            modelBuilder.Entity<Room>()
                .Property(r => r.IsActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<Room>()
                .Property(r => r.CreatedDate)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Room>()
                .ToTable(t => t.HasCheckConstraint("CK_Rooms_Status",
                    "Status IN ('Available', 'Under Maintenance', 'Occupied')"));

            // Amenities
            modelBuilder.Entity<Amenity>()
                .Property(a => a.IsActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<Amenity>()
                .Property(a => a.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            // RoomAmenities (Composite Key)
            modelBuilder.Entity<RoomAmenity>()
                .HasKey(ra => new { ra.RoomTypeID, ra.AmenityID });

            // Reservations
            modelBuilder.Entity<Reservation>()
                .Property(r => r.BookingDate)
                .HasColumnType("date");
            modelBuilder.Entity<Reservation>()
                .Property(r => r.CheckInDate)
                .HasColumnType("date");
            modelBuilder.Entity<Reservation>()
                .Property(r => r.CheckOutDate)
                .HasColumnType("date");
            modelBuilder.Entity<Reservation>()
                .Property(r => r.Status)
                .HasConversion<string>()
                .IsRequired();
            modelBuilder.Entity<Reservation>()
                .Property(r => r.CreatedDate)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Reservation>()
                .ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Reservations_Status",
                        "Status IN ('Reserved', 'Checked-in', 'Checked-out', 'Cancelled')");
                    t.HasCheckConstraint("CK_Reservations_CheckOutDate",
                        "CheckOutDate > CheckInDate");
                });
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // PaymentBatches
            modelBuilder.Entity<PaymentBatch>()
                .Property(pb => pb.TotalAmount)
                .HasColumnType("decimal(10,2)");
            modelBuilder.Entity<PaymentBatch>()
                .HasOne(pb => pb.User)
                .WithMany()
                .HasForeignKey(pb => pb.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Payments
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(10,2)");

            // Cancellations
            modelBuilder.Entity<Cancellation>()
                .Property(c => c.CancellationFee)
                .HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Cancellation>()
                .Property(c => c.CancellationStatus)
                .HasConversion<string>()
                .IsRequired();
            modelBuilder.Entity<Cancellation>()
                .Property(c => c.CreatedDate)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Cancellation>()
                .ToTable(t => t.HasCheckConstraint("CK_Cancellations_CancellationStatus",
                    "CancellationStatus IN ('Pending', 'Approved', 'Denied')"));

            // RefundMethods
            modelBuilder.Entity<RefundMethod>()
                .Property(rm => rm.IsActive)
                .HasDefaultValue(true);

            // Refunds
            modelBuilder.Entity<Refund>()
                .Property(r => r.RefundAmount)
                .HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Refund>()
                .Property(r => r.RefundDate)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Refund>()
                .HasOne(r => r.ProcessedByUser)
                .WithMany()
                .HasForeignKey(r => r.ProcessedByUserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Feedbacks
            modelBuilder.Entity<Feedback>()
                .Property(f => f.Rating)
                .IsRequired();
            modelBuilder.Entity<Feedback>()
                .ToTable(t => t.HasCheckConstraint("CK_Feedbacks_Rating",
                    "Rating BETWEEN 1 AND 5"));
        }
    }
    }
