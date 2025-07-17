# 🏨 Hotel Booking System - Demo Accounts

## 📋 Tài khoản Demo

Hệ thống cung cấp 3 tài khoản demo cho 3 chức vụ khác nhau:

### 1. 👑 **ADMIN** - Quản trị viên hệ thống
```
Email: admin@demo.com
Username: admin
Password: 123456
```

**Quyền truy cập:**
- ✅ Quản lý người dùng (User Management)
- ✅ Quản lý phòng và loại phòng (Room Management)
- ✅ Quản lý tiện ích (Amenities Management)
- ✅ Xem báo cáo chi tiết (Reports)
- ✅ Quản lý feedback và thống kê
- ✅ Gửi thông báo hàng loạt
- ✅ Cài đặt hệ thống
- ✅ Truy cập Admin Dashboard

### 2. 👔 **STAFF** - Nhân viên khách sạn
```
Email: staff@demo.com
Username: staff
Password: 123456
```

**Quyền truy cập:**
- ✅ Quản lý đặt phòng (Reservation Management)
- ✅ Xử lý thanh toán (Payment Processing)
- ✅ Quản lý hồ sơ khách hàng (Guest Profiles)
- ✅ Quản lý feedback khách hàng
- ✅ Gửi thông báo cho khách hàng
- ✅ Xem báo cáo cơ bản
- ❌ Không thể quản lý người dùng
- ❌ Không thể cài đặt hệ thống

### 3. 👤 **CUSTOMER** - Khách hàng
```
Email: customer@demo.com
Username: customer
Password: 123456
```

**Quyền truy cập:**
- ✅ Tìm kiếm và đặt phòng
- ✅ Xem lịch sử đặt phòng
- ✅ Quản lý thông tin cá nhân
- ✅ Đánh giá và feedback dịch vụ
- ✅ Xem thông báo từ khách sạn
- ❌ Không thể truy cập Admin Dashboard
- ❌ Không thể quản lý người dùng khác

## 🚀 Cách sử dụng

### Phương pháp 1: Demo Login (Nhanh)
1. Truy cập trang đăng nhập: `http://localhost:5000/Account/Login`
2. Nhấn vào nút **Admin**, **Staff**, hoặc **Customer** trong phần "Demo Accounts"
3. Hệ thống sẽ tự động đăng nhập với tài khoản tương ứng

### Phương pháp 2: Đăng nhập thông thường
1. Truy cập trang đăng nhập: `http://localhost:5000/Account/Login`
2. Nhập email/username và password từ bảng trên
3. Nhấn "Sign In"

### Phương pháp 3: Xem chi tiết tài khoản
1. Truy cập: `http://localhost:5000/Account/DemoAccounts`
2. Xem thông tin chi tiết và chức năng của từng tài khoản
3. Nhấn nút "Đăng nhập với [Role]" để đăng nhập trực tiếp

## 🔧 Tính năng theo từng Role

### Admin Dashboard
- **URL:** `/Admin/Dashboard`
- **Quyền:** Chỉ Admin
- **Chức năng:** Tổng quan hệ thống, thống kê, quản lý tổng thể

### Management Menu
- **URL:** Dropdown "Management" trong navbar
- **Quyền:** Admin và Staff (một số chức năng)
- **Chức năng:**
  - Guest Profiles Management
  - Payment Processing
  - Feedback Management + Statistics
  - Generate Reports (Booking/Payment/Guest)
  - Send Notifications

### User Management
- **URL:** `/UserManagement`
- **Quyền:** Chỉ Admin
- **Chức năng:** CRUD operations cho users

### Room Management
- **URL:** `/Rooms`
- **Quyền:** Admin và Staff
- **Chức năng:** Quản lý phòng, tìm kiếm, đặt phòng

## 📊 Dữ liệu Demo

Hệ thống sử dụng **InMemory Database** nên:
- ✅ Dữ liệu được tạo tự động khi khởi động
- ✅ Có sẵn rooms, room types, amenities mẫu
- ✅ Có sẵn countries và states
- ⚠️ Dữ liệu sẽ bị reset khi restart ứng dụng
- ⚠️ Không lưu trữ vĩnh viễn

## 🔒 Bảo mật

- Tất cả tài khoản demo đều sử dụng password đơn giản: `123456`
- Chỉ dành cho mục đích demo và testing
- Trong production, nên sử dụng password phức tạp hơn
- Hệ thống có phân quyền rõ ràng theo role

## 🌐 URLs quan trọng

- **Trang chủ:** `http://localhost:5000`
- **Đăng nhập:** `http://localhost:5000/Account/Login`
- **Demo Accounts:** `http://localhost:5000/Account/DemoAccounts`
- **Admin Dashboard:** `http://localhost:5000/Admin/Dashboard`
- **Room Search:** `http://localhost:5000/Rooms/Search`

## 💡 Tips

1. **Khám phá theo role:** Đăng nhập với từng role để trải nghiệm các quyền khác nhau
2. **Test workflow:** Thử quy trình đặt phòng từ Customer → Staff xử lý → Admin quản lý
3. **Xem reports:** Sử dụng tài khoản Admin để xem các báo cáo với charts tương tác
4. **Test notifications:** Gửi thông báo từ Staff/Admin và xem từ Customer
5. **Feedback system:** Tạo feedback từ Customer và quản lý từ Staff/Admin

---

**🎯 Mục tiêu:** Trải nghiệm đầy đủ tính năng của Hotel Booking System với 3 góc nhìn khác nhau!
