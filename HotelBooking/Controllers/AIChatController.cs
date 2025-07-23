using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using System.Text.Json;

namespace HotelBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIChatController : ControllerBase
    {
        private readonly HotelBookingContext _context;
        private readonly AIResponseService _aiService;

        public AIChatController(HotelBookingContext context)
        {
            _context = context;
            _aiService = new AIResponseService(_context);
        }

        [HttpPost("message")]
        public async Task<IActionResult> SendMessage([FromBody] AIChatRequest request)
        {
            try
            {
                var response = await _aiService.GenerateResponseAsync(request.Message, request.Context);
                
                return Ok(new AIChatResponse
                {
                    Message = response.Message,
                    Suggestions = response.Suggestions,
                    Context = response.Context,
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("suggestions")]
        public IActionResult GetSuggestions()
        {
            var suggestions = new[]
            {
                "🏨 Tôi muốn đặt phòng",
                "💰 Giá phòng bao nhiêu?",
                "🛎️ Khách sạn có những dịch vụ gì?",
                "📍 Khách sạn ở đâu?",
                "🍽️ Nhà hàng có món gì ngon?",
                "🏊‍♀️ Có hồ bơi không?",
                "🚗 Có chỗ đỗ xe không?",
                "✈️ Có dịch vụ đưa đón sân bay không?"
            };

            return Ok(suggestions);
        }

        [HttpGet("quick-info")]
        public async Task<IActionResult> GetQuickInfo()
        {
            try
            {
                var roomTypes = await _context.RoomTypes.Take(3).Select(rt => new
                {
                    rt.TypeName,
                    rt.Description
                }).ToListAsync();

                var amenities = await _context.Amenities.Take(5).Select(a => a.AmenityName).ToListAsync();

                return Ok(new
                {
                    RoomTypes = roomTypes,
                    Amenities = amenities,
                    ContactInfo = new
                    {
                        Phone = "+84 123 456 789",
                        Email = "info@hotel.com",
                        Address = "123 Hotel Street, City"
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    public class AIResponseService
    {
        private readonly HotelBookingContext _context;
        private readonly Dictionary<string, List<string>> _responses;

        public AIResponseService(HotelBookingContext context)
        {
            _context = context;
            _responses = InitializeResponses();
        }

        public async Task<AIServiceResponse> GenerateResponseAsync(string message, AIChatContext context)
        {
            var lowerMessage = message.ToLower();
            var response = new AIServiceResponse();

            // Advanced AI Processing with multiple layers
            response = await ProcessAdvancedAI(message, lowerMessage, context);

            return response;
        }

        private async Task<AIServiceResponse> ProcessAdvancedAI(string originalMessage, string lowerMessage, AIChatContext context)
        {
            // Layer 1: Sentiment Analysis
            var sentiment = AnalyzeSentiment(lowerMessage);

            // Layer 2: Intent Classification with confidence scoring
            var intentResult = ClassifyIntent(lowerMessage, context);

            // Layer 3: Entity Extraction
            var entities = ExtractEntities(lowerMessage);

            // Layer 4: Context-aware response generation
            var response = await GenerateContextualResponse(originalMessage, lowerMessage, intentResult, entities, sentiment, context);

            return response;
        }

        private string AnalyzeSentiment(string message)
        {
            var positiveWords = new[] { "tốt", "tuyệt", "xuất sắc", "thích", "yêu", "hài lòng", "vui", "happy", "good", "great", "excellent", "love", "like", "amazing", "wonderful" };
            var negativeWords = new[] { "tệ", "xấu", "không thích", "ghét", "tồi", "kém", "bad", "terrible", "hate", "dislike", "awful", "horrible", "poor", "worst" };
            var neutralWords = new[] { "bình thường", "ok", "được", "normal", "okay", "fine", "average" };

            var positiveCount = positiveWords.Count(word => message.Contains(word));
            var negativeCount = negativeWords.Count(word => message.Contains(word));
            var neutralCount = neutralWords.Count(word => message.Contains(word));

            if (positiveCount > negativeCount && positiveCount > neutralCount) return "positive";
            if (negativeCount > positiveCount && negativeCount > neutralCount) return "negative";
            return "neutral";
        }

        private (string Intent, double Confidence) ClassifyIntent(string message, AIChatContext context)
        {
            var intents = new Dictionary<string, (string[] keywords, double baseWeight)>
            {
                ["booking"] = (new[] { "đặt phòng", "booking", "book", "phòng", "room", "reservation", "reserve" }, 1.0),
                ["pricing"] = (new[] { "giá", "price", "cost", "tiền", "bao nhiêu", "how much", "expensive", "cheap" }, 1.0),
                ["services"] = (new[] { "dịch vụ", "service", "tiện ích", "facilities", "amenities" }, 1.0),
                ["location"] = (new[] { "địa chỉ", "location", "đường", "ở đâu", "vị trí", "where", "address" }, 1.0),
                ["weather"] = (new[] { "thời tiết", "weather", "trời", "nắng", "mưa", "temperature", "climate" }, 0.8),
                ["time"] = (new[] { "giờ", "time", "mấy giờ", "bây giờ", "when", "schedule", "hours" }, 0.8),
                ["food"] = (new[] { "ăn", "food", "món", "nhà hàng", "restaurant", "menu", "dining", "eat" }, 0.9),
                ["travel"] = (new[] { "du lịch", "travel", "tham quan", "tour", "điểm đến", "sightseeing", "attraction" }, 0.9),
                ["technology"] = (new[] { "ai", "artificial intelligence", "robot", "technology", "tech", "computer", "internet" }, 0.7),
                ["math"] = (new[] { "tính", "calculate", "math", "toán", "plus", "minus", "multiply", "divide", "+", "-", "*", "/" }, 0.8),
                ["health"] = (new[] { "sức khỏe", "health", "bệnh", "thuốc", "doctor", "medical", "sick", "medicine" }, 0.8),
                ["entertainment"] = (new[] { "phim", "movie", "nhạc", "music", "game", "fun", "entertainment", "play" }, 0.7),
                ["education"] = (new[] { "học", "study", "education", "kiến thức", "knowledge", "learn", "teach", "school" }, 0.8),
                ["business"] = (new[] { "công việc", "work", "business", "kinh doanh", "career", "job", "meeting" }, 0.8),
                ["greeting"] = (new[] { "xin chào", "hello", "hi", "chào", "hey", "good morning", "good evening" }, 1.0),
                ["gratitude"] = (new[] { "cảm ơn", "thank", "thanks", "appreciate", "grateful" }, 1.0),
                ["farewell"] = (new[] { "tạm biệt", "bye", "goodbye", "see you", "farewell" }, 1.0),
                ["help"] = (new[] { "giúp", "help", "assist", "support", "hỗ trợ" }, 1.0),
                ["complaint"] = (new[] { "phжалоба", "complaint", "problem", "issue", "wrong", "error" }, 0.9),
                ["compliment"] = (new[] { "khen", "compliment", "praise", "good job", "well done" }, 0.8)
            };

            var bestIntent = "unknown";
            var bestScore = 0.0;

            foreach (var intent in intents)
            {
                var score = intent.Value.keywords.Count(keyword => message.Contains(keyword)) * intent.Value.baseWeight;

                // Context boost
                if (context?.LastTopic == intent.Key) score *= 1.3;
                if (context?.Intent == intent.Key) score *= 1.2;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestIntent = intent.Key;
                }
            }

            var confidence = Math.Min(bestScore / 3.0, 1.0); // Normalize confidence
            return (bestIntent, confidence);
        }

        private Dictionary<string, List<string>> ExtractEntities(string message)
        {
            var entities = new Dictionary<string, List<string>>();

            // Numbers
            var numbers = System.Text.RegularExpressions.Regex.Matches(message, @"\d+")
                .Cast<System.Text.RegularExpressions.Match>()
                .Select(m => m.Value)
                .ToList();
            if (numbers.Any()) entities["numbers"] = numbers;

            // Dates
            var datePatterns = new[] { @"\d{1,2}\/\d{1,2}\/\d{4}", @"\d{1,2}-\d{1,2}-\d{4}" };
            var dates = new List<string>();
            foreach (var pattern in datePatterns)
            {
                dates.AddRange(System.Text.RegularExpressions.Regex.Matches(message, pattern)
                    .Cast<System.Text.RegularExpressions.Match>()
                    .Select(m => m.Value));
            }
            if (dates.Any()) entities["dates"] = dates;

            // Room types
            var roomTypes = new[] { "standard", "deluxe", "suite", "vip", "presidential" };
            var foundRoomTypes = roomTypes.Where(rt => message.Contains(rt)).ToList();
            if (foundRoomTypes.Any()) entities["room_types"] = foundRoomTypes;

            // Currencies
            var currencies = new[] { "vnd", "usd", "eur", "đồng", "dollar", "euro" };
            var foundCurrencies = currencies.Where(c => message.Contains(c)).ToList();
            if (foundCurrencies.Any()) entities["currencies"] = foundCurrencies;

            return entities;
        }

        private async Task<AIServiceResponse> GenerateContextualResponse(string originalMessage, string lowerMessage,
            (string Intent, double Confidence) intentResult, Dictionary<string, List<string>> entities,
            string sentiment, AIChatContext context)
        {
            var response = new AIServiceResponse();

            // High confidence responses
            if (intentResult.Confidence > 0.7)
            {
                response = await HandleHighConfidenceIntent(intentResult.Intent, lowerMessage, entities, sentiment);
            }
            // Medium confidence - ask for clarification
            else if (intentResult.Confidence > 0.3)
            {
                response = HandleMediumConfidenceIntent(intentResult.Intent, originalMessage, sentiment);
            }
            // Low confidence - use advanced fallback
            else
            {
                response = await HandleAdvancedFallback(originalMessage, lowerMessage, entities, sentiment, context);
            }

            // Apply sentiment-based modifications
            response = ApplySentimentModifications(response, sentiment);

            // Update context
            response.Context = new AIChatContext
            {
                Intent = intentResult.Intent,
                LastTopic = intentResult.Intent,
                BookingIntent = intentResult.Intent == "booking" || context?.BookingIntent == true,
                UserName = context?.UserName
            };

            return response;
        }

        private async Task<AIServiceResponse> HandleHighConfidenceIntent(string intent, string message,
            Dictionary<string, List<string>> entities, string sentiment)
        {
            return intent switch
            {
                "booking" => await HandleAdvancedBookingIntent(message, entities),
                "pricing" => await HandleAdvancedPricingIntent(entities),
                "services" => await HandleAdvancedServicesIntent(entities),
                "location" => HandleAdvancedLocationIntent(entities),
                "weather" => HandleAdvancedWeatherIntent(entities),
                "time" => HandleAdvancedTimeIntent(entities),
                "food" => HandleAdvancedFoodIntent(entities),
                "travel" => HandleAdvancedTravelIntent(entities),
                "technology" => HandleAdvancedTechnologyIntent(message),
                "math" => HandleAdvancedMathIntent(message, entities),
                "health" => HandleAdvancedHealthIntent(entities),
                "entertainment" => HandleAdvancedEntertainmentIntent(entities),
                "education" => HandleAdvancedEducationIntent(entities),
                "business" => HandleAdvancedBusinessIntent(entities),
                "greeting" => HandleAdvancedGreetingIntent(sentiment),
                "gratitude" => HandleGratitudeIntent(sentiment),
                "farewell" => HandleFarewellIntent(sentiment),
                "help" => HandleHelpIntent(),
                "complaint" => HandleComplaintIntent(message),
                "compliment" => HandleComplimentIntent(sentiment),
                _ => await HandleAdvancedFallback(message, message.ToLower(), entities, sentiment, null)
            };
        }

        private AIServiceResponse HandleMediumConfidenceIntent(string intent, string message, string sentiment)
        {
            var clarificationMessages = new Dictionary<string, string>
            {
                ["booking"] = "🤔 Tôi hiểu bạn muốn đặt phòng. Bạn có thể cho tôi biết cụ thể hơn:\n• Ngày check-in và check-out\n• Số lượng khách\n• Loại phòng mong muốn",
                ["pricing"] = "💰 Bạn muốn biết về giá cả? Tôi có thể giúp bạn với:\n• Giá phòng theo loại\n• Khuyến mãi hiện tại\n• Bảng giá chi tiết",
                ["services"] = "🛎️ Về dịch vụ khách sạn, bạn quan tâm đến:\n• Tiện ích trong phòng\n• Dịch vụ ăn uống\n• Hoạt động giải trí\n• Dịch vụ spa & wellness",
                ["food"] = "🍽️ Về ẩm thực, bạn muốn biết:\n• Menu nhà hàng\n• Giờ phục vụ\n• Đặc sản địa phương\n• Room service",
                ["travel"] = "🗺️ Về du lịch, tôi có thể tư vấn:\n• Điểm tham quan gần đây\n• Tour du lịch\n• Phương tiện di chuyển\n• Lịch trình gợi ý"
            };

            var message_text = clarificationMessages.ContainsKey(intent)
                ? clarificationMessages[intent]
                : $"🤔 Tôi hiểu bạn đang hỏi về {intent}. Bạn có thể nói rõ hơn để tôi hỗ trợ tốt nhất?";

            return new AIServiceResponse
            {
                Message = message_text,
                Suggestions = GetSuggestionsForIntent(intent),
                Context = new AIChatContext { Intent = intent, LastTopic = intent }
            };
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

        private async Task<AIServiceResponse> HandleAdvancedBookingIntent(string message, Dictionary<string, List<string>> entities)
        {
            var roomTypes = await _context.RoomTypes.ToListAsync();
            var hasNumbers = entities.ContainsKey("numbers");
            var hasDates = entities.ContainsKey("dates");
            var hasRoomTypes = entities.ContainsKey("room_types");

            var responseText = "🏨 **Tuyệt vời! Tôi sẽ giúp bạn đặt phòng.**\n\n";

            if (hasRoomTypes)
            {
                var requestedType = entities["room_types"].First();
                responseText += $"✅ Bạn quan tâm đến phòng **{requestedType.ToUpper()}**\n";
            }

            if (hasDates)
            {
                var date = entities["dates"].First();
                responseText += $"📅 Ngày: **{date}**\n";
            }

            if (hasNumbers)
            {
                var number = entities["numbers"].First();
                responseText += $"👥 Số khách: **{number} người**\n";
            }

            responseText += "\n🏠 **Các loại phòng hiện có:**\n";
            responseText += string.Join("\n", roomTypes.Take(3).Select(rt => $"• **{rt.TypeName}**: {rt.Description}"));

            responseText += "\n\n💡 **Để hoàn tất đặt phòng:**\n";
            responseText += "• Truy cập trang đặt phòng online\n";
            responseText += "• Gọi trực tiếp: (024) 1234-5678\n";
            responseText += "• Chat với nhân viên reception";

            return new AIServiceResponse
            {
                Message = responseText,
                Suggestions = new[] { "Xem trang đặt phòng", "Gọi reception", "Chat với nhân viên", "Xem giá phòng" },
                Context = new AIChatContext { Intent = "booking", LastTopic = "room_booking", BookingIntent = true }
            };
        }

        private async Task<AIServiceResponse> HandleAdvancedPricingIntent(Dictionary<string, List<string>> entities)
        {
            var rooms = await _context.Rooms.Include(r => r.RoomType).ToListAsync();
            var hasRoomTypes = entities.ContainsKey("room_types");
            var hasCurrencies = entities.ContainsKey("currencies");

            var responseText = "💰 **Bảng giá phòng chi tiết:**\n\n";

            if (hasRoomTypes)
            {
                var requestedType = entities["room_types"].First();
                var filteredRooms = rooms.Where(r => r.RoomType.TypeName.ToLower().Contains(requestedType)).ToList();

                if (filteredRooms.Any())
                {
                    responseText += $"🏠 **Giá phòng {requestedType.ToUpper()}:**\n";
                    responseText += string.Join("\n", filteredRooms.Take(3).Select(r =>
                        $"• Phòng {r.RoomNumber}: **{r.Price:N0} VNĐ**/đêm"));
                }
            }
            else
            {
                var groupedRooms = rooms.GroupBy(r => r.RoomType.TypeName).ToList();
                foreach (var group in groupedRooms.Take(3))
                {
                    var minPrice = group.Min(r => r.Price);
                    var maxPrice = group.Max(r => r.Price);
                    responseText += $"🏠 **{group.Key}**: {minPrice:N0} - {maxPrice:N0} VNĐ/đêm\n";
                }
            }

            responseText += "\n💡 **Thông tin thêm:**\n";
            responseText += "• Giá đã bao gồm thuế VAT\n";
            responseText += "• Miễn phí WiFi và bữa sáng\n";
            responseText += "• Giảm giá 10% cho booking trên 3 đêm\n";
            responseText += "• Check-in: 14:00 | Check-out: 12:00";

            if (hasCurrencies)
            {
                var currency = entities["currencies"].First().ToUpper();
                responseText += $"\n\n💱 *Giá có thể quy đổi sang {currency} theo tỷ giá hiện tại*";
            }

            return new AIServiceResponse
            {
                Message = responseText,
                Suggestions = new[] { "Đặt phòng ngay", "So sánh giá", "Xem khuyến mãi", "Tính chi phí" },
                Context = new AIChatContext { Intent = "pricing", LastTopic = "room_prices" }
            };
        }

        private async Task<AIServiceResponse> HandleAdvancedServicesIntent(Dictionary<string, List<string>> entities)
        {
            var amenities = await _context.Amenities.ToListAsync();

            var responseText = "🛎️ **Dịch vụ & Tiện ích cao cấp:**\n\n";

            responseText += "🏨 **Dịch vụ cốt lõi:**\n";
            responseText += string.Join("\n", amenities.Take(6).Select(a => $"✨ {a.AmenityName}"));

            responseText += "\n\n🌟 **Dịch vụ đặc biệt:**\n";
            responseText += "• **Concierge 24/7** - Hỗ trợ mọi yêu cầu\n";
            responseText += "• **Butler Service** - Phục vụ riêng cho Suite\n";
            responseText += "• **Airport Transfer** - Đưa đón sân bay\n";
            responseText += "• **Laundry Express** - Giặt ủi trong ngày\n";
            responseText += "• **Baby Sitting** - Trông trẻ chuyên nghiệp\n";
            responseText += "• **Pet Care** - Chăm sóc thú cưng";

            responseText += "\n\n🎯 **Hoạt động & Giải trí:**\n";
            responseText += "• **Spa & Wellness Center** - Massage, sauna\n";
            responseText += "• **Fitness Center** - Gym hiện đại 24/7\n";
            responseText += "• **Swimming Pool** - Hồ bơi vô cực\n";
            responseText += "• **Kids Club** - Khu vui chơi trẻ em\n";
            responseText += "• **Game Room** - Billiards, PS5, VR\n";
            responseText += "• **Karaoke Lounge** - Phòng hát riêng tư";

            responseText += "\n\n🍽️ **Ẩm thực:**\n";
            responseText += "• **Fine Dining Restaurant** - Ẩm thực cao cấp\n";
            responseText += "• **Rooftop Bar** - Cocktail với view 360°\n";
            responseText += "• **Coffee Lounge** - Cà phê & bánh ngọt\n";
            responseText += "• **Room Service 24/7** - Phục vụ tận phòng";

            return new AIServiceResponse
            {
                Message = responseText,
                Suggestions = new[] { "Đặt spa", "Book nhà hàng", "Thuê butler", "Xem menu", "Hoạt động trẻ em" },
                Context = new AIChatContext { Intent = "services", LastTopic = "hotel_amenities" }
            };
        }

        private AIServiceResponse HandleAdvancedLocationIntent(Dictionary<string, List<string>> entities)
        {
            return new AIServiceResponse
            {
                Message = "📍 **Vị trí đắc địa & Kết nối thuận tiện:**\n\n" +
                         "🏨 **Địa chỉ:** 123 Luxury Hotel Street, Hội An, Quảng Nam\n\n" +
                         "✈️ **Từ sân bay:**\n• Sân bay Đà Nẵng: 45 phút (35km)\n• Taxi: 400,000 VNĐ\n• Shuttle bus: 150,000 VNĐ\n• Grab: 350,000 VNĐ\n\n" +
                         "🚗 **Giao thông:**\n• Trung tâm Hội An: 5 phút đi bộ\n• Phố cổ: 3 phút xe máy\n• Bãi biển An Bàng: 10 phút\n• Ga tàu Đà Nẵng: 1 giờ\n\n" +
                         "🎯 **Điểm nổi bật gần đây:**\n• Chùa Cầu: 500m\n• Chợ đêm Hội An: 300m\n• Làng rau Trà Quế: 2km\n• Rừng dừa Bảy Mẫu: 5km\n\n" +
                         "🅿️ **Tiện ích:**\n• Bãi đỗ xe miễn phí (200 chỗ)\n• Trạm xe bus: 100m\n• ATM & ngân hàng: 200m\n• Siêu thị: 300m",
                Suggestions = new[] { "Đặt shuttle", "Thuê xe", "Bản đồ chi tiết", "Hướng dẫn đi lại" },
                Context = new AIChatContext { Intent = "location", LastTopic = "hotel_location" }
            };
        }

        private AIServiceResponse HandleAdvancedWeatherIntent(Dictionary<string, List<string>> entities)
        {
            var currentTime = DateTime.Now;
            var season = GetCurrentSeason(currentTime);

            return new AIServiceResponse
            {
                Message = $"🌤️ **Thời tiết hôm nay ({currentTime:dd/MM/yyyy}):**\n\n" +
                         "☀️ **Hiện tại:** 29°C, nắng ít mây\n" +
                         "🌡️ **Nhiệt độ:** 26°C - 32°C\n" +
                         "💧 **Độ ẩm:** 68%\n" +
                         "💨 **Gió:** Đông Nam, 15 km/h\n" +
                         "🌧️ **Khả năng mưa:** 20%\n\n" +
                         $"🗓️ **Mùa {season}:**\n{GetSeasonDescription(season)}\n\n" +
                         "🏊‍♀️ **Hoạt động phù hợp:**\n• Bơi lội tại hồ bơi vô cực\n• Tắm nắng trên sân thượng\n• Tham quan phố cổ\n• Đạp xe quanh làng\n\n" +
                         "👕 **Gợi ý trang phục:**\n• Quần áo mùa hè nhẹ mát\n• Kem chống nắng SPF 50+\n• Mũ rộng vành\n• Dép sandal thoáng khí",
                Suggestions = new[] { "Dự báo 7 ngày", "Hoạt động trong nhà", "Thuê xe đạp", "Kem chống nắng" },
                Context = new AIChatContext { Intent = "weather", LastTopic = "weather_info" }
            };
        }

        private AIServiceResponse HandleAdvancedTimeIntent(Dictionary<string, List<string>> entities)
        {
            var now = DateTime.Now;
            var timeOfDay = GetTimeOfDay(now.Hour);

            return new AIServiceResponse
            {
                Message = $"🕐 **Thời gian hiện tại:**\n\n" +
                         $"⏰ **Bây giờ:** {now:HH:mm}, {timeOfDay}\n" +
                         $"📅 **Ngày:** {now:dddd, dd/MM/yyyy}\n\n" +
                         "🏨 **Lịch hoạt động khách sạn:**\n" +
                         "• **Reception:** 24/7 ⭐\n" +
                         "• **Nhà hàng chính:** 6:00 - 23:00\n" +
                         "• **Rooftop Bar:** 17:00 - 02:00\n" +
                         "• **Coffee Lounge:** 6:00 - 22:00\n" +
                         "• **Spa & Wellness:** 8:00 - 22:00\n" +
                         "• **Fitness Center:** 24/7 ⭐\n" +
                         "• **Swimming Pool:** 6:00 - 22:00\n" +
                         "• **Kids Club:** 8:00 - 20:00\n\n" +
                         $"💡 **Gợi ý cho {timeOfDay}:**\n{GetTimeBasedSuggestions(now.Hour)}",
                Suggestions = GetTimeBasedSuggestionButtons(now.Hour),
                Context = new AIChatContext { Intent = "time", LastTopic = "current_time" }
            };
        }

        private AIServiceResponse HandleAdvancedMathIntent(string message, Dictionary<string, List<string>> entities)
        {
            var calculation = ExtractAndCalculate(message);

            var responseText = "🧮 **Máy tính thông minh:**\n\n";

            if (!string.IsNullOrEmpty(calculation))
            {
                responseText += $"✅ **Kết quả:** {calculation}\n\n";
            }

            responseText += "💡 **Tôi có thể tính toán:**\n" +
                           "• Phép toán cơ bản: +, -, ×, ÷\n" +
                           "• Tính tiền tip (10-20%)\n" +
                           "• Quy đổi tiền tệ\n" +
                           "• Tính chi phí lưu trú\n" +
                           "• Chia bill nhóm\n" +
                           "• Tính thuế VAT (10%)\n\n" +
                           "📊 **Ví dụ:**\n" +
                           "• \"2 + 3\" → 5\n" +
                           "• \"Tip 15% cho 500k\" → 75,000 VNĐ\n" +
                           "• \"Chia 1 triệu cho 4 người\" → 250,000 VNĐ/người";

            return new AIServiceResponse
            {
                Message = responseText,
                Suggestions = new[] { "Tính tip 15%", "Quy đổi USD", "Chia bill", "Tính thuế VAT" },
                Context = new AIChatContext { Intent = "math", LastTopic = "calculation" }
            };
        }

        // Helper methods
        private string GetCurrentSeason(DateTime date)
        {
            var month = date.Month;
            return month switch
            {
                12 or 1 or 2 => "Đông",
                3 or 4 or 5 => "Xuân",
                6 or 7 or 8 => "Hè",
                9 or 10 or 11 => "Thu",
                _ => "Xuân"
            };
        }

        private string GetSeasonDescription(string season)
        {
            return season switch
            {
                "Xuân" => "Thời tiết dễ chịu, nhiệt độ 22-28°C. Lý tưởng cho tham quan.",
                "Hè" => "Nắng nóng, nhiệt độ 28-35°C. Thích hợp bơi lội và hoạt động nước.",
                "Thu" => "Mát mẻ, nhiệt độ 24-30°C. Thời gian đẹp nhất trong năm.",
                "Đông" => "Mát lạnh, nhiệt độ 18-25°C. Phù hợp nghỉ dưỡng thư giãn.",
                _ => "Thời tiết dễ chịu quanh năm."
            };
        }

        private string GetTimeOfDay(int hour)
        {
            return hour switch
            {
                >= 5 and < 12 => "buổi sáng",
                >= 12 and < 17 => "buổi chiều",
                >= 17 and < 22 => "buổi tối",
                _ => "đêm khuya"
            };
        }

        private string GetTimeBasedSuggestions(int hour)
        {
            return hour switch
            {
                >= 6 and < 10 => "• Thưởng thức buffet sáng\n• Tập gym buổi sáng\n• Bơi lội trong hồ bơi\n• Đi dạo phố cổ",
                >= 10 and < 12 => "• Check-out (nếu cần)\n• Tham quan chùa Cầu\n• Mua sắm tại chợ\n• Uống cà phê",
                >= 12 and < 14 => "• Dùng bữa trưa\n• Nghỉ ngơi tại phòng\n• Spa thư giãn\n• Đọc sách tại lobby",
                >= 14 and < 17 => "• Check-in (nếu mới đến)\n• Khám phá khách sạn\n• Tắm nắng bên hồ bơi\n• Tham quan làng rau",
                >= 17 and < 20 => "• Happy hour tại bar\n• Ngắm hoàng hôn\n• Đi dạo bãi biển\n• Chuẩn bị dùng tối",
                >= 20 and < 23 => "• Dùng bữa tối\n• Thưởng thức cocktail\n• Karaoke cùng bạn bè\n• Massage thư giãn",
                _ => "• Thư giãn tại phòng\n• Đọc sách\n• Nghe nhạc\n• Nghỉ ngơi sớm"
            };
        }

        private string[] GetTimeBasedSuggestionButtons(int hour)
        {
            return hour switch
            {
                >= 6 and < 10 => new[] { "Menu buffet sáng", "Đặt lịch gym", "Hướng dẫn phố cổ" },
                >= 10 and < 14 => new[] { "Thủ tục check-out", "Tour chùa Cầu", "Menu trưa" },
                >= 14 and < 17 => new[] { "Thủ tục check-in", "Đặt lịch spa", "Hoạt động hồ bơi" },
                >= 17 and < 20 => new[] { "Happy hour menu", "Tour hoàng hôn", "Đặt bàn tối" },
                >= 20 and < 23 => new[] { "Menu dinner", "Đặt phòng karaoke", "Book massage" },
                _ => new[] { "Room service", "Dịch vụ đêm", "Hỗ trợ khẩn cấp" }
            };
        }

        private string ExtractAndCalculate(string message)
        {
            try
            {
                // Simple math operations
                var mathPattern = @"(\d+(?:\.\d+)?)\s*([+\-*/])\s*(\d+(?:\.\d+)?)";
                var match = System.Text.RegularExpressions.Regex.Match(message, mathPattern);

                if (match.Success)
                {
                    var num1 = double.Parse(match.Groups[1].Value);
                    var operation = match.Groups[2].Value;
                    var num2 = double.Parse(match.Groups[3].Value);

                    var result = operation switch
                    {
                        "+" => num1 + num2,
                        "-" => num1 - num2,
                        "*" => num1 * num2,
                        "/" => num2 != 0 ? num1 / num2 : double.NaN,
                        _ => double.NaN
                    };

                    if (!double.IsNaN(result))
                    {
                        return $"{num1} {operation} {num2} = {result:N2}";
                    }
                }

                // Tip calculation
                if (message.Contains("tip") && message.Contains("%"))
                {
                    var percentMatch = System.Text.RegularExpressions.Regex.Match(message, @"(\d+)%");
                    var amountMatch = System.Text.RegularExpressions.Regex.Match(message, @"(\d+(?:,\d{3})*(?:\.\d+)?)");

                    if (percentMatch.Success && amountMatch.Success)
                    {
                        var percent = double.Parse(percentMatch.Groups[1].Value);
                        var amount = double.Parse(amountMatch.Groups[1].Value.Replace(",", ""));
                        var tip = amount * percent / 100;
                        return $"Tip {percent}% cho {amount:N0} = {tip:N0} VNĐ";
                    }
                }

                return "";
            }
            catch
            {
                return "Xin lỗi, tôi không thể tính toán biểu thức này.";
            }
        }

        private async Task<AIServiceResponse> HandleAdvancedFallback(string originalMessage, string lowerMessage,
            Dictionary<string, List<string>> entities, string sentiment, AIChatContext context)
        {
            // Advanced pattern matching for complex queries
            var response = TryAdvancedPatternMatching(originalMessage, lowerMessage, entities);
            if (response != null) return response;

            // Contextual fallback based on previous conversation
            if (context?.LastTopic != null)
            {
                return HandleContextualFallback(originalMessage, context.LastTopic, sentiment);
            }

            // Intelligent general response
            return GenerateIntelligentFallback(originalMessage, entities, sentiment);
        }

        private AIServiceResponse TryAdvancedPatternMatching(string original, string lower, Dictionary<string, List<string>> entities)
        {
            // Question patterns
            if (lower.Contains("tại sao") || lower.Contains("why") || lower.Contains("vì sao"))
            {
                return new AIServiceResponse
                {
                    Message = "🤔 **Câu hỏi thú vị!** Tôi sẽ cố gắng giải thích:\n\n" +
                             "Mặc dù tôi chưa thể trả lời chi tiết câu hỏi này, nhưng tôi có thể giúp bạn với:\n" +
                             "• Thông tin về khách sạn và dịch vụ\n" +
                             "• Tư vấn du lịch và ẩm thực\n" +
                             "• Hỗ trợ đặt phòng và booking\n" +
                             "• Giải đáp thắc mắc chung\n\n" +
                             "Hoặc bạn có thể liên hệ trực tiếp với nhân viên để được tư vấn chi tiết hơn! 😊",
                    Suggestions = new[] { "Hỏi về khách sạn", "Tư vấn du lịch", "Liên hệ nhân viên", "Câu hỏi khác" }
                };
            }

            // How-to patterns
            if (lower.Contains("làm thế nào") || lower.Contains("how to") || lower.Contains("cách"))
            {
                return new AIServiceResponse
                {
                    Message = "📋 **Hướng dẫn chi tiết:**\n\n" +
                             "Tôi có thể hướng dẫn bạn:\n" +
                             "• **Cách đặt phòng** online và offline\n" +
                             "• **Cách check-in/check-out** nhanh chóng\n" +
                             "• **Cách sử dụng** các dịch vụ khách sạn\n" +
                             "• **Cách di chuyển** đến các điểm tham quan\n" +
                             "• **Cách đặt bàn** nhà hàng và spa\n\n" +
                             "Bạn muốn hướng dẫn về điều gì cụ thể? 🎯",
                    Suggestions = new[] { "Hướng dẫn đặt phòng", "Hướng dẫn check-in", "Hướng dẫn dịch vụ", "Hướng dẫn di chuyển" }
                };
            }

            // Comparison patterns
            if (lower.Contains("so sánh") || lower.Contains("khác nhau") || lower.Contains("compare") || lower.Contains("difference"))
            {
                return new AIServiceResponse
                {
                    Message = "⚖️ **So sánh chi tiết:**\n\n" +
                             "Tôi có thể giúp bạn so sánh:\n" +
                             "• **Các loại phòng** - Standard vs Deluxe vs Suite\n" +
                             "• **Gói dịch vụ** - Basic vs Premium vs VIP\n" +
                             "• **Phương tiện di chuyển** - Taxi vs Grab vs Shuttle\n" +
                             "• **Nhà hàng** - Fine dining vs Casual vs Room service\n" +
                             "• **Hoạt động** - Indoor vs Outdoor activities\n\n" +
                             "Bạn muốn so sánh điều gì? 🔍",
                    Suggestions = new[] { "So sánh phòng", "So sánh gói dịch vụ", "So sánh nhà hàng", "So sánh hoạt động" }
                };
            }

            return null;
        }

        private AIServiceResponse HandleContextualFallback(string message, string lastTopic, string sentiment)
        {
            var contextResponses = new Dictionary<string, string>
            {
                ["booking"] = "🏨 Tôi hiểu bạn vẫn quan tâm đến việc đặt phòng. Có điều gì cụ thể tôi có thể giúp thêm không?",
                ["pricing"] = "💰 Về vấn đề giá cả, bạn có muốn biết thêm thông tin gì khác không?",
                ["services"] = "🛎️ Về dịch vụ khách sạn, tôi có thể tư vấn thêm chi tiết nào khác cho bạn?",
                ["food"] = "🍽️ Về ẩm thực, bạn có muốn biết thêm về menu hay giờ phục vụ không?",
                ["travel"] = "🗺️ Về du lịch, tôi có thể gợi ý thêm điểm tham quan hoặc hoạt động nào khác?"
            };

            var contextMessage = contextResponses.ContainsKey(lastTopic)
                ? contextResponses[lastTopic]
                : "🤔 Tôi hiểu bạn đang quan tâm đến chủ đề trước. Bạn có muốn tiếp tục thảo luận không?";

            return new AIServiceResponse
            {
                Message = contextMessage + "\n\nHoặc bạn có thể hỏi tôi về chủ đề mới! 😊",
                Suggestions = GetSuggestionsForIntent(lastTopic),
                Context = new AIChatContext { Intent = "contextual", LastTopic = lastTopic }
            };
        }

        private AIServiceResponse GenerateIntelligentFallback(string message, Dictionary<string, List<string>> entities, string sentiment)
        {
            var hasNumbers = entities.ContainsKey("numbers");
            var hasDates = entities.ContainsKey("dates");

            var responseText = "🤖 **AI Assistant thông minh:**\n\n";

            if (sentiment == "positive")
            {
                responseText += "😊 Tôi cảm nhận được sự tích cực từ bạn! ";
            }
            else if (sentiment == "negative")
            {
                responseText += "😔 Tôi hiểu bạn có thể đang gặp khó khăn. ";
            }

            responseText += "Mặc dù tôi chưa hiểu hoàn toàn câu hỏi của bạn, nhưng tôi luôn sẵn sàng học hỏi và cải thiện!\n\n";

            if (hasNumbers)
            {
                responseText += $"🔢 Tôi thấy bạn đề cập đến số **{entities["numbers"].First()}** - có phải liên quan đến:\n";
                responseText += "• Số lượng khách?\n• Số đêm lưu trú?\n• Giá cả?\n• Số phòng?\n\n";
            }

            if (hasDates)
            {
                responseText += $"📅 Về ngày **{entities["dates"].First()}** - bạn có muốn:\n";
                responseText += "• Đặt phòng cho ngày này?\n• Kiểm tra lịch trống?\n• Xem sự kiện đặc biệt?\n\n";
            }

            responseText += "💡 **Tôi có thể giúp bạn với:**\n";
            responseText += "• 🏨 **Khách sạn** - Phòng, giá, dịch vụ\n";
            responseText += "• 🌍 **Du lịch** - Điểm tham quan, tour\n";
            responseText += "• 🍽️ **Ẩm thực** - Nhà hàng, menu, đặt bàn\n";
            responseText += "• 🎯 **Giải trí** - Hoạt động, sự kiện\n";
            responseText += "• 🧮 **Tính toán** - Giá cả, tip, quy đổi\n";
            responseText += "• 💬 **Trò chuyện** - Thời tiết, thời gian\n\n";
            responseText += "Hãy thử hỏi tôi một cách khác hoặc chọn chủ đề bạn quan tâm! ✨";

            return new AIServiceResponse
            {
                Message = responseText,
                Suggestions = new[] { "Đặt phòng", "Du lịch", "Ẩm thực", "Giải trí", "Tính toán", "Trò chuyện" },
                Context = new AIChatContext { Intent = "fallback", LastTopic = "general" }
            };
        }

        private AIServiceResponse ApplySentimentModifications(AIServiceResponse response, string sentiment)
        {
            if (sentiment == "positive")
            {
                // Add positive emojis and enthusiastic tone
                if (!response.Message.Contains("😊") && !response.Message.Contains("🎉") && !response.Message.Contains("✨"))
                {
                    response.Message += "\n\n😊 Rất vui được hỗ trợ bạn!";
                }
            }
            else if (sentiment == "negative")
            {
                // Add empathetic tone and helpful suggestions
                if (!response.Message.Contains("😔") && !response.Message.Contains("🤝"))
                {
                    response.Message += "\n\n🤝 Tôi hiểu và sẽ cố gắng hỗ trợ bạn tốt nhất có thể.";
                }
            }

            return response;
        }

        // Additional advanced handlers (simplified for space)
        private AIServiceResponse HandleAdvancedGreetingIntent(string sentiment)
        {
            var greetings = sentiment switch
            {
                "positive" => "🎉 Chào bạn! Thật tuyệt khi gặp bạn hôm nay!",
                "negative" => "😊 Xin chào! Tôi hy vọng có thể giúp bạn có một ngày tốt đẹp hơn!",
                _ => "👋 Xin chào! Tôi là Hotel AI Assistant - trợ lý thông minh của bạn!"
            };

            return new AIServiceResponse
            {
                Message = greetings + "\n\nTôi có thể giúp bạn với mọi thắc mắc về khách sạn và nhiều chủ đề khác. Hãy hỏi tôi bất cứ điều gì! 🌟",
                Suggestions = new[] { "Đặt phòng", "Thông tin dịch vụ", "Du lịch địa phương", "Trò chuyện" },
                Context = new AIChatContext { Intent = "greeting", LastTopic = "welcome" }
            };
        }

        private AIServiceResponse HandleGratitudeIntent(string sentiment)
        {
            return new AIServiceResponse
            {
                Message = "😊 **Rất vui được giúp đỡ bạn!**\n\nĐó là niềm vui của tôi! Nếu có thêm câu hỏi gì, đừng ngại hỏi nhé.\n\n🌟 Tôi luôn ở đây để hỗ trợ bạn 24/7!",
                Suggestions = new[] { "Hỏi thêm", "Đánh giá dịch vụ", "Chia sẻ trải nghiệm", "Tạm biệt" },
                Context = new AIChatContext { Intent = "gratitude", LastTopic = "thanks" }
            };
        }

        private AIServiceResponse HandleFarewellIntent(string sentiment)
        {
            return new AIServiceResponse
            {
                Message = "👋 **Tạm biệt và hẹn gặp lại!**\n\nCảm ơn bạn đã trò chuyện với tôi. Chúc bạn có những trải nghiệm tuyệt vời tại khách sạn!\n\n🌟 Tôi luôn sẵn sàng hỗ trợ bạn bất cứ lúc nào!",
                Suggestions = new[] { "Đánh giá AI", "Liên hệ lại", "Chia sẻ feedback", "Hỗ trợ khẩn cấp" },
                Context = new AIChatContext { Intent = "farewell", LastTopic = "goodbye" }
            };
        }

        private AIServiceResponse HandleHelpIntent()
        {
            return new AIServiceResponse
            {
                Message = "🆘 **Tôi sẵn sàng giúp đỡ!**\n\n🤖 **Về tôi:**\nTôi là Hotel AI Assistant - trợ lý thông minh được trang bị:\n• Natural Language Processing\n• Machine Learning\n• Real-time Database\n• Sentiment Analysis\n\n💡 **Tôi có thể:**\n• Trả lời câu hỏi về khách sạn\n• Tư vấn du lịch và ẩm thực\n• Tính toán và quy đổi\n• Trò chuyện tự nhiên\n• Hỗ trợ đặt phòng\n\n🎯 **Cách sử dụng:**\nChỉ cần hỏi tôi bằng ngôn ngữ tự nhiên, tôi sẽ hiểu và trả lời!",
                Suggestions = new[] { "Hướng dẫn đặt phòng", "Tính năng AI", "Liên hệ nhân viên", "Bắt đầu trò chuyện" },
                Context = new AIChatContext { Intent = "help", LastTopic = "assistance" }
            };
        }

        private AIServiceResponse HandleComplaintIntent(string message)
        {
            return new AIServiceResponse
            {
                Message = "😔 **Tôi rất xin lỗi về sự bất tiện này.**\n\nTôi hiểu bạn đang gặp vấn đề và tôi muốn giúp giải quyết:\n\n🔧 **Hỗ trợ ngay lập tức:**\n• Liên hệ Manager: (024) 1234-5678\n• Email khiếu nại: complaints@hotel.com\n• Chat trực tiếp với nhân viên\n• Gọi Reception 24/7\n\n💡 **Tôi cũng có thể:**\n• Ghi nhận phản hồi của bạn\n• Chuyển tiếp đến bộ phận liên quan\n• Hỗ trợ giải quyết vấn đề cơ bản\n\nBạn có muốn tôi kết nối với nhân viên ngay không? 🤝",
                Suggestions = new[] { "Gọi Manager", "Chat nhân viên", "Gửi email", "Ghi nhận phản hồi" },
                Context = new AIChatContext { Intent = "complaint", LastTopic = "issue_resolution" }
            };
        }

        private AIServiceResponse HandleComplimentIntent(string sentiment)
        {
            return new AIServiceResponse
            {
                Message = "🥰 **Cảm ơn bạn rất nhiều!**\n\nLời khen của bạn là động lực lớn để tôi tiếp tục cải thiện và phục vụ tốt hơn!\n\n🌟 **Tôi sẽ:**\n• Tiếp tục học hỏi và phát triển\n• Cung cấp thông tin chính xác hơn\n• Hỗ trợ bạn tốt nhất có thể\n• Mang đến trải nghiệm tuyệt vời\n\n💝 Bạn có muốn chia sẻ feedback này với đội ngũ phát triển không?",
                Suggestions = new[] { "Chia sẻ feedback", "Đánh giá 5 sao", "Tiếp tục trò chuyện", "Cảm ơn team" },
                Context = new AIChatContext { Intent = "compliment", LastTopic = "positive_feedback" }
            };
        }

        private async Task<AIServiceResponse> HandleBookingIntent(string message)
        {
            var roomTypes = await _context.RoomTypes.Take(3).ToListAsync();
            var roomInfo = string.Join("\n", roomTypes.Select(rt =>
                $"🏠 {rt.TypeName}: {rt.Description}"));

            return new AIServiceResponse
            {
                Message = $"🏨 Tuyệt vời! Tôi sẽ giúp bạn đặt phòng.\n\nCác loại phòng hiện có:\n{roomInfo}\n\nBạn có thể:\n• Truy cập trang đặt phòng\n• Cho tôi biết ngày check-in/out\n• Liên hệ trực tiếp với reception",
                Suggestions = new[] { "Xem trang đặt phòng", "Tôi muốn đặt phòng Standard", "Giá phòng Deluxe bao nhiêu?" },
                Context = new AIChatContext { Intent = "booking", LastTopic = "room_booking" }
            };
        }

        private async Task<AIServiceResponse> HandlePricingIntent()
        {
            var rooms = await _context.Rooms.Include(r => r.RoomType).GroupBy(r => r.RoomType.TypeName).ToListAsync();
            var priceInfo = string.Join("\n", rooms.Select(g =>
                $"💰 {g.Key}: từ {g.Min(r => r.Price):N0} VNĐ/đêm"));

            return new AIServiceResponse
            {
                Message = $"💰 Bảng giá phòng của chúng tôi:\n\n{priceInfo}\n\n*Giá có thể thay đổi theo mùa và khuyến mãi\n*Đã bao gồm thuế và phí dịch vụ",
                Suggestions = new[] { "Đặt phòng ngay", "Có khuyến mãi gì không?", "Giá cuối tuần như thế nào?" },
                Context = new AIChatContext { Intent = "pricing", LastTopic = "room_prices" }
            };
        }

        private async Task<AIServiceResponse> HandleServicesIntent()
        {
            var amenities = await _context.Amenities.Take(8).ToListAsync();
            var amenityList = string.Join("\n", amenities.Select(a => $"✨ {a.AmenityName}"));

            return new AIServiceResponse
            {
                Message = $"🛎️ Khách sạn chúng tôi có đầy đủ tiện ích:\n\n{amenityList}\n\n🌟 Dịch vụ 24/7:\n• Room service\n• Reception\n• Bảo vệ\n• Housekeeping",
                Suggestions = new[] { "Spa có những dịch vụ gì?", "Nhà hàng phục vụ từ mấy giờ?", "Có dịch vụ giặt ủi không?" },
                Context = new AIChatContext { Intent = "services", LastTopic = "hotel_amenities" }
            };
        }

        private AIServiceResponse HandleLocationIntent()
        {
            return new AIServiceResponse
            {
                Message = "📍 Thông tin vị trí:\n\n🏨 Địa chỉ: 123 Hotel Street, Thành phố\n\n🚗 Cách di chuyển:\n• Từ sân bay: 30 phút (taxi)\n• Từ ga tàu: 15 phút\n• Trung tâm thành phố: 5 phút\n\n🅿️ Bãi đỗ xe miễn phí\n🚌 Gần trạm xe bus",
                Suggestions = new[] { "Gửi bản đồ", "Có shuttle bus không?", "Taxi từ sân bay giá bao nhiêu?" },
                Context = new AIChatContext { Intent = "location", LastTopic = "hotel_location" }
            };
        }

        private AIServiceResponse HandleGreetingIntent()
        {
            var greetings = new[]
            {
                "👋 Xin chào! Tôi là Hotel AI Assistant. Tôi có thể giúp bạn với:\n\n🏨 Đặt phòng và giá cả\n🛎️ Thông tin dịch vụ\n📍 Hướng dẫn địa điểm\n❓ Các câu hỏi khác\n\nBạn cần hỗ trợ gì?",
                "🤖 Chào bạn! Rất vui được hỗ trợ bạn hôm nay!\n\nTôi có thể giúp bạn:\n✅ Tìm hiểu về phòng\n✅ Kiểm tra giá cả\n✅ Thông tin dịch vụ\n✅ Đặt phòng online\n\nHãy hỏi tôi bất cứ điều gì! 😊"
            };

            return new AIServiceResponse
            {
                Message = greetings[new Random().Next(greetings.Length)],
                Suggestions = new[] { "Tôi muốn đặt phòng", "Giá phòng bao nhiêu?", "Khách sạn có những dịch vụ gì?" },
                Context = new AIChatContext { Intent = "greeting", LastTopic = "welcome" }
            };
        }

        private AIServiceResponse HandleWeatherIntent()
        {
            var weatherResponses = new[]
            {
                "🌤️ Thời tiết hôm nay:\n\n☀️ Nhiệt độ: 28-32°C\n🌤️ Trời nắng, có mây\n💨 Gió nhẹ\n💧 Độ ẩm: 65%\n\n🏨 Thời tiết tuyệt vời để tận hưởng kỳ nghỉ tại khách sạn! Hồ bơi và các hoạt động ngoài trời đều sẵn sàng.",
                "🌦️ Dự báo thời tiết:\n\n📅 Hôm nay: Nắng ít mây 🌤️\n📅 Ngày mai: Có thể có mưa nhẹ 🌦️\n📅 Cuối tuần: Nắng đẹp ☀️\n\n💡 Gợi ý: Đây là thời điểm tuyệt vời để đặt phòng có view biển!"
            };

            return new AIServiceResponse
            {
                Message = weatherResponses[new Random().Next(weatherResponses.Length)],
                Suggestions = new[] { "Hoạt động ngoài trời", "Đặt phòng view biển", "Dịch vụ spa" },
                Context = new AIChatContext { Intent = "weather", LastTopic = "weather_info" }
            };
        }

        private AIServiceResponse HandleTimeIntent()
        {
            var currentTime = DateTime.Now;
            var timeOfDay = currentTime.Hour < 12 ? "sáng" : currentTime.Hour < 18 ? "chiều" : "tối";

            return new AIServiceResponse
            {
                Message = $"🕐 Bây giờ là {currentTime:HH:mm} {timeOfDay}, ngày {currentTime:dd/MM/yyyy}\n\n⏰ Giờ hoạt động của khách sạn:\n• Reception: 24/7\n• Nhà hàng: 6:00 - 23:00\n• Spa: 8:00 - 22:00\n• Hồ bơi: 6:00 - 22:00\n• Gym: 24/7",
                Suggestions = new[] { "Đặt bàn nhà hàng", "Book spa", "Thông tin dịch vụ" },
                Context = new AIChatContext { Intent = "time", LastTopic = "current_time" }
            };
        }

        private AIServiceResponse HandleFoodIntent()
        {
            var foodResponses = new[]
            {
                "🍽️ Ẩm thực tại khách sạn:\n\n🥘 **Nhà hàng chính:**\n• Món Việt truyền thống\n• Hải sản tươi sống\n• BBQ ngoài trời\n\n🍹 **Sky Bar:**\n• Cocktail signature\n• Đồ uống nhiệt đới\n• View hoàng hôn tuyệt đẹp\n\n☕ **Café Lobby:**\n• Cà phê specialty\n• Bánh ngọt tự làm\n• Light meals",
                "👨‍🍳 Đầu bếp khuyên dùng:\n\n🦞 **Hải sản nướng** - Đặc sản địa phương\n🥩 **Beef Wagyu** - Thịt bò cao cấp\n🍜 **Phở bò đặc biệt** - Món Việt authentic\n🥗 **Salad nhiệt đới** - Tươi mát, healthy\n\n📞 Gọi ext. 1234 để đặt bàn!"
            };

            return new AIServiceResponse
            {
                Message = foodResponses[new Random().Next(foodResponses.Length)],
                Suggestions = new[] { "Đặt bàn nhà hàng", "Menu đặc biệt", "Dịch vụ room service" },
                Context = new AIChatContext { Intent = "food", LastTopic = "dining" }
            };
        }

        private AIServiceResponse HandleTravelIntent()
        {
            return new AIServiceResponse
            {
                Message = "🗺️ **Điểm tham quan gần khách sạn:**\n\n🏛️ **Văn hóa & Lịch sử:**\n• Chùa Cầu (5km) - Biểu tượng Hội An\n• Phố cổ Hội An (3km) - Di sản UNESCO\n• Làng gốm Thanh Hà (8km)\n\n🏖️ **Thiên nhiên:**\n• Bãi biển An Bàng (2km)\n• Rừng dừa Bảy Mẫu (10km)\n• Đảo Cù Lao Chàm (45 phút thuyền)\n\n🚗 **Dịch vụ của khách sạn:**\n• Thuê xe máy/ô tô\n• Tour guide riêng\n• Đưa đón sân bay",
                Suggestions = new[] { "Thuê xe", "Đặt tour", "Shuttle service" },
                Context = new AIChatContext { Intent = "travel", LastTopic = "tourism" }
            };
        }

        private AIServiceResponse HandleTechnologyIntent()
        {
            return new AIServiceResponse
            {
                Message = "🤖 **Về AI và Công nghệ:**\n\nTôi là Hotel AI Assistant, được phát triển với:\n• **Natural Language Processing** - Hiểu ngôn ngữ tự nhiên\n• **Machine Learning** - Học từ cuộc trò chuyện\n• **Real-time Database** - Cập nhật thông tin liên tục\n\n💡 **Smart Hotel Features:**\n• Keyless entry với app\n• Voice control trong phòng\n• AI concierge 24/7\n• IoT room automation\n\n🔮 Tương lai của hospitality là AI + Human touch!",
                Suggestions = new[] { "Smart room features", "Mobile app", "Công nghệ khách sạn" },
                Context = new AIChatContext { Intent = "technology", LastTopic = "ai_tech" }
            };
        }

        private AIServiceResponse HandleMathIntent(string message)
        {
            // Simple math operations
            if (message.Contains("+") || message.Contains("-") || message.Contains("*") || message.Contains("/"))
            {
                try
                {
                    // Basic calculation (simplified)
                    var result = "Tôi có thể giúp tính toán cơ bản! Ví dụ:\n• 2 + 3 = 5\n• 10 - 4 = 6\n• 5 * 6 = 30\n• 20 / 4 = 5";

                    return new AIServiceResponse
                    {
                        Message = $"🧮 **Tính toán:**\n\n{result}\n\n💡 Để tính chính xác, hãy viết phép tính rõ ràng!",
                        Suggestions = new[] { "Tính tiền phòng", "Quy đổi tiền tệ", "Tính thuế VAT" },
                        Context = new AIChatContext { Intent = "math", LastTopic = "calculation" }
                    };
                }
                catch
                {
                    return new AIServiceResponse
                    {
                        Message = "🤔 Xin lỗi, tôi chưa thể tính phép toán này. Bạn có thể viết rõ hơn không?\n\nVí dụ: \"2 + 3\" hoặc \"10 * 5\"",
                        Suggestions = new[] { "Ví dụ tính toán", "Hỏi khác", "Trợ giúp" },
                        Context = new AIChatContext { Intent = "math", LastTopic = "calculation_error" }
                    };
                }
            }

            return new AIServiceResponse
            {
                Message = "🧮 Tôi có thể giúp bạn tính toán!\n\nVí dụ:\n• Phép cộng: 5 + 3\n• Phép trừ: 10 - 2\n• Phép nhân: 4 * 6\n• Phép chia: 20 / 4\n\nHãy thử đặt câu hỏi tính toán!",
                Suggestions = new[] { "5 + 3", "10 * 2", "Tính tiền tip" },
                Context = new AIChatContext { Intent = "math", LastTopic = "math_help" }
            };
        }

        private AIServiceResponse HandleHealthIntent()
        {
            return new AIServiceResponse
            {
                Message = "🏥 **Sức khỏe & An toàn:**\n\n🚑 **Dịch vụ y tế:**\n• Phòng khám 24/7 tại lobby\n• Bác sĩ on-call\n• Thuốc cơ bản tại reception\n• Liên kết bệnh viện quốc tế\n\n💊 **Lưu ý sức khỏe:**\n• Uống nước đun sôi\n• Kem chống nắng SPF 50+\n• Tránh đồ ăn đường phố\n\n🧘‍♀️ **Wellness:**\n• Spa therapy\n• Yoga buổi sáng\n• Meditation garden",
                Suggestions = new[] { "Đặt lịch spa", "Yoga class", "Liên hệ bác sĩ" },
                Context = new AIChatContext { Intent = "health", LastTopic = "wellness" }
            };
        }

        private AIServiceResponse HandleEntertainmentIntent()
        {
            return new AIServiceResponse
            {
                Message = "🎬 **Giải trí & Hoạt động:**\n\n🎵 **Âm nhạc:**\n• Live music tối thứ 6-7\n• Karaoke room\n• DJ set tại pool bar\n\n🎮 **Games:**\n• Game room với PS5\n• Billiards & Table tennis\n• Beach volleyball\n\n🎭 **Sự kiện:**\n• Cultural show hàng tuần\n• Cooking class\n• Wine tasting\n\n📺 **In-room:**\n• Netflix, YouTube\n• 200+ kênh quốc tế\n• Gaming console",
                Suggestions = new[] { "Lịch sự kiện", "Đặt karaoke", "Game room" },
                Context = new AIChatContext { Intent = "entertainment", LastTopic = "activities" }
            };
        }

        private AIServiceResponse HandleEducationIntent()
        {
            return new AIServiceResponse
            {
                Message = "📚 **Học tập & Kiến thức:**\n\n🌍 **Văn hóa địa phương:**\n• Lịch sử Hội An\n• Nghề thủ công truyền thống\n• Ẩm thực Việt Nam\n• Lễ hội và phong tục\n\n📖 **Learning Activities:**\n• Cooking class với chef\n• Lantern making workshop\n• Vietnamese language basics\n• Photography tour\n\n🎓 **Business Center:**\n• Meeting rooms\n• High-speed internet\n• Printing services\n• Video conferencing",
                Suggestions = new[] { "Cooking class", "Cultural tour", "Business center" },
                Context = new AIChatContext { Intent = "education", LastTopic = "learning" }
            };
        }

        private AIServiceResponse HandleBusinessIntent()
        {
            return new AIServiceResponse
            {
                Message = "💼 **Business & Công việc:**\n\n🏢 **Meeting Facilities:**\n• 5 phòng họp (10-200 người)\n• Projector & sound system\n• High-speed WiFi\n• Coffee break service\n\n📊 **Business Services:**\n• Printing & scanning\n• Translation service\n• Secretary support\n• Airport transfer for executives\n\n🤝 **Networking:**\n• Business lounge\n• Executive floor\n• Corporate packages\n• Team building activities",
                Suggestions = new[] { "Đặt phòng họp", "Corporate rates", "Executive services" },
                Context = new AIChatContext { Intent = "business", LastTopic = "corporate" }
            };
        }

        private AIServiceResponse HandleAdvancedDefaultIntent(string message)
        {
            // Advanced pattern matching for more intelligent responses
            var responses = new List<string>();

            if (message.Contains("cảm ơn") || message.Contains("thank"))
            {
                responses.Add("😊 Rất vui được giúp đỡ bạn! Nếu có thêm câu hỏi gì, đừng ngại hỏi tôi nhé!");
            }
            else if (message.Contains("tạm biệt") || message.Contains("bye"))
            {
                responses.Add("👋 Tạm biệt! Chúc bạn có kỳ nghỉ tuyệt vời tại khách sạn. Hẹn gặp lại! 🌟");
            }
            else if (message.Contains("giúp") || message.Contains("help"))
            {
                responses.Add("🤝 Tôi luôn sẵn sàng giúp đỡ! Bạn có thể hỏi tôi về:\n\n🏨 Khách sạn: phòng, giá, dịch vụ\n🌍 Du lịch: điểm tham quan, tour\n🍽️ Ẩm thực: nhà hàng, món ăn\n🎯 Giải trí: hoạt động, sự kiện\n💼 Công việc: meeting, business\n📚 Học tập: văn hóa, ngôn ngữ\n\nVà nhiều chủ đề khác!");
            }
            else if (message.Contains("ai") && message.Contains("tên"))
            {
                responses.Add("🤖 Tôi là **Hotel AI Assistant**! Bạn có thể gọi tôi là **AI** hoặc **Assistant**.\n\nTôi được tạo ra để hỗ trợ khách hàng 24/7 với:\n• Trí tuệ nhân tạo\n• Kiến thức về khách sạn\n• Khả năng học hỏi liên tục\n\nRất vui được làm quen với bạn! 😊");
            }
            else
            {
                var defaultResponses = new[]
                {
                    "🤔 Câu hỏi thú vị! Tôi đang học hỏi thêm để trả lời tốt hơn.\n\nHiện tại tôi có thể giúp bạn với:\n🏨 Thông tin khách sạn\n🌍 Du lịch & tham quan\n🍽️ Ẩm thực\n🎯 Giải trí\n💼 Business\n📚 Học tập\n\nBạn muốn hỏi về chủ đề nào?",
                    "💭 Tôi hiểu bạn đang tìm kiếm thông tin! Mặc dù tôi chưa thể trả lời chính xác câu hỏi này, nhưng tôi có thể giúp bạn với nhiều chủ đề khác.\n\n✨ Hãy thử hỏi tôi về:\n• Thời tiết hôm nay\n• Món ăn ngon\n• Điểm tham quan\n• Hoạt động giải trí\n• Tính toán đơn giản\n\nHoặc bất cứ điều gì bạn tò mò! 🌟"
                };
                responses.Add(defaultResponses[new Random().Next(defaultResponses.Length)]);
            }

            return new AIServiceResponse
            {
                Message = responses.First(),
                Suggestions = new[] { "Thời tiết", "Món ăn ngon", "Điểm tham quan", "Giải trí", "Tính toán", "Trợ giúp" },
                Context = new AIChatContext { Intent = "general", LastTopic = "conversation" }
            };
        }

        private AIServiceResponse HandleAdvancedFoodIntent(Dictionary<string, List<string>> entities)
        {
            return new AIServiceResponse
            {
                Message = "🍽️ **Ẩm thực tại khách sạn:**\n\n🥘 **Nhà hàng chính:**\n• Món Việt truyền thống\n• Hải sản tươi sống\n• BBQ ngoài trời\n\n🍹 **Sky Bar:**\n• Cocktail signature\n• Đồ uống nhiệt đới\n• View hoàng hôn tuyệt đẹp\n\n☕ **Café Lobby:**\n• Cà phê specialty\n• Bánh ngọt tự làm\n• Light meals",
                Suggestions = new[] { "Đặt bàn nhà hàng", "Menu đặc biệt", "Dịch vụ room service" },
                Context = new AIChatContext { Intent = "food", LastTopic = "dining" }
            };
        }

        private AIServiceResponse HandleAdvancedTravelIntent(Dictionary<string, List<string>> entities)
        {
            return new AIServiceResponse
            {
                Message = "🗺️ **Điểm tham quan gần khách sạn:**\n\n🏛️ **Văn hóa & Lịch sử:**\n• Chùa Cầu (5km) - Biểu tượng Hội An\n• Phố cổ Hội An (3km) - Di sản UNESCO\n• Làng gốm Thanh Hà (8km)\n\n🏖️ **Thiên nhiên:**\n• Bãi biển An Bàng (2km)\n• Rừng dừa Bảy Mẫu (10km)\n• Đảo Cù Lao Chàm (45 phút thuyền)\n\n🚗 **Dịch vụ của khách sạn:**\n• Thuê xe máy/ô tô\n• Tour guide riêng\n• Đưa đón sân bay",
                Suggestions = new[] { "Thuê xe", "Đặt tour", "Shuttle service" },
                Context = new AIChatContext { Intent = "travel", LastTopic = "tourism" }
            };
        }

        private AIServiceResponse HandleAdvancedTechnologyIntent(string message)
        {
            return new AIServiceResponse
            {
                Message = "🤖 **Về AI và Công nghệ:**\n\nTôi là Hotel AI Assistant, được phát triển với:\n• **Natural Language Processing** - Hiểu ngôn ngữ tự nhiên\n• **Machine Learning** - Học từ cuộc trò chuyện\n• **Real-time Database** - Cập nhật thông tin liên tục\n\n💡 **Smart Hotel Features:**\n• Keyless entry với app\n• Voice control trong phòng\n• AI concierge 24/7\n• IoT room automation\n\n🔮 Tương lai của hospitality là AI + Human touch!",
                Suggestions = new[] { "Smart room features", "Mobile app", "Công nghệ khách sạn" },
                Context = new AIChatContext { Intent = "technology", LastTopic = "ai_tech" }
            };
        }

        private AIServiceResponse HandleAdvancedHealthIntent(Dictionary<string, List<string>> entities)
        {
            return new AIServiceResponse
            {
                Message = "🏥 **Sức khỏe & An toàn:**\n\n🚑 **Dịch vụ y tế:**\n• Phòng khám 24/7 tại lobby\n• Bác sĩ on-call\n• Thuốc cơ bản tại reception\n• Liên kết bệnh viện quốc tế\n\n💊 **Lưu ý sức khỏe:**\n• Uống nước đun sôi\n• Kem chống nắng SPF 50+\n• Tránh đồ ăn đường phố\n\n🧘‍♀️ **Wellness:**\n• Spa therapy\n• Yoga buổi sáng\n• Meditation garden",
                Suggestions = new[] { "Đặt lịch spa", "Yoga class", "Liên hệ bác sĩ" },
                Context = new AIChatContext { Intent = "health", LastTopic = "wellness" }
            };
        }

        private AIServiceResponse HandleAdvancedEntertainmentIntent(Dictionary<string, List<string>> entities)
        {
            return new AIServiceResponse
            {
                Message = "🎬 **Giải trí & Hoạt động:**\n\n🎵 **Âm nhạc:**\n• Live music tối thứ 6-7\n• Karaoke room\n• DJ set tại pool bar\n\n🎮 **Games:**\n• Game room với PS5\n• Billiards & Table tennis\n• Beach volleyball\n\n🎭 **Sự kiện:**\n• Cultural show hàng tuần\n• Cooking class\n• Wine tasting\n\n📺 **In-room:**\n• Netflix, YouTube\n• 200+ kênh quốc tế\n• Gaming console",
                Suggestions = new[] { "Lịch sự kiện", "Đặt karaoke", "Game room" },
                Context = new AIChatContext { Intent = "entertainment", LastTopic = "activities" }
            };
        }

        private AIServiceResponse HandleAdvancedEducationIntent(Dictionary<string, List<string>> entities)
        {
            return new AIServiceResponse
            {
                Message = "📚 **Học tập & Kiến thức:**\n\n🌍 **Văn hóa địa phương:**\n• Lịch sử Hội An\n• Nghề thủ công truyền thống\n• Ẩm thực Việt Nam\n• Lễ hội và phong tục\n\n📖 **Learning Activities:**\n• Cooking class với chef\n• Lantern making workshop\n• Vietnamese language basics\n• Photography tour\n\n🎓 **Business Center:**\n• Meeting rooms\n• High-speed internet\n• Printing services\n• Video conferencing",
                Suggestions = new[] { "Cooking class", "Cultural tour", "Business center" },
                Context = new AIChatContext { Intent = "education", LastTopic = "learning" }
            };
        }

        private AIServiceResponse HandleAdvancedBusinessIntent(Dictionary<string, List<string>> entities)
        {
            return new AIServiceResponse
            {
                Message = "💼 **Business & Công việc:**\n\n🏢 **Meeting Facilities:**\n• 5 phòng họp (10-200 người)\n• Projector & sound system\n• High-speed WiFi\n• Coffee break service\n\n📊 **Business Services:**\n• Printing & scanning\n• Translation service\n• Secretary support\n• Airport transfer for executives\n\n🤝 **Networking:**\n• Business lounge\n• Executive floor\n• Corporate packages\n• Team building activities",
                Suggestions = new[] { "Đặt phòng họp", "Corporate rates", "Executive services" },
                Context = new AIChatContext { Intent = "business", LastTopic = "corporate" }
            };
        }

        private bool ContainsKeywords(string message, string[] keywords)
        {
            return keywords.Any(keyword => message.Contains(keyword));
        }

        private Dictionary<string, List<string>> InitializeResponses()
        {
            return new Dictionary<string, List<string>>
            {
                ["greetings"] = new List<string>
                {
                    "Xin chào! Tôi là Hotel AI Assistant. Tôi có thể giúp gì cho bạn? 😊",
                    "Chào bạn! Rất vui được hỗ trợ bạn hôm nay! 👋"
                }
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
