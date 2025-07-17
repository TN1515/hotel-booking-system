# 🏨 Hotel Booking System

A comprehensive hotel booking management system built with ASP.NET Core MVC, Entity Framework Core, and modern web technologies.

## ✨ Features

### 🔐 Authentication & Authorization
- **Multi-role system**: Admin, Staff, Customer
- **Demo accounts** ready to use
- **Secure login/registration** with ASP.NET Core Identity

### 🏠 Core Functionality
- **Room Management**: CRUD operations with image upload
- **Booking System**: Single and multiple room reservations
- **Guest Management**: Customer profiles and history
- **Payment Processing**: Complete payment workflow
- **Dashboard**: Analytics and reporting

### 🎨 User Interface
- **Responsive Design**: Works on all devices
- **Modern UI**: Beautiful, intuitive interface
- **Image Galleries**: Room photos with carousel display
- **Search & Filter**: Advanced room search capabilities

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK
- SQL Server (optional - uses InMemory by default)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd hotel-booking-system-main/HotelBooking
   ```

2. **Install dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   dotnet run --urls http://localhost:5001
   ```

4. **Open browser**
   Navigate to: http://localhost:5001

## 👥 Demo Accounts

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@demo.com | 123456 |
| Staff | staff@demo.com | 123456 |
| Customer | customer@demo.com | 123456 |

## 🗄️ Database Configuration

### Current Setup: InMemory Database
- ✅ **Quick start** - no setup required
- ⚠️ **Data is temporary** - lost on restart
- 💡 **Perfect for testing** and development

### Switch to SQL Server
1. **Check current status**
   ```powershell
   .\check-database.ps1
   ```

2. **Switch to SQL Server**
   ```powershell
   .\switch-to-sqlserver.ps1
   ```

3. **Connection String** (in appsettings.json)
   ```json
   "ConnectionStrings": {
     "DBContextConnection": "Server=MSI\\SQLEXPRESS;Database=khachsan;User Id=sa;Password=Swpfpt12345;TrustServerCertificate=True;"
   }
   ```

## 📁 Project Structure

```
HotelBooking/
├── Controllers/         # MVC Controllers
├── Models/             # Data Models
├── Views/              # Razor Views
├── Data/               # Database Context & Seeding
├── Services/           # Business Logic Services
├── wwwroot/            # Static Files (CSS, JS, Images)
├── Migrations/         # EF Core Migrations
└── Scripts/            # PowerShell utilities
```

## 🛠️ Available Scripts

| Script | Description |
|--------|-------------|
| `.\check-database.ps1` | Check database status and configuration |
| `.\switch-to-sqlserver.ps1` | Switch from InMemory to SQL Server |

## 🎯 Key Features Breakdown

### 🛏️ Room Management
- Create, edit, delete rooms
- Upload multiple images per room
- Room types and amenities
- Availability tracking

### 📅 Booking System
- Single room booking
- Multiple room booking
- Date range selection
- Booking status management

### 💳 Payment Processing
- Payment workflow
- Payment history
- Refund system
- Invoice generation

### 📊 Reports & Analytics
- Booking reports
- Revenue analytics
- Guest statistics
- Occupancy rates

## 🔧 Development

### Build
```bash
dotnet build
```

### Run Tests
```bash
dotnet test
```

### Create Migration
```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

## 📝 License

This project is licensed under the MIT License.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## 📞 Support

For support and questions, please create an issue in the repository.

---

**🎉 Happy Coding!** 🏨✨
