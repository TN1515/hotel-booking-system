using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Services;
using HotelBooking.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

// Đổi hàm Main thành async Task để dùng await
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.UseSetting("dotnetRunMessages", "true");
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.SetMinimumLevel(LogLevel.Information);
        builder.Services.AddControllersWithViews();
        builder.Services.AddSignalR();
        builder.Services.AddDbContext<HotelBookingContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DBContextConnection")));
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<ISmsService, SmsService>();
        builder.Services.AddScoped<ILoyaltyService, LoyaltyService>();
        builder.Services.AddScoped<IPdfService, PdfService>();
        builder.Services.AddScoped<IQRPaymentService, QRPaymentService>();
        builder.Services.AddIdentity<CustomUser, CustomRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.Lockout.AllowedForNewUsers = true;
        })
        .AddEntityFrameworkStores<HotelBookingContext>()
        .AddDefaultTokenProviders();
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
            options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
            options.Cookie.Path = "/";
        });
        var app = builder.Build();
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        if (app.Environment.IsDevelopment())
        {
            var httpsPort = builder.Configuration["HTTPS_PORT"];
            if (!string.IsNullOrEmpty(httpsPort))
            {
                app.UseHttpsRedirection();
            }
        }
        else
        {
            app.UseHttpsRedirection();
        }
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapHub<ChatHub>("/chatHub");
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {
                var context = services.GetRequiredService<HotelBookingContext>();
                await context.Database.MigrateAsync();
                logger.LogInformation("Database created successfully.");
                await HotelBooking.Data.SeedData.Initialize(services);
                logger.LogInformation("Database seeded successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred creating database or seeding the DB.");
            }
        }
        app.Run();
    }
}