# 🗺️ HotelBooking System - Navigation Flow Map

## 📋 **Complete User Journey Map**

### 🏠 **1. HOME PAGE** (`/` or `/Home/Index`)
**Purpose:** Landing page with featured rooms and search
**Navigation Options:**
- ➡️ **Rooms** → Browse all available rooms
- ➡️ **Search** → Search for specific rooms
- ➡️ **Login/Register** → Authentication (if not logged in)
- ➡️ **My Bookings** → View reservations (if logged in)
- ➡️ **Admin Panel** → Admin dashboard (if admin)

### 🛏️ **2. ROOMS SECTION**
#### **2.1 Rooms Index** (`/Rooms/Index`)
- **Purpose:** Browse all available rooms
- **Navigation:**
  - ➡️ **Room Details** → View specific room
  - ➡️ **Book Now** → Start booking process
  - ➡️ **Search** → Filter rooms

#### **2.2 Room Search** (`/Rooms/Search`)
- **Purpose:** Search and filter rooms
- **Navigation:**
  - ➡️ **Search Results** → View filtered results
  - ➡️ **Book Room** → Start booking

### 🔐 **3. AUTHENTICATION FLOW**
#### **3.1 Login** (`/Account/Login`)
- **Purpose:** User authentication
- **Navigation:**
  - ➡️ **Sign Up Tab** → Switch to registration
  - ➡️ **Home** → After successful login
  - ➡️ **Demo Accounts** → Quick login for testing

#### **3.2 Register** (`/Account/Register`)
- **Purpose:** New user registration
- **Navigation:**
  - ➡️ **Sign In Tab** → Switch to login
  - ➡️ **Home** → After successful registration (auto-login)

### 📅 **4. USER BOOKINGS** (Authenticated Users)
#### **4.1 My Bookings** (`/Reservations/Index`)
- **Purpose:** View user's reservations
- **Navigation:**
  - ➡️ **Book New Room** → Go to rooms
  - ➡️ **View Details** → Reservation details
  - ➡️ **Cancel Booking** → Cancel reservation

#### **4.2 Booking Details** (`/Reservations/Details/{id}`)
- **Purpose:** Detailed view of specific reservation
- **Navigation:**
  - ➡️ **Back to Bookings** → Return to list
  - ➡️ **Cancel** → Cancel reservation

### 👨‍💼 **5. ADMIN PANEL** (Admin Users Only)
#### **5.1 Admin Dashboard** (`/Admin/Dashboard`)
- **Purpose:** Overview of hotel operations
- **Navigation:**
  - ➡️ **Reservations** → Manage all bookings
  - ➡️ **Hotels** → Manage properties
  - ➡️ **Rooms** → Manage room inventory
  - ➡️ **Guests** → Manage customer data

#### **5.2 Admin Reservations** (`/Admin/Reservations`)
- **Purpose:** Manage all system reservations
- **Navigation:**
  - ➡️ **Dashboard** → Return to overview
  - ➡️ **Filter/Search** → Find specific bookings
  - ➡️ **View Details** → Reservation management

## 🔄 **Navigation Logic Rules**

### **Authentication-Based Navigation:**
```
NOT LOGGED IN:
├── Home
├── Rooms (Browse/Search)
├── Login
├── Register
└── About/Contact

LOGGED IN (Customer):
├── Home
├── Rooms (Browse/Search/Book)
├── My Bookings
├── Profile Menu
└── Logout

LOGGED IN (Admin):
├── Home
├── Rooms (Browse/Search/Book)
├── My Bookings
├── Admin Panel
│   ├── Dashboard
│   ├── Reservations
│   ├── Hotels
│   ├── Rooms
│   └── Guests
├── Profile Menu
└── Logout
```

### **User Flow Examples:**

#### **New User Journey:**
1. **Home** → View featured rooms
2. **Register** → Create account
3. **Auto-login** → Redirect to Home
4. **Rooms** → Browse available rooms
5. **Book Room** → Make reservation
6. **My Bookings** → View confirmation

#### **Returning User Journey:**
1. **Home** → Landing page
2. **Login** → Authenticate
3. **My Bookings** → Check existing reservations
4. **Rooms** → Book additional rooms
5. **Admin Panel** → Manage system (if admin)

#### **Admin Journey:**
1. **Login** → Admin authentication
2. **Admin Dashboard** → System overview
3. **Reservations** → Manage all bookings
4. **Hotels/Rooms** → Manage inventory
5. **Reports** → Business analytics

## 🎯 **Key Navigation Features:**

### **Smart Navigation:**
- ✅ **Role-based menus** - Different options for guests/customers/admins
- ✅ **Context-aware links** - Relevant actions based on current page
- ✅ **Breadcrumb navigation** - Clear path tracking
- ✅ **Quick actions** - One-click common tasks

### **User Experience:**
- ✅ **Consistent layout** - Same navigation across all pages
- ✅ **Visual feedback** - Active states and hover effects
- ✅ **Mobile responsive** - Works on all devices
- ✅ **Logical grouping** - Related functions grouped together

### **Security:**
- ✅ **Protected routes** - Admin areas require authentication
- ✅ **Role validation** - Users only see allowed options
- ✅ **Secure redirects** - Proper authentication flow
- ✅ **Session management** - Automatic logout handling

This navigation map ensures every user can easily find what they need and complete their tasks efficiently! 🎉
