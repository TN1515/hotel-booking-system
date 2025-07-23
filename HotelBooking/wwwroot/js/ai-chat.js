// AI Chat Bot JavaScript
class AIChatBot {
    constructor() {
        this.isOpen = false;
        this.isTyping = false;
        this.responses = this.initializeResponses();
        this.context = {
            userName: null,
            lastTopic: null,
            bookingIntent: false,
            conversationHistory: [],
            userPreferences: {},
            sessionStartTime: new Date()
        };
        this.messageCount = 0;
        this.lastMessageTime = null;
        this.typingSpeed = 50; // Characters per second
    }

    initializeResponses() {
        return {
            greetings: [
                "👋 **Xin chào! Tôi là Hotel AI Assistant!**\n\nTôi có thể giúp bạn với:\n• 🏨 Thông tin khách sạn & đặt phòng\n• 🌍 Du lịch & tham quan\n• 🍽️ Ẩm thực & nhà hàng\n• 🧮 Tính toán & quy đổi\n• 💬 Trò chuyện thân thiện\n\n**Bạn cần tôi hỗ trợ điều gì?** 😊",
                "🌟 **Chào mừng bạn đến với khách sạn!**\n\nTôi là AI Assistant thông minh, sẵn sàng hỗ trợ bạn 24/7!\n\n✨ **Hãy hỏi tôi về bất cứ điều gì:**\n• Thông tin phòng & dịch vụ\n• Điểm tham quan địa phương\n• Thời tiết & thời gian\n• Tính toán đơn giản\n• Và nhiều chủ đề khác!\n\n**Hôm nay tôi có thể giúp gì cho bạn?** 🤖",
                "🎉 **Hello! Rất vui được gặp bạn!**\n\nTôi là trợ lý AI thông minh của khách sạn, luôn sẵn sàng giúp đỡ!\n\n🚀 **Khả năng của tôi:**\n• Trả lời mọi câu hỏi về khách sạn\n• Tư vấn du lịch chuyên sâu\n• Hỗ trợ tính toán & quy đổi\n• Cung cấp thông tin thời tiết\n• Trò chuyện tự nhiên như con người\n\n**Bắt đầu cuộc trò chuyện nào!** ✨"
            ],
            
            booking: [
                "🏨 **Tuyệt vời! Tôi sẽ giúp bạn đặt phòng.**\n\n🏠 **Các loại phòng hiện có:**\n• Standard Room - 1,500,000 VNĐ/đêm\n• Deluxe Room - 2,200,000 VNĐ/đêm\n• Suite Room - 3,500,000 VNĐ/đêm\n• Presidential Suite - 8,000,000 VNĐ/đêm\n\n📋 **Thông tin cần thiết:**\n• Ngày check-in & check-out\n• Số lượng khách\n• Loại phòng mong muốn\n\n**Bạn có thể cho tôi biết chi tiết hơn không?**",
                "🛏️ **Rất vui được hỗ trợ bạn đặt phòng!**\n\n✨ **Ưu đãi đặc biệt:**\n• Đặt trước 7 ngày: Giảm 10%\n• Lưu trú từ 3 đêm: Giảm 15%\n• Honeymoon package: Giảm 20%\n\n🎯 **Gói dịch vụ:**\n• Basic: Chỉ phòng\n• Standard: Phòng + bữa sáng\n• Premium: All-inclusive\n\n**Bạn quan tâm đến gói nào?**",
                "📅 **Chúng tôi có phòng trống!**\n\n🔍 **Để tìm phòng phù hợp nhất:**\n1. Ngày lưu trú mong muốn\n2. Số người lớn & trẻ em\n3. Sở thích về view (biển/vườn)\n4. Ngân sách dự kiến\n5. Yêu cầu đặc biệt (nếu có)\n\n💡 **Tip:** Đặt phòng online được giảm thêm 5%!\n\n**Hãy chia sẻ thông tin để tôi tư vấn tốt nhất!**"
            ],

            pricing: [
                "💰 Giá phòng của chúng tôi phụ thuộc vào:\n• Loại phòng (Standard, Deluxe, Suite)\n• Thời gian lưu trú\n• Dịch vụ kèm theo\n\nGiá từ 500,000 VNĐ/đêm. Bạn muốn xem chi tiết giá phòng nào?",
                "Chúng tôi có nhiều mức giá phù hợp:\n🏠 Standard: 500,000 - 800,000 VNĐ\n🏨 Deluxe: 800,000 - 1,200,000 VNĐ\n🏰 Suite: 1,200,000 - 2,000,000 VNĐ\n\nGiá có thể thay đổi theo mùa. Bạn muốn kiểm tra giá cụ thể cho ngày nào?",
                "Để biết giá chính xác nhất, bạn có thể:\n• Chọn ngày cụ thể trên trang đặt phòng\n• Liên hệ trực tiếp với reception\n• Hoặc cho tôi biết ngày bạn muốn đặt! 💳"
            ],

            services: [
                "🛎️ Khách sạn chúng tôi có đầy đủ tiện ích:\n• Nhà hàng & Bar\n• Spa & Massage\n• Hồ bơi & Gym\n• Dịch vụ giặt ủi\n• WiFi miễn phí\n• Đưa đón sân bay\n\nBạn muốn biết chi tiết dịch vụ nào?",
                "Chúng tôi cung cấp:\n🍽️ Ẩm thực đa dạng\n🏊‍♀️ Khu vui chơi giải trí\n🚗 Bãi đỗ xe miễn phí\n🧳 Dịch vụ concierge 24/7\n🎯 Tổ chức sự kiện\n\nCó dịch vụ nào bạn quan tâm đặc biệt?",
                "Dịch vụ nổi bật:\n✨ Room service 24/7\n🌟 Housekeeping hàng ngày\n🎪 Kids club\n💼 Business center\n🚕 Tour du lịch\n\nTôi có thể tư vấn chi tiết hơn! 😊"
            ],

            location: [
                "📍 Khách sạn tọa lạc tại vị trí đắc địa:\n• Gần trung tâm thành phố\n• Dễ dàng di chuyển đến các điểm tham quan\n• Gần sân bay và ga tàu\n\nBạn cần hướng dẫn đường đi cụ thể không?",
                "Vị trí thuận lợi:\n🏙️ Trung tâm thành phố - 5 phút\n✈️ Sân bay - 30 phút\n🚂 Ga tàu - 15 phút\n🏖️ Bãi biển - 20 phút\n\nTôi có thể gửi bản đồ chi tiết cho bạn!",
                "Địa chỉ và cách di chuyển:\n📧 Địa chỉ: [Địa chỉ khách sạn]\n🚌 Xe bus: Tuyến số 15, 23\n🚕 Taxi: Khoảng 200,000 VNĐ từ sân bay\n🚗 Ô tô: Có bãi đỗ xe miễn phí"
            ],

            default: [
                "🤖 Tôi đang học hỏi để trả lời tốt hơn! Hiện tại tôi có thể giúp bạn:\n• 🏨 Khách sạn & đặt phòng\n• 🌤️ Thời tiết & thời gian\n• 🍽️ Ẩm thực & nhà hàng\n• 🗺️ Du lịch & tham quan\n• 🧮 Tính toán đơn giản\n• 💼 Business & công việc\n• 🎯 Giải trí & hoạt động\n• 📚 Học tập & văn hóa\n\nHãy thử hỏi tôi bất cứ điều gì! ✨",
                "💭 Câu hỏi thú vị! Tôi có thể không biết tất cả như ChatGPT, nhưng tôi đang cố gắng học hỏi.\n\n🌟 Tôi giỏi nhất về:\n✅ Thông tin khách sạn\n✅ Thời tiết địa phương\n✅ Ẩm thực & du lịch\n✅ Tính toán cơ bản\n✅ Trò chuyện thân thiện\n\nBạn muốn thử hỏi gì khác không? 😊",
                "🚀 Tôi luôn sẵn sàng hỗ trợ! Mặc dù chưa thông minh bằng ChatGPT, nhưng tôi có thể:\n\n🎯 Trả lời về khách sạn\n🌍 Thông tin du lịch\n🍜 Gợi ý ẩm thực\n⏰ Thời tiết & thời gian\n🧮 Tính toán đơn giản\n💬 Trò chuyện vui vẻ\n\nHãy thử hỏi tôi điều gì đó! 🤖✨"
            ]
        };
    }

