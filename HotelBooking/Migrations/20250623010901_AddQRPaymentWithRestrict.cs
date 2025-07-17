using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddQRPaymentWithRestrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QRPayments",
                columns: table => new
                {
                    QRPaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    BankCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    QRCodeData = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TransactionDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TransactionReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRPayments", x => x.QRPaymentID);
                    table.ForeignKey(
                        name: "FK_QRPayments_AspNetUsers_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QRPayments_Reservations_ReservationID",
                        column: x => x.ReservationID,
                        principalTable: "Reservations",
                        principalColumn: "ReservationID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9777));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9779));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9780));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9781));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9783));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9784));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9786));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9788));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9789));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9790));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 11,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9792));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 12,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9793));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 13,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9794));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 14,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9795));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 15,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9796));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9705));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9708));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9709));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9264));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9286));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 505, DateTimeKind.Local).AddTicks(45));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 505, DateTimeKind.Local).AddTicks(52));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 505, DateTimeKind.Local).AddTicks(54));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 505, DateTimeKind.Local).AddTicks(57));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9744));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9746));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9747));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9847));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9849));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9851));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9854));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9855));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9857));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9859));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9861));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9862));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9864));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 11,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9866));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 12,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9867));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 13,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9869));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 14,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9872));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 15,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9873));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 505, DateTimeKind.Local).AddTicks(93));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 505, DateTimeKind.Local).AddTicks(96));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 505, DateTimeKind.Local).AddTicks(100));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9923));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9925));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9927));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9929));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9932));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9992));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9994));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9996));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9998));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 11,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9999));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 12,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 505, DateTimeKind.Local).AddTicks(1));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 13,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 505, DateTimeKind.Local).AddTicks(2));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 14,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 505, DateTimeKind.Local).AddTicks(4));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 15,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 505, DateTimeKind.Local).AddTicks(5));

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "StateID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9653));

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "StateID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 23, 8, 9, 0, 504, DateTimeKind.Local).AddTicks(9655));

            migrationBuilder.CreateIndex(
                name: "IX_QRPayments_CreatedByUserID",
                table: "QRPayments",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_QRPayments_ReservationID",
                table: "QRPayments",
                column: "ReservationID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QRPayments");

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6248));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6250));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6252));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6253));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6254));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6255));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6257));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6258));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6259));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6261));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 11,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6262));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 12,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6264));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 13,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6265));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 14,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6268));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 15,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6269));

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
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6382));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6385));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6387));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6389));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6390));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6392));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6394));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6396));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6398));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6400));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 11,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6402));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 12,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6403));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 13,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6405));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 14,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6407));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 15,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6408));

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
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6442));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6444));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6449));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6451));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6452));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6454));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6455));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6457));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6459));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6460));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 11,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6462));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 12,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6463));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 13,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6465));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 14,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6467));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 15,
                column: "CreatedDate",
                value: new DateTime(2025, 6, 20, 22, 21, 16, 942, DateTimeKind.Local).AddTicks(6468));

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
    }
}
