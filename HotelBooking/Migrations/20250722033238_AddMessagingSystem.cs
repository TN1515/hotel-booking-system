using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddMessagingSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5816));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5820));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5823));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5828));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5830));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5836));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5842));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5845));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5847));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5853));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 11,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5854));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 12,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5859));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 13,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5861));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 14,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5863));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 15,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5865));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5548));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5554));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5557));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(4445));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(4495));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "HotelID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6821));

            migrationBuilder.UpdateData(
                table: "LoyaltyPrograms",
                keyColumn: "LoyaltyProgramID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7235), new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7232) });

            migrationBuilder.UpdateData(
                table: "LoyaltyPrograms",
                keyColumn: "LoyaltyProgramID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7245), new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7244) });

            migrationBuilder.UpdateData(
                table: "LoyaltyPrograms",
                keyColumn: "LoyaltyProgramID",
                keyValue: 3,
                columns: new[] { "CreatedDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7250), new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7249) });

            migrationBuilder.UpdateData(
                table: "LoyaltyPrograms",
                keyColumn: "LoyaltyProgramID",
                keyValue: 4,
                columns: new[] { "CreatedDate", "StartDate" },
                values: new object[] { new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7256), new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7255) });

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6595));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6602));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6606));

            migrationBuilder.UpdateData(
                table: "LoyaltyTiers",
                keyColumn: "LoyaltyTierID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6609));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7048));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7052));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7061));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7064));

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "PaymentMethodID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7068));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5688));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5696));

            migrationBuilder.UpdateData(
                table: "RoomTypes",
                keyColumn: "RoomTypeID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5698));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6027));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6081));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6129));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6136));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6139));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6148));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6150));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6156));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6160));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6163));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 11,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6168));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 12,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6173));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 13,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6176));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 14,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6180));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 15,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6182));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6913));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6922));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6925));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6931));

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "ServiceCategoryID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6936));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6708));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6717));

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6721));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6307));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6313));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6316));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6318));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6321));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6324));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6419));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6422));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6428));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6432));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 11,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6435));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 12,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6440));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 13,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6442));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 14,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6445));

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 15,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(6482));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7335));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7340));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7343));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7349));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7352));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7549));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingID",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(7553));

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "StateID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5420));

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "StateID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 10, 32, 35, 410, DateTimeKind.Local).AddTicks(5427));

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

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
