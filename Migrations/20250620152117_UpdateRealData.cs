using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelBooking.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRealData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 1,
                columns: new[] { "AmenityName", "Category", "CreatedDate", "Description", "Icon" },
                values: new object[] { "WiFi miễn phí", "Technology", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6248), "WiFi tốc độ cao miễn phí trong toàn bộ khách sạn", "wifi" });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 2,
                columns: new[] { "AmenityName", "Category", "CreatedDate", "Description", "Icon" },
                values: new object[] { "Điều hòa không khí", "Comfort", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6250), "Hệ thống điều hòa hiện đại", "ac" });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 3,
                columns: new[] { "AmenityName", "Category", "CreatedDate", "Description", "Icon" },
                values: new object[] { "TV màn hình phẳng", "Entertainment", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6252), "TV LED 55 inch với truyền hình cáp", "tv" });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 4,
                columns: new[] { "Category", "CreatedDate", "Description", "Icon" },
                values: new object[] { "Food & Beverage", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6253), "Tủ lạnh mini với đồ uống và snack", "minibar" });

            migrationBuilder.InsertData(
                table: "Amenities",
                columns: new[] { "AmenityID", "AmenityName", "Category", "CreatedBy", "CreatedDate", "Description", "Icon", "IsActive", "ModifiedBy", "ModifiedDate" },
                values: new object[,]
                {
                    { 5, "Két an toàn", "Security", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6254), "Két sắt điện tử bảo mật cao", "safe", true, null, null },
                    { 6, "Phòng tắm riêng", "Bathroom", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6255), "Phòng tắm đầy đủ tiện nghi với bồn tắm", "bathroom", true, null, null },
                    { 7, "Máy sấy tóc", "Bathroom", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6257), "Máy sấy tóc chuyên nghiệp", "hairdryer", true, null, null },
                    { 8, "Dép đi trong phòng", "Comfort", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6258), "Dép cotton cao cấp", "slippers", true, null, null },
                    { 9, "Áo choàng tắm", "Comfort", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6259), "Áo choàng cotton mềm mại", "bathrobe", true, null, null },
                    { 10, "Bàn làm việc", "Business", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6261), "Bàn làm việc rộng rãi với ghế ergonomic", "desk", true, null, null },
                    { 11, "Ban công riêng", "View", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6262), "Ban công với view đẹp", "balcony", true, null, null },
                    { 12, "Dịch vụ phòng 24/7", "Service", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6264), "Phục vụ đồ ăn uống 24 giờ", "roomservice", true, null, null },
                    { 13, "Máy pha cà phê", "Food & Beverage", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6265), "Máy pha cà phê Nespresso", "coffee", true, null, null },
                    { 14, "Tủ quần áo", "Storage", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6268), "Tủ quần áo rộng rãi với móc treo", "wardrobe", true, null, null },
                    { 15, "Điện thoại", "Communication", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6269), "Điện thoại nội bộ và quốc tế", "phone", true, null, null }
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6188));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6191));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6193));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(5878));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(5890));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6497));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6500));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6503));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6504));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6220));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6223));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6224));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Description", "Price", "ViewType" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6382), "Phòng đơn tiêu chuẩn với view vườn", 1200000.00m, "Garden" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "Description", "Price" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6385), "Phòng đôi tiêu chuẩn với view thành phố", 1300000.00m });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 3,
                columns: new[] { "BedType", "CreatedDate", "Description", "Price", "RoomNumber", "RoomTypeID", "ViewType" },
                values: new object[] { "Single", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6387), "Phòng đơn tiêu chuẩn với view vườn", 1200000.00m, "103", 1, "Garden" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 4,
                columns: new[] { "BedType", "CreatedDate", "Description", "Price", "RoomNumber", "RoomTypeID", "ViewType" },
                values: new object[] { "Double", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6389), "Phòng đôi tiêu chuẩn với view thành phố", 1300000.00m, "104", 1, "City" });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomID", "BedType", "CreatedBy", "CreatedDate", "Description", "IsActive", "ModifiedBy", "ModifiedDate", "Price", "RoomNumber", "RoomTypeID", "Status", "ViewType" },
                values: new object[,]
                {
                    { 5, "Twin", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6390), "Phòng twin với view hồ bơi", true, null, null, 1400000.00m, "105", 1, "Available", "Pool" },
                    { 6, "Queen", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6392), "Phòng deluxe với giường queen và view biển", true, null, null, 1800000.00m, "201", 2, "Available", "Ocean" },
                    { 7, "King", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6394), "Phòng deluxe với giường king và view biển", true, null, null, 1900000.00m, "202", 2, "Available", "Ocean" },
                    { 8, "Queen", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6396), "Phòng deluxe với view thành phố", true, null, null, 1700000.00m, "203", 2, "Available", "City" },
                    { 9, "King", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6398), "Phòng deluxe với view hồ bơi", true, null, null, 1800000.00m, "204", 2, "Available", "Pool" },
                    { 10, "King", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6400), "Phòng deluxe cao cấp với view biển", true, null, null, 1900000.00m, "205", 2, "Available", "Ocean" },
                    { 11, "King", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6402), "Suite cao cấp với view biển toàn cảnh", true, null, null, 3500000.00m, "301", 3, "Available", "Ocean" },
                    { 12, "King", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6403), "Suite với view thành phố", true, null, null, 3200000.00m, "302", 3, "Available", "City" },
                    { 13, "King", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6405), "Presidential Suite với view biển", true, null, null, 3800000.00m, "303", 3, "Available", "Ocean" },
                    { 14, "King", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6407), "Suite với view hồ bơi và vườn", true, null, null, 3300000.00m, "304", 3, "Available", "Pool" },
                    { 15, "King", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6408), "Penthouse Suite với view biển 360 độ", true, null, null, 4000000.00m, "305", 3, "Available", "Ocean" }
                });

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6533));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6536));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6538));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Description", "ServiceName", "Unit", "UnitPrice" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6442), "Buffet sáng phong phú với món Á và Âu", "Buffet sáng", "per person", 350000.00m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "Description", "ServiceName", "UnitPrice" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6444), "Thực đơn bữa trưa giao tận phòng", "Room Service - Bữa trưa", 450000.00m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 3,
                columns: new[] { "CreatedDate", "Description", "ServiceName", "UnitPrice" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6449), "Thực đơn bữa tối cao cấp giao tận phòng", "Room Service - Bữa tối", 650000.00m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 4,
                columns: new[] { "CreatedDate", "Description", "ServiceName", "UnitPrice" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6451), "Nước khoáng nhập khẩu", "Nước uống cao cấp", 50000.00m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 5,
                columns: new[] { "CreatedDate", "Description", "ServiceName", "UnitPrice" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6452), "Các loại nước ngọt cao cấp", "Đồ uống có gas", 80000.00m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 6,
                columns: new[] { "CreatedDate", "Description", "ServiceName", "UnitPrice" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6454), "Dịch vụ giặt ủi chuyên nghiệp", "Giặt ủi cao cấp", 150000.00m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 7,
                columns: new[] { "CreatedDate", "Description", "ServiceName", "UnitPrice" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6455), "Massage toàn thân 90 phút", "Massage thư giãn", 1200000.00m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 8,
                columns: new[] { "CreatedDate", "Description", "ServiceName", "UnitPrice" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6457), "Xe riêng đưa đón sân bay", "Đưa đón sân bay", 800000.00m });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "ServiceID", "Category", "CreatedBy", "CreatedDate", "Description", "IsActive", "ModifiedBy", "ModifiedDate", "ServiceName", "Unit", "UnitPrice" },
                values: new object[,]
                {
                    { 9, "Spa", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6459), "Liệu trình chăm sóc da mặt 60 phút", true, null, null, "Spa chăm sóc da mặt", "per session", 800000.00m },
                    { 10, "Transportation", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6460), "Thuê xe máy theo ngày", true, null, null, "Thuê xe máy", "per day", 200000.00m },
                    { 11, "Tour", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6462), "Tour tham quan thành phố nửa ngày", true, null, null, "Tour thành phố", "per person", 600000.00m },
                    { 12, "Childcare", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6463), "Dịch vụ trông trẻ chuyên nghiệp", true, null, null, "Dịch vụ giữ trẻ", "per hour", 300000.00m },
                    { 13, "Fitness", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6465), "Sử dụng phòng gym và hồ bơi", true, null, null, "Fitness & Gym", "per day", 200000.00m },
                    { 14, "Business", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6467), "Thuê phòng hội nghị với thiết bị", true, null, null, "Dịch vụ hội nghị", "per day", 2000000.00m },
                    { 15, "Event", "System", new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6468), "Tổ chức tiệc cưới trọn gói", true, null, null, "Dịch vụ cưới hỏi", "per event", 50000000.00m }
                });

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "StateID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6157));

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "StateID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6160));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 15);

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 1,
                columns: new[] { "AmenityName", "Category", "CreatedDate", "Description", "Icon" },
                values: new object[] { "WiFi", null, new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9611), "Free WiFi", null });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 2,
                columns: new[] { "AmenityName", "Category", "CreatedDate", "Description", "Icon" },
                values: new object[] { "Air Conditioning", null, new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9613), "Air Conditioning", null });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 3,
                columns: new[] { "AmenityName", "Category", "CreatedDate", "Description", "Icon" },
                values: new object[] { "TV", null, new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9614), "Television", null });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 4,
                columns: new[] { "Category", "CreatedDate", "Description", "Icon" },
                values: new object[] { null, new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9615), "Mini Bar", null });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9536));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9538));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9540));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9055));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9124));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9773));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9799));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9801));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9802));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9575));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9577));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9579));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Description", "Price", "ViewType" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9669), null, 100.00m, "City" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "Description", "Price" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9671), null, 100.00m });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 3,
                columns: new[] { "BedType", "CreatedDate", "Description", "Price", "RoomNumber", "RoomTypeID", "ViewType" },
                values: new object[] { "Queen", new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9672), null, 150.00m, "201", 2, "Ocean" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 4,
                columns: new[] { "BedType", "CreatedDate", "Description", "Price", "RoomNumber", "RoomTypeID", "ViewType" },
                values: new object[] { "King", new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9674), null, 250.00m, "301", 3, "Ocean" });

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9847));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9850));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9852));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Description", "ServiceName", "Unit", "UnitPrice" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9702), "Continental breakfast delivered to room", "Room Service - Breakfast", "per meal", 25.00m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "Description", "ServiceName", "UnitPrice" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9704), "Lunch menu delivered to room", "Room Service - Lunch", 35.00m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 3,
                columns: new[] { "CreatedDate", "Description", "ServiceName", "UnitPrice" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9706), "Dinner menu delivered to room", "Room Service - Dinner", 45.00m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 4,
                columns: new[] { "CreatedDate", "Description", "ServiceName", "UnitPrice" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9710), "Premium bottled water", "Bottled Water", 5.00m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 5,
                columns: new[] { "CreatedDate", "Description", "ServiceName", "UnitPrice" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9711), "Assorted soft drinks", "Soft Drinks", 8.00m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 6,
                columns: new[] { "CreatedDate", "Description", "ServiceName", "UnitPrice" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9713), "Professional laundry and dry cleaning", "Laundry Service", 15.00m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 7,
                columns: new[] { "CreatedDate", "Description", "ServiceName", "UnitPrice" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9714), "60-minute relaxing massage", "Spa Massage", 80.00m });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 8,
                columns: new[] { "CreatedDate", "Description", "ServiceName", "UnitPrice" },
                values: new object[] { new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9717), "Private car to/from airport", "Airport Transfer", 50.00m });

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "StateID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9489));

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "StateID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 18, 15, 480, DateTimeKind.Local).AddTicks(9493));
        }
    }
}
