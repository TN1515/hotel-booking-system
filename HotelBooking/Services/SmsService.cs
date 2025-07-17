using Microsoft.Extensions.Configuration;

namespace HotelBooking.Services
{
    public interface ISmsService
    {
        Task SendSmsAsync(string phoneNumber, string message);
        Task SendBulkSmsAsync(List<string> phoneNumbers, string message);
        Task SendNotificationSmsAsync(string phoneNumber, string title, string message);
    }

    public class SmsService : ISmsService
    {
        private readonly ILogger<SmsService> _logger;
        private readonly IConfiguration _configuration;

        public SmsService(ILogger<SmsService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task SendSmsAsync(string phoneNumber, string message)
        {
            try
            {
                // Log the SMS for debugging
                _logger.LogInformation($"📱 SENDING SMS TO: {phoneNumber}");
                _logger.LogInformation($"📱 MESSAGE: {message}");

                // For demo purposes, we'll simulate successful SMS sending
                // In production, you would integrate with SMS providers like Twilio, AWS SNS, etc.
                
                // Validate phone number format
                if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length < 10)
                {
                    throw new ArgumentException("Invalid phone number format");
                }

                // Simulate SMS sending process
                await Task.Delay(300); // Simulate network delay
                
                // For demo, we'll create a simple "SMS sent" confirmation
                var smsLog = $@"
=== SMS SENT SUCCESSFULLY ===
TO: {phoneNumber}
MESSAGE: {message}
SENT AT: {DateTime.Now:yyyy-MM-dd HH:mm:ss}
STATUS: ✅ DELIVERED
PROVIDER: Demo SMS Service
=============================";
                
                _logger.LogInformation(smsLog);
                
                // TODO: In production, integrate with real SMS service:
                /*
                // Example with Twilio:
                var accountSid = _configuration["Twilio:AccountSid"];
                var authToken = _configuration["Twilio:AuthToken"];
                var fromNumber = _configuration["Twilio:FromNumber"];
                
                TwilioClient.Init(accountSid, authToken);
                
                var message = MessageResource.Create(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(fromNumber),
                    to: new Twilio.Types.PhoneNumber(phoneNumber)
                );
                */
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Failed to send SMS to {phoneNumber}");
                throw;
            }
        }

        public async Task SendBulkSmsAsync(List<string> phoneNumbers, string message)
        {
            var tasks = new List<Task>();
            
            foreach (var phoneNumber in phoneNumbers)
            {
                tasks.Add(SendSmsAsync(phoneNumber, message));
            }
            
            await Task.WhenAll(tasks);
            _logger.LogInformation($"📱 Bulk SMS sent to {phoneNumbers.Count} recipients");
        }

        public async Task SendNotificationSmsAsync(string phoneNumber, string title, string message)
        {
            var smsMessage = $"🏨 {title}\n\n{message}\n\n- Hotel Booking System";
            await SendSmsAsync(phoneNumber, smsMessage);
        }
    }
}
