using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace HotelBooking.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        Task SendRoomChangeNotificationAsync(string customerEmail, string customerName, string oldRoom, string newRoom, DateTime changeDate, decimal priceDifference);
        Task SendServiceAddedNotificationAsync(string customerEmail, string customerName, string serviceName, int quantity, decimal totalCost);
        Task SendCheckoutReceiptAsync(string customerEmail, string customerName, string reservationDetails, decimal totalAmount);
        Task SendBookingConfirmationAsync(string customerEmail, string customerName, string bookingDetails);
        Task SendCancellationNotificationAsync(string customerEmail, string customerName, string bookingDetails);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                // Log the email for debugging
                _logger.LogInformation($"📧 SENDING EMAIL TO: {to}");
                _logger.LogInformation($"📧 SUBJECT: {subject}");
                _logger.LogInformation($"📧 BODY: {body}");

                // For demo purposes, we'll simulate successful email sending
                // In production, you would configure real SMTP settings

                // Simulate email sending process
                await Task.Delay(500); // Simulate network delay

                // For demo, we'll create a simple "email sent" confirmation
                var emailLog = $@"
=== EMAIL SENT SUCCESSFULLY ===
TO: {to}
SUBJECT: {subject}
SENT AT: {DateTime.Now:yyyy-MM-dd HH:mm:ss}
STATUS: ✅ DELIVERED
================================";

                _logger.LogInformation(emailLog);

                // TODO: In production, uncomment and configure this:
                /*
                var smtpClient = new SmtpClient(_configuration["Email:SmtpServer"])
                {
                    Port = int.Parse(_configuration["Email:Port"]),
                    Credentials = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"]),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["Email:FromAddress"], _configuration["Email:FromName"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml,
                };
                mailMessage.To.Add(to);

                await smtpClient.SendMailAsync(mailMessage);
                */
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Failed to send email to {to}");
                throw;
            }
        }

        public async Task SendRoomChangeNotificationAsync(string customerEmail, string customerName, string oldRoom, string newRoom, DateTime changeDate, decimal priceDifference)
        {
            var subject = "Room Change Confirmation - Hotel Booking System";
            var body = $@"
                <html>
                <body>
                    <h2>Room Change Confirmation</h2>
                    <p>Dear {customerName},</p>
                    <p>Your room change request has been processed successfully.</p>
                    <h3>Change Details:</h3>
                    <ul>
                        <li><strong>From Room:</strong> {oldRoom}</li>
                        <li><strong>To Room:</strong> {newRoom}</li>
                        <li><strong>Change Date:</strong> {changeDate:MMM dd, yyyy}</li>
                        <li><strong>Price Difference:</strong> {(priceDifference >= 0 ? "+" : "")}${priceDifference:F2}</li>
                    </ul>
                    <p>Thank you for choosing our hotel!</p>
                    <p>Best regards,<br>Hotel Management Team</p>
                </body>
                </html>";

            await SendEmailAsync(customerEmail, subject, body);
        }

        public async Task SendServiceAddedNotificationAsync(string customerEmail, string customerName, string serviceName, int quantity, decimal totalCost)
        {
            var subject = "Service Added to Your Booking - Hotel Booking System";
            var body = $@"
                <html>
                <body>
                    <h2>Service Added Confirmation</h2>
                    <p>Dear {customerName},</p>
                    <p>A new service has been added to your booking.</p>
                    <h3>Service Details:</h3>
                    <ul>
                        <li><strong>Service:</strong> {serviceName}</li>
                        <li><strong>Quantity:</strong> {quantity}</li>
                        <li><strong>Total Cost:</strong> ${totalCost:F2}</li>
                        <li><strong>Added Date:</strong> {DateTime.Now:MMM dd, yyyy HH:mm}</li>
                    </ul>
                    <p>This amount will be added to your final bill.</p>
                    <p>Thank you for choosing our services!</p>
                    <p>Best regards,<br>Hotel Management Team</p>
                </body>
                </html>";

            await SendEmailAsync(customerEmail, subject, body);
        }

        public async Task SendCheckoutReceiptAsync(string customerEmail, string customerName, string reservationDetails, decimal totalAmount)
        {
            var subject = "Checkout Receipt - Hotel Booking System";
            var body = $@"
                <html>
                <body>
                    <h2>Checkout Receipt</h2>
                    <p>Dear {customerName},</p>
                    <p>Thank you for staying with us! Here's your checkout receipt.</p>
                    <div style='border: 1px solid #ccc; padding: 15px; margin: 10px 0;'>
                        {reservationDetails}
                    </div>
                    <h3>Total Amount: ${totalAmount:F2}</h3>
                    <p>We hope you enjoyed your stay and look forward to welcoming you again!</p>
                    <p>Best regards,<br>Hotel Management Team</p>
                </body>
                </html>";

            await SendEmailAsync(customerEmail, subject, body);
        }

        public async Task SendBookingConfirmationAsync(string customerEmail, string customerName, string bookingDetails)
        {
            var subject = "Booking Confirmation - Hotel Booking System";
            var body = $@"
                <html>
                <body>
                    <h2>Booking Confirmation</h2>
                    <p>Dear {customerName},</p>
                    <p>Your booking has been confirmed!</p>
                    <div style='border: 1px solid #ccc; padding: 15px; margin: 10px 0;'>
                        {bookingDetails}
                    </div>
                    <p>We look forward to welcoming you!</p>
                    <p>Best regards,<br>Hotel Management Team</p>
                </body>
                </html>";

            await SendEmailAsync(customerEmail, subject, body);
        }

        public async Task SendCancellationNotificationAsync(string customerEmail, string customerName, string bookingDetails)
        {
            var subject = "Booking Cancellation - Hotel Booking System";
            var body = $@"
                <html>
                <body>
                    <h2>Booking Cancellation</h2>
                    <p>Dear {customerName},</p>
                    <p>Your booking has been cancelled as requested.</p>
                    <div style='border: 1px solid #ccc; padding: 15px; margin: 10px 0;'>
                        {bookingDetails}
                    </div>
                    <p>If you have any questions, please contact us.</p>
                    <p>Best regards,<br>Hotel Management Team</p>
                </body>
                </html>";

            await SendEmailAsync(customerEmail, subject, body);
        }
    }
}
