using HotelBooking.Data;
using HotelBooking.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace HotelBooking.Services
{
    public interface IAIChatbotService
    {
        Task<AIChatResponse> ProcessMessageAsync(string message, int? userId = null);
        Task<List<string>> GetSuggestionsAsync(string partialMessage);
        Task<AIChatResponse> GetWelcomeMessageAsync(int? userId = null);
    }

    public class AIChatbotService : IAIChatbotService
    {
        private readonly HotelBookingContext _context;
        private readonly ILogger<AIChatbotService> _logger;
        private readonly Dictionary<string, Func<string, int?, Task<AIChatResponse>>> _intentHandlers;
        private readonly List<AIKnowledgeItem> _knowledgeBase;

        public AIChatbotService(HotelBookingContext context, ILogger<AIChatbotService> logger)
        {
            _context = context;
            _logger = logger;
            _knowledgeBase = InitializeKnowledgeBase();
            _intentHandlers = InitializeIntentHandlers();
        }

        public async Task<AIChatResponse> ProcessMessageAsync(string message, int? userId = null)
        {
            try
            {
                _logger.LogInformation($"🤖 AI Processing message: {message} from user: {userId}");

                // Normalize message
                var normalizedMessage = NormalizeMessage(message);
                
                // Detect intent
                var intent = await DetectIntentAsync(normalizedMessage);
                
                // Handle intent
                if (_intentHandlers.ContainsKey(intent))
                {
                    var response = await _intentHandlers[intent](normalizedMessage, userId);
                    response.Intent = intent;
                    response.Confidence = CalculateConfidence(normalizedMessage, intent);
                    return response;
                }

                // Fallback to knowledge base search
                return await SearchKnowledgeBaseAsync(normalizedMessage, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing AI message");
                return new AIChatResponse
                {
                    Message = "Xin lỗi, tôi đang gặp một chút vấn đề kỹ thuật. Bạn có thể thử lại sau hoặc liên hệ với nhân viên hỗ trợ không?",
                    Intent = "error",
                    Confidence = 1.0f,
                    Suggestions = new List<string> { "Liên hệ nhân viên", "Thử lại", "Xem câu hỏi thường gặp" }
                };
            }
        }

        public async Task<List<string>> GetSuggestionsAsync(string partialMessage)
        {
            var suggestions = new List<string>();
            
            if (string.IsNullOrWhiteSpace(partialMessage))
            {
                return new List<string>
                {
                    "Tôi muốn đặt phòng",
                    "Giá phòng như thế nào?",
                    "Khách sạn có những tiện ích gì?",
                    "Làm thế nào để hủy đặt phòng?",
                    "Chính sách thanh toán ra sao?"
                };
            }

            var normalized = NormalizeMessage(partialMessage);
            
            // Search in knowledge base for matching questions
            var matches = _knowledgeBase
                .Where(kb => kb.Keywords.Any(k => k.Contains(normalized, StringComparison.OrdinalIgnoreCase)))
                .Take(5)
                .Select(kb => kb.Question)
                .ToList();

            if (matches.Any())
            {
                suggestions.AddRange(matches);
            }

            // Add common follow-up questions
            suggestions.AddRange(new[]
            {
                "Tôi cần thêm thông tin",
                "Cảm ơn bạn",
                "Liên hệ nhân viên"
            });

            return suggestions.Distinct().Take(5).ToList();
        }

        public async Task<AIChatResponse> GetWelcomeMessageAsync(int? userId = null)
        {
            var userName = "bạn";
            
            if (userId.HasValue)
            {
                var user = await _context.Users.FindAsync(userId.Value);
                if (user != null)
                {
                    userName = user.UserName ?? "bạn";
                }
            }

            return new AIChatResponse
            {
                Message = $"Xin chào {userName}! 👋\n\nTôi là AI Assistant của khách sạn. Tôi có thể giúp bạn:\n\n🏨 Tìm hiểu về phòng và giá cả\n📅 Hướng dẫn đặt phòng\n💳 Thông tin thanh toán\n🎁 Chương trình khách hàng thân thiết\n📞 Liên hệ hỗ trợ\n\nBạn cần tôi hỗ trợ điều gì?",
                Intent = "welcome",
                Confidence = 1.0f,
                Suggestions = new List<string>
                {
                    "Xem phòng trống",
                    "Bảng giá phòng",
                    "Tiện ích khách sạn",
                    "Cách đặt phòng",
                    "Chính sách hủy"
                }
            };
        }

        private string NormalizeMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return "";
            
            // Convert to lowercase and remove special characters
            var normalized = message.ToLowerInvariant();
            normalized = Regex.Replace(normalized, @"[^\w\s]", " ");
            normalized = Regex.Replace(normalized, @"\s+", " ").Trim();
            
            return normalized;
        }

        private async Task<string> DetectIntentAsync(string normalizedMessage)
        {
            // Intent detection based on keywords
            var intentKeywords = new Dictionary<string, string[]>
            {
                ["booking"] = new[] { "đặt phòng", "book", "reservation", "đặt", "booking" },
                ["pricing"] = new[] { "giá", "price", "cost", "tiền", "phí", "bao nhiêu" },
                ["availability"] = new[] { "phòng trống", "available", "còn phòng", "trống" },
                ["amenities"] = new[] { "tiện ích", "amenity", "dịch vụ", "facilities" },
                ["cancellation"] = new[] { "hủy", "cancel", "hủy bỏ", "không đặt" },
                ["payment"] = new[] { "thanh toán", "payment", "pay", "trả tiền" },
                ["loyalty"] = new[] { "khách hàng thân thiết", "loyalty", "điểm", "point" },
                ["contact"] = new[] { "liên hệ", "contact", "gọi", "phone", "email" },
                ["help"] = new[] { "giúp", "help", "hỗ trợ", "support" },
                ["greeting"] = new[] { "xin chào", "hello", "hi", "chào" }
            };

            foreach (var intent in intentKeywords)
            {
                if (intent.Value.Any(keyword => normalizedMessage.Contains(keyword)))
                {
                    return intent.Key;
                }
            }

            return "general";
        }

        private float CalculateConfidence(string message, string intent)
        {
            // Simple confidence calculation based on keyword matches
            var baseConfidence = 0.7f;
            var keywordBonus = 0.1f;
            
            var intentKeywords = GetIntentKeywords(intent);
            var matchCount = intentKeywords.Count(keyword => message.Contains(keyword));
            
            return Math.Min(1.0f, baseConfidence + (matchCount * keywordBonus));
        }

        private string[] GetIntentKeywords(string intent)
        {
            return intent switch
            {
                "booking" => new[] { "đặt phòng", "book", "reservation" },
                "pricing" => new[] { "giá", "price", "cost" },
                "availability" => new[] { "phòng trống", "available" },
                "amenities" => new[] { "tiện ích", "amenity", "dịch vụ" },
                "cancellation" => new[] { "hủy", "cancel" },
                "payment" => new[] { "thanh toán", "payment" },
                "loyalty" => new[] { "khách hàng thân thiết", "loyalty" },
                "contact" => new[] { "liên hệ", "contact" },
                _ => new string[0]
            };
        }

        private Dictionary<string, Func<string, int?, Task<AIChatResponse>>> InitializeIntentHandlers()
        {
            return new Dictionary<string, Func<string, int?, Task<AIChatResponse>>>
            {
                ["booking"] = HandleBookingIntent,
                ["pricing"] = HandlePricingIntent,
                ["availability"] = HandleAvailabilityIntent,
                ["amenities"] = HandleAmenitiesIntent,
                ["cancellation"] = HandleCancellationIntent,
                ["payment"] = HandlePaymentIntent,
                ["loyalty"] = HandleLoyaltyIntent,
                ["contact"] = HandleContactIntent,
                ["greeting"] = HandleGreetingIntent,
                ["help"] = HandleHelpIntent
            };
        }

        private async Task<AIChatResponse> HandleBookingIntent(string message, int? userId)
        {
            var availableRooms = await _context.Rooms
                .Include(r => r.RoomType)
                .Where(r => r.IsActive && r.Status == "Available")
                .Take(3)
                .ToListAsync();

            var roomInfo = string.Join("\n", availableRooms.Select(r =>
                $"🏨 {r.RoomNumber} - {r.RoomType?.TypeName} - ${r.Price}/đêm"));

            return new AIChatResponse
            {
                Message = $"Tuyệt vời! Tôi sẽ giúp bạn đặt phòng. 📅\n\nHiện tại chúng tôi có những phòng trống:\n\n{roomInfo}\n\nĐể đặt phòng, bạn cần:\n1️⃣ Chọn ngày check-in và check-out\n2️⃣ Số lượng khách\n3️⃣ Loại phòng mong muốn\n\nBạn muốn đặt phòng nào và khi nào?",
                Intent = "booking",
                Suggestions = new List<string>
                {
                    "Đặt phòng Standard",
                    "Đặt phòng Deluxe",
                    "Xem tất cả phòng trống",
                    "Hướng dẫn đặt phòng",
                    "Liên hệ nhân viên"
                }
            };
        }

        private async Task<AIChatResponse> HandlePricingIntent(string message, int? userId)
        {
            var roomTypes = await _context.RoomTypes
                .Where(rt => rt.IsActive)
                .ToListAsync();

            var rooms = await _context.Rooms
                .Include(r => r.RoomType)
                .Where(r => r.IsActive)
                .GroupBy(r => r.RoomType!.TypeName)
                .Select(g => new {
                    RoomType = g.Key,
                    MinPrice = g.Min(r => r.Price),
                    MaxPrice = g.Max(r => r.Price)
                })
                .ToListAsync();

            var priceInfo = string.Join("\n", rooms.Select(r =>
                $"💰 {r.RoomType}: ${r.MinPrice} - ${r.MaxPrice}/đêm"));

            return new AIChatResponse
            {
                Message = $"Đây là bảng giá phòng của chúng tôi: 💳\n\n{priceInfo}\n\n📋 Lưu ý:\n• Giá có thể thay đổi theo mùa\n• Giảm giá cho khách hàng thân thiết\n• Miễn phí cho trẻ em dưới 6 tuổi\n• Bao gồm WiFi và bữa sáng\n\nBạn quan tâm đến loại phòng nào?",
                Intent = "pricing",
                Suggestions = new List<string>
                {
                    "Phòng Standard",
                    "Phòng Deluxe",
                    "Phòng Suite",
                    "Ưu đãi đặc biệt",
                    "So sánh giá phòng"
                }
            };
        }

        private async Task<AIChatResponse> HandleAvailabilityIntent(string message, int? userId)
        {
            var totalRooms = await _context.Rooms.CountAsync(r => r.IsActive);
            var availableRooms = await _context.Rooms.CountAsync(r => r.IsActive && r.Status == "Available");
            var occupiedRooms = totalRooms - availableRooms;

            var availableByType = await _context.Rooms
                .Include(r => r.RoomType)
                .Where(r => r.IsActive && r.Status == "Available")
                .GroupBy(r => r.RoomType!.TypeName)
                .Select(g => new { RoomType = g.Key, Count = g.Count() })
                .ToListAsync();

            var availabilityInfo = string.Join("\n", availableByType.Select(a =>
                $"✅ {a.RoomType}: {a.Count} phòng trống"));

            return new AIChatResponse
            {
                Message = $"Tình trạng phòng hiện tại: 🏨\n\n📊 Tổng quan:\n• Tổng số phòng: {totalRooms}\n• Phòng trống: {availableRooms}\n• Phòng đã đặt: {occupiedRooms}\n\n{availabilityInfo}\n\nBạn muốn đặt phòng nào? Tôi có thể kiểm tra tình trạng cụ thể cho ngày bạn muốn.",
                Intent = "availability",
                Suggestions = new List<string>
                {
                    "Kiểm tra ngày cụ thể",
                    "Đặt phòng ngay",
                    "Xem chi tiết phòng",
                    "Thông báo khi có phòng",
                    "Gọi đặt phòng"
                }
            };
        }

        private async Task<AIChatResponse> HandleAmenitiesIntent(string message, int? userId)
        {
            var amenities = await _context.Amenities
                .Where(a => a.IsActive)
                .GroupBy(a => a.Category)
                .ToListAsync();

            var amenityInfo = "";
            foreach (var group in amenities)
            {
                amenityInfo += $"\n🏷️ **{group.Key}**\n";
                foreach (var amenity in group)
                {
                    amenityInfo += $"   {amenity.Icon} {amenity.AmenityName}\n";
                }
            }

            return new AIChatResponse
            {
                Message = $"Khách sạn chúng tôi có đầy đủ tiện ích hiện đại: ✨\n{amenityInfo}\n\n🌟 Dịch vụ đặc biệt:\n• Dịch vụ phòng 24/7\n• Concierge service\n• Valet parking\n• Business center\n• Spa & Wellness\n\nBạn quan tâm đến tiện ích nào đặc biệt?",
                Intent = "amenities",
                Suggestions = new List<string>
                {
                    "Dịch vụ Spa",
                    "Nhà hàng",
                    "Hồ bơi",
                    "Phòng gym",
                    "Dịch vụ đưa đón"
                }
            };
        }

        private async Task<AIChatResponse> HandleCancellationIntent(string message, int? userId)
        {
            return new AIChatResponse
            {
                Message = "Chính sách hủy đặt phòng của chúng tôi: 📋\n\n🕐 **Hủy miễn phí:**\n• Trước 24h: Hoàn tiền 100%\n• Trước 12h: Hoàn tiền 50%\n• Trong 12h: Không hoàn tiền\n\n💳 **Quy trình hủy:**\n1️⃣ Đăng nhập tài khoản\n2️⃣ Vào 'Lịch sử đặt phòng'\n3️⃣ Chọn 'Hủy đặt phòng'\n4️⃣ Xác nhận hủy\n\n📞 **Hỗ trợ:** Gọi hotline nếu cần hỗ trợ khẩn cấp\n\nBạn cần hủy đặt phòng nào đó không?",
                Intent = "cancellation",
                Suggestions = new List<string>
                {
                    "Hủy đặt phòng của tôi",
                    "Xem lịch sử đặt phòng",
                    "Thay đổi ngày đặt",
                    "Liên hệ hỗ trợ",
                    "Chính sách hoàn tiền"
                }
            };
        }

        private async Task<AIChatResponse> HandlePaymentIntent(string message, int? userId)
        {
            return new AIChatResponse
            {
                Message = "Chúng tôi hỗ trợ nhiều phương thức thanh toán: 💳\n\n💰 **Phương thức thanh toán:**\n• 💳 Thẻ tín dụng/ghi nợ\n• 🏦 Chuyển khoản ngân hàng\n• 📱 QR Code VietinBank\n• 💵 Tiền mặt tại quầy\n• 🎫 Ví điện tử (MoMo, ZaloPay)\n\n🔒 **Bảo mật:**\n• Mã hóa SSL 256-bit\n• Tuân thủ chuẩn PCI DSS\n• Xác thực 3D Secure\n\n📋 **Chính sách:**\n• Thanh toán 50% khi đặt\n• 50% còn lại khi check-in\n• Hoàn tiền trong 3-5 ngày\n\nBạn muốn thanh toán bằng phương thức nào?",
                Intent = "payment",
                Suggestions = new List<string>
                {
                    "Thanh toán QR Code",
                    "Thanh toán thẻ",
                    "Chuyển khoản",
                    "Thanh toán tại quầy",
                    "Hướng dẫn thanh toán"
                }
            };
        }

        private async Task<AIChatResponse> HandleLoyaltyIntent(string message, int? userId)
        {
            var loyaltyInfo = "";

            if (userId.HasValue)
            {
                var customerLoyalty = await _context.CustomerLoyalties
                    .Include(cl => cl.LoyaltyTier)
                    .FirstOrDefaultAsync(cl => cl.UserID == userId.Value);

                if (customerLoyalty != null)
                {
                    loyaltyInfo = $"\n🎯 **Thông tin của bạn:**\n• Hạng: {customerLoyalty.LoyaltyTier?.TierName}\n• Điểm hiện tại: {customerLoyalty.CurrentPoints}\n• Tổng chi tiêu: ${customerLoyalty.TotalAmountSpent}\n\n";
                }
            }

            return new AIChatResponse
            {
                Message = $"Chương trình khách hàng thân thiết: 🌟\n{loyaltyInfo}🏆 **Các hạng thành viên:**\n• 🥉 Bronze (0-999 điểm): Ưu đãi cơ bản\n• 🥈 Silver (1000-4999 điểm): Giảm 5%, ưu tiên hỗ trợ\n• 🥇 Gold (5000+ điểm): Giảm 10%, nâng hạng phòng\n\n💎 **Cách tích điểm:**\n• 10 điểm/$1 đặt phòng\n• 5 điểm/$1 dịch vụ\n• 100 điểm chào mừng\n\n🎁 **Ưu đãi đặc biệt:**\n• Sinh nhật: Giảm 20%\n• Check-out muộn miễn phí\n• Nâng hạng phòng (tùy tình trạng)\n\nBạn muốn biết thêm về chương trình nào?",
                Intent = "loyalty",
                Suggestions = new List<string>
                {
                    "Xem điểm của tôi",
                    "Cách tích điểm",
                    "Ưu đãi sinh nhật",
                    "Nâng hạng thành viên",
                    "Đổi điểm lấy quà"
                }
            };
        }

        private async Task<AIChatResponse> HandleContactIntent(string message, int? userId)
        {
            return new AIChatResponse
            {
                Message = "Thông tin liên hệ khách sạn: 📞\n\n🏨 **Khách sạn ABC Hotel**\n📍 123 Đường ABC, Quận 1, TP.HCM\n\n📞 **Hotline 24/7:**\n• Đặt phòng: (028) 1234-5678\n• Hỗ trợ khách hàng: (028) 8765-4321\n• Khẩn cấp: (028) 9999-0000\n\n📧 **Email:**\n• Tổng đài: info@abchotel.com\n• Đặt phòng: booking@abchotel.com\n• Khiếu nại: complaint@abchotel.com\n\n🌐 **Mạng xã hội:**\n• Facebook: /ABCHotelOfficial\n• Instagram: @abchotel\n• Website: www.abchotel.com\n\n⏰ **Giờ làm việc:**\n• Lễ tân: 24/7\n• Văn phòng: 8:00 - 22:00\n\nBạn muốn liên hệ về vấn đề gì?",
                Intent = "contact",
                Suggestions = new List<string>
                {
                    "Gọi đặt phòng",
                    "Email hỗ trợ",
                    "Chat với nhân viên",
                    "Khiếu nại dịch vụ",
                    "Địa chỉ khách sạn"
                }
            };
        }

        private async Task<AIChatResponse> HandleGreetingIntent(string message, int? userId)
        {
            return await GetWelcomeMessageAsync(userId);
        }

        private async Task<AIChatResponse> HandleHelpIntent(string message, int? userId)
        {
            return new AIChatResponse
            {
                Message = "Tôi có thể giúp bạn những gì sau: 🤝\n\n🏨 **Về khách sạn:**\n• Thông tin phòng và giá cả\n• Tiện ích và dịch vụ\n• Địa chỉ và liên hệ\n\n📅 **Đặt phòng:**\n• Kiểm tra phòng trống\n• Hướng dẫn đặt phòng\n• Thay đổi/hủy đặt phòng\n\n💳 **Thanh toán:**\n• Phương thức thanh toán\n• Chính sách hoàn tiền\n• Hóa đơn và biên lai\n\n🌟 **Khách hàng thân thiết:**\n• Tích điểm và ưu đãi\n• Nâng hạng thành viên\n• Quà tặng đặc biệt\n\n❓ **Khác:**\n• Câu hỏi thường gặp\n• Khiếu nại và góp ý\n• Liên hệ nhân viên\n\nBạn cần hỗ trợ về vấn đề nào?",
                Intent = "help",
                Suggestions = new List<string>
                {
                    "Hướng dẫn đặt phòng",
                    "Câu hỏi thường gặp",
                    "Liên hệ nhân viên",
                    "Khiếu nại dịch vụ",
                    "Ưu đãi hiện tại"
                }
            };
        }

        private async Task<AIChatResponse> SearchKnowledgeBaseAsync(string message, int? userId)
        {
            var bestMatch = _knowledgeBase
                .Select(kb => new {
                    Item = kb,
                    Score = CalculateMatchScore(message, kb.Keywords)
                })
                .Where(x => x.Score > 0.3)
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            if (bestMatch != null)
            {
                return new AIChatResponse
                {
                    Message = bestMatch.Item.Answer,
                    Intent = "knowledge",
                    Confidence = bestMatch.Score,
                    Suggestions = bestMatch.Item.RelatedQuestions
                };
            }

            // Fallback response
            return new AIChatResponse
            {
                Message = "Xin lỗi, tôi chưa hiểu rõ câu hỏi của bạn. 🤔\n\nBạn có thể:\n• Hỏi lại bằng cách khác\n• Chọn một trong các gợi ý bên dưới\n• Liên hệ trực tiếp với nhân viên hỗ trợ\n\nTôi luôn sẵn sàng học hỏi để phục vụ bạn tốt hơn! 😊",
                Intent = "fallback",
                Confidence = 0.5f,
                Suggestions = new List<string>
                {
                    "Tôi muốn đặt phòng",
                    "Giá phòng như thế nào?",
                    "Khách sạn ở đâu?",
                    "Liên hệ nhân viên",
                    "Xem menu trợ giúp"
                }
            };
        }

        private float CalculateMatchScore(string message, List<string> keywords)
        {
            if (string.IsNullOrWhiteSpace(message) || !keywords.Any())
                return 0f;

            var messageWords = message.ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var matchCount = 0;

            foreach (var keyword in keywords)
            {
                if (messageWords.Any(word => word.Contains(keyword.ToLowerInvariant()) ||
                                           keyword.ToLowerInvariant().Contains(word)))
                {
                    matchCount++;
                }
            }

            return (float)matchCount / keywords.Count;
        }

        private List<AIKnowledgeItem> InitializeKnowledgeBase()
        {
            return new List<AIKnowledgeItem>
            {
                new AIKnowledgeItem
                {
                    Question = "Khách sạn có WiFi miễn phí không?",
                    Answer = "Có! Chúng tôi cung cấp WiFi miễn phí tốc độ cao trong tất cả các phòng và khu vực công cộng. 📶",
                    Keywords = new List<string> { "wifi", "internet", "miễn phí", "mạng" },
                    RelatedQuestions = new List<string> { "Tốc độ WiFi như thế nào?", "Có giới hạn dung lượng không?", "WiFi có ổn định không?" }
                },
                new AIKnowledgeItem
                {
                    Question = "Giờ check-in và check-out là mấy giờ?",
                    Answer = "⏰ Giờ check-in: 14:00\n⏰ Giờ check-out: 12:00\n\nBạn có thể yêu cầu check-in sớm hoặc check-out muộn (có thể phát sinh phí).",
                    Keywords = new List<string> { "check-in", "check-out", "giờ", "thời gian" },
                    RelatedQuestions = new List<string> { "Check-in sớm có được không?", "Phí check-out muộn bao nhiêu?", "Có thể gửi hành lý không?" }
                },
                new AIKnowledgeItem
                {
                    Question = "Khách sạn có chỗ đậu xe không?",
                    Answer = "Có! Chúng tôi có:\n🚗 Bãi đậu xe miễn phí\n🏍️ Chỗ để xe máy\n🚐 Chỗ cho xe lớn\n\nBảo vệ 24/7 và có camera an ninh.",
                    Keywords = new List<string> { "đậu xe", "parking", "xe", "bãi xe" },
                    RelatedQuestions = new List<string> { "Có valet parking không?", "Bãi xe có an toàn không?", "Phí đậu xe bao nhiêu?" }
                },
                new AIKnowledgeItem
                {
                    Question = "Có cho phép mang thú cưng không?",
                    Answer = "Rất tiếc, hiện tại chúng tôi chưa cho phép mang thú cưng. 🐕\n\nTuy nhiên, chúng tôi có thể giới thiệu dịch vụ giữ thú cưng uy tín gần khách sạn.",
                    Keywords = new List<string> { "thú cưng", "pet", "chó", "mèo", "động vật" },
                    RelatedQuestions = new List<string> { "Dịch vụ giữ thú cưng ở đâu?", "Có ngoại lệ nào không?", "Chính sách có thay đổi không?" }
                },
                new AIKnowledgeItem
                {
                    Question = "Khách sạn có nhà hàng không?",
                    Answer = "Có! Chúng tôi có:\n🍽️ Nhà hàng chính (6:00-22:00)\n☕ Café & Bar (24/7)\n🍕 Room service (24/7)\n🥐 Buffet sáng (6:00-10:00)\n\nMenu đa dạng từ Á đến Âu.",
                    Keywords = new List<string> { "nhà hàng", "restaurant", "ăn", "thức ăn", "buffet" },
                    RelatedQuestions = new List<string> { "Menu có gì?", "Giá buffet sáng?", "Có món chay không?", "Đặt bàn như thế nào?" }
                }
            };
        }
    }

    // Supporting classes
    public class AIChatResponse
    {
        public string Message { get; set; } = "";
        public string Intent { get; set; } = "";
        public float Confidence { get; set; }
        public List<string> Suggestions { get; set; } = new();
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    public class AIKnowledgeItem
    {
        public string Question { get; set; } = "";
        public string Answer { get; set; } = "";
        public List<string> Keywords { get; set; } = new();
        public List<string> RelatedQuestions { get; set; } = new();
    }
}
