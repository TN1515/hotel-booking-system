using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddEssentialHotelTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentBatches_PaymentBatchID",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "ServiceCategoryID",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceID1",
                table: "ServiceInventories",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PaymentBatchID",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Payments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodID",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcessedBy",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessedDate",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionReference",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    AuditLogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Operation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrimaryKeyValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangedColumns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequestUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CorrelationId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ApplicationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApplicationVersion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.AuditLogID);
                    table.ForeignKey(
                        name: "FK_AuditLogs_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GuestProfiles",
                columns: table => new
                {
                    GuestProfileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IDType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IDNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmergencyContactName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmergencyContactPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SpecialRequests = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DietaryRestrictions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Preferences = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsVIP = table.Column<bool>(type: "bit", nullable: false),
                    LoyaltyTier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TotalStays = table.Column<int>(type: "int", nullable: false),
                    LastStayDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestProfiles", x => x.GuestProfileID);
                    table.ForeignKey(
                        name: "FK_GuestProfiles_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hotels",
                columns: table => new
                {
                    HotelID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StateID = table.Column<int>(type: "int", nullable: true),
                    CountryID = table.Column<int>(type: "int", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    StarRating = table.Column<int>(type: "int", nullable: false),
                    Amenities = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CheckInTime = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CheckOutTime = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Policies = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.HotelID);
                    table.ForeignKey(
                        name: "FK_Hotels_Countries_CountryID",
                        column: x => x.CountryID,
                        principalTable: "Countries",
                        principalColumn: "CountryID");
                    table.ForeignKey(
                        name: "FK_Hotels_States_StateID",
                        column: x => x.StateID,
                        principalTable: "States",
                        principalColumn: "StateID");
                });

            migrationBuilder.CreateTable(
                name: "LoyaltyPrograms",
                columns: table => new
                {
                    LoyaltyProgramID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PointsPerVND = table.Column<int>(type: "int", nullable: false),
                    MinimumSpend = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TierLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RequiredPoints = table.Column<int>(type: "int", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Benefits = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyPrograms", x => x.LoyaltyProgramID);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    PaymentMethodID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MethodName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MethodType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RequiresVerification = table.Column<bool>(type: "bit", nullable: false),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.PaymentMethodID);
                });

            migrationBuilder.CreateTable(
                name: "RoomChangeRequests",
                columns: table => new
                {
                    RoomChangeRequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationID = table.Column<int>(type: "int", nullable: false),
                    RequestedByUserID = table.Column<int>(type: "int", nullable: false),
                    CurrentRoomID = table.Column<int>(type: "int", nullable: true),
                    RequestedRoomID = table.Column<int>(type: "int", nullable: true),
                    RequestReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RequestDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PreferredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProcessedByUserID = table.Column<int>(type: "int", nullable: true),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessingNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdditionalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ApprovedRoomID = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomChangeRequests", x => x.RoomChangeRequestID);
                    table.ForeignKey(
                        name: "FK_RoomChangeRequests_AspNetUsers_ProcessedByUserID",
                        column: x => x.ProcessedByUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoomChangeRequests_AspNetUsers_RequestedByUserID",
                        column: x => x.RequestedByUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoomChangeRequests_Reservations_ReservationID",
                        column: x => x.ReservationID,
                        principalTable: "Reservations",
                        principalColumn: "ReservationID");
                    table.ForeignKey(
                        name: "FK_RoomChangeRequests_Rooms_ApprovedRoomID",
                        column: x => x.ApprovedRoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID");
                    table.ForeignKey(
                        name: "FK_RoomChangeRequests_Rooms_CurrentRoomID",
                        column: x => x.CurrentRoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID");
                    table.ForeignKey(
                        name: "FK_RoomChangeRequests_Rooms_RequestedRoomID",
                        column: x => x.RequestedRoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID");
                });

            migrationBuilder.CreateTable(
                name: "RoomChanges",
                columns: table => new
                {
                    RoomChangeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationID = table.Column<int>(type: "int", nullable: false),
                    FromRoomID = table.Column<int>(type: "int", nullable: false),
                    ToRoomID = table.Column<int>(type: "int", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangeReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PriceDifference = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ChangedByUserID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomChanges", x => x.RoomChangeID);
                    table.ForeignKey(
                        name: "FK_RoomChanges_AspNetUsers_ChangedByUserID",
                        column: x => x.ChangedByUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoomChanges_Reservations_ReservationID",
                        column: x => x.ReservationID,
                        principalTable: "Reservations",
                        principalColumn: "ReservationID");
                    table.ForeignKey(
                        name: "FK_RoomChanges_Rooms_FromRoomID",
                        column: x => x.FromRoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID");
                    table.ForeignKey(
                        name: "FK_RoomChanges_Rooms_ToRoomID",
                        column: x => x.ToRoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID");
                });

            migrationBuilder.CreateTable(
                name: "ServiceCategories",
                columns: table => new
                {
                    ServiceCategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCategories", x => x.ServiceCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "ServiceHistories",
                columns: table => new
                {
                    ServiceHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ReservationID = table.Column<int>(type: "int", nullable: true),
                    ServiceID = table.Column<int>(type: "int", nullable: false),
                    ServiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SpecialInstructions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequestedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServicedByUserID = table.Column<int>(type: "int", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceHistories", x => x.ServiceHistoryID);
                    table.ForeignKey(
                        name: "FK_ServiceHistories_AspNetUsers_ServicedByUserID",
                        column: x => x.ServicedByUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceHistories_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceHistories_Reservations_ReservationID",
                        column: x => x.ReservationID,
                        principalTable: "Reservations",
                        principalColumn: "ReservationID");
                    table.ForeignKey(
                        name: "FK_ServiceHistories_Services_ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "Services",
                        principalColumn: "ServiceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SettingKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SettingValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DataType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsEncrypted = table.Column<bool>(type: "bit", nullable: false),
                    IsReadOnly = table.Column<bool>(type: "bit", nullable: false),
                    RequiresRestart = table.Column<bool>(type: "bit", nullable: false),
                    ValidationRules = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DefaultValue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAccessDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAccessedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SettingID);
                });

            migrationBuilder.CreateTable(
                name: "SystemLogs",
                columns: table => new
                {
                    SystemLogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    IPAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequestUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HttpMethod = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    ResponseTime = table.Column<long>(type: "bigint", nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrelationId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MachineName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApplicationVersion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLogs", x => x.SystemLogID);
                    table.ForeignKey(
                        name: "FK_SystemLogs_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LoyaltyTransactions",
                columns: table => new
                {
                    LoyaltyTransactionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    LoyaltyProgramID = table.Column<int>(type: "int", nullable: true),
                    ReservationID = table.Column<int>(type: "int", nullable: true),
                    TransactionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PointsEarned = table.Column<int>(type: "int", nullable: false),
                    PointsRedeemed = table.Column<int>(type: "int", nullable: false),
                    PointsBalance = table.Column<int>(type: "int", nullable: false),
                    AmountSpent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyTransactions", x => x.LoyaltyTransactionID);
                    table.ForeignKey(
                        name: "FK_LoyaltyTransactions_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoyaltyTransactions_LoyaltyPrograms_LoyaltyProgramID",
                        column: x => x.LoyaltyProgramID,
                        principalTable: "LoyaltyPrograms",
                        principalColumn: "LoyaltyProgramID");
                    table.ForeignKey(
                        name: "FK_LoyaltyTransactions_Reservations_ReservationID",
                        column: x => x.ReservationID,
                        principalTable: "Reservations",
                        principalColumn: "ReservationID");
                });

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

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "HotelID", "Address", "Amenities", "CheckInTime", "CheckOutTime", "City", "CountryID", "CreatedBy", "CreatedDate", "Description", "Email", "HotelName", "IsActive", "ModifiedBy", "ModifiedDate", "Phone", "Policies", "PostalCode", "StarRating", "StateID", "Website" },
                values: new object[] { 1, "123 Đường Nguyễn Huệ, Quận 1", "WiFi miễn phí, Hồ bơi, Spa, Gym, Nhà hàng, Bar, Dịch vụ phòng 24/7, Đưa đón sân bay", "14:00", "12:00", "Hồ Chí Minh", 1, "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(214), "Khách sạn 5 sao sang trọng tại trung tâm thành phố Hồ Chí Minh với đầy đủ tiện nghi hiện đại và dịch vụ đẳng cấp quốc tế.", "info@grandpalacehotel.vn", "Grand Palace Hotel & Resort", true, null, null, "+84 28 3829 2185", "Không hút thuốc, Không thú cưng, Hủy miễn phí trước 24h", "70000", 5, 1, "https://grandpalacehotel.vn" });

            migrationBuilder.InsertData(
                table: "LoyaltyPrograms",
                columns: new[] { "LoyaltyProgramID", "Benefits", "CreatedBy", "CreatedDate", "Description", "DiscountPercentage", "EndDate", "IsActive", "MinimumSpend", "ModifiedBy", "ModifiedDate", "PointsPerVND", "ProgramName", "RequiredPoints", "StartDate", "TierLevel" },
                values: new object[,]
                {
                    { 1, "Tích điểm cơ bản, Hỗ trợ khách hàng", "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(618), "Chương trình khách hàng thân thiết hạng Đồng", 0m, null, true, 0m, null, null, 1, "Grand Rewards Bronze", 0, new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(616), "Bronze" },
                    { 2, "Giảm giá 5%, Hỗ trợ ưu tiên, Check-out muộn", "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(623), "Chương trình khách hàng thân thiết hạng Bạc", 5m, null, true, 5000000m, null, null, 1, "Grand Rewards Silver", 1000, new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(622), "Silver" },
                    { 3, "Giảm giá 10%, Nâng cấp phòng, Buffet sáng miễn phí", "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(627), "Chương trình khách hàng thân thiết hạng Vàng", 10m, null, true, 15000000m, null, null, 2, "Grand Rewards Gold", 5000, new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(626), "Gold" },
                    { 4, "Giảm giá 15%, Nâng cấp suite, Dịch vụ concierge, Đưa đón sân bay", "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(631), "Chương trình khách hàng thân thiết hạng Bạch Kim", 15m, null, true, 50000000m, null, null, 3, "Grand Rewards Platinum", 15000, new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(630), "Platinum" }
                });

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

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "PaymentMethodID", "CreatedBy", "CreatedDate", "Description", "DisplayOrder", "Icon", "Instructions", "IsActive", "IsOnline", "MethodName", "MethodType", "ModifiedBy", "ModifiedDate", "Provider", "RequiresVerification" },
                values: new object[,]
                {
                    { 1, "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(339), "Thanh toán bằng tiền mặt tại quầy lễ tân", 1, "fas fa-money-bill", "Vui lòng thanh toán tại quầy lễ tân khi check-in hoặc check-out", true, false, "Tiền mặt", "Cash", null, null, "Hotel", false },
                    { 2, "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(561), "Thanh toán bằng thẻ Visa, MasterCard, JCB", 2, "fas fa-credit-card", "Chấp nhận các loại thẻ quốc tế Visa, MasterCard, JCB", true, true, "Thẻ tín dụng/ghi nợ", "Card", null, null, "Bank", true },
                    { 3, "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(564), "Chuyển khoản qua QR Code VietinBank", 3, "fas fa-qrcode", "Quét mã QR để chuyển khoản, sau đó xác nhận thanh toán", true, true, "Chuyển khoản VietinBank", "QR Code", null, null, "VietinBank", true },
                    { 4, "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(567), "Thanh toán qua ví MoMo", 4, "fas fa-mobile-alt", "Sử dụng app MoMo để quét mã QR thanh toán", true, true, "Ví điện tử MoMo", "E-Wallet", null, null, "MoMo", true },
                    { 5, "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(569), "Thanh toán qua ví ZaloPay", 5, "fas fa-wallet", "Sử dụng app ZaloPay để thanh toán", true, true, "ZaloPay", "E-Wallet", null, null, "ZaloPay", true }
                });

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

            migrationBuilder.InsertData(
                table: "ServiceCategories",
                columns: new[] { "ServiceCategoryID", "CategoryName", "CreatedBy", "CreatedDate", "Description", "DisplayOrder", "Icon", "IsActive", "ModifiedBy", "ModifiedDate" },
                values: new object[,]
                {
                    { 1, "Ăn uống", "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(264), "Dịch vụ ăn uống và đồ uống", 1, "fas fa-utensils", true, null, null },
                    { 2, "Spa & Wellness", "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(269), "Dịch vụ chăm sóc sức khỏe và làm đẹp", 2, "fas fa-spa", true, null, null },
                    { 3, "Vận chuyển", "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(272), "Dịch vụ đưa đón và thuê xe", 3, "fas fa-car", true, null, null },
                    { 4, "Giải trí", "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(274), "Dịch vụ giải trí và thể thao", 4, "fas fa-gamepad", true, null, null },
                    { 5, "Dịch vụ khác", "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(276), "Các dịch vụ bổ sung khác", 5, "fas fa-concierge-bell", true, null, null }
                });

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ServiceID1" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(155), null });

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ServiceID1" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(159), null });

            migrationBuilder.UpdateData(
                table: "ServiceInventories",
                keyColumn: "ServiceInventoryID",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ServiceID1" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(162), null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ServiceCategoryID" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(14), null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ServiceCategoryID" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(16), null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ServiceCategoryID" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(18), null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ServiceCategoryID" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(20), null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ServiceCategoryID" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(21), null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 6,
                columns: new[] { "CreatedDate", "ServiceCategoryID" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(23), null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 7,
                columns: new[] { "CreatedDate", "ServiceCategoryID" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(25), null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 8,
                columns: new[] { "CreatedDate", "ServiceCategoryID" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(26), null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 9,
                columns: new[] { "CreatedDate", "ServiceCategoryID" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(28), null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 10,
                columns: new[] { "CreatedDate", "ServiceCategoryID" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(30), null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 11,
                columns: new[] { "CreatedDate", "ServiceCategoryID" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(32), null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 12,
                columns: new[] { "CreatedDate", "ServiceCategoryID" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(33), null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 13,
                columns: new[] { "CreatedDate", "ServiceCategoryID" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(35), null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 14,
                columns: new[] { "CreatedDate", "ServiceCategoryID" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(37), null });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "ServiceID",
                keyValue: 15,
                columns: new[] { "CreatedDate", "ServiceCategoryID" },
                values: new object[] { new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(45), null });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "SettingID", "Category", "CreatedBy", "CreatedDate", "DataType", "DefaultValue", "Description", "DisplayOrder", "IsActive", "IsEncrypted", "IsReadOnly", "LastAccessDate", "LastAccessedBy", "ModifiedBy", "ModifiedDate", "RequiresRestart", "SettingKey", "SettingValue", "ValidationRules" },
                values: new object[,]
                {
                    { 1, "Hotel", "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(671), "String", "Grand Palace Hotel", "Tên khách sạn", 1, true, false, false, null, null, null, null, false, "Hotel.Name", "Grand Palace Hotel & Resort", null },
                    { 2, "Hotel", "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(721), "String", "14:00", "Giờ check-in tiêu chuẩn", 2, true, false, false, null, null, null, null, false, "Hotel.CheckInTime", "14:00", null },
                    { 3, "Hotel", "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(724), "String", "12:00", "Giờ check-out tiêu chuẩn", 3, true, false, false, null, null, null, null, false, "Hotel.CheckOutTime", "12:00", null },
                    { 4, "Payment", "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(727), "String", "", "Số tài khoản VietinBank", 4, true, true, false, null, null, null, null, false, "Payment.VietinBank.AccountNumber", "1038766815877", null },
                    { 5, "Payment", "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(730), "String", "", "Tên tài khoản VietinBank", 5, true, false, false, null, null, null, null, false, "Payment.VietinBank.AccountName", "LUU VAN HIEN", null },
                    { 6, "Notification", "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(732), "Boolean", "true", "Tự động gửi thông báo", 6, true, false, false, null, null, null, null, false, "Notification.AutoSend", "true", null },
                    { 7, "System", "System", new DateTime(2025, 6, 24, 8, 3, 13, 574, DateTimeKind.Local).AddTicks(735), "Boolean", "false", "Chế độ bảo trì hệ thống", 7, true, false, false, null, null, null, null, true, "System.MaintenanceMode", "false", null }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceCategoryID",
                table: "Services",
                column: "ServiceCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInventories_ServiceID1",
                table: "ServiceInventories",
                column: "ServiceID1");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethodID",
                table: "Payments",
                column: "PaymentMethodID");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserID",
                table: "AuditLogs",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_GuestProfiles_UserID",
                table: "GuestProfiles",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_CountryID",
                table: "Hotels",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_StateID",
                table: "Hotels",
                column: "StateID");

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyTransactions_LoyaltyProgramID",
                table: "LoyaltyTransactions",
                column: "LoyaltyProgramID");

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyTransactions_ReservationID",
                table: "LoyaltyTransactions",
                column: "ReservationID");

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyTransactions_UserID",
                table: "LoyaltyTransactions",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomChangeRequests_ApprovedRoomID",
                table: "RoomChangeRequests",
                column: "ApprovedRoomID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomChangeRequests_CurrentRoomID",
                table: "RoomChangeRequests",
                column: "CurrentRoomID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomChangeRequests_ProcessedByUserID",
                table: "RoomChangeRequests",
                column: "ProcessedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomChangeRequests_RequestedByUserID",
                table: "RoomChangeRequests",
                column: "RequestedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomChangeRequests_RequestedRoomID",
                table: "RoomChangeRequests",
                column: "RequestedRoomID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomChangeRequests_ReservationID",
                table: "RoomChangeRequests",
                column: "ReservationID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomChanges_ChangedByUserID",
                table: "RoomChanges",
                column: "ChangedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomChanges_FromRoomID",
                table: "RoomChanges",
                column: "FromRoomID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomChanges_ReservationID",
                table: "RoomChanges",
                column: "ReservationID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomChanges_ToRoomID",
                table: "RoomChanges",
                column: "ToRoomID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistories_ReservationID",
                table: "ServiceHistories",
                column: "ReservationID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistories_ServicedByUserID",
                table: "ServiceHistories",
                column: "ServicedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistories_ServiceID",
                table: "ServiceHistories",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistories_UserID",
                table: "ServiceHistories",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SystemLogs_UserID",
                table: "SystemLogs",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentBatches_PaymentBatchID",
                table: "Payments",
                column: "PaymentBatchID",
                principalTable: "PaymentBatches",
                principalColumn: "PaymentBatchID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentMethods_PaymentMethodID",
                table: "Payments",
                column: "PaymentMethodID",
                principalTable: "PaymentMethods",
                principalColumn: "PaymentMethodID");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceInventories_Services_ServiceID1",
                table: "ServiceInventories",
                column: "ServiceID1",
                principalTable: "Services",
                principalColumn: "ServiceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceCategories_ServiceCategoryID",
                table: "Services",
                column: "ServiceCategoryID",
                principalTable: "ServiceCategories",
                principalColumn: "ServiceCategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentBatches_PaymentBatchID",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentMethods_PaymentMethodID",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceInventories_Services_ServiceID1",
                table: "ServiceInventories");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceCategories_ServiceCategoryID",
                table: "Services");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "GuestProfiles");

            migrationBuilder.DropTable(
                name: "Hotels");

            migrationBuilder.DropTable(
                name: "LoyaltyTransactions");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "RoomChangeRequests");

            migrationBuilder.DropTable(
                name: "RoomChanges");

            migrationBuilder.DropTable(
                name: "ServiceCategories");

            migrationBuilder.DropTable(
                name: "ServiceHistories");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "SystemLogs");

            migrationBuilder.DropTable(
                name: "LoyaltyPrograms");

            migrationBuilder.DropIndex(
                name: "IX_Services_ServiceCategoryID",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_ServiceInventories_ServiceID1",
                table: "ServiceInventories");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentMethodID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ServiceCategoryID",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServiceID1",
                table: "ServiceInventories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentMethodID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ProcessedBy",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ProcessedDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TransactionReference",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentBatchID",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentBatches_PaymentBatchID",
                table: "Payments",
                column: "PaymentBatchID",
                principalTable: "PaymentBatches",
                principalColumn: "PaymentBatchID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
