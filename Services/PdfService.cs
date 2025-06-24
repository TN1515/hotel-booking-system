using HotelBooking.Models;
using HotelBooking.Models.ViewModels;
using System.Text;

namespace HotelBooking.Services
{
    public interface IPdfService
    {
        Task<byte[]> GenerateInvoicePdfAsync(CheckoutViewModel checkoutData);
        Task<byte[]> GenerateBookingConfirmationPdfAsync(Reservation reservation);
        Task<string> GenerateInvoiceHtmlAsync(CheckoutViewModel checkoutData);
        Task<string> GenerateBookingConfirmationHtmlAsync(Reservation reservation);
    }

    public class PdfService : IPdfService
    {
        private readonly ILogger<PdfService> _logger;

        public PdfService(ILogger<PdfService> logger)
        {
            _logger = logger;
        }

        public async Task<byte[]> GenerateInvoicePdfAsync(CheckoutViewModel checkoutData)
        {
            // For demo purposes, we'll generate HTML and convert to bytes
            // In a real application, you would use a library like iTextSharp, PuppeteerSharp, or wkhtmltopdf
            var html = await GenerateInvoiceHtmlAsync(checkoutData);
            return Encoding.UTF8.GetBytes(html);
        }

        public async Task<byte[]> GenerateBookingConfirmationPdfAsync(Reservation reservation)
        {
            var html = await GenerateBookingConfirmationHtmlAsync(reservation);
            return Encoding.UTF8.GetBytes(html);
        }