    toggle() {
        if (this.isOpen) {
            this.close();
        } else {
            this.open();
        }
    }

    open() {
        document.getElementById('aiChatWindow').style.display = 'block';
        this.isOpen = true;
        
        // Focus input
        setTimeout(() => {
            document.getElementById('aiChatInput').focus();
        }, 300);
    }

    close() {
        document.getElementById('aiChatWindow').style.display = 'none';
        this.isOpen = false;
    }

    minimize() {
        this.close();
    }

    handleKeyPress(event) {
        if (event.key === 'Enter') {
            this.sendMessage();
        }
    }

    sendMessage() {
        const input = document.getElementById('aiChatInput');
        const message = input.value.trim();

        if (!message) return;

        // Update conversation tracking
        this.messageCount++;
        this.lastMessageTime = new Date();
        this.context.conversationHistory.push({
            type: 'user',
            message: message,
            timestamp: this.lastMessageTime
        });

        // Add user message with enhanced display
        this.addMessage(message, 'user');
        input.value = '';

        // Hide suggestions after first message
        if (this.messageCount === 1) {
            document.getElementById('aiSuggestions').style.display = 'none';
        }

        // Show intelligent typing indicator
        this.showIntelligentTyping(message);

        // Generate AI response with context
        this.getAdvancedAIResponse(message);
    }

    sendSuggestion(suggestion) {
        document.getElementById('aiChatInput').value = suggestion;
        this.sendMessage();
    }

    addMessage(content, sender) {
        const chatBody = document.getElementById('aiChatBody');
        const messageDiv = document.createElement('div');
        messageDiv.className = `ai-message ai-message-${sender}`;

        const time = new Date().toLocaleTimeString('vi-VN', { 
            hour: '2-digit', 
            minute: '2-digit' 
        });

        if (sender === 'bot') {
            messageDiv.innerHTML = `
                <div class="ai-message-avatar">
                    <div class="ai-gradient-circle-tiny">
                        <i class="fas fa-robot"></i>
                    </div>
                </div>
                <div class="ai-message-content">
                    <div class="ai-message-bubble">
                        ${this.formatMessage(content)}
                    </div>
                    <div class="ai-message-time">${time}</div>
                </div>
            `;
        } else {
            messageDiv.innerHTML = `
                <div class="ai-message-content">
                    <div class="ai-message-bubble">
                        <p>${content}</p>
                    </div>
                    <div class="ai-message-time">${time}</div>
                </div>
            `;
        }

        chatBody.appendChild(messageDiv);
        chatBody.scrollTop = chatBody.scrollHeight;
    }

    formatMessage(content) {
        // Convert line breaks and format lists
        return content
            .replace(/\n/g, '<br>')
            .replace(/•/g, '•')
            .replace(/(\d+\.)/g, '<strong>$1</strong>');
    }

    showTyping() {
        const chatBody = document.getElementById('aiChatBody');
        const typingDiv = document.createElement('div');
        typingDiv.id = 'aiTypingIndicator';
        typingDiv.className = 'ai-message ai-message-bot';
        typingDiv.innerHTML = `
            <div class="ai-message-avatar">
                <div class="ai-gradient-circle-tiny">
                    <i class="fas fa-robot"></i>
                </div>
            </div>
            <div class="ai-message-content">
                <div class="ai-message-bubble">
                    <div class="typing-dots">
                        <span></span>
                        <span></span>
                        <span></span>
                    </div>
                </div>
            </div>
        `;

        chatBody.appendChild(typingDiv);
        chatBody.scrollTop = chatBody.scrollHeight;
        this.isTyping = true;
    }

