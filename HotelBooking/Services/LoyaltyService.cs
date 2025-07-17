using HotelBooking.Data;
using HotelBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Services
{
    public interface ILoyaltyService
    {
        Task<int> CalculatePointsForRoomBooking(decimal amount);
        Task<int> CalculatePointsForService(decimal amount);
        Task AddPointsAsync(int userId, int points, string pointType, decimal amountSpent, string description, int? reservationId = null, int? serviceUsageId = null);
        Task<CustomerLoyalty> GetCustomerLoyaltyAsync(int userId);
        Task UpdateCustomerTierAsync(int userId);
        Task<LoyaltyTier?> GetTierByPointsAsync(int points);
        Task<decimal> CalculateDiscountAsync(int userId, decimal amount);
        Task<bool> UsePointsAsync(int userId, int points, string description);
    }

    public class LoyaltyService : ILoyaltyService
    {
        private readonly HotelBookingContext _context;
        private readonly ILogger<LoyaltyService> _logger;

        // Points calculation rules
        private const int POINTS_PER_DOLLAR_ROOM = 10; // 10 points per $1 spent on rooms
        private const int POINTS_PER_DOLLAR_SERVICE = 5; // 5 points per $1 spent on services
        private const int BONUS_POINTS_NEW_CUSTOMER = 100; // Welcome bonus

        public LoyaltyService(HotelBookingContext context, ILogger<LoyaltyService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<int> CalculatePointsForRoomBooking(decimal amount)
        {
            return Task.FromResult((int)(amount * POINTS_PER_DOLLAR_ROOM));
        }

        public Task<int> CalculatePointsForService(decimal amount)
        {
            return Task.FromResult((int)(amount * POINTS_PER_DOLLAR_SERVICE));
        }

        public async Task AddPointsAsync(int userId, int points, string pointType, decimal amountSpent, string description, int? reservationId = null, int? serviceUsageId = null)
        {
            try
            {
                // Create loyalty point record
                var loyaltyPoint = new LoyaltyPoint
                {
                    UserID = userId,
                    ReservationID = reservationId,
                    ServiceUsageID = serviceUsageId,
                    PointType = pointType,
                    PointsEarned = points,
                    PointsUsed = 0,
                    AmountSpent = amountSpent,
                    Description = description,
                    EarnedDate = DateTime.Now,
                    Status = "Active",
                    ExpiryDate = DateTime.Now.AddYears(2), // Points expire in 2 years
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now
                };

                _context.LoyaltyPoints.Add(loyaltyPoint);

                // Update customer loyalty summary
                var customerLoyalty = await GetCustomerLoyaltyAsync(userId);
                customerLoyalty.TotalPointsEarned += points;
                customerLoyalty.CurrentPoints += points;
                customerLoyalty.TotalAmountSpent += amountSpent;
                customerLoyalty.LastActivityDate = DateTime.Now;
                customerLoyalty.ModifiedBy = "System";
                customerLoyalty.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                // Update customer tier based on new points
                await UpdateCustomerTierAsync(userId);

                _logger.LogInformation($"Added {points} loyalty points for user {userId}. Type: {pointType}, Amount: ${amountSpent}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to add loyalty points for user {userId}");
                throw;
            }
        }

        public async Task<CustomerLoyalty> GetCustomerLoyaltyAsync(int userId)
        {
            var customerLoyalty = await _context.CustomerLoyalties
                .Include(cl => cl.LoyaltyTier)
                .FirstOrDefaultAsync(cl => cl.UserID == userId);

            if (customerLoyalty == null)
            {
                // Create new customer loyalty record
                var bronzeTier = await _context.LoyaltyTiers
                    .FirstOrDefaultAsync(lt => lt.TierName == "Bronze");

                customerLoyalty = new CustomerLoyalty
                {
                    UserID = userId,
                    LoyaltyTierID = bronzeTier?.LoyaltyTierID ?? 1,
                    TotalPointsEarned = BONUS_POINTS_NEW_CUSTOMER,
                    TotalPointsUsed = 0,
                    CurrentPoints = BONUS_POINTS_NEW_CUSTOMER,
                    TotalAmountSpent = 0,
                    JoinDate = DateTime.Now,
                    LastActivityDate = DateTime.Now,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now
                };

                _context.CustomerLoyalties.Add(customerLoyalty);

                // Add welcome bonus points
                var welcomePoints = new LoyaltyPoint
                {
                    UserID = userId,
                    PointType = "Bonus",
                    PointsEarned = BONUS_POINTS_NEW_CUSTOMER,
                    PointsUsed = 0,
                    AmountSpent = 0,
                    Description = "Welcome bonus for new customer",
                    EarnedDate = DateTime.Now,
                    Status = "Active",
                    ExpiryDate = DateTime.Now.AddYears(2),
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now
                };

                _context.LoyaltyPoints.Add(welcomePoints);
                await _context.SaveChangesAsync();
            }

            return customerLoyalty;
        }

        public async Task UpdateCustomerTierAsync(int userId)
        {
            var customerLoyalty = await _context.CustomerLoyalties
                .FirstOrDefaultAsync(cl => cl.UserID == userId);

            if (customerLoyalty != null)
            {
                var newTier = await GetTierByPointsAsync(customerLoyalty.CurrentPoints);
                if (newTier != null && newTier.LoyaltyTierID != customerLoyalty.LoyaltyTierID)
                {
                    customerLoyalty.LoyaltyTierID = newTier.LoyaltyTierID;
                    customerLoyalty.ModifiedBy = "System";
                    customerLoyalty.ModifiedDate = DateTime.Now;

                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Customer {userId} upgraded to {newTier.TierName} tier");
                }
            }
        }

        public async Task<LoyaltyTier?> GetTierByPointsAsync(int points)
        {
            var tier = await _context.LoyaltyTiers
                .Where(lt => lt.IsActive && points >= lt.MinPoints && points <= lt.MaxPoints)
                .OrderByDescending(lt => lt.MinPoints)
                .FirstOrDefaultAsync();

            if (tier == null)
            {
                tier = await _context.LoyaltyTiers.FirstOrDefaultAsync(lt => lt.TierName == "Bronze");
            }

            return tier;
        }

        public async Task<decimal> CalculateDiscountAsync(int userId, decimal amount)
        {
            var customerLoyalty = await GetCustomerLoyaltyAsync(userId);
            if (customerLoyalty?.LoyaltyTier != null)
            {
                return amount * (customerLoyalty.LoyaltyTier.DiscountPercentage / 100);
            }
            return 0;
        }

        public async Task<bool> UsePointsAsync(int userId, int points, string description)
        {
            var customerLoyalty = await GetCustomerLoyaltyAsync(userId);
            if (customerLoyalty.CurrentPoints >= points)
            {
                // Create usage record
                var pointUsage = new LoyaltyPoint
                {
                    UserID = userId,
                    PointType = "Usage",
                    PointsEarned = 0,
                    PointsUsed = points,
                    AmountSpent = 0,
                    Description = description,
                    EarnedDate = DateTime.Now,
                    UsedDate = DateTime.Now,
                    Status = "Used",
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now
                };

                _context.LoyaltyPoints.Add(pointUsage);

                // Update customer loyalty
                customerLoyalty.TotalPointsUsed += points;
                customerLoyalty.CurrentPoints -= points;
                customerLoyalty.LastActivityDate = DateTime.Now;
                customerLoyalty.ModifiedBy = "System";
                customerLoyalty.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
