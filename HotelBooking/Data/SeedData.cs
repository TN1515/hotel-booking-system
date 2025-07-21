using Microsoft.AspNetCore.Identity;
using HotelBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            try
            {
                using var context = new HotelBookingContext(
                    serviceProvider.GetRequiredService<DbContextOptions<HotelBookingContext>>());

            var roleManager = serviceProvider.GetRequiredService<RoleManager<CustomRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<CustomUser>>();

            // Create roles if they don't exist
            string[] roleNames = { "Admin", "Customer", "Staff" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var role = new CustomRole
                    {
                        Name = roleName,
                            NormalizedName = roleName.ToUpper(),
                        RoleName = roleName,
                        Description = $"{roleName} role",
                        IsActive = true,
                        CreatedBy = "System",
                        CreatedDate = DateTime.Now
                    };
                    await roleManager.CreateAsync(role);
                }
            }

                // Create demo users with correct emails and strong passwords
                var testUsers = new[]
                {
                    new { Email = "admin@hotel.com", UserName = "admin", Role = "Admin", Password = "Admin123!" },
                    new { Email = "staff@hotel.com", UserName = "staff", Role = "Staff", Password = "Staff123!" },
                    new { Email = "customer@hotel.com", UserName = "customer", Role = "Customer", Password = "Customer123!" }
                };

                foreach (var userData in testUsers)
                {
                    var user = await userManager.FindByEmailAsync(userData.Email);
                    if (user == null)
            {
                        // Get role ID first
                        var role = await roleManager.FindByNameAsync(userData.Role);

                        user = new CustomUser
                {
                            UserName = userData.UserName,
                            Email = userData.Email,
                    EmailConfirmed = true,
                            PhoneNumber = "0123456789",
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                            CustomRoleId = role?.Id ?? 1 // Set CustomRoleId
                };

                        var result = await userManager.CreateAsync(user, userData.Password);
                if (result.Succeeded)
                {
                            await userManager.AddToRoleAsync(user, userData.Role);
                            Console.WriteLine($"Created user {userData.UserName} with role {userData.Role}");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to create user {userData.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                    else
                    {
                        // Ensure existing user has the correct role
                        var userRoles = await userManager.GetRolesAsync(user);
                        if (!userRoles.Contains(userData.Role))
                        {
                            await userManager.AddToRoleAsync(user, userData.Role);
                            Console.WriteLine($"Added role {userData.Role} to existing user {userData.UserName}");
                        }
                    }
                }

                // Seed sample data if database is empty
                await SeedSampleData(context);

            }
            catch (Exception ex)
            {
                // Log error but don't throw to avoid breaking app startup
                Console.WriteLine($"Error seeding data: {ex.Message}");
            }
        }

        private static async Task SeedSampleData(HotelBookingContext context)
        {
            // Seed Countries
            if (!context.Countries.Any())
            {
                var countries = new[]
                {
                    new Country { CountryName = "Vietnam", CountryCode = "VN", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                    new Country { CountryName = "United States", CountryCode = "US", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                    new Country { CountryName = "Japan", CountryCode = "JP", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now }
                };
                context.Countries.AddRange(countries);
                await context.SaveChangesAsync();
            }

            // Seed States
            if (!context.States.Any())
            {
                var vietnamCountry = context.Countries.First(c => c.CountryCode == "VN");
                var states = new[]
                {
                    new State { StateName = "Ho Chi Minh City", StateCode = "HCM", CountryID = vietnamCountry.CountryID, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                    new State { StateName = "Hanoi", StateCode = "HN", CountryID = vietnamCountry.CountryID, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                    new State { StateName = "Da Nang", StateCode = "DN", CountryID = vietnamCountry.CountryID, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now }
                };
                context.States.AddRange(states);
                await context.SaveChangesAsync();
            }

            // Seed Room Types
            if (!context.RoomTypes.Any())
            {
                var roomTypes = new[]
                {
                    new RoomType { TypeName = "Standard Room", Description = "Comfortable standard room with basic amenities", MaxOccupancy = 2, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                    new RoomType { TypeName = "Deluxe Room", Description = "Spacious deluxe room with premium amenities", MaxOccupancy = 3, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                    new RoomType { TypeName = "Suite", Description = "Luxury suite with separate living area", MaxOccupancy = 4, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                    new RoomType { TypeName = "Presidential Suite", Description = "Ultimate luxury with panoramic views", MaxOccupancy = 6, IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now }
                };
                context.RoomTypes.AddRange(roomTypes);
                await context.SaveChangesAsync();
                }

            // Seed Amenities
            if (!context.Amenities.Any())
            {
                var amenities = new[]
                {
                    new Amenity { AmenityName = "WiFi", Description = "High-speed wireless internet", Category = "Technology", Icon = "fas fa-wifi", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                    new Amenity { AmenityName = "Air Conditioning", Description = "Climate control system", Category = "Comfort", Icon = "fas fa-snowflake", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                    new Amenity { AmenityName = "Mini Bar", Description = "In-room refreshments", Category = "Food & Beverage", Icon = "fas fa-glass-martini", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                    new Amenity { AmenityName = "Room Service", Description = "24/7 room service", Category = "Service", Icon = "fas fa-concierge-bell", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now },
                    new Amenity { AmenityName = "Balcony", Description = "Private balcony with view", Category = "Comfort", Icon = "fas fa-building", IsActive = true, CreatedBy = "System", CreatedDate = DateTime.Now }
                };
                context.Amenities.AddRange(amenities);
                await context.SaveChangesAsync();
            }

            // Seed Rooms
            if (!context.Rooms.Any())
            {
                var roomTypes = context.RoomTypes.ToList();
                var rooms = new List<Room>();

                var basePrices = new decimal[] { 100, 150, 250, 500 }; // Corresponding to room types

                for (int i = 1; i <= 20; i++)
                {
                    var roomTypeIndex = (i - 1) % roomTypes.Count;
                    var roomType = roomTypes[roomTypeIndex];
                    var basePrice = basePrices[roomTypeIndex];

                    var room = new Room
                    {
                        RoomNumber = $"R{i:D3}",
                        RoomTypeID = roomType.RoomTypeID,
                        Price = basePrice + (i * 10),
                        Status = "Available",
                        Description = $"Beautiful {roomType.TypeName?.ToLower()} with modern amenities and comfortable furnishing.",
                        BedType = i % 2 == 0 ? "King Bed" : "Queen Bed",
                        ViewType = i % 3 == 0 ? "Ocean View" : (i % 3 == 1 ? "City View" : "Garden View"),
                    IsActive = true,
                    CreatedBy = "System",
                        CreatedDate = DateTime.Now
                };
                    rooms.Add(room);
                }

                context.Rooms.AddRange(rooms);
                await context.SaveChangesAsync();
            }
        }
    }
}