    hideTyping() {
        const typingIndicator = document.getElementById('aiTypingIndicator');
        if (typingIndicator) {
            typingIndicator.remove();
        }
        this.isTyping = false;
    }

    generateResponse(message) {
        const lowerMessage = message.toLowerCase();

        // Enhanced help and support responses
        if (this.containsKeywords(lowerMessage, ['giúp', 'help', 'hỗ trợ', 'support', 'assist'])) {
            return "🤝 **Tôi sẵn sàng giúp đỡ bạn!**\n\n💡 **Tôi có thể hỗ trợ:**\n• 🏨 Thông tin khách sạn & đặt phòng\n• 💰 Giá cả & khuyến mãi\n• 🛎️ Dịch vụ & tiện ích\n• 🌍 Du lịch & tham quan\n• 🍽️ Ẩm thực & nhà hàng\n• 🧮 Tính toán & quy đổi\n• ⏰ Thời tiết & thời gian\n• 💬 Trò chuyện thân thiện\n\n🎯 **Hãy nói rõ hơn bạn cần giúp gì nhé!**";
        }

        // Enhanced general conversation
        if (this.containsKeywords(lowerMessage, ['hôm nay', 'today', 'ngày hôm nay'])) {
            const today = new Date();
            const dayName = ['Chủ nhật', 'Thứ hai', 'Thứ ba', 'Thứ tư', 'Thứ năm', 'Thứ sáu', 'Thứ bảy'][today.getDay()];
            return `📅 **Hôm nay là ${dayName}, ngày ${today.getDate()}/${today.getMonth() + 1}/${today.getFullYear()}**\n\n🌤️ **Thời tiết:** Nắng đẹp, 28-32°C\n⏰ **Giờ hiện tại:** ${today.getHours()}:${today.getMinutes().toString().padStart(2, '0')}\n\n🏨 **Hoạt động khách sạn hôm nay:**\n• Buffet sáng: 6:00-10:00\n• Pool party: 15:00-18:00\n• Live music: 19:00-22:00\n\nBạn có kế hoạch gì đặc biệt không? 😊`;
        }

        if (this.containsKeywords(lowerMessage, ['thứ mấy', 'ngày gì', 'what day'])) {
            const today = new Date();
            const dayName = ['Chủ nhật', 'Thứ hai', 'Thứ ba', 'Thứ tư', 'Thứ năm', 'Thứ sáu', 'Thứ bảy'][today.getDay()];
            return `📅 Hôm nay là **${dayName}**, ngày ${today.getDate()}/${today.getMonth() + 1}/${today.getFullYear()}\n\n🎯 Bạn có kế hoạch gì cho ${dayName} này không?`;
        }

        // Enhanced topic discussions
        if (this.containsKeywords(lowerMessage, ['chủ đề', 'topic', 'nói về', 'talk about', 'thảo luận'])) {
            return "💭 **Chủ đề thú vị để trò chuyện:**\n\n🏨 **Về khách sạn:**\n• Dịch vụ cao cấp\n• Trải nghiệm khách hàng\n• Ẩm thực đặc sắc\n\n🌍 **Du lịch:**\n• Điểm đến hot\n• Văn hóa địa phương\n• Tips du lịch\n\n🤖 **Công nghệ:**\n• AI trong hospitality\n• Smart hotel\n• Tương lai du lịch\n\n💬 **Hoặc bạn muốn nói về chủ đề gì khác?**";
        }

        // Enhanced conversation starters
        if (this.containsKeywords(lowerMessage, ['bạn thế nào', 'how are you', 'sao rồi', 'khỏe không'])) {
            return "😊 **Tôi rất tốt, cảm ơn bạn đã hỏi!**\n\nTôi đang:\n• 🧠 Học hỏi từ mỗi cuộc trò chuyện\n• 💡 Cải thiện khả năng hỗ trợ\n• 🌟 Sẵn sàng giúp đỡ khách hàng 24/7\n\n**Còn bạn thì sao? Hôm nay có gì thú vị không?** 🤔";
        }

        // Greetings with enhanced responses
        if (this.containsKeywords(lowerMessage, ['xin chào', 'hello', 'hi', 'chào', 'hey'])) {
            const greetings = [
                "👋 **Xin chào! Rất vui được gặp bạn!**\n\nTôi là Hotel AI Assistant - trợ lý thông minh của khách sạn. Tôi có thể giúp bạn với mọi thắc mắc về:\n• Đặt phòng & dịch vụ\n• Du lịch & ẩm thực\n• Thông tin địa phương\n• Và nhiều chủ đề khác!\n\n**Bạn cần tôi hỗ trợ điều gì?** 😊",
                "🌟 **Chào bạn! Chào mừng đến với khách sạn!**\n\nTôi ở đây để làm cho kỳ nghỉ của bạn trở nên tuyệt vời nhất! Hãy hỏi tôi bất cứ điều gì - từ thông tin phòng đến gợi ý du lịch.\n\n**Hôm nay tôi có thể giúp gì cho bạn?** ✨"
            ];
            return greetings[Math.floor(Math.random() * greetings.length)];
        }

        // Hotel-specific with enhanced responses
        if (this.containsKeywords(lowerMessage, ['đặt phòng', 'booking', 'book', 'phòng', 'room'])) {
            this.context.bookingIntent = true;
            return "🏨 **Tuyệt vời! Tôi sẽ giúp bạn đặt phòng.**\n\n🏠 **Các loại phòng hiện có:**\n• **Standard Room** - 1,500,000 VNĐ/đêm\n• **Deluxe Room** - 2,200,000 VNĐ/đêm\n• **Suite Room** - 3,500,000 VNĐ/đêm\n• **Presidential Suite** - 8,000,000 VNĐ/đêm\n\n📅 **Thông tin cần thiết:**\n• Ngày check-in & check-out\n• Số lượng khách\n• Loại phòng mong muốn\n\n💡 **Bạn có thể cho tôi biết chi tiết hơn không?**";
        }

        if (this.containsKeywords(lowerMessage, ['giá', 'price', 'cost', 'tiền', 'bao nhiêu'])) {
            return "💰 **Bảng giá phòng chi tiết:**\n\n🏠 **Standard Room:** 1,500,000 VNĐ/đêm\n• Diện tích: 25m²\n• View vườn\n• WiFi miễn phí\n\n🏠 **Deluxe Room:** 2,200,000 VNĐ/đêm\n• Diện tích: 35m²\n• View biển\n• Bữa sáng miễn phí\n\n🏠 **Suite Room:** 3,500,000 VNĐ/đêm\n• Diện tích: 50m²\n• View biển + ban công\n• Butler service\n\n🏠 **Presidential Suite:** 8,000,000 VNĐ/đêm\n• Diện tích: 100m²\n• Penthouse view\n• All-inclusive\n\n💡 **Giá đã bao gồm thuế VAT và phí dịch vụ**";
        }

        if (this.containsKeywords(lowerMessage, ['dịch vụ', 'service', 'tiện ích', 'facilities'])) {
            return "🛎️ **Dịch vụ & Tiện ích cao cấp:**\n\n🏊‍♀️ **Giải trí:**\n• Hồ bơi vô cực với view biển\n• Spa & Wellness Center\n• Fitness Center 24/7\n• Kids Club & Playground\n\n🍽️ **Ẩm thực:**\n• 3 nhà hàng cao cấp\n• Rooftop Bar với cocktail\n• Room Service 24/7\n• BBQ Beach Party\n\n🏨 **Dịch vụ:**\n• Concierge 24/7\n• Airport Transfer\n• Laundry Service\n• Tour Booking\n• Car Rental\n\n**Bạn quan tâm đến dịch vụ nào đặc biệt?**";
        }

        if (this.containsKeywords(lowerMessage, ['địa chỉ', 'location', 'đường', 'ở đâu', 'vị trí'])) {
            return "📍 **Vị trí đắc địa:**\n\n🏨 **Địa chỉ:** 123 Luxury Beach Resort, Hội An, Quảng Nam\n\n✈️ **Khoảng cách:**\n• Sân bay Đà Nẵng: 45 phút (35km)\n• Trung tâm Hội An: 5 phút đi bộ\n• Phố cổ Hội An: 3 phút xe máy\n• Bãi biển An Bàng: 2 phút đi bộ\n\n🚗 **Phương tiện:**\n• Taxi từ sân bay: 400,000 VNĐ\n• Shuttle bus khách sạn: 150,000 VNĐ\n• Grab/Be: 350,000 VNĐ\n\n🅿️ **Bãi đỗ xe miễn phí cho khách lưu trú**";
        }

        // Weather and time with enhanced info
        if (this.containsKeywords(lowerMessage, ['thời tiết', 'weather', 'trời', 'nắng', 'mưa'])) {
            const weather = this.getWeatherInfo();
            return weather;
        }

        if (this.containsKeywords(lowerMessage, ['giờ', 'time', 'mấy giờ', 'bây giờ'])) {
            const timeInfo = this.getTimeInfo();
            return timeInfo;
        }

        // Food and dining with enhanced menu
        if (this.containsKeywords(lowerMessage, ['ăn', 'food', 'món', 'nhà hàng', 'restaurant', 'menu'])) {
            return "🍽️ **Ẩm thực đẳng cấp:**\n\n🥘 **Ocean View Restaurant:**\n• Hải sản tươi sống\n• Món Việt truyền thống\n• Món Âu hiện đại\n• Giờ phục vụ: 6:00-23:00\n\n🍹 **Sky Lounge Bar:**\n• Cocktail signature\n• Tapas & finger food\n• View hoàng hôn tuyệt đẹp\n• Giờ phục vụ: 17:00-02:00\n\n☕ **Lobby Café:**\n• Cà phê specialty\n• Bánh ngọt tự làm\n• Light meals\n• Giờ phục vụ: 6:00-22:00\n\n📞 **Đặt bàn:** Ext. 1234 hoặc qua app khách sạn";
        }

        // Travel and tourism with detailed info
        if (this.containsKeywords(lowerMessage, ['du lịch', 'travel', 'tham quan', 'tour', 'điểm đến'])) {
            return "🗺️ **Khám phá Hội An & vùng lân cận:**\n\n🏛️ **Must-visit (0-5km):**\n• Phố cổ Hội An - Di sản UNESCO\n• Chùa Cầu - Biểu tượng Hội An\n• Chợ đêm Hội An - Mua sắm & ẩm thực\n• Bãi biển An Bàng - Top 25 thế giới\n\n🌿 **Trải nghiệm văn hóa:**\n• Làng rau Trà Quế (2km)\n• Làng gốm Thanh Hà (8km)\n• Rừng dừa Bảy Mẫu (10km)\n• Đảo Cù Lao Chàm (45 phút thuyền)\n\n🚗 **Dịch vụ tour:**\n• Half-day city tour: 500,000 VNĐ\n• Full-day countryside: 800,000 VNĐ\n• Sunset basket boat: 300,000 VNĐ\n\n**Bạn muốn book tour nào?**";
        }

        // Math and calculations with real examples
        if (this.containsKeywords(lowerMessage, ['tính', 'calculate', 'math', 'toán']) ||
            /[\+\-\*\/]/.test(lowerMessage) || /\d+/.test(lowerMessage)) {
            return this.handleMathCalculation(lowerMessage);
        }

        // Technology with detailed explanation
        if (this.containsKeywords(lowerMessage, ['ai', 'artificial intelligence', 'robot', 'technology', 'chatgpt'])) {
            return "🤖 **Về AI và Công nghệ:**\n\nTôi là **Hotel AI Assistant** - được phát triển với:\n• **Natural Language Processing** - Hiểu ngôn ngữ tự nhiên\n• **Machine Learning** - Học từ cuộc trò chuyện\n• **Real-time Database** - Thông tin cập nhật liên tục\n• **Sentiment Analysis** - Phân tích cảm xúc\n\n🏨 **Smart Hotel Features:**\n• Keyless entry với mobile app\n• Voice control trong phòng\n• AI concierge 24/7\n• IoT room automation\n• Personalized recommendations\n\n🔮 **Tương lai:** AI sẽ cách mạng hóa ngành hospitality, mang đến trải nghiệm cá nhân hóa tuyệt vời!\n\n**Bạn có muốn biết thêm về công nghệ nào?**";
        }

        // Enhanced polite responses
        if (this.containsKeywords(lowerMessage, ['cảm ơn', 'thank', 'thanks', 'cám ơn'])) {
            return "😊 **Rất vui được giúp đỡ bạn!**\n\nĐó là niềm vui của tôi! Nếu có thêm câu hỏi gì, đừng ngại hỏi nhé.\n\n🌟 **Tôi luôn ở đây để hỗ trợ bạn 24/7!**\n\nChúc bạn có những trải nghiệm tuyệt vời tại khách sạn! ✨";
        }

        if (this.containsKeywords(lowerMessage, ['tạm biệt', 'bye', 'goodbye', 'chào tạm biệt'])) {
            return "👋 **Tạm biệt và hẹn gặp lại!**\n\nCảm ơn bạn đã trò chuyện với tôi. Chúc bạn có những kỷ niệm đẹp tại khách sạn!\n\n🌟 **Tôi luôn sẵn sàng hỗ trợ bạn bất cứ lúc nào!**\n\nHẹn gặp lại bạn sớm! 😊✨";
        }

        // Context-aware responses
        if (this.context.bookingIntent) {
            if (this.containsKeywords(lowerMessage, ['ngày', 'date', 'khi nào', 'when'])) {
                return "📅 **Chọn ngày lưu trú:**\n\n🗓️ **Cách đặt phòng:**\n1. Truy cập website: booking.hotel.com\n2. Chọn ngày check-in & check-out\n3. Chọn số lượng khách\n4. Chọn loại phòng\n5. Thanh toán online hoặc tại khách sạn\n\n📞 **Hoặc gọi trực tiếp:**\n• Hotline: (024) 1234-5678\n• WhatsApp: +84 123 456 789\n\n💡 **Đặt trước 7 ngày được giảm 10%!**";
            }
        }

        // Enhanced default responses
        return this.getEnhancedDefaultResponse(lowerMessage);
    }