        public async Task<string> GenerateInvoiceHtmlAsync(CheckoutViewModel checkoutData)
        {
            var html = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Invoice - Hotel Booking System</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 20px; }}
        .header {{ text-align: center; border-bottom: 2px solid #333; padding-bottom: 20px; margin-bottom: 30px; }}
        .invoice-details {{ margin-bottom: 30px; }}
        .table {{ width: 100%; border-collapse: collapse; margin-bottom: 20px; }}
        .table th, .table td {{ border: 1px solid #ddd; padding: 8px; text-align: left; }}
        .table th {{ background-color: #f2f2f2; }}
        .total-row {{ font-weight: bold; background-color: #f9f9f9; }}
        .footer {{ margin-top: 50px; text-align: center; font-size: 12px; color: #666; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>HOTEL BOOKING SYSTEM</h1>
        <h2>INVOICE</h2>
        <p>Invoice Date: {DateTime.Now:MMM dd, yyyy}</p>
        <p>Invoice #: INV-{checkoutData.ReservationID:D6}</p>
    </div>

    <div class='invoice-details'>
        <h3>Reservation Details</h3>
        <table class='table'>
            <tr><td><strong>Room:</strong></td><td>{checkoutData.RoomNumber} - {checkoutData.RoomType}</td></tr>
            <tr><td><strong>Check-in:</strong></td><td>{checkoutData.CheckInDate:MMM dd, yyyy}</td></tr>
            <tr><td><strong>Check-out:</strong></td><td>{checkoutData.CheckOutDate:MMM dd, yyyy}</td></tr>
            <tr><td><strong>Number of Nights:</strong></td><td>{checkoutData.NumberOfNights}</td></tr>
        </table>
    </div>

    <div class='charges'>
        <h3>Charges Breakdown</h3>
        <table class='table'>
            <thead>
                <tr>
                    <th>Description</th>
                    <th>Amount</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Room Charges ({checkoutData.NumberOfNights} nights)</td>
                    <td>${checkoutData.RoomCost:F2}</td>
                </tr>";

            if (checkoutData.ServicesUsed != null && checkoutData.ServicesUsed.Any())
            {
                html += @"
                <tr>
                    <td colspan='2'><strong>Additional Services:</strong></td>
                </tr>";
                foreach (var service in checkoutData.ServicesUsed)
                {
                    html += $@"
                <tr>
                    <td>&nbsp;&nbsp;{service.Service?.ServiceName} (Qty: {service.Quantity})</td>
                    <td>${service.TotalPrice:F2}</td>
                </tr>";
                }
            }

            if (checkoutData.RoomChanges != null && checkoutData.RoomChanges.Any())
            {
                html += @"
                <tr>
                    <td colspan='2'><strong>Room Changes:</strong></td>
                </tr>";
                foreach (var change in checkoutData.RoomChanges)
                {
                    html += $@"
                <tr>
                    <td>&nbsp;&nbsp;Room change on {change.ChangeDate:MMM dd}</td>
                    <td>{(change.PriceDifference >= 0 ? "+" : "")}${change.PriceDifference:F2}</td>
                </tr>";
                }
            }

            html += $@"
                <tr>
                    <td>Tax (10%)</td>
                    <td>${checkoutData.TaxAmount:F2}</td>
                </tr>";

            if (checkoutData.DiscountAmount > 0)
            {
                html += $@"
                <tr>
                    <td>Discount</td>
                    <td>-${checkoutData.DiscountAmount:F2}</td>
                </tr>";
            }

            html += $@"
                <tr class='total-row'>
                    <td><strong>TOTAL AMOUNT</strong></td>
                    <td><strong>${checkoutData.TotalCost:F2}</strong></td>
                </tr>
            </tbody>
        </table>
    </div>

    <div class='footer'>
        <p>Thank you for choosing Hotel Booking System!</p>
        <p>For any questions, please contact us at info@hotelbooking.com</p>
        <p>Generated on {DateTime.Now:MMM dd, yyyy HH:mm}</p>
    </div>
</body>
</html>";

            return await Task.FromResult(html);
        }

        public async Task<string> GenerateBookingConfirmationHtmlAsync(Reservation reservation)
        {
            var html = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Booking Confirmation - Hotel Booking System</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 20px; }}
        .header {{ text-align: center; border-bottom: 2px solid #333; padding-bottom: 20px; margin-bottom: 30px; }}
        .confirmation-details {{ margin-bottom: 30px; }}
        .table {{ width: 100%; border-collapse: collapse; margin-bottom: 20px; }}
        .table th, .table td {{ border: 1px solid #ddd; padding: 8px; text-align: left; }}
        .table th {{ background-color: #f2f2f2; }}
        .footer {{ margin-top: 50px; text-align: center; font-size: 12px; color: #666; }}
        .status {{ padding: 5px 10px; border-radius: 5px; color: white; background-color: #28a745; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>HOTEL BOOKING SYSTEM</h1>
        <h2>BOOKING CONFIRMATION</h2>
        <p>Confirmation Date: {DateTime.Now:MMM dd, yyyy}</p>
        <p>Booking Reference: BK-{reservation.ReservationID:D6}</p>
        <span class='status'>{reservation.Status}</span>
    </div>

    <div class='confirmation-details'>
        <h3>Booking Details</h3>
        <table class='table'>
            <tr><td><strong>Guest Name:</strong></td><td>{reservation.User?.UserName}</td></tr>
            <tr><td><strong>Email:</strong></td><td>{reservation.User?.Email}</td></tr>
            <tr><td><strong>Room:</strong></td><td>{reservation.Room?.RoomNumber} - {reservation.Room?.RoomType?.TypeName}</td></tr>
            <tr><td><strong>Check-in Date:</strong></td><td>{reservation.CheckInDate:MMM dd, yyyy}</td></tr>
            <tr><td><strong>Check-out Date:</strong></td><td>{reservation.CheckOutDate:MMM dd, yyyy}</td></tr>
            <tr><td><strong>Number of Guests:</strong></td><td>{reservation.NumberOfGuests}</td></tr>
            <tr><td><strong>Number of Nights:</strong></td><td>{(reservation.CheckOutDate - reservation.CheckInDate).Days}</td></tr>
            <tr><td><strong>Room Rate:</strong></td><td>${reservation.Room?.Price:F2} per night</td></tr>
            <tr><td><strong>Total Room Cost:</strong></td><td>${(reservation.Room?.Price ?? 0) * (reservation.CheckOutDate - reservation.CheckInDate).Days:F2}</td></tr>
        </table>
    </div>

    <div class='important-info'>
        <h3>Important Information</h3>
        <ul>
            <li>Please arrive at the hotel after 3:00 PM on your check-in date</li>
            <li>Check-out time is 11:00 AM</li>
            <li>Please bring a valid ID for check-in</li>
            <li>Cancellations must be made at least 24 hours before check-in</li>
            <li>Additional services can be added during your stay</li>
        </ul>
    </div>

    <div class='footer'>
        <p>We look forward to welcoming you!</p>
        <p>For any questions, please contact us at info@hotelbooking.com or call +1-234-567-8900</p>
        <p>Generated on {DateTime.Now:MMM dd, yyyy HH:mm}</p>
    </div>
</body>
</html>";

            return await Task.FromResult(html);
        }
    }
}
