using Microsoft.AspNetCore.Identity;
using HotelBooking.Models;

namespace HotelBooking.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
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
                        RoleName = roleName,
                        Description = $"{roleName} role",
                        IsActive = true,
                        CreatedBy = "System",
                        CreatedDate = DateTime.Now
                    };
                    await roleManager.CreateAsync(role);
                }
            }

            // Create admin user if doesn't exist
            var adminUser = await userManager.FindByEmailAsync("admin@hotel.com");
            if (adminUser == null)
            {
                adminUser = new CustomUser
                {
                    UserName = "admin@hotel.com",
                    Email = "admin@hotel.com",
                    EmailConfirmed = true,
                    PhoneNumber = "1234567890",
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    RoleID = 1 // Admin role
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Create customer user if doesn't exist
            var customerUser = await userManager.FindByEmailAsync("customer@hotel.com");
            if (customerUser == null)
            {
                customerUser = new CustomUser
                {
                    UserName = "customer@hotel.com",
                    Email = "customer@hotel.com",
                    EmailConfirmed = true,
                    PhoneNumber = "0987654321",
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    RoleID = 2 // Customer role
                };

                var result = await userManager.CreateAsync(customerUser, "Customer123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customerUser, "Customer");
                }
            }
        }
    }
}