    getWeatherInfo() {
        const today = new Date();
        const season = this.getCurrentSeason(today);
        const temp = Math.floor(Math.random() * 5) + 28; // 28-32°C

        return `🌤️ **Thời tiết hôm nay (${today.getDate()}/${today.getMonth() + 1}):**\n\n☀️ **Hiện tại:** ${temp}°C, nắng ít mây\n🌡️ **Nhiệt độ:** ${temp-2}°C - ${temp+2}°C\n💧 **Độ ẩm:** 65-70%\n💨 **Gió:** Đông Nam, nhẹ\n🌧️ **Khả năng mưa:** 20%\n\n🗓️ **Mùa ${season}:** ${this.getSeasonDescription(season)}\n\n🏊‍♀️ **Hoạt động phù hợp:**\n• Bơi lội tại hồ bơi vô cực\n• Tắm nắng trên bãi biển\n• Tham quan phố cổ\n• Đạp xe quanh làng\n\n👕 **Gợi ý trang phục:** Quần áo mùa hè, kem chống nắng SPF 50+`;
    }

    getTimeInfo() {
        const now = new Date();
        const timeOfDay = this.getTimeOfDay(now.getHours());
        const dayName = ['Chủ nhật', 'Thứ hai', 'Thứ ba', 'Thứ tư', 'Thứ năm', 'Thứ sáu', 'Thứ bảy'][now.getDay()];

        return `🕐 **Thời gian hiện tại:**\n\n⏰ **Bây giờ:** ${now.getHours()}:${now.getMinutes().toString().padStart(2, '0')}, ${timeOfDay}\n📅 **Hôm nay:** ${dayName}, ${now.getDate()}/${now.getMonth() + 1}/${now.getFullYear()}\n\n🏨 **Lịch hoạt động khách sạn:**\n• **Reception:** 24/7 ⭐\n• **Nhà hàng chính:** 6:00 - 23:00\n• **Rooftop Bar:** 17:00 - 02:00\n• **Spa & Wellness:** 8:00 - 22:00\n• **Fitness Center:** 24/7 ⭐\n• **Hồ bơi:** 6:00 - 22:00\n\n💡 **Gợi ý cho ${timeOfDay}:**\n${this.getTimeBasedSuggestions(now.getHours())}`;
    }

