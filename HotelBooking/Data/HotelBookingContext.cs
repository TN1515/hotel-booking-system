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
        public DbSet<RoomImage> RoomImages { get; set; }
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
        public DbSet<QRPayment> QRPayments { get; set; }
        public DbSet<Cancellation> Cancellations { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<RoomChangeHistory> RoomChangeHistories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<BookingServiceUsage> BookingServiceUsages { get; set; }
        public DbSet<LoyaltyPoint> LoyaltyPoints { get; set; }
        public DbSet<LoyaltyTier> LoyaltyTiers { get; set; }
        public DbSet<CustomerLoyalty> CustomerLoyalties { get; set; }
        public DbSet<ServiceInventory> ServiceInventories { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<ServiceAvailability> ServiceAvailabilities { get; set; }

        // Additional missing DbSets for complete hotel management
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<LoyaltyProgram> LoyaltyPrograms { get; set; }
        public DbSet<LoyaltyTransaction> LoyaltyTransactions { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<RoomChange> RoomChanges { get; set; }
        public DbSet<RoomChangeRequest> RoomChangeRequests { get; set; }
        public DbSet<GuestProfile> GuestProfiles { get; set; }
        public DbSet<ServiceHistory> ServiceHistories { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure cascade delete behavior to avoid cycles
            modelBuilder.Entity<RoomChangeRequest>()
                .HasOne(r => r.Reservation)
                .WithMany()
                .HasForeignKey(r => r.ReservationID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RoomChangeRequest>()
                .HasOne(r => r.RequestedByUser)
                .WithMany()
                .HasForeignKey(r => r.RequestedByUserID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RoomChange>()
                .HasOne(r => r.Reservation)
                .WithMany()
                .HasForeignKey(r => r.ReservationID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RoomChange>()
                .HasOne(r => r.ChangedByUser)
                .WithMany()
                .HasForeignKey(r => r.ChangedByUserID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ServiceHistory>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ServiceHistory>()
                .HasOne(s => s.ServicedByUser)
                .WithMany()
                .HasForeignKey(s => s.ServicedByUserID)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure ServiceInventory relationship properly
            modelBuilder.Entity<ServiceInventory>()
                .HasOne(si => si.Service)
                .WithMany(s => s.ServiceInventories)
                .HasForeignKey(si => si.ServiceID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RoomChange>()
                .HasOne(r => r.FromRoom)
                .WithMany()
                .HasForeignKey(r => r.FromRoomID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RoomChange>()
                .HasOne(r => r.ToRoom)
                .WithMany()
                .HasForeignKey(r => r.ToRoomID)
                .OnDelete(DeleteBehavior.NoAction);

            // Minimal configuration - just composite keys
            modelBuilder.Entity<RoomAmenity>()
                .HasKey(ra => new { ra.RoomID, ra.AmenityID });

            modelBuilder.Entity<ReservationGuest>()
                .HasKey(rg => new { rg.ReservationID, rg.GuestID });

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

            // Configure Notification relationships
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure RoomChangeHistory relationships
            modelBuilder.Entity<RoomChangeHistory>()
                .HasOne(rch => rch.Reservation)
                .WithMany()
                .HasForeignKey(rch => rch.ReservationID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RoomChangeHistory>()
                .HasOne(rch => rch.OldRoom)
                .WithMany()
                .HasForeignKey(rch => rch.OldRoomID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RoomChangeHistory>()
                .HasOne(rch => rch.NewRoom)
                .WithMany()
                .HasForeignKey(rch => rch.NewRoomID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure BookingServiceUsage relationships
            modelBuilder.Entity<BookingServiceUsage>()
                .HasOne(bsu => bsu.Reservation)
                .WithMany()
                .HasForeignKey(bsu => bsu.ReservationID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookingServiceUsage>()
                .HasOne(bsu => bsu.Service)
                .WithMany(s => s.BookingServiceUsages)
                .HasForeignKey(bsu => bsu.ServiceID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Loyalty relationships
            modelBuilder.Entity<LoyaltyPoint>()
                .HasOne(lp => lp.User)
                .WithMany()
                .HasForeignKey(lp => lp.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomerLoyalty>()
                .HasOne(cl => cl.User)
                .WithMany()
                .HasForeignKey(cl => cl.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomerLoyalty>()
                .HasOne(cl => cl.LoyaltyTier)
                .WithMany(lt => lt.CustomerLoyalties)
                .HasForeignKey(cl => cl.LoyaltyTierID)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<InventoryTransaction>()
                .HasOne(it => it.ServiceInventory)
                .WithMany(si => si.InventoryTransactions)
                .HasForeignKey(it => it.ServiceInventoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure QRPayment relationships
            modelBuilder.Entity<QRPayment>()
                .HasOne(q => q.Reservation)
                .WithMany()
                .HasForeignKey(q => q.ReservationID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QRPayment>()
                .HasOne(q => q.CreatedByUser)
                .WithMany()
                .HasForeignKey(q => q.CreatedByUserID)
                .OnDelete(DeleteBehavior.Restrict);

            // UserRole configuration is handled by Identity framework
            // No custom configuration needed

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
                new CustomRole { Id = 1, Name = "Admin", NormalizedName = "ADMIN", RoleName = "Admin", Description = "Administrator", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new CustomRole { Id = 2, Name = "Customer", NormalizedName = "CUSTOMER", RoleName = "Customer", Description = "Customer", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new CustomRole { Id = 3, Name = "Staff", NormalizedName = "STAFF", RoleName = "Staff", Description = "Hotel Staff", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now }
            );

            // Seed Room Types
            modelBuilder.Entity<RoomType>().HasData(
                new RoomType { RoomTypeID = 1, TypeName = "Standard", Description = "Standard Room", MaxOccupancy = 2, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new RoomType { RoomTypeID = 2, TypeName = "Deluxe", Description = "Deluxe Room", MaxOccupancy = 3, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new RoomType { RoomTypeID = 3, TypeName = "Suite", Description = "Suite Room", MaxOccupancy = 4, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now }
            );

            // Seed Amenities - Tiện nghi thật của khách sạn
            modelBuilder.Entity<Amenity>().HasData(
                new Amenity { AmenityID = 1, AmenityName = "WiFi miễn phí", Description = "WiFi tốc độ cao miễn phí trong toàn bộ khách sạn", Category = "Technology", Icon = "wifi", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Amenity { AmenityID = 2, AmenityName = "Điều hòa không khí", Description = "Hệ thống điều hòa hiện đại", Category = "Comfort", Icon = "ac", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Amenity { AmenityID = 3, AmenityName = "TV màn hình phẳng", Description = "TV LED 55 inch với truyền hình cáp", Category = "Entertainment", Icon = "tv", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Amenity { AmenityID = 4, AmenityName = "Mini Bar", Description = "Tủ lạnh mini với đồ uống và snack", Category = "Food & Beverage", Icon = "minibar", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Amenity { AmenityID = 5, AmenityName = "Két an toàn", Description = "Két sắt điện tử bảo mật cao", Category = "Security", Icon = "safe", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Amenity { AmenityID = 6, AmenityName = "Phòng tắm riêng", Description = "Phòng tắm đầy đủ tiện nghi với bồn tắm", Category = "Bathroom", Icon = "bathroom", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Amenity { AmenityID = 7, AmenityName = "Máy sấy tóc", Description = "Máy sấy tóc chuyên nghiệp", Category = "Bathroom", Icon = "hairdryer", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Amenity { AmenityID = 8, AmenityName = "Dép đi trong phòng", Description = "Dép cotton cao cấp", Category = "Comfort", Icon = "slippers", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Amenity { AmenityID = 9, AmenityName = "Áo choàng tắm", Description = "Áo choàng cotton mềm mại", Category = "Comfort", Icon = "bathrobe", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Amenity { AmenityID = 10, AmenityName = "Bàn làm việc", Description = "Bàn làm việc rộng rãi với ghế ergonomic", Category = "Business", Icon = "desk", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Amenity { AmenityID = 11, AmenityName = "Ban công riêng", Description = "Ban công với view đẹp", Category = "View", Icon = "balcony", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Amenity { AmenityID = 12, AmenityName = "Dịch vụ phòng 24/7", Description = "Phục vụ đồ ăn uống 24 giờ", Category = "Service", Icon = "roomservice", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Amenity { AmenityID = 13, AmenityName = "Máy pha cà phê", Description = "Máy pha cà phê Nespresso", Category = "Food & Beverage", Icon = "coffee", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Amenity { AmenityID = 14, AmenityName = "Tủ quần áo", Description = "Tủ quần áo rộng rãi với móc treo", Category = "Storage", Icon = "wardrobe", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Amenity { AmenityID = 15, AmenityName = "Điện thoại", Description = "Điện thoại nội bộ và quốc tế", Category = "Communication", Icon = "phone", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now }
            );

            // Seed Rooms - Dữ liệu thật cho khách sạn
            modelBuilder.Entity<Room>().HasData(
                // Tầng 1 - Standard Rooms
                new Room { RoomID = 1, RoomNumber = "101", RoomTypeID = 1, Price = 1200000.00m, BedType = "Single", ViewType = "Garden", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now, Description = "Phòng đơn tiêu chuẩn với view vườn" },
                new Room { RoomID = 2, RoomNumber = "102", RoomTypeID = 1, Price = 1300000.00m, BedType = "Double", ViewType = "City", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now, Description = "Phòng đôi tiêu chuẩn với view thành phố" },
                new Room { RoomID = 3, RoomNumber = "103", RoomTypeID = 1, Price = 1200000.00m, BedType = "Single", ViewType = "Garden", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now, Description = "Phòng đơn tiêu chuẩn với view vườn" },
                new Room { RoomID = 4, RoomNumber = "104", RoomTypeID = 1, Price = 1300000.00m, BedType = "Double", ViewType = "City", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now, Description = "Phòng đôi tiêu chuẩn với view thành phố" },
                new Room { RoomID = 5, RoomNumber = "105", RoomTypeID = 1, Price = 1400000.00m, BedType = "Twin", ViewType = "Pool", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now, Description = "Phòng twin với view hồ bơi" },

                // Tầng 2 - Deluxe Rooms
                new Room { RoomID = 6, RoomNumber = "201", RoomTypeID = 2, Price = 1800000.00m, BedType = "Queen", ViewType = "Ocean", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now, Description = "Phòng deluxe với giường queen và view biển" },
                new Room { RoomID = 7, RoomNumber = "202", RoomTypeID = 2, Price = 1900000.00m, BedType = "King", ViewType = "Ocean", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now, Description = "Phòng deluxe với giường king và view biển" },
                new Room { RoomID = 8, RoomNumber = "203", RoomTypeID = 2, Price = 1700000.00m, BedType = "Queen", ViewType = "City", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now, Description = "Phòng deluxe với view thành phố" },
                new Room { RoomID = 9, RoomNumber = "204", RoomTypeID = 2, Price = 1800000.00m, BedType = "King", ViewType = "Pool", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now, Description = "Phòng deluxe với view hồ bơi" },
                new Room { RoomID = 10, RoomNumber = "205", RoomTypeID = 2, Price = 1900000.00m, BedType = "King", ViewType = "Ocean", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now, Description = "Phòng deluxe cao cấp với view biển" },

                // Tầng 3 - Suite Rooms
                new Room { RoomID = 11, RoomNumber = "301", RoomTypeID = 3, Price = 3500000.00m, BedType = "King", ViewType = "Ocean", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now, Description = "Suite cao cấp với view biển toàn cảnh" },
                new Room { RoomID = 12, RoomNumber = "302", RoomTypeID = 3, Price = 3200000.00m, BedType = "King", ViewType = "City", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now, Description = "Suite với view thành phố" },
                new Room { RoomID = 13, RoomNumber = "303", RoomTypeID = 3, Price = 3800000.00m, BedType = "King", ViewType = "Ocean", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now, Description = "Presidential Suite với view biển" },
                new Room { RoomID = 14, RoomNumber = "304", RoomTypeID = 3, Price = 3300000.00m, BedType = "King", ViewType = "Pool", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now, Description = "Suite với view hồ bơi và vườn" },
                new Room { RoomID = 15, RoomNumber = "305", RoomTypeID = 3, Price = 4000000.00m, BedType = "King", ViewType = "Ocean", Status = "Available", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now, Description = "Penthouse Suite với view biển 360 độ" }
            );

            // Seed Services - Dịch vụ thật của khách sạn
            modelBuilder.Entity<Service>().HasData(
                // Dịch vụ ăn uống
                new Service { ServiceID = 1, ServiceName = "Buffet sáng", Description = "Buffet sáng phong phú với món Á và Âu", Category = "Food", UnitPrice = 350000.00m, Unit = "per person", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Service { ServiceID = 2, ServiceName = "Room Service - Bữa trưa", Description = "Thực đơn bữa trưa giao tận phòng", Category = "Food", UnitPrice = 450000.00m, Unit = "per meal", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Service { ServiceID = 3, ServiceName = "Room Service - Bữa tối", Description = "Thực đơn bữa tối cao cấp giao tận phòng", Category = "Food", UnitPrice = 650000.00m, Unit = "per meal", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Service { ServiceID = 4, ServiceName = "Nước uống cao cấp", Description = "Nước khoáng nhập khẩu", Category = "Beverage", UnitPrice = 50000.00m, Unit = "per bottle", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Service { ServiceID = 5, ServiceName = "Đồ uống có gas", Description = "Các loại nước ngọt cao cấp", Category = "Beverage", UnitPrice = 80000.00m, Unit = "per can", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },

                // Dịch vụ chăm sóc
                new Service { ServiceID = 6, ServiceName = "Giặt ủi cao cấp", Description = "Dịch vụ giặt ủi chuyên nghiệp", Category = "Laundry", UnitPrice = 150000.00m, Unit = "per item", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Service { ServiceID = 7, ServiceName = "Massage thư giãn", Description = "Massage toàn thân 90 phút", Category = "Spa", UnitPrice = 1200000.00m, Unit = "per session", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Service { ServiceID = 8, ServiceName = "Đưa đón sân bay", Description = "Xe riêng đưa đón sân bay", Category = "Transportation", UnitPrice = 800000.00m, Unit = "per trip", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },

                // Dịch vụ bổ sung
                new Service { ServiceID = 9, ServiceName = "Spa chăm sóc da mặt", Description = "Liệu trình chăm sóc da mặt 60 phút", Category = "Spa", UnitPrice = 800000.00m, Unit = "per session", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Service { ServiceID = 10, ServiceName = "Thuê xe máy", Description = "Thuê xe máy theo ngày", Category = "Transportation", UnitPrice = 200000.00m, Unit = "per day", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Service { ServiceID = 11, ServiceName = "Tour thành phố", Description = "Tour tham quan thành phố nửa ngày", Category = "Tour", UnitPrice = 600000.00m, Unit = "per person", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Service { ServiceID = 12, ServiceName = "Dịch vụ giữ trẻ", Description = "Dịch vụ trông trẻ chuyên nghiệp", Category = "Childcare", UnitPrice = 300000.00m, Unit = "per hour", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Service { ServiceID = 13, ServiceName = "Fitness & Gym", Description = "Sử dụng phòng gym và hồ bơi", Category = "Fitness", UnitPrice = 200000.00m, Unit = "per day", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Service { ServiceID = 14, ServiceName = "Dịch vụ hội nghị", Description = "Thuê phòng hội nghị với thiết bị", Category = "Business", UnitPrice = 2000000.00m, Unit = "per day", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Service { ServiceID = 15, ServiceName = "Dịch vụ cưới hỏi", Description = "Tổ chức tiệc cưới trọn gói", Category = "Event", UnitPrice = 50000000.00m, Unit = "per event", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now }
            );

            // Seed Loyalty Tiers
            modelBuilder.Entity<LoyaltyTier>().HasData(
                new LoyaltyTier { LoyaltyTierID = 1, TierName = "Bronze", MinPoints = 0, MaxPoints = 999, DiscountPercentage = 0, PointMultiplier = 1.0m, Benefits = "Welcome bonus, Basic support", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new LoyaltyTier { LoyaltyTierID = 2, TierName = "Silver", MinPoints = 1000, MaxPoints = 4999, DiscountPercentage = 5, PointMultiplier = 1.2m, Benefits = "5% discount, Priority support, Late checkout", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new LoyaltyTier { LoyaltyTierID = 3, TierName = "Gold", MinPoints = 5000, MaxPoints = 14999, DiscountPercentage = 10, PointMultiplier = 1.5m, Benefits = "10% discount, Room upgrade, Free breakfast", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new LoyaltyTier { LoyaltyTierID = 4, TierName = "Platinum", MinPoints = 15000, MaxPoints = 999999, DiscountPercentage = 15, PointMultiplier = 2.0m, Benefits = "15% discount, Suite upgrade, Concierge service, Airport transfer", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now }
            );

            // Seed Service Inventory
            modelBuilder.Entity<ServiceInventory>().HasData(
                new ServiceInventory { ServiceInventoryID = 1, ServiceID = 4, ItemName = "Premium Water Bottles", CurrentStock = 100, MinimumStock = 20, MaximumStock = 200, ReorderLevel = 30, CostPerUnit = 2.50m, Unit = "bottles", Supplier = "AquaPure Co.", Status = "In Stock", CreatedBy = "System", CreatedDate = DateTime.Now },
                new ServiceInventory { ServiceInventoryID = 2, ServiceID = 5, ItemName = "Assorted Soft Drinks", CurrentStock = 80, MinimumStock = 15, MaximumStock = 150, ReorderLevel = 25, CostPerUnit = 3.00m, Unit = "cans", Supplier = "Beverage Plus", Status = "In Stock", CreatedBy = "System", CreatedDate = DateTime.Now },
                new ServiceInventory { ServiceInventoryID = 3, ServiceID = 6, ItemName = "Laundry Supplies", CurrentStock = 50, MinimumStock = 10, MaximumStock = 100, ReorderLevel = 15, CostPerUnit = 5.00m, Unit = "sets", Supplier = "CleanCorp", Status = "In Stock", CreatedBy = "System", CreatedDate = DateTime.Now }
            );

            // Seed Hotel Information
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    HotelID = 1,
                    HotelName = "Grand Palace Hotel & Resort",
                    Address = "123 Đường Nguyễn Huệ, Quận 1",
                    City = "Hồ Chí Minh",
                    StateID = 1,
                    CountryID = 1,
                    PostalCode = "70000",
                    Phone = "+84 28 3829 2185",
                    Email = "info@grandpalacehotel.vn",
                    Website = "https://grandpalacehotel.vn",
                    Description = "Khách sạn 5 sao sang trọng tại trung tâm thành phố Hồ Chí Minh với đầy đủ tiện nghi hiện đại và dịch vụ đẳng cấp quốc tế.",
                    StarRating = 5,
                    Amenities = "WiFi miễn phí, Hồ bơi, Spa, Gym, Nhà hàng, Bar, Dịch vụ phòng 24/7, Đưa đón sân bay",
                    CheckInTime = "14:00",
                    CheckOutTime = "12:00",
                    Policies = "Không hút thuốc, Không thú cưng, Hủy miễn phí trước 24h",
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now
                }
            );

            // Seed Service Categories
            modelBuilder.Entity<ServiceCategory>().HasData(
                new ServiceCategory { ServiceCategoryID = 1, CategoryName = "Ăn uống", Description = "Dịch vụ ăn uống và đồ uống", Icon = "fas fa-utensils", DisplayOrder = 1, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new ServiceCategory { ServiceCategoryID = 2, CategoryName = "Spa & Wellness", Description = "Dịch vụ chăm sóc sức khỏe và làm đẹp", Icon = "fas fa-spa", DisplayOrder = 2, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new ServiceCategory { ServiceCategoryID = 3, CategoryName = "Vận chuyển", Description = "Dịch vụ đưa đón và thuê xe", Icon = "fas fa-car", DisplayOrder = 3, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new ServiceCategory { ServiceCategoryID = 4, CategoryName = "Giải trí", Description = "Dịch vụ giải trí và thể thao", Icon = "fas fa-gamepad", DisplayOrder = 4, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new ServiceCategory { ServiceCategoryID = 5, CategoryName = "Dịch vụ khác", Description = "Các dịch vụ bổ sung khác", Icon = "fas fa-concierge-bell", DisplayOrder = 5, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now }
            );

            // Seed Payment Methods
            modelBuilder.Entity<PaymentMethod>().HasData(
                new PaymentMethod { PaymentMethodID = 1, MethodName = "Tiền mặt", Description = "Thanh toán bằng tiền mặt tại quầy lễ tân", MethodType = "Cash", Provider = "Hotel", Icon = "fas fa-money-bill", RequiresVerification = false, IsOnline = false, IsActive = true, DisplayOrder = 1, Instructions = "Vui lòng thanh toán tại quầy lễ tân khi check-in hoặc check-out", CreatedBy = "System", CreatedDate = DateTime.Now },
                new PaymentMethod { PaymentMethodID = 2, MethodName = "Thẻ tín dụng/ghi nợ", Description = "Thanh toán bằng thẻ Visa, MasterCard, JCB", MethodType = "Card", Provider = "Bank", Icon = "fas fa-credit-card", RequiresVerification = true, IsOnline = true, IsActive = true, DisplayOrder = 2, Instructions = "Chấp nhận các loại thẻ quốc tế Visa, MasterCard, JCB", CreatedBy = "System", CreatedDate = DateTime.Now },
                new PaymentMethod { PaymentMethodID = 3, MethodName = "Chuyển khoản VietinBank", Description = "Chuyển khoản qua QR Code VietinBank", MethodType = "QR Code", Provider = "VietinBank", Icon = "fas fa-qrcode", RequiresVerification = true, IsOnline = true, IsActive = true, DisplayOrder = 3, Instructions = "Quét mã QR để chuyển khoản, sau đó xác nhận thanh toán", CreatedBy = "System", CreatedDate = DateTime.Now },
                new PaymentMethod { PaymentMethodID = 4, MethodName = "Ví điện tử MoMo", Description = "Thanh toán qua ví MoMo", MethodType = "E-Wallet", Provider = "MoMo", Icon = "fas fa-mobile-alt", RequiresVerification = true, IsOnline = true, IsActive = true, DisplayOrder = 4, Instructions = "Sử dụng app MoMo để quét mã QR thanh toán", CreatedBy = "System", CreatedDate = DateTime.Now },
                new PaymentMethod { PaymentMethodID = 5, MethodName = "ZaloPay", Description = "Thanh toán qua ví ZaloPay", MethodType = "E-Wallet", Provider = "ZaloPay", Icon = "fas fa-wallet", RequiresVerification = true, IsOnline = true, IsActive = true, DisplayOrder = 5, Instructions = "Sử dụng app ZaloPay để thanh toán", CreatedBy = "System", CreatedDate = DateTime.Now }
            );

            // Seed Loyalty Programs
            modelBuilder.Entity<LoyaltyProgram>().HasData(
                new LoyaltyProgram { LoyaltyProgramID = 1, ProgramName = "Grand Rewards Bronze", Description = "Chương trình khách hàng thân thiết hạng Đồng", PointsPerVND = 1, MinimumSpend = 0, TierLevel = "Bronze", RequiredPoints = 0, DiscountPercentage = 0, Benefits = "Tích điểm cơ bản, Hỗ trợ khách hàng", StartDate = DateTime.Now, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new LoyaltyProgram { LoyaltyProgramID = 2, ProgramName = "Grand Rewards Silver", Description = "Chương trình khách hàng thân thiết hạng Bạc", PointsPerVND = 1, MinimumSpend = 5000000, TierLevel = "Silver", RequiredPoints = 1000, DiscountPercentage = 5, Benefits = "Giảm giá 5%, Hỗ trợ ưu tiên, Check-out muộn", StartDate = DateTime.Now, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new LoyaltyProgram { LoyaltyProgramID = 3, ProgramName = "Grand Rewards Gold", Description = "Chương trình khách hàng thân thiết hạng Vàng", PointsPerVND = 2, MinimumSpend = 15000000, TierLevel = "Gold", RequiredPoints = 5000, DiscountPercentage = 10, Benefits = "Giảm giá 10%, Nâng cấp phòng, Buffet sáng miễn phí", StartDate = DateTime.Now, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new LoyaltyProgram { LoyaltyProgramID = 4, ProgramName = "Grand Rewards Platinum", Description = "Chương trình khách hàng thân thiết hạng Bạch Kim", PointsPerVND = 3, MinimumSpend = 50000000, TierLevel = "Platinum", RequiredPoints = 15000, DiscountPercentage = 15, Benefits = "Giảm giá 15%, Nâng cấp suite, Dịch vụ concierge, Đưa đón sân bay", StartDate = DateTime.Now, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now }
            );

            // Seed System Settings
            modelBuilder.Entity<Setting>().HasData(
                new Setting { SettingID = 1, SettingKey = "Hotel.Name", SettingValue = "Grand Palace Hotel & Resort", Description = "Tên khách sạn", Category = "Hotel", DataType = "String", IsEncrypted = false, IsReadOnly = false, RequiresRestart = false, DefaultValue = "Grand Palace Hotel", DisplayOrder = 1, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Setting { SettingID = 2, SettingKey = "Hotel.CheckInTime", SettingValue = "14:00", Description = "Giờ check-in tiêu chuẩn", Category = "Hotel", DataType = "String", IsEncrypted = false, IsReadOnly = false, RequiresRestart = false, DefaultValue = "14:00", DisplayOrder = 2, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Setting { SettingID = 3, SettingKey = "Hotel.CheckOutTime", SettingValue = "12:00", Description = "Giờ check-out tiêu chuẩn", Category = "Hotel", DataType = "String", IsEncrypted = false, IsReadOnly = false, RequiresRestart = false, DefaultValue = "12:00", DisplayOrder = 3, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Setting { SettingID = 4, SettingKey = "Payment.VietinBank.AccountNumber", SettingValue = "1038766815877", Description = "Số tài khoản VietinBank", Category = "Payment", DataType = "String", IsEncrypted = true, IsReadOnly = false, RequiresRestart = false, DefaultValue = "", DisplayOrder = 4, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Setting { SettingID = 5, SettingKey = "Payment.VietinBank.AccountName", SettingValue = "LUU VAN HIEN", Description = "Tên tài khoản VietinBank", Category = "Payment", DataType = "String", IsEncrypted = false, IsReadOnly = false, RequiresRestart = false, DefaultValue = "", DisplayOrder = 5, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Setting { SettingID = 6, SettingKey = "Notification.AutoSend", SettingValue = "true", Description = "Tự động gửi thông báo", Category = "Notification", DataType = "Boolean", IsEncrypted = false, IsReadOnly = false, RequiresRestart = false, DefaultValue = "true", DisplayOrder = 6, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                new Setting { SettingID = 7, SettingKey = "System.MaintenanceMode", SettingValue = "false", Description = "Chế độ bảo trì hệ thống", Category = "System", DataType = "Boolean", IsEncrypted = false, IsReadOnly = false, RequiresRestart = true, DefaultValue = "false", DisplayOrder = 7, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now }
            );
        }
    }
}
