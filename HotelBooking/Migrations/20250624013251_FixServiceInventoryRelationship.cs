using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Migrations
{
    /// <inheritdoc />
    public partial class FixServiceInventoryRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8642));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8646));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8648));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8650));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8652));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8654));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8656));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8658));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8661));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8663));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 11,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8665));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 12,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8668));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 13,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8670));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 14,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8682));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 15,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8684));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8439));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8443));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8446));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8128));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8148));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9158));

            migrationBuilder.UpdateData(
                table: "LoyaltyPrograms",
                keyColumn: "LoyaltyProgramID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "StartDate" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9326), new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9324) });

            migrationBuilder.UpdateData(
                table: "LoyaltyPrograms",
                keyColumn: "LoyaltyProgramID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "StartDate" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9333), new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9332) });

            migrationBuilder.UpdateData(
                table: "LoyaltyPrograms",
                keyColumn: "LoyaltyProgramID",
                keyValue: 3,
                columns: new[] { "CreatedDate", "StartDate" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9338), new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9337) });

            migrationBuilder.UpdateData(
                table: "LoyaltyPrograms",
                keyColumn: "LoyaltyProgramID",
                keyValue: 4,
                columns: new[] { "CreatedDate", "StartDate" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9342), new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9341) });

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9051));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9056));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9059));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9061));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9264));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9268));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9271));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9274));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9278));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8591));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8594));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8596));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8749));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8761));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8794));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8797));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8799));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8803));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8805));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8808));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8810));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8813));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 11,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8815));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 12,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8818));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 13,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8820));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 14,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8823));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 15,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8826));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9197));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9201));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9203));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9206));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9208));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9105));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9111));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9114));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8888));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8891));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8893));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8896));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8898));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8900));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8903));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8905));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8907));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8910));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 11,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8913));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 12,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8915));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 13,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8984));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 14,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8987));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 15,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9382));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9387));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9390));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9394));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9397));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9401));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9404));

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "StateID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8387));

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "StateID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(8391));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9787));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9789));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9791));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9793));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9795));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9796));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9797));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9799));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9800));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9801));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 11,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9803));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 12,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9804));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 13,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9806));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 14,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9807));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 15,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9808));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9700));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9703));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9705));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9362));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9386));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(214));

            migrationBuilder.UpdateData(
                table: "LoyaltyPrograms",
                keyColumn: "LoyaltyProgramID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "StartDate" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(618), new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(616) });

            migrationBuilder.UpdateData(
                table: "LoyaltyPrograms",
                keyColumn: "LoyaltyProgramID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "StartDate" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(623), new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(622) });

            migrationBuilder.UpdateData(
                table: "LoyaltyPrograms",
                keyColumn: "LoyaltyProgramID",
                keyValue: 3,
                columns: new[] { "CreatedDate", "StartDate" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(627), new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(626) });

            migrationBuilder.UpdateData(
                table: "LoyaltyPrograms",
                keyColumn: "LoyaltyProgramID",
                keyValue: 4,
                columns: new[] { "CreatedDate", "StartDate" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(631), new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(630) });

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(95));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(100));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(101));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(103));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(339));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(561));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(564));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(567));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(569));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9746));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9748));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9750));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9871));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9888));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9909));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9911));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9912));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9914));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9916));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9918));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9920));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9922));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 11,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9924));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 12,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9926));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 13,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9928));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 14,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 15,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9931));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(264));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(269));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(272));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(274));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(276));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(155));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(159));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(162));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(14));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(16));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(18));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(20));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(21));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(23));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(25));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(26));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(28));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(30));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 11,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(32));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 12,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(33));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 13,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(35));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 14,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(37));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 15,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(45));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(671));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(721));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(724));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(727));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(730));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(732));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(735));

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "StateID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9655));

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "StateID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 24, 8, 3, 13, 573, DateTimeKind.Local).AddTicks(9658));
        }
    }
}