    getCurrentSeason(date) {
        const month = date.getMonth() + 1;
        if (month >= 3 && month <= 5) return "Xuân";
        if (month >= 6 && month <= 8) return "Hè";
        if (month >= 9 && month <= 11) return "Thu";
        return "Đông";
    }

    getSeasonDescription(season) {
        const descriptions = {
            "Xuân": "Thời tiết dễ chịu, nhiệt độ 22-28°C. Lý tưởng cho tham quan.",
            "Hè": "Nắng nóng, nhiệt độ 28-35°C. Thích hợp bơi lội và hoạt động nước.",
            "Thu": "Mát mẻ, nhiệt độ 24-30°C. Thời gian đẹp nhất trong năm.",
            "Đông": "Mát lạnh, nhiệt độ 18-25°C. Phù hợp nghỉ dưỡng thư giãn."
        };
        return descriptions[season] || "Thời tiết dễ chịu quanh năm.";
    }

    getTimeOfDay(hour) {
        if (hour >= 5 && hour < 12) return "buổi sáng";
        if (hour >= 12 && hour < 17) return "buổi chiều";
        if (hour >= 17 && hour < 22) return "buổi tối";
        return "đêm khuya";
    }

    getTimeBasedSuggestions(hour) {
        if (hour >= 6 && hour < 10) {
            return "• Thưởng thức buffet sáng\n• Tập gym buổi sáng\n• Bơi lội trong hồ bơi\n• Đi dạo phố cổ";
        } else if (hour >= 10 && hour < 12) {
            return "• Check-out (nếu cần)\n• Tham quan chùa Cầu\n• Mua sắm tại chợ\n• Uống cà phê";
        } else if (hour >= 12 && hour < 14) {
            return "• Dùng bữa trưa\n• Nghỉ ngơi tại phòng\n• Spa thư giãn\n• Đọc sách tại lobby";
        } else if (hour >= 14 && hour < 17) {
            return "• Check-in (nếu mới đến)\n• Khám phá khách sạn\n• Tắm nắng bên hồ bơi\n• Tham quan làng rau";
        } else if (hour >= 17 && hour < 20) {
            return "• Happy hour tại bar\n• Ngắm hoàng hôn\n• Đi dạo bãi biển\n• Chuẩn bị dùng tối";
        } else if (hour >= 20 && hour < 23) {
            return "• Dùng bữa tối\n• Thưởng thức cocktail\n• Live music\n• Massage thư giãn";
        } else {
            return "• Thư giãn tại phòng\n• Đọc sách\n• Nghe nhạc\n• Nghỉ ngơi sớm";
        }
    }

