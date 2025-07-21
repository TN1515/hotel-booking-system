using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddReadDateToNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceInventories_Services_ServiceID1",
                table: "ServiceInventories");

            migrationBuilder.DropIndex(
                name: "IX_ServiceInventories_ServiceID1",
                table: "ServiceInventories");

            migrationBuilder.DropColumn(
                name: "ServiceID1",
                table: "ServiceInventories");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReadDate",
                table: "Notifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(4985));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(4988));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(4990));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(4993));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(4995));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(4997));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(4999));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5001));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5003));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5005));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 11,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5007));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 12,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5009));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 13,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5011));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 14,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5013));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 15,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5022));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(4801));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(4807));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(4810));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(4394));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(4443));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5493));

            migrationBuilder.UpdateData(
                table: "LoyaltyPrograms",
                keyColumn: "LoyaltyProgramID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5636), new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5635) });

            migrationBuilder.UpdateData(
                table: "LoyaltyPrograms",
                keyColumn: "LoyaltyProgramID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5643), new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5641) });

            migrationBuilder.UpdateData(
                table: "LoyaltyPrograms",
                keyColumn: "LoyaltyProgramID",
                keyValue: 3,
                columns: new[] { "CreatedDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5647), new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5646) });

            migrationBuilder.UpdateData(
                table: "LoyaltyPrograms",
                keyColumn: "LoyaltyProgramID",
                keyValue: 4,
                columns: new[] { "CreatedDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5652), new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5651) });

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5379));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5386));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5389));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5392));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5574));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5579));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5582));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5585));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5588));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(4925));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(4928));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(4930));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5084));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5109));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5134));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5137));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5140));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5143));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5145));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5148));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5152));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5160));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 11,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5163));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 12,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5165));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 13,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5168));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 14,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5171));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 15,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5174));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5522));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5528));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5531));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5533));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5431));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5436));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5439));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5218));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5221));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5224));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5230));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5233));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5235));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5237));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5239));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5242));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5244));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 11,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5246));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 12,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5248));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 13,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5250));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 14,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5314));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 15,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5333));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5683));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5688));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5692));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5695));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5698));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5701));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(5705));

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "StateID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(4752));

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "StateID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 21, 15, 49, 9, 633, DateTimeKind.Local).AddTicks(4755));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadDate",
                table: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "ServiceID1",
                table: "ServiceInventories",
                type: "int",
                nullable: true);

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
                columns: new[] { "CreatedDate", "ServiceID1" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9105), null });

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ServiceID1" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9111), null });

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ServiceID1" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 32, 47, 259, DateTimeKind.Local).AddTicks(9114), null });

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

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInventories_ServiceID1",
                table: "ServiceInventories",
                column: "ServiceID1");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceInventories_Services_ServiceID1",
                table: "ServiceInventories",
                column: "ServiceID1",
                principalTable: "Services",
                principalColumn: "ServiceID");
        }
    }
}
