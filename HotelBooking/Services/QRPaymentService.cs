using HotelBooking.Models;
using System.Text;

namespace HotelBooking.Services
{
    public interface IQRPaymentService
    {
        string GenerateVietinBankQRCode(decimal amount, string description, string accountNumber = "1038766815877");
        QRPayment CreateQRPayment(int reservationId, decimal amount, string description, int userId);
        bool ValidateQRPayment(string transactionRef, decimal amount);
    }

    public class QRPaymentService : IQRPaymentService
    {
        private readonly string _bankCode = "VietinBank";
        private readonly string _accountNumber = "1038766815877";
        private readonly string _accountName = "LUU VAN HIEN";
        private readonly string _bankBranch = "VietinBank CN KCN PHU TAI - PGD AN";

        public string GenerateVietinBankQRCode(decimal amount, string description, string accountNumber = "1038766815877")
        {
            // Use the actual QR code data from the provided image
            // This is the real VietinBank QR code for account 1038766815877
            var baseQRData = "00020101021238570010A00000072701270006970454011410387668158770208QRIBFTTA5303704";

            var qrData = new StringBuilder(baseQRData);

            // Add amount if specified
            if (amount > 0)
            {
                var amountStr = amount.ToString("F0"); // No decimals for VND
                qrData.Append($"54{amountStr.Length:D2}{amountStr}");
            }

            qrData.Append("5802VN"); // Country code (Vietnam)
            qrData.Append($"59{_accountName.Length:D2}{_accountName}"); // Account holder name: LUU VAN HIEN
            qrData.Append($"60{_bankBranch.Length:D2}{_bankBranch}"); // Bank branch

            // Additional data (transaction description)
            if (!string.IsNullOrEmpty(description))
            {
                var descLength = description.Length;
                qrData.Append($"62{descLength + 4:D2}08{descLength:D2}{description}");
            }

            // Calculate and append checksum
            qrData.Append("6304");
            var checksum = CalculateChecksum(qrData.ToString());
            qrData.Append(checksum);

            return qrData.ToString();
        }

        public QRPayment CreateQRPayment(int reservationId, decimal amount, string description, int userId)
        {
            var qrCodeData = GenerateVietinBankQRCode(amount, description);

            return new QRPayment
            {
                ReservationID = reservationId,
                Amount = amount,
                BankCode = _bankCode,
                AccountNumber = _accountNumber,
                AccountName = _accountName,
                QRCodeData = qrCodeData,
                TransactionDescription = description,
                CreatedDate = DateTime.Now,
                Status = "Pending",
                TransactionReference = GenerateTransactionReference(),
                CreatedByUserID = userId
            };
        }

        public bool ValidateQRPayment(string transactionRef, decimal amount)
        {
            // In a real implementation, this would check with the bank's API
            // For demo purposes, we'll simulate validation
            return !string.IsNullOrEmpty(transactionRef) && amount > 0;
        }

        private string GenerateTransactionReference()
        {
            return $"QR{DateTime.Now:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";
        }

        private string CalculateChecksum(string data)
        {
            // Simplified checksum calculation for demo
            // In real implementation, use proper CRC16 algorithm
            int sum = 0;
            foreach (char c in data)
            {
                sum += (int)c;
            }
            return (sum % 10000).ToString("D4");
        }
    }
}