    handleMathCalculation(message) {
        // Simple math operations
        const mathPattern = /(\d+(?:\.\d+)?)\s*([+\-*/])\s*(\d+(?:\.\d+)?)/;
        const match = message.match(mathPattern);

        if (match) {
            const num1 = parseFloat(match[1]);
            const operation = match[2];
            const num2 = parseFloat(match[3]);
            let result;

            switch (operation) {
                case '+': result = num1 + num2; break;
                case '-': result = num1 - num2; break;
                case '*': result = num1 * num2; break;
                case '/': result = num2 !== 0 ? num1 / num2 : 'Không thể chia cho 0'; break;
                default: result = 'Phép tính không hợp lệ';
            }

            if (typeof result === 'number') {
                return `🧮 **Kết quả tính toán:**\n\n✅ ${num1} ${operation} ${num2} = **${result.toLocaleString('vi-VN')}**\n\n💡 **Tôi cũng có thể:**\n• Tính tip: "tip 15% cho 500k"\n• Chia bill: "chia 1 triệu cho 4 người"\n• Quy đổi tiền tệ\n• Tính thuế VAT`;
            }
        }

        // Tip calculation
        if (message.includes('tip') && message.includes('%')) {
            const percentMatch = message.match(/(\d+)%/);
            const amountMatch = message.match(/(\d+(?:[,.]?\d+)*)/);

            if (percentMatch && amountMatch) {
                const percent = parseInt(percentMatch[1]);
                const amount = parseFloat(amountMatch[1].replace(/,/g, ''));
                const tip = amount * percent / 100;
                const total = amount + tip;

                return `💰 **Tính tip:**\n\n💵 **Hóa đơn:** ${amount.toLocaleString('vi-VN')} VNĐ\n🎯 **Tip ${percent}%:** ${tip.toLocaleString('vi-VN')} VNĐ\n💳 **Tổng cộng:** ${total.toLocaleString('vi-VN')} VNĐ\n\n💡 **Tip thông thường:** 10-20% cho dịch vụ tốt`;
            }
        }

        return "🧮 **Máy tính thông minh:**\n\n💡 **Tôi có thể tính:**\n• Phép toán cơ bản: 2 + 3, 10 * 5\n• Tính tip: tip 15% cho 500k\n• Chia bill: chia 2 triệu cho 5 người\n• Quy đổi tiền tệ\n• Tính thuế VAT\n\n📝 **Ví dụ:** Hãy thử \"5 + 3\" hoặc \"tip 20% cho 1 triệu\"";
    }

    getEnhancedDefaultResponse(message) {
        // Analyze message for better responses
        if (message.includes('?') || message.includes('sao') || message.includes('why') || message.includes('tại sao')) {
            return "🤔 **Câu hỏi thú vị!**\n\nTôi đang cố gắng hiểu câu hỏi của bạn. Mặc dù tôi chưa thể trả lời chính xác, nhưng tôi có thể giúp bạn với:\n\n🏨 **Về khách sạn:**\n• Thông tin phòng & giá cả\n• Dịch vụ & tiện ích\n• Đặt phòng & check-in\n\n🌍 **Về du lịch:**\n• Điểm tham quan gần đây\n• Tour & hoạt động\n• Phương tiện di chuyển\n\n💬 **Hoặc bạn có thể:**\n• Hỏi cụ thể hơn\n• Liên hệ nhân viên: (024) 1234-5678\n• Chat trực tiếp với reception";
        }

        if (message.includes('không') || message.includes('chưa') || message.includes('not')) {
            return "😔 **Tôi hiểu bạn có thể gặp khó khăn.**\n\nHãy để tôi hỗ trợ bạn tốt hơn:\n\n🤝 **Tôi có thể giúp:**\n• Giải đáp thắc mắc\n• Hướng dẫn chi tiết\n• Kết nối với nhân viên\n• Giải quyết vấn đề\n\n💡 **Gợi ý:**\n• Nói rõ hơn vấn đề bạn gặp phải\n• Hỏi về chủ đề cụ thể\n• Yêu cầu hỗ trợ trực tiếp\n\n📞 **Hỗ trợ khẩn cấp:** (024) 1234-5678";
        }

        // General intelligent responses
        const intelligentResponses = [
            "💭 **Tôi đang học hỏi để hiểu bạn tốt hơn!**\n\nMặc dù tôi chưa thể trả lời chính xác câu hỏi này, nhưng tôi luôn cố gắng cải thiện.\n\n🌟 **Tôi giỏi nhất về:**\n• Thông tin khách sạn & booking\n• Du lịch & ẩm thực địa phương\n• Tính toán & quy đổi\n• Thời tiết & thời gian\n• Trò chuyện thân thiện\n\n💡 **Hãy thử hỏi tôi về những chủ đề này!**",

            "🤖 **AI đang phát triển mỗi ngày!**\n\nTôi có thể chưa hiểu hoàn toàn, nhưng tôi sẵn sàng học hỏi từ bạn.\n\n✨ **Điều tôi có thể làm:**\n• Hỗ trợ thông tin khách sạn\n• Tư vấn du lịch Hội An\n• Giải đáp về dịch vụ\n• Tính toán đơn giản\n• Trò chuyện vui vẻ\n\n🎯 **Bạn muốn thử hỏi gì khác không?**",

            "🌟 **Cảm ơn bạn đã kiên nhẫn với tôi!**\n\nTôi đang không ngừng học hỏi để phục vụ bạn tốt hơn.\n\n💪 **Tôi tự tin về:**\n• Thông tin chi tiết khách sạn\n• Gợi ý du lịch địa phương\n• Hỗ trợ đặt phòng\n• Tư vấn ẩm thực\n• Giải đáp thắc mắc cơ bản\n\n🤝 **Hoặc tôi có thể kết nối bạn với nhân viên chuyên nghiệp!**"
        ];

        return intelligentResponses[Math.floor(Math.random() * intelligentResponses.length)];
    }

