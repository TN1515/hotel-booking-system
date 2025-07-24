using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelBooking.Services;
using System.Security.Claims;
using HotelBooking.Models.ViewModels;

namespace HotelBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIChatController : ControllerBase
    {
        private readonly IAIChatbotService _aiChatbotService;
        private readonly ILogger<AIChatController> _logger;

        public AIChatController(IAIChatbotService aiChatbotService, ILogger<AIChatController> logger)
        {
            _aiChatbotService = aiChatbotService;
            _logger = logger;
        }

        [HttpPost("message")]
        public async Task<IActionResult> ProcessMessage([FromBody] AIChatRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Message))
                {
                    return BadRequest(new { error = "Message cannot be empty" });
                }

                var userId = GetCurrentUserId();
                var response = await _aiChatbotService.ProcessMessageAsync(request.Message, userId);

                _logger.LogInformation($"AI Chat - User: {userId}, Message: {request.Message}, Intent: {response.Intent}");

                return Ok(new
                {
                    success = true,
                    data = response,
                    timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing AI chat message");
                return StatusCode(500, new {
                    error = "Internal server error",
                    message = "Xin lỗi, tôi đang gặp sự cố. Vui lòng thử lại sau."
                });
            }
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetSuggestions([FromQuery] string? partialMessage = "")
        {
            try
            {
                var suggestions = await _aiChatbotService.GetSuggestionsAsync(partialMessage ?? "");

                return Ok(new
                {
                    success = true,
                    suggestions = suggestions,
                    timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting AI chat suggestions");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpGet("welcome")]
        public async Task<IActionResult> GetWelcomeMessage()
        {
            try
            {
                var userId = GetCurrentUserId();
                var response = await _aiChatbotService.GetWelcomeMessageAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = response,
                    timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting AI welcome message");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpPost("feedback")]
        public async Task<IActionResult> SubmitFeedback([FromBody] AIChatFeedbackRequest request)
        {
            try
            {
                // Log feedback for AI improvement
                _logger.LogInformation($"AI Chat Feedback - Rating: {request.Rating}, Message: {request.Message}, Helpful: {request.IsHelpful}");

                // In a real implementation, you would save this to database for AI training

                return Ok(new
                {
                    success = true,
                    message = "Cảm ơn bạn đã đánh giá! Phản hồi của bạn giúp tôi cải thiện tốt hơn.",
                    timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting AI chat feedback");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                status = "healthy",
                service = "AI Chatbot",
                timestamp = DateTime.Now,
                version = "1.0.0"
            });
        }

        [HttpGet("analytics")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAnalytics([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            try
            {
                // In a real implementation, you would query analytics from database
                var analytics = new
                {
                    totalConversations = 1250,
                    totalMessages = 8500,
                    averageResponseTime = "0.8s",
                    satisfactionRate = 94.5,
                    topIntents = new[]
                    {
                        new { intent = "booking", count = 3200, percentage = 37.6 },
                        new { intent = "pricing", count = 2100, percentage = 24.7 },
                        new { intent = "amenities", count = 1800, percentage = 21.2 },
                        new { intent = "contact", count = 900, percentage = 10.6 },
                        new { intent = "other", count = 500, percentage = 5.9 }
                    },
                    dailyStats = GenerateDailyStats(fromDate ?? DateTime.Now.AddDays(-30), toDate ?? DateTime.Now)
                };

                return Ok(new
                {
                    success = true,
                    data = analytics,
                    timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting AI chat analytics");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        private int? GetCurrentUserId()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    return userId;
                }
            }
            return null;
        }

        private object[] GenerateDailyStats(DateTime fromDate, DateTime toDate)
        {
            var stats = new List<object>();
            var random = new Random();

            for (var date = fromDate.Date; date <= toDate.Date; date = date.AddDays(1))
            {
                stats.Add(new
                {
                    date = date.ToString("yyyy-MM-dd"),
                    conversations = random.Next(20, 80),
                    messages = random.Next(100, 400),
                    satisfaction = Math.Round(random.NextDouble() * 20 + 80, 1) // 80-100%
                });
            }

            return stats.ToArray();
        }

        // AI Processing Methods
        private async Task<AIServiceResponse> ProcessMessage(string message, AIChatContext? context)
        {
            var lowerMessage = message.ToLower();

            // Extract entities from message
            var entities = ExtractEntities(message);

            // Detect intent with entity context
            var intent = DetectIntent(lowerMessage, entities);

            return intent switch
            {
                "greeting" => HandleGreetingIntent(),
                "booking" => await HandleBookingIntent(message, entities),
                "booking_with_date" => await HandleBookingWithDateIntent(message, entities),
                "date_inquiry" => HandleDateInquiryIntent(message),
                "clarification" => HandleClarificationIntent(message),
                "pricing" => await HandlePricingIntent(message, entities),
                "services" => await HandleServicesIntent(message, entities),
                "location" => HandleLocationIntent(),
                "room_search" => await HandleRoomSearchIntent(entities),
                "availability" => await HandleAvailabilityIntent(entities),
                "amenities" => await HandleAmenitiesIntent(message, entities),
                "contact" => HandleContactIntent(),
                "help" => HandleHelpIntent(),
                _ => HandleDefaultIntent(message)
            };
        }

        private Dictionary<string, object> ExtractEntities(string message)
        {
            var entities = new Dictionary<string, object>();
            var lowerMessage = message.ToLower();

            // Extract numbers (guests, price, room number)
            var numberMatches = System.Text.RegularExpressions.Regex.Matches(message, @"\d+");
            var numbers = numberMatches.Cast<System.Text.RegularExpressions.Match>()
                .Select(m => int.Parse(m.Value)).ToList();
            if (numbers.Any()) entities["numbers"] = numbers;

            // Extract price ranges
            if (lowerMessage.Contains("dưới") || lowerMessage.Contains("under") || lowerMessage.Contains("below"))
            {
                var priceLimit = numbers.FirstOrDefault();
                if (priceLimit > 0) entities["max_price"] = priceLimit;
            }
            else if (lowerMessage.Contains("trên") || lowerMessage.Contains("over") || lowerMessage.Contains("above"))
            {
                var priceLimit = numbers.FirstOrDefault();
                if (priceLimit > 0) entities["min_price"] = priceLimit;
            }
            else if (numbers.Any() && (lowerMessage.Contains("giá") || lowerMessage.Contains("price")))
            {
                entities["target_price"] = numbers.First();
            }

            // Extract guest count
            if (lowerMessage.Contains("người") || lowerMessage.Contains("guest") || lowerMessage.Contains("pax"))
            {
                var guestCount = numbers.FirstOrDefault();
                if (guestCount > 0) entities["guests"] = guestCount;
            }

            // Extract room types
            var roomTypes = new[] { "standard", "deluxe", "suite", "vip", "presidential", "single", "double", "twin" };
            var foundRoomTypes = roomTypes.Where(rt => lowerMessage.Contains(rt)).ToList();
            if (foundRoomTypes.Any()) entities["room_types"] = foundRoomTypes;

            // Extract dates
            var datePatterns = new[] {
                @"\d{1,2}\/\d{1,2}\/\d{4}",
                @"\d{1,2}-\d{1,2}-\d{4}",
                @"\d{1,2}\/\d{1,2}",
                @"\d{1,2}-\d{1,2}"
            };
            var dates = new List<string>();
            foreach (var pattern in datePatterns)
            {
                dates.AddRange(System.Text.RegularExpressions.Regex.Matches(message, pattern)
                    .Cast<System.Text.RegularExpressions.Match>()
                    .Select(m => m.Value));
            }

            // Extract relative dates
            if (lowerMessage.Contains("hôm nay") || lowerMessage.Contains("today"))
            {
                dates.Add("today");
                entities["relative_date"] = "today";
            }
            else if (lowerMessage.Contains("ngày mai") || lowerMessage.Contains("tomorrow"))
            {
                dates.Add("tomorrow");
                entities["relative_date"] = "tomorrow";
            }
            else if (lowerMessage.Contains("tuần này") || lowerMessage.Contains("this week"))
            {
                entities["relative_date"] = "this_week";
            }

            if (dates.Any()) entities["dates"] = dates;

            // Extract amenities
            var amenityKeywords = new[] { "wifi", "pool", "spa", "gym", "restaurant", "bar", "parking", "breakfast" };
            var foundAmenities = amenityKeywords.Where(a => lowerMessage.Contains(a)).ToList();
            if (foundAmenities.Any()) entities["amenities"] = foundAmenities;

            return entities;
        }

        private string DetectIntent(string message, Dictionary<string, object> entities)
        {
            var lowerMessage = message.ToLower();

            // Greeting patterns
            if (ContainsKeywords(message, new[] { "xin chào", "hello", "hi", "chào", "hey" }))
                return "greeting";

            // Booking patterns - Enhanced detection
            if (ContainsKeywords(message, new[] { "đặt phòng", "booking", "book", "reservation", "reserve", "muốn đặt", "cần đặt", "đặt", "thuê phòng" }))
                return "booking";

            // Date-related booking (today, tomorrow, specific dates)
            if (ContainsKeywords(message, new[] { "hôm nay", "ngày mai", "today", "tomorrow", "tuần này", "tháng này" }) &&
                ContainsKeywords(message, new[] { "đặt", "phòng", "booking", "check-in", "nhận phòng" }))
                return "booking_with_date";

            // Room search with specific criteria
            if (entities.ContainsKey("guests") || entities.ContainsKey("room_types"))
                return "room_search";

            // Pricing patterns
            if (ContainsKeywords(message, new[] { "giá", "price", "cost", "tiền", "bao nhiêu", "how much", "chi phí" }) ||
                entities.ContainsKey("target_price") || entities.ContainsKey("max_price") || entities.ContainsKey("min_price"))
                return "pricing";

            // Availability check
            if (ContainsKeywords(message, new[] { "còn phòng", "available", "trống", "free", "vacant", "có phòng" }) ||
                entities.ContainsKey("dates"))
                return "availability";

            // Date-only queries
            if (ContainsKeywords(message, new[] { "hôm nay", "ngày mai", "today", "tomorrow" }) &&
                !ContainsKeywords(message, new[] { "đặt", "booking" }))
                return "date_inquiry";

            // Services and amenities
            if (ContainsKeywords(message, new[] { "dịch vụ", "service", "tiện ích", "amenities", "facilities" }))
                return "services";

            // Specific amenity queries
            if (entities.ContainsKey("amenities") ||
                ContainsKeywords(message, new[] { "wifi", "pool", "spa", "gym", "restaurant", "breakfast" }))
                return "amenities";

            // Location
            if (ContainsKeywords(message, new[] { "địa chỉ", "location", "ở đâu", "vị trí", "where", "address" }))
                return "location";

            // Contact
            if (ContainsKeywords(message, new[] { "liên hệ", "contact", "phone", "email", "gọi" }))
                return "contact";

            // Help
            if (ContainsKeywords(message, new[] { "giúp", "help", "hỗ trợ", "support" }))
                return "help";

            // Follow-up or continuation
            if (ContainsKeywords(message, new[] { "vẫn chưa", "chưa trả lời", "không hiểu", "không rõ", "cụ thể hơn" }))
                return "clarification";

            return "unknown";
        }

        private bool ContainsKeywords(string message, string[] keywords)
        {
            return keywords.Any(keyword => message.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        // Intent Handlers
        private AIServiceResponse HandleGreetingIntent()
        {
            return new AIServiceResponse
            {
                Message = "👋 **Xin chào! Tôi là Hotel AI Assistant**\n\n" +
                         "Tôi có thể giúp bạn:\n" +
                         "🏨 **Tìm và đặt phòng** phù hợp\n" +
                         "💰 **Kiểm tra giá cả** theo yêu cầu\n" +
                         "🛎️ **Thông tin dịch vụ** khách sạn\n" +
                         "📍 **Hướng dẫn địa điểm** và di chuyển\n" +
                         "❓ **Trả lời mọi câu hỏi** về khách sạn\n\n" +
                         "Hãy cho tôi biết bạn cần gì! 😊",
                Suggestions = new[] {
                    "Tôi muốn đặt phòng cho 2 người",
                    "Phòng giá 1000k có không?",
                    "Khách sạn có những dịch vụ gì?"
                },
                Context = new AIChatContext { Intent = "greeting", LastTopic = "welcome" }
            };
        }

        private async Task<AIServiceResponse> HandleBookingIntent(string message, Dictionary<string, object> entities)
        {
            var responseText = "🏨 **Tuyệt vời! Tôi sẽ giúp bạn tìm phòng phù hợp**\n\n";
            var suggestions = new List<string>();

            // Get room data from database
            var roomsQuery = _context.Rooms.Include(r => r.RoomType).AsQueryable();

            // Filter by guest count if specified
            if (entities.ContainsKey("guests"))
            {
                var guestCount = (int)entities["guests"];
                roomsQuery = roomsQuery.Where(r => r.RoomType.MaxOccupancy >= guestCount);
                responseText += $"👥 **Số khách:** {guestCount} người\n";
                suggestions.Add($"Phòng cho {guestCount} người giá tốt");
            }

            // Filter by room type if specified
            if (entities.ContainsKey("room_types"))
            {
                var roomTypes = (List<string>)entities["room_types"];
                var roomType = roomTypes.First();
                roomsQuery = roomsQuery.Where(r => r.RoomType.TypeName.ToLower().Contains(roomType));
                responseText += $"🏠 **Loại phòng:** {roomType.ToUpper()}\n";
                suggestions.Add($"Xem tất cả phòng {roomType}");
            }

            var availableRooms = await roomsQuery.Take(5).ToListAsync();

            if (availableRooms.Any())
            {
                responseText += "\n🎯 **Các phòng phù hợp:**\n";
                foreach (var room in availableRooms)
                {
                    responseText += $"• **{room.RoomType.TypeName}** - Phòng {room.RoomNumber}\n";
                    responseText += $"  💰 {room.Price:N0} VNĐ/đêm | 👥 Tối đa {room.RoomType.MaxOccupancy} người\n";
                    responseText += $"  📝 {room.RoomType.Description}\n\n";
                }

                responseText += "💡 **Để đặt phòng:**\n";
                responseText += "• Chọn phòng và ngày check-in/out\n";
                responseText += "• Gọi hotline: (024) 1234-5678\n";
                responseText += "• Hoặc đặt online ngay";

                suggestions.AddRange(new[] {
                    "Đặt phòng online",
                    "Gọi hotline",
                    "Xem thêm phòng khác"
                });
            }
            else
            {
                responseText += "😔 **Không tìm thấy phòng phù hợp**\n\n";
                responseText += "Hãy thử:\n";
                responseText += "• Thay đổi số lượng khách\n";
                responseText += "• Chọn loại phòng khác\n";
                responseText += "• Liên hệ trực tiếp để được tư vấn";

                suggestions.AddRange(new[] {
                    "Xem tất cả phòng",
                    "Liên hệ tư vấn",
                    "Thay đổi yêu cầu"
                });
            }

            return new AIServiceResponse
            {
                Message = responseText,
                Suggestions = suggestions.ToArray(),
                Context = new AIChatContext { Intent = "booking", LastTopic = "room_booking", BookingIntent = true }
            };
        }

        private async Task<AIServiceResponse> HandleBookingWithDateIntent(string message, Dictionary<string, object> entities)
        {
            var today = DateTime.Now;
            var responseText = "🗓️ **Đặt phòng cho hôm nay - " + today.ToString("dd/MM/yyyy") + "**\n\n";

            if (message.ToLower().Contains("hôm nay") || message.ToLower().Contains("today"))
            {
                responseText += "✅ **Check-in:** Hôm nay (" + today.ToString("dd/MM/yyyy") + ")\n";
                responseText += "📅 **Check-out:** Ngày mai (" + today.AddDays(1).ToString("dd/MM/yyyy") + ")\n\n";
            }
            else if (message.ToLower().Contains("ngày mai") || message.ToLower().Contains("tomorrow"))
            {
                responseText += "✅ **Check-in:** Ngày mai (" + today.AddDays(1).ToString("dd/MM/yyyy") + ")\n";
                responseText += "📅 **Check-out:** " + today.AddDays(2).ToString("dd/MM/yyyy") + "\n\n";
            }

            // Get available rooms for today
            var availableRooms = await _context.Rooms
                .Include(r => r.RoomType)
                .Take(3)
                .ToListAsync();

            responseText += "🏨 **Phòng có sẵn hôm nay:**\n\n";

            foreach (var room in availableRooms)
            {
                responseText += $"✨ **{room.RoomType.TypeName}** - Phòng {room.RoomNumber}\n";
                responseText += $"   💰 **{room.Price:N0} VNĐ/đêm**\n";
                responseText += $"   👥 Tối đa {room.RoomType.MaxOccupancy} người\n";
                responseText += $"   📝 {room.RoomType.Description}\n\n";
            }

            responseText += "🎯 **Để đặt phòng ngay:**\n";
            responseText += "• **Gọi hotline:** (024) 1234-5678\n";
            responseText += "• **Đặt online:** Click 'Đặt phòng ngay'\n";
            responseText += "• **Đến trực tiếp:** 123 Luxury Hotel Street, Hội An\n\n";
            responseText += "💡 **Lưu ý:** Đặt phòng cùng ngày có thể phụ thu 10%";

            return new AIServiceResponse
            {
                Message = responseText,
                Suggestions = new[] { "Đặt phòng ngay", "Gọi hotline", "Xem thêm phòng", "Thay đổi ngày" },
                Context = new AIChatContext { Intent = "booking_with_date", LastTopic = "same_day_booking", BookingIntent = true }
            };
        }

        private AIServiceResponse HandleDateInquiryIntent(string message)
        {
            var today = DateTime.Now;
            var responseText = "📅 **Thông tin ngày hôm nay:**\n\n";

            responseText += $"🗓️ **Hôm nay:** {today.ToString("dddd, dd/MM/yyyy")}\n";
            responseText += $"🕐 **Giờ hiện tại:** {today.ToString("HH:mm")}\n\n";

            responseText += "🏨 **Về việc đặt phòng hôm nay:**\n";
            responseText += "• **Check-in sớm nhất:** 14:00\n";
            responseText += "• **Check-in muộn nhất:** 23:00\n";
            responseText += "• **Đặt phòng cùng ngày:** Có thể, tùy tình trạng phòng\n\n";

            responseText += "💡 **Bạn muốn:**\n";
            responseText += "• Đặt phòng cho hôm nay?\n";
            responseText += "• Kiểm tra phòng trống?\n";
            responseText += "• Xem giá phòng?";

            return new AIServiceResponse
            {
                Message = responseText,
                Suggestions = new[] { "Đặt phòng hôm nay", "Kiểm tra phòng trống", "Xem giá phòng", "Gọi tư vấn" },
                Context = new AIChatContext { Intent = "date_inquiry", LastTopic = "date_info" }
            };
        }

        private AIServiceResponse HandleClarificationIntent(string message)
        {
            var responseText = "😊 **Xin lỗi vì sự nhầm lẫn! Tôi sẽ giúp bạn rõ ràng hơn.**\n\n";

            responseText += "🎯 **Để đặt phòng, tôi cần biết:**\n\n";
            responseText += "📅 **1. Ngày nhận phòng:**\n";
            responseText += "   • Hôm nay (" + DateTime.Now.ToString("dd/MM") + ")\n";
            responseText += "   • Ngày mai (" + DateTime.Now.AddDays(1).ToString("dd/MM") + ")\n";
            responseText += "   • Ngày cụ thể khác\n\n";

            responseText += "👥 **2. Số lượng khách:**\n";
            responseText += "   • 1 người (phòng đơn)\n";
            responseText += "   • 2 người (phòng đôi)\n";
            responseText += "   • Gia đình (3+ người)\n\n";

            responseText += "🏠 **3. Loại phòng mong muốn:**\n";
            responseText += "   • Standard (tiết kiệm)\n";
            responseText += "   • Deluxe (thoải mái)\n";
            responseText += "   • Suite (cao cấp)\n\n";

            responseText += "💰 **4. Ngân sách:**\n";
            responseText += "   • Dưới 500k/đêm\n";
            responseText += "   • 500k - 1 triệu/đêm\n";
            responseText += "   • Trên 1 triệu/đêm\n\n";

            responseText += "💬 **Ví dụ câu hỏi rõ ràng:**\n";
            responseText += "• \"Tôi muốn đặt phòng đôi cho hôm nay\"\n";
            responseText += "• \"Phòng 2 người giá 800k có không?\"\n";
            responseText += "• \"Còn phòng deluxe ngày 25/12 không?\"";

            return new AIServiceResponse
            {
                Message = responseText,
                Suggestions = new[] {
                    "Đặt phòng đôi hôm nay",
                    "Phòng giá 500k",
                    "Phòng deluxe 2 người",
                    "Gọi tư vấn trực tiếp"
                },
                Context = new AIChatContext { Intent = "clarification", LastTopic = "booking_help" }
            };
        }

        private async Task<AIServiceResponse> HandlePricingIntent(string message, Dictionary<string, object> entities)
        {
            var responseText = "💰 **Thông tin giá phòng chi tiết:**\n\n";
            var suggestions = new List<string>();

            var roomsQuery = _context.Rooms.Include(r => r.RoomType).AsQueryable();

            // Handle specific price queries
            if (entities.ContainsKey("target_price"))
            {
                var targetPrice = (int)entities["target_price"];
                var priceRange = targetPrice * 0.1; // 10% tolerance

                var roomsInRange = await roomsQuery
                    .Where(r => r.Price >= targetPrice - priceRange && r.Price <= targetPrice + priceRange)
                    .ToListAsync();

                responseText += $"🎯 **Phòng quanh mức giá {targetPrice:N0} VNĐ:**\n\n";

                if (roomsInRange.Any())
                {
                    foreach (var room in roomsInRange.Take(5))
                    {
                        responseText += $"✅ **{room.RoomType.TypeName}** - Phòng {room.RoomNumber}\n";
                        responseText += $"   💰 **{room.Price:N0} VNĐ/đêm** | 👥 {room.RoomType.MaxOccupancy} người\n";
                        responseText += $"   📝 {room.RoomType.Description}\n\n";
                    }
                    suggestions.AddRange(new[] {
                        $"Đặt phòng {targetPrice:N0}k",
                        "So sánh giá phòng",
                        "Xem khuyến mãi"
                    });
                }
                else
                {
                    responseText += $"😔 Không có phòng chính xác giá {targetPrice:N0} VNĐ\n\n";

                    // Suggest alternatives
                    var cheaperRooms = await roomsQuery.Where(r => r.Price < targetPrice).OrderByDescending(r => r.Price).Take(2).ToListAsync();
                    var expensiveRooms = await roomsQuery.Where(r => r.Price > targetPrice).OrderBy(r => r.Price).Take(2).ToListAsync();

                    if (cheaperRooms.Any())
                    {
                        responseText += "💡 **Phòng rẻ hơn:**\n";
                        foreach (var room in cheaperRooms)
                        {
                            responseText += $"• {room.RoomType.TypeName}: {room.Price:N0} VNĐ/đêm\n";
                        }
                        responseText += "\n";
                    }

                    if (expensiveRooms.Any())
                    {
                        responseText += "⭐ **Phòng cao cấp hơn:**\n";
                        foreach (var room in expensiveRooms)
                        {
                            responseText += $"• {room.RoomType.TypeName}: {room.Price:N0} VNĐ/đêm\n";
                        }
                    }

                    suggestions.AddRange(new[] {
                        "Xem phòng rẻ hơn",
                        "Xem phòng cao cấp",
                        "Tư vấn giá phù hợp"
                    });
                }
            }
            else if (entities.ContainsKey("max_price"))
            {
                var maxPrice = (int)entities["max_price"];
                var affordableRooms = await roomsQuery
                    .Where(r => r.Price <= maxPrice)
                    .OrderBy(r => r.Price)
                    .ToListAsync();

                responseText += $"🏷️ **Phòng dưới {maxPrice:N0} VNĐ:**\n\n";

                if (affordableRooms.Any())
                {
                    foreach (var room in affordableRooms.Take(5))
                    {
                        responseText += $"✅ **{room.RoomType.TypeName}** - {room.Price:N0} VNĐ/đêm\n";
                        responseText += $"   👥 {room.RoomType.MaxOccupancy} người | 📝 {room.RoomType.Description}\n\n";
                    }
                    suggestions.AddRange(new[] {
                        "Đặt phòng giá tốt",
                        "So sánh tiện ích",
                        "Xem khuyến mãi"
                    });
                }
                else
                {
                    responseText += $"😔 Không có phòng dưới {maxPrice:N0} VNĐ\n\n";
                    var cheapestRoom = await roomsQuery.OrderBy(r => r.Price).FirstOrDefaultAsync();
                    if (cheapestRoom != null)
                    {
                        responseText += $"💡 **Phòng rẻ nhất:** {cheapestRoom.RoomType.TypeName} - {cheapestRoom.Price:N0} VNĐ/đêm";
                    }
                    suggestions.AddRange(new[] {
                        "Xem phòng rẻ nhất",
                        "Tư vấn ngân sách",
                        "Khuyến mãi đặc biệt"
                    });
                }
            }
            else
            {
                // General pricing overview
                var roomsByType = await roomsQuery
                    .GroupBy(r => r.RoomType.TypeName)
                    .Select(g => new {
                        Type = g.Key,
                        MinPrice = g.Min(r => r.Price),
                        MaxPrice = g.Max(r => r.Price),
                        Count = g.Count()
                    })
                    .ToListAsync();

                responseText += "📊 **Bảng giá tổng quan:**\n\n";
                foreach (var type in roomsByType)
                {
                    if (type.MinPrice == type.MaxPrice)
                    {
                        responseText += $"🏠 **{type.Type}**: {type.MinPrice:N0} VNĐ/đêm ({type.Count} phòng)\n";
                    }
                    else
                    {
                        responseText += $"🏠 **{type.Type}**: {type.MinPrice:N0} - {type.MaxPrice:N0} VNĐ/đêm ({type.Count} phòng)\n";
                    }
                }

                responseText += "\n💡 **Thông tin thêm:**\n";
                responseText += "• Giá đã bao gồm thuế VAT\n";
                responseText += "• Miễn phí WiFi và bữa sáng\n";
                responseText += "• Giảm giá 10% cho booking trên 3 đêm";

                suggestions.AddRange(new[] {
                    "Phòng giá 500k",
                    "Phòng giá 1000k",
                    "Phòng cao cấp",
                    "Khuyến mãi hiện tại"
                });
            }

            return new AIServiceResponse
            {
                Message = responseText,
                Suggestions = suggestions.ToArray(),
                Context = new AIChatContext { Intent = "pricing", LastTopic = "room_prices" }
            };
        }

        private async Task<AIServiceResponse> HandleRoomSearchIntent(Dictionary<string, object> entities)
        {
            var responseText = "🔍 **Tìm kiếm phòng theo yêu cầu:**\n\n";
            var roomsQuery = _context.Rooms.Include(r => r.RoomType).AsQueryable();

            // Apply filters based on entities
            if (entities.ContainsKey("guests"))
            {
                var guestCount = (int)entities["guests"];
                roomsQuery = roomsQuery.Where(r => r.RoomType.MaxOccupancy >= guestCount);
                responseText += $"👥 Phù hợp cho **{guestCount} người**\n";
            }

            if (entities.ContainsKey("room_types"))
            {
                var roomTypes = (List<string>)entities["room_types"];
                var roomType = roomTypes.First();
                roomsQuery = roomsQuery.Where(r => r.RoomType.TypeName.ToLower().Contains(roomType));
                responseText += $"🏠 Loại phòng: **{roomType.ToUpper()}**\n";
            }

            var matchingRooms = await roomsQuery.Take(5).ToListAsync();

            if (matchingRooms.Any())
            {
                responseText += "\n✨ **Kết quả tìm kiếm:**\n\n";
                foreach (var room in matchingRooms)
                {
                    responseText += $"🏨 **{room.RoomType.TypeName}** - Phòng {room.RoomNumber}\n";
                    responseText += $"   💰 {room.Price:N0} VNĐ/đêm\n";
                    responseText += $"   👥 Tối đa {room.RoomType.MaxOccupancy} người\n";
                    responseText += $"   📝 {room.RoomType.Description}\n\n";
                }
            }
            else
            {
                responseText += "\n😔 Không tìm thấy phòng phù hợp với yêu cầu của bạn.\n";
                responseText += "Hãy thử điều chỉnh tiêu chí tìm kiếm!";
            }

            return new AIServiceResponse
            {
                Message = responseText,
                Suggestions = new[] { "Đặt phòng ngay", "Thay đổi yêu cầu", "Xem tất cả phòng", "Liên hệ tư vấn" },
                Context = new AIChatContext { Intent = "room_search", LastTopic = "search_results" }
            };
        }

        private async Task<AIServiceResponse> HandleServicesIntent(string message, Dictionary<string, object> entities)
        {
            var amenities = await _context.Amenities.ToListAsync();

            var responseText = "🛎️ **Dịch vụ & Tiện ích khách sạn:**\n\n";

            if (amenities.Any())
            {
                responseText += "✨ **Tiện ích có sẵn:**\n";
                foreach (var amenity in amenities.Take(10))
                {
                    responseText += $"• {amenity.AmenityName}\n";
                }

                if (amenities.Count > 10)
                {
                    responseText += $"• ... và {amenities.Count - 10} tiện ích khác\n";
                }
            }

            responseText += "\n🌟 **Dịch vụ đặc biệt:**\n";
            responseText += "• Room Service 24/7\n";
            responseText += "• Concierge hỗ trợ\n";
            responseText += "• Đưa đón sân bay\n";
            responseText += "• Giặt ủi express\n";
            responseText += "• Tour du lịch\n\n";

            responseText += "📞 **Liên hệ đặt dịch vụ:** (024) 1234-5678";

            return new AIServiceResponse
            {
                Message = responseText,
                Suggestions = new[] { "Đặt room service", "Tour du lịch", "Đưa đón sân bay", "Spa massage" },
                Context = new AIChatContext { Intent = "services", LastTopic = "hotel_amenities" }
            };
        }

        private AIServiceResponse HandleLocationIntent()
        {
            return new AIServiceResponse
            {
                Message = "📍 **Thông tin vị trí khách sạn:**\n\n" +
                         "🏨 **Địa chỉ:** 123 Luxury Hotel Street, Hội An, Quảng Nam\n\n" +
                         "🚗 **Cách di chuyển:**\n" +
                         "• Từ sân bay Đà Nẵng: 45 phút (35km)\n" +
                         "• Từ trung tâm Hội An: 5 phút đi bộ\n" +
                         "• Đến phố cổ: 3 phút xe máy\n" +
                         "• Ra bãi biển An Bàng: 10 phút\n\n" +
                         "🎯 **Điểm nổi bật gần đây:**\n" +
                         "• Chùa Cầu: 500m\n" +
                         "• Chợ đêm Hội An: 300m\n" +
                         "• Làng rau Trà Quế: 2km\n" +
                         "• Rừng dừa Bảy Mẫu: 5km\n\n" +
                         "🅿️ **Tiện ích:** Bãi đỗ xe miễn phí, gần trạm xe bus",
                Suggestions = new[] { "Đặt shuttle bus", "Thuê xe máy", "Bản đồ chi tiết", "Tour tham quan" },
                Context = new AIChatContext { Intent = "location", LastTopic = "hotel_location" }
            };
        }

        private AIServiceResponse HandleContactIntent()
        {
            return new AIServiceResponse
            {
                Message = "📞 **Thông tin liên hệ:**\n\n" +
                         "🏨 **Reception 24/7:** (024) 1234-5678\n" +
                         "📧 **Email:** info@luxuryhotel.com\n" +
                         "💬 **WhatsApp:** +84 123 456 789\n" +
                         "🌐 **Website:** www.luxuryhotel.com\n\n" +
                         "⏰ **Giờ hỗ trợ:**\n" +
                         "• Reception: 24/7\n" +
                         "• Booking: 6:00 - 22:00\n" +
                         "• Concierge: 7:00 - 23:00\n\n" +
                         "🚨 **Khẩn cấp:** (024) 1234-5679",
                Suggestions = new[] { "Gọi ngay", "Gửi email", "Chat WhatsApp", "Đặt lịch gọi lại" },
                Context = new AIChatContext { Intent = "contact", LastTopic = "contact_info" }
            };
        }

        private AIServiceResponse HandleHelpIntent()
        {
            return new AIServiceResponse
            {
                Message = "🆘 **Tôi có thể giúp bạn với:**\n\n" +
                         "🏨 **Đặt phòng:**\n" +
                         "• Tìm phòng theo số người: \"Phòng cho 2 người\"\n" +
                         "• Tìm theo giá: \"Phòng giá 1000k\"\n" +
                         "• Tìm theo loại: \"Phòng deluxe\"\n\n" +
                         "💰 **Kiểm tra giá:**\n" +
                         "• \"Giá phòng bao nhiêu?\"\n" +
                         "• \"Phòng dưới 500k\"\n" +
                         "• \"Phòng trên 1 triệu\"\n\n" +
                         "🛎️ **Dịch vụ & tiện ích:**\n" +
                         "• \"Khách sạn có spa không?\"\n" +
                         "• \"Có wifi miễn phí không?\"\n" +
                         "• \"Dịch vụ gì có sẵn?\"\n\n" +
                         "📍 **Vị trí & liên hệ:**\n" +
                         "• \"Khách sạn ở đâu?\"\n" +
                         "• \"Số điện thoại là gì?\"\n\n" +
                         "💡 **Mẹo:** Hãy nói tự nhiên, tôi sẽ hiểu!",
                Suggestions = new[] { "Phòng cho 2 người", "Giá phòng 1000k", "Có spa không?", "Khách sạn ở đâu?" },
                Context = new AIChatContext { Intent = "help", LastTopic = "assistance" }
            };
        }

        private AIServiceResponse HandleDefaultIntent(string message)
        {
            return new AIServiceResponse
            {
                Message = "🤔 **Tôi chưa hiểu rõ câu hỏi của bạn.**\n\n" +
                         "Bạn có thể hỏi tôi về:\n" +
                         "🏨 **Đặt phòng:** \"Tôi muốn đặt phòng cho 2 người\"\n" +
                         "💰 **Giá cả:** \"Phòng giá 1000k có không?\"\n" +
                         "🛎️ **Dịch vụ:** \"Khách sạn có những dịch vụ gì?\"\n" +
                         "📍 **Vị trí:** \"Khách sạn ở đâu?\"\n" +
                         "📞 **Liên hệ:** \"Số điện thoại là gì?\"\n\n" +
                         "Hoặc hãy thử hỏi cách khác! 😊",
                Suggestions = new[] { "Đặt phòng", "Xem giá", "Dịch vụ", "Vị trí", "Liên hệ", "Trợ giúp" },
                Context = new AIChatContext { Intent = "unknown", LastTopic = "general" }
            };
        }

        private async Task<AIServiceResponse> HandleAvailabilityIntent(Dictionary<string, object> entities)
        {
            var responseText = "📅 **Kiểm tra tình trạng phòng:**\n\n";

            if (entities.ContainsKey("dates"))
            {
                var dates = (List<string>)entities["dates"];
                var dateStr = dates.First();
                responseText += $"🗓️ **Ngày quan tâm:** {dateStr}\n\n";
            }

            // In a real implementation, you would check actual availability
            var availableRooms = await _context.Rooms
                .Include(r => r.RoomType)
                .Take(5)
                .ToListAsync();

            responseText += "✅ **Phòng hiện có sẵn:**\n\n";
            foreach (var room in availableRooms)
            {
                responseText += $"🏨 **{room.RoomType.TypeName}** - Phòng {room.RoomNumber}\n";
                responseText += $"   💰 {room.Price:N0} VNĐ/đêm | 👥 {room.RoomType.MaxOccupancy} người\n\n";
            }

            responseText += "💡 **Lưu ý:** Tình trạng phòng có thể thay đổi. Vui lòng liên hệ để xác nhận chính xác.";

            return new AIServiceResponse
            {
                Message = responseText,
                Suggestions = new[] { "Đặt phòng ngay", "Kiểm tra ngày khác", "Gọi xác nhận", "Xem thêm phòng" },
                Context = new AIChatContext { Intent = "availability", LastTopic = "room_availability" }
            };
        }

        private async Task<AIServiceResponse> HandleAmenitiesIntent(string message, Dictionary<string, object> entities)
        {
            var responseText = "🏨 **Tiện ích & Dịch vụ khách sạn:**\n\n";

            if (entities.ContainsKey("amenities"))
            {
                var requestedAmenities = (List<string>)entities["amenities"];
                var amenity = requestedAmenities.First();

                responseText += $"🔍 **Về {amenity.ToUpper()}:**\n\n";

                switch (amenity.ToLower())
                {
                    case "wifi":
                        responseText += "📶 **WiFi miễn phí:**\n";
                        responseText += "• Tốc độ cao trong toàn bộ khách sạn\n";
                        responseText += "• Không giới hạn thiết bị\n";
                        responseText += "• Hỗ trợ streaming và gaming\n";
                        responseText += "• Mật khẩu: LuxuryHotel2024";
                        break;

                    case "pool":
                        responseText += "🏊‍♀️ **Hồ bơi:**\n";
                        responseText += "• Hồ bơi vô cực tầng thượng\n";
                        responseText += "• Giờ mở: 6:00 - 22:00\n";
                        responseText += "• Khu vực trẻ em riêng biệt\n";
                        responseText += "• Bar bên hồ bơi\n";
                        responseText += "• Ghế tắm nắng miễn phí";
                        break;

                    case "spa":
                        responseText += "💆‍♀️ **Spa & Wellness:**\n";
                        responseText += "• Massage truyền thống Việt Nam\n";
                        responseText += "• Sauna và steam room\n";
                        responseText += "• Giờ hoạt động: 8:00 - 22:00\n";
                        responseText += "• Đặt lịch: (024) 1234-5678\n";
                        responseText += "• Giảm giá 20% cho khách lưu trú";
                        break;

                    case "gym":
                        responseText += "💪 **Phòng gym:**\n";
                        responseText += "• Trang thiết bị hiện đại\n";
                        responseText += "• Mở cửa 24/7\n";
                        responseText += "• Huấn luyện viên cá nhân\n";
                        responseText += "• Lớp yoga buổi sáng\n";
                        responseText += "• Miễn phí cho khách lưu trú";
                        break;

                    case "restaurant":
                        responseText += "🍽️ **Nhà hàng:**\n";
                        responseText += "• Ẩm thực Việt Nam và quốc tế\n";
                        responseText += "• Giờ phục vụ: 6:00 - 23:00\n";
                        responseText += "• Buffet sáng: 6:00 - 10:00\n";
                        responseText += "• Rooftop bar: 17:00 - 02:00\n";
                        responseText += "• Đặt bàn: ext. 1234";
                        break;

                    default:
                        responseText += $"✅ **{amenity}** có sẵn tại khách sạn\n";
                        responseText += "Liên hệ reception để biết thêm chi tiết!";
                        break;
                }
            }
            else
            {
                var amenities = await _context.Amenities.ToListAsync();
                responseText += "🌟 **Tất cả tiện ích:**\n\n";

                foreach (var amenity in amenities)
                {
                    responseText += $"✨ {amenity.AmenityName}\n";
                }

                responseText += "\n💡 **Hỏi cụ thể:** \"Có wifi không?\", \"Spa mở mấy giờ?\", \"Hồ bơi ở đâu?\"";
            }

            return new AIServiceResponse
            {
                Message = responseText,
                Suggestions = new[] { "Có wifi không?", "Spa mở mấy giờ?", "Hồ bơi ở đâu?", "Nhà hàng phục vụ gì?" },
                Context = new AIChatContext { Intent = "amenities", LastTopic = "facility_info" }
            };
        }

        private string[] GetSuggestionsForIntent(string intent)
        {
            return intent switch
            {
                "booking" => new[] { "Đặt phòng Standard", "Đặt phòng Deluxe", "Xem lịch trống" },
                "pricing" => new[] { "Giá phòng Standard", "Giá phòng Deluxe", "Khuyến mãi" },
                "services" => new[] { "Dịch vụ spa", "Nhà hàng", "Hồ bơi" },
                "room_search" => new[] { "Phòng cho 2 người", "Phòng deluxe", "Phòng giá tốt" },
                "amenities" => new[] { "Có wifi không?", "Spa mở mấy giờ?", "Hồ bơi ở đâu?" },
                "location" => new[] { "Bản đồ", "Đưa đón sân bay", "Tour tham quan" },
                "contact" => new[] { "Gọi ngay", "Gửi email", "Chat WhatsApp" },
                _ => new[] { "Giúp tôi", "Thông tin khách sạn", "Liên hệ nhân viên" }
            };
        }
    }

    // Models for AI Chat
    public class AIChatContext
    {
        public string? Intent { get; set; }
        public string? LastTopic { get; set; }
        public bool BookingIntent { get; set; }
        public string? UserName { get; set; }
    }

    public class AIServiceResponse
    {
        public string Message { get; set; } = string.Empty;
        public string[]? Suggestions { get; set; }
        public AIChatContext? Context { get; set; }
    }
}

// Request/Response models
namespace HotelBooking.Models.ViewModels
{
    public class AIChatRequest
    {
        public string Message { get; set; } = "";
        public string? ConversationId { get; set; }
        public string? Context { get; set; }
    }

    public class AIChatFeedbackRequest
    {
        public int Rating { get; set; } // 1-5 stars
        public string? Message { get; set; }
        public bool IsHelpful { get; set; }
        public string? ConversationId { get; set; }
        public string? Intent { get; set; }
    }

    public class AIChatAnalyticsResponse
    {
        public int TotalConversations { get; set; }
        public int TotalMessages { get; set; }
        public string AverageResponseTime { get; set; } = "";
        public double SatisfactionRate { get; set; }
        public List<IntentStatistic> TopIntents { get; set; } = new();
        public List<DailyStatistic> DailyStats { get; set; } = new();
    }

    public class IntentStatistic
    {
        public string Intent { get; set; } = "";
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class DailyStatistic
    {
        public string Date { get; set; } = "";
        public int Conversations { get; set; }
        public int Messages { get; set; }
        public double Satisfaction { get; set; }
    }
}










    }
}








        private string[] GetSuggestionsForIntent(string intent)
        {
            return intent switch
            {
                "booking" => new[] { "Đặt phòng Standard", "Đặt phòng Deluxe", "Xem lịch trống" },
                "pricing" => new[] { "Giá phòng Standard", "Giá phòng Deluxe", "Khuyến mãi" },
                "services" => new[] { "Dịch vụ spa", "Nhà hàng", "Hồ bơi" },
                "food" => new[] { "Menu nhà hàng", "Đặt bàn", "Room service" },
                "travel" => new[] { "Tour Hội An", "Thuê xe", "Điểm tham quan" },
                _ => new[] { "Giúp tôi", "Thông tin khách sạn", "Liên hệ nhân viên" }
            };
        }


    }

    // Models
    public class AIChatRequest
    {
        public string Message { get; set; } = string.Empty;
        public AIChatContext? Context { get; set; }
    }

    public class AIChatResponse
    {
        public string Message { get; set; } = string.Empty;
        public string[]? Suggestions { get; set; }
        public AIChatContext? Context { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class AIChatContext
    {
        public string? Intent { get; set; }
        public string? LastTopic { get; set; }
        public bool BookingIntent { get; set; }
        public string? UserName { get; set; }
    }

    public class AIServiceResponse
    {
        public string Message { get; set; } = string.Empty;
        public string[]? Suggestions { get; set; }
        public AIChatContext? Context { get; set; }
    }
}