    containsKeywords(message, keywords) {
        return keywords.some(keyword => message.includes(keyword));
    }

    getRandomResponse(category) {
        const responses = this.responses[category];
        return responses[Math.floor(Math.random() * responses.length)];
    }

    async getAdvancedAIResponse(message) {
        try {
            // Enhanced context with conversation history
            const enhancedContext = {
                ...this.context,
                conversationHistory: this.context.conversationHistory.slice(-5), // Last 5 messages
                messageCount: this.messageCount,
                sessionDuration: Math.floor((new Date() - this.context.sessionStartTime) / 1000),
                userPreferences: this.context.userPreferences
            };

            const response = await fetch('/api/aichat/message', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    message: message,
                    context: enhancedContext
                })
            });

            if (response.ok) {
                const data = await response.json();

                // Calculate realistic typing delay based on response length
                const typingDelay = this.calculateTypingDelay(data.message);

                setTimeout(() => {
                    this.hideTyping();
                    this.addEnhancedMessage(data.message, 'bot', data);

                    // Update context with AI response
                    if (data.context) {
                        this.context = { ...this.context, ...data.context };
                    }

                    // Add conversation to history
                    this.context.conversationHistory.push({
                        type: 'bot',
                        message: data.message,
                        timestamp: new Date(),
                        intent: data.context?.intent,
                        confidence: data.confidence
                    });

                    // Add intelligent suggestions
                    if (data.suggestions && data.suggestions.length > 0) {
                        this.addIntelligentSuggestions(data.suggestions, data.context?.intent);
                    }

                    // Learn from interaction
                    this.learnFromInteraction(message, data);

                }, typingDelay);
            } else {
                throw new Error('API call failed');
            }
        } catch (error) {
            console.error('AI Chat Error:', error);

            // Enhanced fallback with context awareness
            setTimeout(() => {
                this.hideTyping();
                const fallbackResponse = this.generateIntelligentFallback(message);
                this.addMessage(fallbackResponse, 'bot');
            }, 1000);
        }
    }

    calculateTypingDelay(message) {
        // Simulate realistic typing speed
        const baseDelay = 800;
        const characterDelay = message.length * (1000 / this.typingSpeed);
        const randomVariation = Math.random() * 500;
        return Math.min(baseDelay + characterDelay + randomVariation, 4000); // Max 4 seconds
    }

    showIntelligentTyping(userMessage) {
        // Show different typing messages based on user input
        const typingMessages = [
            "AI đang suy nghĩ...",
            "Đang phân tích câu hỏi...",
            "Đang tìm kiếm thông tin...",
            "Đang chuẩn bị câu trả lời...",
            "Đang xử lý yêu cầu..."
        ];

        let typingMessage = typingMessages[0];

        if (userMessage.includes('đặt phòng') || userMessage.includes('booking')) {
            typingMessage = "Đang kiểm tra phòng trống...";
        } else if (userMessage.includes('giá') || userMessage.includes('price')) {
            typingMessage = "Đang tính toán giá cả...";
        } else if (userMessage.includes('thời tiết') || userMessage.includes('weather')) {
            typingMessage = "Đang cập nhật thời tiết...";
        } else if (userMessage.includes('tính') || /[\+\-\*\/]/.test(userMessage)) {
            typingMessage = "Đang tính toán...";
        }

        this.showTypingWithMessage(typingMessage);
    }

    showTypingWithMessage(message) {
        const chatBody = document.getElementById('aiChatBody');
        const typingDiv = document.createElement('div');
        typingDiv.id = 'aiTypingIndicator';
        typingDiv.className = 'ai-message ai-message-bot';
        typingDiv.innerHTML = `
            <div class="ai-message-avatar">
                <div class="ai-gradient-circle-tiny">
                    <i class="fas fa-robot"></i>
                </div>
            </div>
            <div class="ai-message-content">
                <div class="ai-message-bubble">
                    <div class="typing-container">
                        <span class="typing-text">${message}</span>
                        <div class="typing-dots">
                            <span></span>
                            <span></span>
                            <span></span>
                        </div>
                    </div>
                </div>
            </div>
        `;

        chatBody.appendChild(typingDiv);
        chatBody.scrollTop = chatBody.scrollHeight;
        this.isTyping = true;
    }

    addEnhancedMessage(content, sender, aiData = null) {
        const chatBody = document.getElementById('aiChatBody');
        const messageDiv = document.createElement('div');
        messageDiv.className = `ai-message ai-message-${sender}`;

        const time = new Date().toLocaleTimeString('vi-VN', {
            hour: '2-digit',
            minute: '2-digit'
        });

        if (sender === 'bot') {
            // Add confidence indicator for AI responses
            const confidenceIndicator = aiData?.confidence ?
                `<div class="ai-confidence" title="Độ tin cậy: ${Math.round(aiData.confidence * 100)}%">
                    <i class="fas fa-brain"></i> ${Math.round(aiData.confidence * 100)}%
                </div>` : '';

            messageDiv.innerHTML = `
                <div class="ai-message-avatar">
                    <div class="ai-gradient-circle-tiny">
                        <i class="fas fa-robot"></i>
                    </div>
                </div>
                <div class="ai-message-content">
                    <div class="ai-message-bubble">
                        ${this.formatAdvancedMessage(content)}
                    </div>
                    <div class="ai-message-meta">
                        <span class="ai-message-time">${time}</span>
                        ${confidenceIndicator}
                    </div>
                </div>
            `;
        } else {
            messageDiv.innerHTML = `
                <div class="ai-message-content">
                    <div class="ai-message-bubble">
                        <p>${content}</p>
                    </div>
                    <div class="ai-message-time">${time}</div>
                </div>
            `;
        }

        chatBody.appendChild(messageDiv);
        chatBody.scrollTop = chatBody.scrollHeight;

        // Add smooth animation
        messageDiv.style.opacity = '0';
        messageDiv.style.transform = 'translateY(10px)';
        setTimeout(() => {
            messageDiv.style.transition = 'all 0.3s ease';
            messageDiv.style.opacity = '1';
            messageDiv.style.transform = 'translateY(0)';
        }, 50);
    }

    formatAdvancedMessage(content) {
        // Enhanced message formatting with better markdown support
        return content
            .replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>') // Bold
            .replace(/\*(.*?)\*/g, '<em>$1</em>') // Italic
            .replace(/\n/g, '<br>') // Line breaks
            .replace(/•/g, '•') // Bullet points
            .replace(/(\d+\.)/g, '<strong>$1</strong>') // Numbered lists
            .replace(/(https?:\/\/[^\s]+)/g, '<a href="$1" target="_blank">$1</a>') // Links
            .replace(/`(.*?)`/g, '<code>$1</code>'); // Code
    }

    learnFromInteraction(userMessage, aiResponse) {
        // Simple learning mechanism
        const intent = aiResponse.context?.intent;
        if (intent) {
            if (!this.context.userPreferences[intent]) {
                this.context.userPreferences[intent] = 0;
            }
            this.context.userPreferences[intent]++;
        }

        // Store frequently asked questions
        const lowerMessage = userMessage.toLowerCase();
        if (!this.context.frequentQuestions) {
            this.context.frequentQuestions = {};
        }

        if (!this.context.frequentQuestions[lowerMessage]) {
            this.context.frequentQuestions[lowerMessage] = 0;
        }
        this.context.frequentQuestions[lowerMessage]++;
    }

    generateIntelligentFallback(message) {
        // Context-aware fallback based on conversation history
        const recentIntents = this.context.conversationHistory
            .slice(-3)
            .map(h => h.intent)
            .filter(i => i);

        if (recentIntents.length > 0) {
            const lastIntent = recentIntents[recentIntents.length - 1];
            return `🤔 Tôi hiểu bạn vẫn quan tâm đến ${lastIntent}. Bạn có thể hỏi cụ thể hơn không?\n\nHoặc thử hỏi tôi về chủ đề khác! 😊`;
        }

        return this.getRandomResponse('default');
    }

    addSuggestions(suggestions) {
        const chatBody = document.getElementById('aiChatBody');
        const suggestionsDiv = document.createElement('div');
        suggestionsDiv.className = 'ai-inline-suggestions';

        const suggestionsHTML = suggestions.map(suggestion =>
            `<button class="ai-inline-suggestion" onclick="aiChatBot.sendSuggestion('${suggestion}')">${suggestion}</button>`
        ).join('');

        suggestionsDiv.innerHTML = `
            <div class="ai-message ai-message-bot">
                <div class="ai-message-avatar">
                    <div class="ai-gradient-circle-tiny">
                        <i class="fas fa-robot"></i>
                    </div>
                </div>
                <div class="ai-message-content">
                    <div class="ai-suggestions-container">
                        ${suggestionsHTML}
                    </div>
                </div>
            </div>
        `;

        chatBody.appendChild(suggestionsDiv);
        chatBody.scrollTop = chatBody.scrollHeight;
    }
}

// Initialize AI Chat Bot
const aiChatBot = new AIChatBot();

// Add typing animation and suggestions CSS
const additionalCSS = `
<style>
.typing-dots {
    display: flex;
    align-items: center;
    gap: 4px;
    padding: 8px 0;
}

.typing-dots span {
    width: 6px;
    height: 6px;
    border-radius: 50%;
    background: #667eea;
    animation: typing 1.4s infinite ease-in-out;
}

.typing-dots span:nth-child(1) { animation-delay: -0.32s; }
.typing-dots span:nth-child(2) { animation-delay: -0.16s; }

@keyframes typing {
    0%, 80%, 100% { transform: scale(0.8); opacity: 0.5; }
    40% { transform: scale(1); opacity: 1; }
}

.ai-suggestions-container {
    display: flex;
    flex-wrap: wrap;
    gap: 8px;
    margin-top: 8px;
}

.ai-inline-suggestion {
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    color: white;
    border: none;
    padding: 8px 12px;
    border-radius: 16px;
    font-size: 12px;
    cursor: pointer;
    transition: all 0.2s ease;
    box-shadow: 0 2px 8px rgba(102, 126, 234, 0.3);
}

.ai-inline-suggestion:hover {
    transform: translateY(-1px);
    box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
}

.ai-inline-suggestions {
    margin-bottom: 16px;
}
</style>
`;

document.head.insertAdjacentHTML('beforeend', additionalCSS);
