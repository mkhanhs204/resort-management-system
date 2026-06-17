using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PhucNguyenResort.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiningTables",
                columns: table => new
                {
                    TableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Available")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiningTables", x => x.TableId);
                });

            migrationBuilder.CreateTable(
                name: "MenuCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCategories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "RoomCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    PricePerNight = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomCategories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Tours",
                columns: table => new
                {
                    TourId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    MaxCapacity = table.Column<int>(type: "int", nullable: true),
                    DurationHours = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tours", x => x.TourId);
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    MenuItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.MenuItemId);
                    table.CheckConstraint("CK_MenuItems_AvailableRequiresCategory", "[IsAvailable] = 0 OR [CategoryId] IS NOT NULL");
                    table.ForeignKey(
                        name: "FK_MenuItems_MenuCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "MenuCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoomCategoryImages",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomCategoryImages", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_RoomCategoryImages_RoomCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "RoomCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    FloorNumber = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Available")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_Rooms_RoomCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "RoomCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TableReservations",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TableId = table.Column<int>(type: "int", nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReservationTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    PartySize = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableReservations", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_TableReservations_DiningTables_TableId",
                        column: x => x.TableId,
                        principalTable: "DiningTables",
                        principalColumn: "TableId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TableReservations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TourSchedules",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourId = table.Column<int>(type: "int", nullable: false),
                    TourDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    AvailableSlots = table.Column<int>(type: "int", nullable: true),
                    GuideId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourSchedules", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_TourSchedules_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "TourId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourSchedules_Users_GuideId",
                        column: x => x.GuideId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PassportNumber = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PassportImage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ConsentGiven = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletionRequested = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuestCount = table.Column<int>(type: "int", nullable: true),
                    BookingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Reserved"),
                    DepositAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookingGuests",
                columns: table => new
                {
                    BookingGuestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PassportNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingGuests", x => x.BookingGuestId);
                    table.ForeignKey(
                        name: "FK_BookingGuests_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoodOrders",
                columns: table => new
                {
                    FoodOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    OrderedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IsChargedToRoom = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ChargedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodOrders", x => x.FoodOrderId);
                    table.ForeignKey(
                        name: "FK_FoodOrders_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuestFolios",
                columns: table => new
                {
                    FolioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: true),
                    TotalRoomCharges = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    TotalFoodCharges = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    TotalTourCharges = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Unpaid")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestFolios", x => x.FolioId);
                    table.ForeignKey(
                        name: "FK_GuestFolios_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Refunds",
                columns: table => new
                {
                    RefundId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: true),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    RefundReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefundStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refunds", x => x.RefundId);
                    table.ForeignKey(
                        name: "FK_Refunds_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    TourId = table.Column<int>(type: "int", nullable: true),
                    BookingId = table.Column<int>(type: "int", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Reviews_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "TourId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TourBookings",
                columns: table => new
                {
                    TourBookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    Participants = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    BookingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourBookings", x => x.TourBookingId);
                    table.ForeignKey(
                        name: "FK_TourBookings_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourBookings_TourSchedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "TourSchedules",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FoodOrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodOrderId = table.Column<int>(type: "int", nullable: true),
                    MenuItemId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodOrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_FoodOrderItems_FoodOrders_FoodOrderId",
                        column: x => x.FoodOrderId,
                        principalTable: "FoodOrders",
                        principalColumn: "FoodOrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodOrderItems_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "MenuItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KOT",
                columns: table => new
                {
                    KotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodOrderId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KOT", x => x.KotId);
                    table.ForeignKey(
                        name: "FK_KOT_FoodOrders_FoodOrderId",
                        column: x => x.FoodOrderId,
                        principalTable: "FoodOrders",
                        principalColumn: "FoodOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FolioTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FolioId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolioTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_FolioTransactions_GuestFolios_FolioId",
                        column: x => x.FolioId,
                        principalTable: "GuestFolios",
                        principalColumn: "FolioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FolioId = table.Column<int>(type: "int", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FinalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    InvoiceStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Draft"),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoices_GuestFolios_FolioId",
                        column: x => x.FolioId,
                        principalTable: "GuestFolios",
                        principalColumn: "FolioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TransactionCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DiningTables",
                columns: new[] { "TableId", "Capacity", "TableNumber" },
                values: new object[] { 1, 4, "T01" });

            migrationBuilder.InsertData(
                table: "DiningTables",
                columns: new[] { "TableId", "Capacity", "Status", "TableNumber" },
                values: new object[,]
                {
                    { 2, 6, "Occupied", "T02" },
                    { 3, 8, "Reserved", "T03" },
                    { 4, 10, "Maintenance", "T04" }
                });

            migrationBuilder.InsertData(
                table: "DiningTables",
                columns: new[] { "TableId", "Capacity", "TableNumber" },
                values: new object[] { 5, 2, "T05" });

            migrationBuilder.InsertData(
                table: "DiningTables",
                columns: new[] { "TableId", "Capacity", "Status", "TableNumber" },
                values: new object[,]
                {
                    { 6, 4, "Occupied", "T06" },
                    { 7, 6, "Reserved", "T07" },
                    { 8, 8, "Maintenance", "T08" }
                });

            migrationBuilder.InsertData(
                table: "DiningTables",
                columns: new[] { "TableId", "Capacity", "TableNumber" },
                values: new object[] { 9, 10, "T09" });

            migrationBuilder.InsertData(
                table: "DiningTables",
                columns: new[] { "TableId", "Capacity", "Status", "TableNumber" },
                values: new object[,]
                {
                    { 10, 2, "Occupied", "T10" },
                    { 11, 4, "Reserved", "T11" },
                    { 12, 6, "Maintenance", "T12" }
                });

            migrationBuilder.InsertData(
                table: "DiningTables",
                columns: new[] { "TableId", "Capacity", "TableNumber" },
                values: new object[] { 13, 8, "T13" });

            migrationBuilder.InsertData(
                table: "DiningTables",
                columns: new[] { "TableId", "Capacity", "Status", "TableNumber" },
                values: new object[,]
                {
                    { 14, 10, "Occupied", "T14" },
                    { 15, 2, "Reserved", "T15" },
                    { 16, 4, "Maintenance", "T16" }
                });

            migrationBuilder.InsertData(
                table: "DiningTables",
                columns: new[] { "TableId", "Capacity", "TableNumber" },
                values: new object[] { 17, 6, "T17" });

            migrationBuilder.InsertData(
                table: "DiningTables",
                columns: new[] { "TableId", "Capacity", "Status", "TableNumber" },
                values: new object[,]
                {
                    { 18, 8, "Occupied", "T18" },
                    { 19, 10, "Reserved", "T19" },
                    { 20, 2, "Maintenance", "T20" }
                });

            migrationBuilder.InsertData(
                table: "MenuCategories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Breakfast" },
                    { 2, "Appetizer" },
                    { 3, "Soup" },
                    { 4, "Salad" },
                    { 5, "Seafood" },
                    { 6, "Vietnamese Food" },
                    { 7, "Western Food" },
                    { 8, "Vegetarian" },
                    { 9, "Dessert" },
                    { 10, "Coffee" },
                    { 11, "Tea" },
                    { 12, "Juice" },
                    { 13, "Cocktail" },
                    { 14, "Mocktail" },
                    { 15, "Kids Meal" },
                    { 16, "Room Service Set" },
                    { 17, "BBQ" },
                    { 18, "Noodle" },
                    { 19, "Rice Dish" },
                    { 20, "Special Combo" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1, "System Admin" },
                    { 2, "Business Manager" },
                    { 3, "Customer" },
                    { 4, "Receptionist" },
                    { 5, "F&B Staff" },
                    { 6, "Kitchen Staff" },
                    { 7, "Tour Guide" },
                    { 8, "Housekeeping Staff" },
                    { 9, "Maintenance Staff" }
                });

            migrationBuilder.InsertData(
                table: "RoomCategories",
                columns: new[] { "CategoryId", "Capacity", "CategoryName", "Description", "ImageUrl", "PricePerNight" },
                values: new object[,]
                {
                    { 1, 2, "Standard Garden", "Standard Garden category with capacity 2 guest(s).", "/images/rooms/category-01.jpg", 1080000.00m },
                    { 2, 2, "Superior Sea View", "Superior Sea View category with capacity 2 guest(s).", "/images/rooms/category-02.jpg", 1260000.00m },
                    { 3, 2, "Deluxe Pool View", "Deluxe Pool View category with capacity 2 guest(s).", "/images/rooms/category-03.jpg", 1440000.00m },
                    { 4, 3, "Premium Ocean", "Premium Ocean category with capacity 3 guest(s).", "/images/rooms/category-04.jpg", 1620000.00m },
                    { 5, 4, "Family Suite", "Family Suite category with capacity 4 guest(s).", "/images/rooms/category-05.jpg", 1800000.00m },
                    { 6, 4, "Executive Suite", "Executive Suite category with capacity 4 guest(s).", "/images/rooms/category-06.jpg", 1980000.00m },
                    { 7, 6, "Presidential Villa", "Presidential Villa category with capacity 6 guest(s).", "/images/rooms/category-07.jpg", 2160000.00m },
                    { 8, 2, "Honeymoon Villa", "Honeymoon Villa category with capacity 2 guest(s).", "/images/rooms/category-08.jpg", 2340000.00m },
                    { 9, 2, "Bungalow Garden", "Bungalow Garden category with capacity 2 guest(s).", "/images/rooms/category-09.jpg", 2520000.00m },
                    { 10, 3, "Bungalow Beach", "Bungalow Beach category with capacity 3 guest(s).", "/images/rooms/category-10.jpg", 2700000.00m },
                    { 11, 2, "Twin Standard", "Twin Standard category with capacity 2 guest(s).", "/images/rooms/category-11.jpg", 2880000.00m },
                    { 12, 3, "Triple Deluxe", "Triple Deluxe category with capacity 3 guest(s).", "/images/rooms/category-12.jpg", 3060000.00m },
                    { 13, 2, "Accessible Room", "Accessible Room category with capacity 2 guest(s).", "/images/rooms/category-13.jpg", 3240000.00m },
                    { 14, 4, "Connecting Room", "Connecting Room category with capacity 4 guest(s).", "/images/rooms/category-14.jpg", 3420000.00m },
                    { 15, 2, "Long Stay Studio", "Long Stay Studio category with capacity 2 guest(s).", "/images/rooms/category-15.jpg", 3600000.00m },
                    { 16, 2, "Business Room", "Business Room category with capacity 2 guest(s).", "/images/rooms/category-16.jpg", 3780000.00m },
                    { 17, 4, "Penthouse Suite", "Penthouse Suite category with capacity 4 guest(s).", "/images/rooms/category-17.jpg", 3960000.00m },
                    { 18, 5, "Lagoon Villa", "Lagoon Villa category with capacity 5 guest(s).", "/images/rooms/category-18.jpg", 4140000.00m },
                    { 19, 2, "Mountain View", "Mountain View category with capacity 2 guest(s).", "/images/rooms/category-19.jpg", 4320000.00m },
                    { 20, 2, "Budget Room", "Budget Room category with capacity 2 guest(s).", "/images/rooms/category-20.jpg", 4500000.00m }
                });

            migrationBuilder.InsertData(
                table: "Tours",
                columns: new[] { "TourId", "Description", "DurationHours", "ImageUrl", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[,]
                {
                    { 1, "Island Hopping experience for resort guests.", 3, "/images/tours/tour-01.jpg", "Phu Quoc Area 1", 10, 425000.00m, "Island Hopping" },
                    { 2, "Snorkeling Coral Reef experience for resort guests.", 4, "/images/tours/tour-02.jpg", "Phu Quoc Area 2", 12, 500000.00m, "Snorkeling Coral Reef" },
                    { 3, "Sunset Fishing experience for resort guests.", 5, "/images/tours/tour-03.jpg", "Phu Quoc Area 3", 14, 575000.00m, "Sunset Fishing" },
                    { 4, "Pearl Farm Visit experience for resort guests.", 6, "/images/tours/tour-04.jpg", "Phu Quoc Area 4", 16, 650000.00m, "Pearl Farm Visit" },
                    { 5, "Forest Trekking experience for resort guests.", 7, "/images/tours/tour-05.jpg", "Phu Quoc Area 5", 18, 725000.00m, "Forest Trekking" },
                    { 6, "Night Market Food Tour experience for resort guests.", 8, "/images/tours/tour-06.jpg", "Phu Quoc Area 6", 20, 800000.00m, "Night Market Food Tour" },
                    { 7, "Kayak Lagoon experience for resort guests.", 2, "/images/tours/tour-07.jpg", "Phu Quoc Area 7", 22, 875000.00m, "Kayak Lagoon" },
                    { 8, "Luxury Yacht Trip experience for resort guests.", 3, "/images/tours/tour-08.jpg", "Phu Quoc Area 8", 8, 950000.00m, "Luxury Yacht Trip" },
                    { 9, "Historical Prison Tour experience for resort guests.", 4, "/images/tours/tour-09.jpg", "Phu Quoc Area 9", 10, 1025000.00m, "Historical Prison Tour" },
                    { 10, "Pepper Farm Tour experience for resort guests.", 5, "/images/tours/tour-10.jpg", "Phu Quoc Area 10", 12, 1100000.00m, "Pepper Farm Tour" },
                    { 11, "Cable Car Adventure experience for resort guests.", 6, "/images/tours/tour-11.jpg", "Phu Quoc Area 11", 14, 1175000.00m, "Cable Car Adventure" },
                    { 12, "Diving Beginner experience for resort guests.", 7, "/images/tours/tour-12.jpg", "Phu Quoc Area 12", 16, 1250000.00m, "Diving Beginner" },
                    { 13, "Diving Advanced experience for resort guests.", 8, "/images/tours/tour-13.jpg", "Phu Quoc Area 13", 18, 1325000.00m, "Diving Advanced" },
                    { 14, "Private Beach Picnic experience for resort guests.", 2, "/images/tours/tour-14.jpg", "Phu Quoc Area 14", 20, 1400000.00m, "Private Beach Picnic" },
                    { 15, "Photography Tour experience for resort guests.", 3, "/images/tours/tour-15.jpg", "Phu Quoc Area 15", 22, 1475000.00m, "Photography Tour" },
                    { 16, "Wellness Retreat experience for resort guests.", 4, "/images/tours/tour-16.jpg", "Phu Quoc Area 16", 8, 1550000.00m, "Wellness Retreat" },
                    { 17, "Cooking Class Tour experience for resort guests.", 5, "/images/tours/tour-17.jpg", "Phu Quoc Area 17", 10, 1625000.00m, "Cooking Class Tour" },
                    { 18, "Cycling Countryside experience for resort guests.", 6, "/images/tours/tour-18.jpg", "Phu Quoc Area 18", 12, 1700000.00m, "Cycling Countryside" },
                    { 19, "Bird Watching experience for resort guests.", 7, "/images/tours/tour-19.jpg", "Phu Quoc Area 19", 14, 1775000.00m, "Bird Watching" },
                    { 20, "Family Discovery experience for resort guests.", 8, "/images/tours/tour-20.jpg", "Phu Quoc Area 20", 16, 1850000.00m, "Family Discovery" }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "MenuItemId", "CategoryId", "Description", "ImageUrl", "IsAvailable", "ItemName", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Pho Bo prepared by resort kitchen.", "/images/menu/item-01.jpg", true, "Pho Bo", 85000.00m },
                    { 2, 2, "Bun Cha prepared by resort kitchen.", "/images/menu/item-02.jpg", true, "Bun Cha", 110000.00m },
                    { 3, 3, "Spring Rolls prepared by resort kitchen.", "/images/menu/item-03.jpg", true, "Spring Rolls", 135000.00m },
                    { 4, 4, "Seafood Soup prepared by resort kitchen.", "/images/menu/item-04.jpg", true, "Seafood Soup", 160000.00m },
                    { 5, 5, "Caesar Salad prepared by resort kitchen.", "/images/menu/item-05.jpg", true, "Caesar Salad", 185000.00m },
                    { 6, 6, "Grilled Prawns prepared by resort kitchen.", "/images/menu/item-06.jpg", true, "Grilled Prawns", 210000.00m },
                    { 7, 7, "Beef Steak prepared by resort kitchen.", "/images/menu/item-07.jpg", true, "Beef Steak", 235000.00m }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "MenuItemId", "CategoryId", "Description", "ImageUrl", "ItemName", "Price" },
                values: new object[] { 8, 8, "Vegetable Curry prepared by resort kitchen.", "/images/menu/item-08.jpg", "Vegetable Curry", 260000.00m });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "MenuItemId", "CategoryId", "Description", "ImageUrl", "IsAvailable", "ItemName", "Price" },
                values: new object[,]
                {
                    { 9, 9, "Chocolate Cake prepared by resort kitchen.", "/images/menu/item-09.jpg", true, "Chocolate Cake", 285000.00m },
                    { 10, 10, "Vietnamese Coffee prepared by resort kitchen.", "/images/menu/item-10.jpg", true, "Vietnamese Coffee", 310000.00m },
                    { 11, 11, "Lotus Tea prepared by resort kitchen.", "/images/menu/item-11.jpg", true, "Lotus Tea", 335000.00m },
                    { 12, 12, "Mango Juice prepared by resort kitchen.", "/images/menu/item-12.jpg", true, "Mango Juice", 360000.00m }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "MenuItemId", "CategoryId", "Description", "ImageUrl", "ItemName", "Price" },
                values: new object[] { 13, 13, "Sunset Cocktail prepared by resort kitchen.", "/images/menu/item-13.jpg", "Sunset Cocktail", 385000.00m });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "MenuItemId", "CategoryId", "Description", "ImageUrl", "IsAvailable", "ItemName", "Price" },
                values: new object[,]
                {
                    { 14, 14, "Blue Lagoon Mocktail prepared by resort kitchen.", "/images/menu/item-14.jpg", true, "Blue Lagoon Mocktail", 410000.00m },
                    { 15, 15, "Kids Pasta prepared by resort kitchen.", "/images/menu/item-15.jpg", true, "Kids Pasta", 435000.00m },
                    { 16, 16, "Club Sandwich prepared by resort kitchen.", "/images/menu/item-16.jpg", true, "Club Sandwich", 460000.00m },
                    { 17, 17, "BBQ Platter prepared by resort kitchen.", "/images/menu/item-17.jpg", true, "BBQ Platter", 485000.00m },
                    { 18, 18, "Seafood Noodle prepared by resort kitchen.", "/images/menu/item-18.jpg", true, "Seafood Noodle", 510000.00m },
                    { 19, 19, "Chicken Fried Rice prepared by resort kitchen.", "/images/menu/item-19.jpg", true, "Chicken Fried Rice", 535000.00m },
                    { 20, 20, "Resort Combo prepared by resort kitchen.", "/images/menu/item-20.jpg", true, "Resort Combo", 560000.00m }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "CategoryId", "FloorNumber", "RoomNumber" },
                values: new object[] { 1, 1, 1, "101" });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "CategoryId", "FloorNumber", "RoomNumber", "Status" },
                values: new object[,]
                {
                    { 2, 2, 1, "102", "Occupied" },
                    { 3, 3, 1, "103", "Maintenance" },
                    { 4, 4, 1, "104", "NeedsCleaning" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "CategoryId", "FloorNumber", "RoomNumber" },
                values: new object[] { 5, 5, 1, "105" });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "CategoryId", "FloorNumber", "RoomNumber", "Status" },
                values: new object[,]
                {
                    { 6, 6, 2, "106", "Occupied" },
                    { 7, 7, 2, "107", "Maintenance" },
                    { 8, 8, 2, "108", "NeedsCleaning" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "CategoryId", "FloorNumber", "RoomNumber" },
                values: new object[] { 9, 9, 2, "109" });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "CategoryId", "FloorNumber", "RoomNumber", "Status" },
                values: new object[,]
                {
                    { 10, 10, 2, "110", "Occupied" },
                    { 11, 11, 3, "111", "Maintenance" },
                    { 12, 12, 3, "112", "NeedsCleaning" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "CategoryId", "FloorNumber", "RoomNumber" },
                values: new object[] { 13, 13, 3, "113" });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "CategoryId", "FloorNumber", "RoomNumber", "Status" },
                values: new object[,]
                {
                    { 14, 14, 3, "114", "Occupied" },
                    { 15, 15, 3, "115", "Maintenance" },
                    { 16, 16, 4, "116", "NeedsCleaning" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "CategoryId", "FloorNumber", "RoomNumber" },
                values: new object[] { 17, 17, 4, "117" });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "CategoryId", "FloorNumber", "RoomNumber", "Status" },
                values: new object[,]
                {
                    { 18, 18, 4, "118", "Occupied" },
                    { 19, 19, 4, "119", "Maintenance" },
                    { 20, 20, 4, "120", "NeedsCleaning" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "PhoneNumber", "RoleId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 8, 30, 0, 0, DateTimeKind.Unspecified), "user01@phucnguyenresort.test", "Nguyen Van An", true, "AQAAAAIAAYagAAAAEAPfHpZqsicuzNWwEz2A+27fHA4T7jfsm+ZJVi/FMknonhnS15owgiKdE/xqvZZHvA==", "0900000001", 1 },
                    { 2, new DateTime(2026, 1, 2, 8, 30, 0, 0, DateTimeKind.Unspecified), "user02@phucnguyenresort.test", "Tran Thi Binh", true, "AQAAAAIAAYagAAAAEMqJfvDTu1nRZ1tWnVSdC2TxoksCbzZBG0Z5+hSqJBH6ubpqvEb9ytkmbnjrBlCJNQ==", "0900000002", 2 },
                    { 3, new DateTime(2026, 1, 3, 8, 30, 0, 0, DateTimeKind.Unspecified), "user03@phucnguyenresort.test", "Le Minh Chau", true, "AQAAAAIAAYagAAAAEL7OmuUdZtRjIWsMK4GpOru5NwFTcgkWIMT0XsKM12jVPo5pBAI3n0mPC42TwT4gQA==", "0900000003", 3 },
                    { 4, new DateTime(2026, 1, 4, 8, 30, 0, 0, DateTimeKind.Unspecified), "user04@phucnguyenresort.test", "Pham Quoc Dung", true, "AQAAAAIAAYagAAAAEPpO/sBlLtEog0z9muGuZ9dJU2By/al3/9rnCHHgIHGaUn3cCQxp8+rOHVNo771raA==", "0900000004", 4 },
                    { 5, new DateTime(2026, 2, 5, 8, 30, 0, 0, DateTimeKind.Unspecified), "user05@phucnguyenresort.test", "Hoang Thu Ha", true, "AQAAAAIAAYagAAAAELLhWKBA7UEbgmc/qhu3mxKfNo1Y1UI6zVG4H6XqUfwyhBZIvWkZmDZrkXQK/KaAXQ==", "0900000005", 5 },
                    { 6, new DateTime(2026, 2, 6, 8, 30, 0, 0, DateTimeKind.Unspecified), "user06@phucnguyenresort.test", "Vo Tuan Kiet", true, "AQAAAAIAAYagAAAAEMos2lHKwL8xBwVjMAzlIWjtRXvVolAJDK4aTarDmRFhEgqc2mtMqB16Gvi4Y7hd7g==", "0900000006", 6 },
                    { 7, new DateTime(2026, 2, 7, 8, 30, 0, 0, DateTimeKind.Unspecified), "user07@phucnguyenresort.test", "Dang Ngoc Linh", true, "AQAAAAIAAYagAAAAEENQal0elXTjB9MuTiDqZw+Ju1XFx+c6PHLzlv+vB49TGulgOELiwxVLzBLR3zWcsA==", "0900000007", 7 },
                    { 8, new DateTime(2026, 2, 8, 8, 30, 0, 0, DateTimeKind.Unspecified), "user08@phucnguyenresort.test", "Bui Gia Minh", true, "AQAAAAIAAYagAAAAEJT25/CCK5rCrhV6iqZaRiccHs5/2PjxovAkY33vQQnePVOlNtFItn9Ej8hl8/GaVg==", "0900000008", 8 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "PasswordHash", "PhoneNumber", "RoleId" },
                values: new object[] { 9, new DateTime(2026, 3, 9, 8, 30, 0, 0, DateTimeKind.Unspecified), "user09@phucnguyenresort.test", "Do Phuong Nam", "AQAAAAIAAYagAAAAEP/y/42LBSamtT9RhWTarQ8Arul0XOAu1F1fy0D9x/QhZvsyBZsXcw4BK2FianAmlQ==", "0900000009", 9 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "PhoneNumber", "RoleId" },
                values: new object[,]
                {
                    { 10, new DateTime(2026, 3, 10, 8, 30, 0, 0, DateTimeKind.Unspecified), "user10@phucnguyenresort.test", "Ngo Thanh Oanh", true, "AQAAAAIAAYagAAAAEHhfJbFM/03T1KAicxYcV1bTBoKZiD9CS841SnAOmTfifLMab/d2Ler9troq3w/uxQ==", "0900000010", 3 },
                    { 11, new DateTime(2026, 3, 11, 8, 30, 0, 0, DateTimeKind.Unspecified), "user11@phucnguyenresort.test", "Huynh Duc Phat", true, "AQAAAAIAAYagAAAAEPaitqXlGlPfjdR10IkWztfw5bkaSETVCtiz1raV6zzlXBjpPzIMqeY4bfviE6e3Wg==", "0900000011", 3 },
                    { 12, new DateTime(2026, 3, 12, 8, 30, 0, 0, DateTimeKind.Unspecified), "user12@phucnguyenresort.test", "Phan My Quyen", true, "AQAAAAIAAYagAAAAEDGl1/D1FrIqtwdXMp4MHKTcKLhrgp0BM7tRkmCJKU4CLs78GCzRBm6BNMw2g5wsMg==", "0900000012", 3 },
                    { 13, new DateTime(2026, 4, 13, 8, 30, 0, 0, DateTimeKind.Unspecified), "user13@phucnguyenresort.test", "Vu Khanh Son", true, "AQAAAAIAAYagAAAAEOUNXbZx/sSqOf8XhutyA8c0i1jpPfwprTXZMpZ+ddTO0eZ6DeGC6SappK1tnuLQtQ==", "0900000013", 3 },
                    { 14, new DateTime(2026, 4, 14, 8, 30, 0, 0, DateTimeKind.Unspecified), "user14@phucnguyenresort.test", "Truong Hai Tam", true, "AQAAAAIAAYagAAAAEJJGfYgjY5DIU1LvrA7IqfQ9N0dG1VIKmKczDlrjsfeYlLZMV3uGZM3KB8t7jPVC0g==", "0900000014", 3 },
                    { 15, new DateTime(2026, 4, 15, 8, 30, 0, 0, DateTimeKind.Unspecified), "user15@phucnguyenresort.test", "Ly Bao Uyen", true, "AQAAAAIAAYagAAAAELC/3eh2uKrbHlslXEaKmDbBCqjLTpF7KJkmuTokz/4ZZtRftNGdc1r18mlfAWJ/zg==", "0900000015", 3 },
                    { 16, new DateTime(2026, 4, 16, 8, 30, 0, 0, DateTimeKind.Unspecified), "user16@phucnguyenresort.test", "Mai Hoai Viet", true, "AQAAAAIAAYagAAAAEK8Tu2bzYxEFkVglibJ2FMF/v+tZLjtO8PlvonraPzVigrXUv+ezf6txnN9tzmUKRg==", "0900000016", 3 },
                    { 17, new DateTime(2026, 5, 17, 8, 30, 0, 0, DateTimeKind.Unspecified), "user17@phucnguyenresort.test", "Dinh Nhat Vy", true, "AQAAAAIAAYagAAAAEEu1vtHH1DPKJyipEF9CLiGKB5nlEfAALCHhcLHUl6Gj7930Z5fC8qFxmwbLTbFcnA==", "0900000017", 3 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "PasswordHash", "PhoneNumber", "RoleId" },
                values: new object[] { 18, new DateTime(2026, 5, 18, 8, 30, 0, 0, DateTimeKind.Unspecified), "user18@phucnguyenresort.test", "Cao Tien Dat", "AQAAAAIAAYagAAAAEBfvYhV6RZ4eaYPLilejiLRhlW1+Yc+NqGZ5EBekRqifvfY6ys/tZUV/GwRQrpr9sw==", "0900000018", 3 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "PhoneNumber", "RoleId" },
                values: new object[,]
                {
                    { 19, new DateTime(2026, 5, 19, 8, 30, 0, 0, DateTimeKind.Unspecified), "user19@phucnguyenresort.test", "Ta Minh Anh", true, "AQAAAAIAAYagAAAAEGtWiO8pUkr97O2XfZppC3E24zWNciBmmNkqv/H5mHZXU64zaC04ozH9WwBorN8/WA==", "0900000019", 3 },
                    { 20, new DateTime(2026, 5, 20, 8, 30, 0, 0, DateTimeKind.Unspecified), "user20@phucnguyenresort.test", "Lam Gia Bao", true, "AQAAAAIAAYagAAAAEOWoOu1dgyXSeUHjo7zAm5WWbYnrLWNWkMpjSUCTsJ3Vlznm0hcvxtyPQ/7cgwZNng==", "0900000020", 3 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { 1, new DateTime(2026, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), 550000.00m, 2, 1, 2050000.00m, 1 });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount", "UserId" },
                values: new object[,]
                {
                    { 2, "CheckedIn", new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), 600000.00m, 3, 2, 2300000.00m, 2 },
                    { 3, "CheckedOut", new DateTime(2026, 6, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 4, 9, 0, 0, 0, DateTimeKind.Unspecified), 650000.00m, 4, 3, 2550000.00m, 3 },
                    { 4, "Cancelled", new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), 700000.00m, 5, 4, 2800000.00m, 4 },
                    { 5, "NoShow", new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), 750000.00m, 1, 5, 3050000.00m, 5 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { 6, new DateTime(2026, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 7, 9, 0, 0, 0, DateTimeKind.Unspecified), 800000.00m, 2, 6, 3300000.00m, 6 });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount", "UserId" },
                values: new object[,]
                {
                    { 7, "CheckedIn", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), 850000.00m, 3, 7, 3550000.00m, 7 },
                    { 8, "CheckedOut", new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 9, 9, 0, 0, 0, DateTimeKind.Unspecified), 900000.00m, 4, 8, 3800000.00m, 8 },
                    { 9, "Cancelled", new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), 950000.00m, 5, 9, 4050000.00m, 9 },
                    { 10, "NoShow", new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 11, 9, 0, 0, 0, DateTimeKind.Unspecified), 1000000.00m, 1, 10, 4300000.00m, 10 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { 11, new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), 1050000.00m, 2, 11, 4550000.00m, 11 });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount", "UserId" },
                values: new object[,]
                {
                    { 12, "CheckedIn", new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 13, 9, 0, 0, 0, DateTimeKind.Unspecified), 1100000.00m, 3, 12, 4800000.00m, 12 },
                    { 13, "CheckedOut", new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 14, 9, 0, 0, 0, DateTimeKind.Unspecified), 1150000.00m, 4, 13, 5050000.00m, 13 },
                    { 14, "Cancelled", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), 1200000.00m, 5, 14, 5300000.00m, 14 },
                    { 15, "NoShow", new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 16, 9, 0, 0, 0, DateTimeKind.Unspecified), 1250000.00m, 1, 15, 5550000.00m, 15 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { 16, new DateTime(2026, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 17, 9, 0, 0, 0, DateTimeKind.Unspecified), 1300000.00m, 2, 16, 5800000.00m, 16 });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount", "UserId" },
                values: new object[,]
                {
                    { 17, "CheckedIn", new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), 1350000.00m, 3, 17, 6050000.00m, 17 },
                    { 18, "CheckedOut", new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 19, 9, 0, 0, 0, DateTimeKind.Unspecified), 1400000.00m, 4, 18, 6300000.00m, 18 },
                    { 19, "Cancelled", new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), 1450000.00m, 5, 19, 6550000.00m, 19 },
                    { 20, "NoShow", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 21, 9, 0, 0, 0, DateTimeKind.Unspecified), 1500000.00m, 1, 20, 6800000.00m, 20 }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "Message", "SentAt", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, "Sample notification message 01 for booking, payment, or service update.", new DateTime(2026, 6, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), "Notification 01", 1 },
                    { 2, "Sample notification message 02 for booking, payment, or service update.", new DateTime(2026, 6, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), "Notification 02", 2 }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "IsRead", "Message", "SentAt", "Title", "UserId" },
                values: new object[] { 3, true, "Sample notification message 03 for booking, payment, or service update.", new DateTime(2026, 6, 4, 11, 0, 0, 0, DateTimeKind.Unspecified), "Notification 03", 3 });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "Message", "SentAt", "Title", "UserId" },
                values: new object[,]
                {
                    { 4, "Sample notification message 04 for booking, payment, or service update.", new DateTime(2026, 6, 5, 12, 0, 0, 0, DateTimeKind.Unspecified), "Notification 04", 4 },
                    { 5, "Sample notification message 05 for booking, payment, or service update.", new DateTime(2026, 6, 6, 13, 0, 0, 0, DateTimeKind.Unspecified), "Notification 05", 5 }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "IsRead", "Message", "SentAt", "Title", "UserId" },
                values: new object[] { 6, true, "Sample notification message 06 for booking, payment, or service update.", new DateTime(2026, 6, 7, 14, 0, 0, 0, DateTimeKind.Unspecified), "Notification 06", 6 });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "Message", "SentAt", "Title", "UserId" },
                values: new object[,]
                {
                    { 7, "Sample notification message 07 for booking, payment, or service update.", new DateTime(2026, 6, 8, 15, 0, 0, 0, DateTimeKind.Unspecified), "Notification 07", 7 },
                    { 8, "Sample notification message 08 for booking, payment, or service update.", new DateTime(2026, 6, 9, 16, 0, 0, 0, DateTimeKind.Unspecified), "Notification 08", 8 }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "IsRead", "Message", "SentAt", "Title", "UserId" },
                values: new object[] { 9, true, "Sample notification message 09 for booking, payment, or service update.", new DateTime(2026, 6, 10, 17, 0, 0, 0, DateTimeKind.Unspecified), "Notification 09", 9 });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "Message", "SentAt", "Title", "UserId" },
                values: new object[,]
                {
                    { 10, "Sample notification message 10 for booking, payment, or service update.", new DateTime(2026, 6, 11, 8, 0, 0, 0, DateTimeKind.Unspecified), "Notification 10", 10 },
                    { 11, "Sample notification message 11 for booking, payment, or service update.", new DateTime(2026, 6, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), "Notification 11", 11 }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "IsRead", "Message", "SentAt", "Title", "UserId" },
                values: new object[] { 12, true, "Sample notification message 12 for booking, payment, or service update.", new DateTime(2026, 6, 13, 10, 0, 0, 0, DateTimeKind.Unspecified), "Notification 12", 12 });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "Message", "SentAt", "Title", "UserId" },
                values: new object[,]
                {
                    { 13, "Sample notification message 13 for booking, payment, or service update.", new DateTime(2026, 6, 14, 11, 0, 0, 0, DateTimeKind.Unspecified), "Notification 13", 13 },
                    { 14, "Sample notification message 14 for booking, payment, or service update.", new DateTime(2026, 6, 15, 12, 0, 0, 0, DateTimeKind.Unspecified), "Notification 14", 14 }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "IsRead", "Message", "SentAt", "Title", "UserId" },
                values: new object[] { 15, true, "Sample notification message 15 for booking, payment, or service update.", new DateTime(2026, 6, 16, 13, 0, 0, 0, DateTimeKind.Unspecified), "Notification 15", 15 });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "Message", "SentAt", "Title", "UserId" },
                values: new object[,]
                {
                    { 16, "Sample notification message 16 for booking, payment, or service update.", new DateTime(2026, 6, 17, 14, 0, 0, 0, DateTimeKind.Unspecified), "Notification 16", 16 },
                    { 17, "Sample notification message 17 for booking, payment, or service update.", new DateTime(2026, 6, 18, 15, 0, 0, 0, DateTimeKind.Unspecified), "Notification 17", 17 }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "IsRead", "Message", "SentAt", "Title", "UserId" },
                values: new object[] { 18, true, "Sample notification message 18 for booking, payment, or service update.", new DateTime(2026, 6, 19, 16, 0, 0, 0, DateTimeKind.Unspecified), "Notification 18", 18 });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "Message", "SentAt", "Title", "UserId" },
                values: new object[,]
                {
                    { 19, "Sample notification message 19 for booking, payment, or service update.", new DateTime(2026, 6, 20, 17, 0, 0, 0, DateTimeKind.Unspecified), "Notification 19", 19 },
                    { 20, "Sample notification message 20 for booking, payment, or service update.", new DateTime(2026, 6, 21, 8, 0, 0, 0, DateTimeKind.Unspecified), "Notification 20", 20 }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "ReviewId", "BookingId", "Comment", "CreatedAt", "IsVisible", "Rating", "TourId", "UserId" },
                values: new object[,]
                {
                    { 1, null, "Sample review 01 for resort service scenario.", new DateTime(2026, 6, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), true, 4, 1, 1 },
                    { 3, null, "Sample review 03 for resort service scenario.", new DateTime(2026, 6, 18, 18, 0, 0, 0, DateTimeKind.Unspecified), true, 1, 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "ReviewId", "BookingId", "Comment", "CreatedAt", "Rating", "TourId", "UserId" },
                values: new object[] { 5, null, "Sample review 05 for resort service scenario.", new DateTime(2026, 6, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), 3, 5, 5 });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "ReviewId", "BookingId", "Comment", "CreatedAt", "IsVisible", "Rating", "TourId", "UserId" },
                values: new object[,]
                {
                    { 7, null, "Sample review 07 for resort service scenario.", new DateTime(2026, 6, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), true, 5, 7, 7 },
                    { 9, null, "Sample review 09 for resort service scenario.", new DateTime(2026, 6, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), true, 2, 9, 9 },
                    { 11, null, "Sample review 11 for resort service scenario.", new DateTime(2026, 6, 26, 18, 0, 0, 0, DateTimeKind.Unspecified), true, 4, 11, 11 },
                    { 13, null, "Sample review 13 for resort service scenario.", new DateTime(2026, 6, 28, 18, 0, 0, 0, DateTimeKind.Unspecified), true, 1, 13, 13 },
                    { 15, null, "Sample review 15 for resort service scenario.", new DateTime(2026, 6, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), true, 3, 15, 15 },
                    { 17, null, "Sample review 17 for resort service scenario.", new DateTime(2026, 7, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), true, 5, 17, 17 },
                    { 19, null, "Sample review 19 for resort service scenario.", new DateTime(2026, 7, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), true, 2, 19, 19 }
                });

            migrationBuilder.InsertData(
                table: "TableReservations",
                columns: new[] { "ReservationId", "PartySize", "ReservationDate", "ReservationTime", "TableId", "UserId" },
                values: new object[] { 1, 3, new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 30, 0, 0), 1, 1 });

            migrationBuilder.InsertData(
                table: "TableReservations",
                columns: new[] { "ReservationId", "PartySize", "ReservationDate", "ReservationTime", "Status", "TableId", "UserId" },
                values: new object[,]
                {
                    { 2, 4, new DateTime(2026, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), "Confirmed", 2, 2 },
                    { 3, 5, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0), "Completed", 3, 3 },
                    { 4, 6, new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 21, 0, 0, 0), "Cancelled", 4, 4 }
                });

            migrationBuilder.InsertData(
                table: "TableReservations",
                columns: new[] { "ReservationId", "PartySize", "ReservationDate", "ReservationTime", "TableId", "UserId" },
                values: new object[] { 5, 7, new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 17, 30, 0, 0), 5, 5 });

            migrationBuilder.InsertData(
                table: "TableReservations",
                columns: new[] { "ReservationId", "PartySize", "ReservationDate", "ReservationTime", "Status", "TableId", "UserId" },
                values: new object[,]
                {
                    { 6, 2, new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 0, 0, 0), "Confirmed", 6, 6 },
                    { 7, 3, new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), "Completed", 7, 7 },
                    { 8, 4, new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 0, 0, 0), "Cancelled", 8, 8 }
                });

            migrationBuilder.InsertData(
                table: "TableReservations",
                columns: new[] { "ReservationId", "PartySize", "ReservationDate", "ReservationTime", "TableId", "UserId" },
                values: new object[] { 9, 5, new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 21, 30, 0, 0), 9, 9 });

            migrationBuilder.InsertData(
                table: "TableReservations",
                columns: new[] { "ReservationId", "PartySize", "ReservationDate", "ReservationTime", "Status", "TableId", "UserId" },
                values: new object[,]
                {
                    { 10, 6, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 17, 0, 0, 0), "Confirmed", 10, 10 },
                    { 11, 7, new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 30, 0, 0), "Completed", 11, 11 },
                    { 12, 2, new DateTime(2026, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), "Cancelled", 12, 12 }
                });

            migrationBuilder.InsertData(
                table: "TableReservations",
                columns: new[] { "ReservationId", "PartySize", "ReservationDate", "ReservationTime", "TableId", "UserId" },
                values: new object[] { 13, 3, new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0), 13, 13 });

            migrationBuilder.InsertData(
                table: "TableReservations",
                columns: new[] { "ReservationId", "PartySize", "ReservationDate", "ReservationTime", "Status", "TableId", "UserId" },
                values: new object[,]
                {
                    { 14, 4, new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 21, 0, 0, 0), "Confirmed", 14, 14 },
                    { 15, 5, new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 17, 30, 0, 0), "Completed", 15, 15 },
                    { 16, 6, new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 0, 0, 0), "Cancelled", 16, 16 }
                });

            migrationBuilder.InsertData(
                table: "TableReservations",
                columns: new[] { "ReservationId", "PartySize", "ReservationDate", "ReservationTime", "TableId", "UserId" },
                values: new object[] { 17, 7, new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), 17, 17 });

            migrationBuilder.InsertData(
                table: "TableReservations",
                columns: new[] { "ReservationId", "PartySize", "ReservationDate", "ReservationTime", "Status", "TableId", "UserId" },
                values: new object[,]
                {
                    { 18, 2, new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 0, 0, 0), "Confirmed", 18, 18 },
                    { 19, 3, new DateTime(2026, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 21, 30, 0, 0), "Completed", 19, 19 },
                    { 20, 4, new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 17, 0, 0, 0), "Cancelled", 20, 20 }
                });

            migrationBuilder.InsertData(
                table: "TourSchedules",
                columns: new[] { "ScheduleId", "AvailableSlots", "GuideId", "StartTime", "TourDate", "TourId" },
                values: new object[,]
                {
                    { 1, 6, 7, new TimeSpan(0, 8, 30, 0, 0), new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 7, 8, new TimeSpan(0, 9, 0, 0, 0), new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, 8, 9, new TimeSpan(0, 10, 30, 0, 0), new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 4, 9, 10, new TimeSpan(0, 11, 0, 0, 0), new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 5, 10, 11, new TimeSpan(0, 12, 30, 0, 0), new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 6, 11, 12, new TimeSpan(0, 7, 0, 0, 0), new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 7, 12, 13, new TimeSpan(0, 8, 30, 0, 0), new DateTime(2026, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 8, 13, 14, new TimeSpan(0, 9, 0, 0, 0), new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 9, 14, 15, new TimeSpan(0, 10, 30, 0, 0), new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 10, 5, 16, new TimeSpan(0, 11, 0, 0, 0), new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 11, 6, 17, new TimeSpan(0, 12, 30, 0, 0), new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 12, 7, 18, new TimeSpan(0, 7, 0, 0, 0), new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 13, 8, 19, new TimeSpan(0, 8, 30, 0, 0), new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 14, 9, 20, new TimeSpan(0, 9, 0, 0, 0), new DateTime(2026, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 14 },
                    { 15, 10, 1, new TimeSpan(0, 10, 30, 0, 0), new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 15 },
                    { 16, 11, 2, new TimeSpan(0, 11, 0, 0, 0), new DateTime(2026, 6, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 16 },
                    { 17, 12, 3, new TimeSpan(0, 12, 30, 0, 0), new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 17 },
                    { 18, 13, 4, new TimeSpan(0, 7, 0, 0, 0), new DateTime(2026, 6, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 18 },
                    { 19, 14, 5, new TimeSpan(0, 8, 30, 0, 0), new DateTime(2026, 6, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 19 },
                    { 20, 5, 6, new TimeSpan(0, 9, 0, 0, 0), new DateTime(2026, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 20 }
                });

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "ProfileId", "Address", "ConsentGiven", "DateOfBirth", "Nationality", "PassportImage", "PassportNumber", "UserId" },
                values: new object[,]
                {
                    { 1, "1 Bien Xanh Street, Phu Quoc", true, new DateTime(1986, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vietnamese", "/uploads/passports/user-01.jpg", new byte[] { 81, 66, 84, 84, 1 }, 1 },
                    { 2, "2 Bien Xanh Street, Phu Quoc", true, new DateTime(1987, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vietnamese", "/uploads/passports/user-02.jpg", new byte[] { 82, 67, 85, 85, 2 }, 2 }
                });

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "ProfileId", "Address", "DateOfBirth", "Nationality", "PassportImage", "PassportNumber", "UserId" },
                values: new object[] { 3, "3 Bien Xanh Street, Phu Quoc", new DateTime(1988, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vietnamese", "/uploads/passports/user-03.jpg", new byte[] { 83, 68, 86, 86, 3 }, 3 });

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "ProfileId", "Address", "ConsentGiven", "DateOfBirth", "Nationality", "PassportImage", "PassportNumber", "UserId" },
                values: new object[,]
                {
                    { 4, "4 Bien Xanh Street, Phu Quoc", true, new DateTime(1989, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vietnamese", "/uploads/passports/user-04.jpg", new byte[] { 84, 69, 87, 87, 4 }, 4 },
                    { 5, "5 Bien Xanh Street, Phu Quoc", true, new DateTime(1990, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vietnamese", "/uploads/passports/user-05.jpg", new byte[] { 85, 70, 88, 88, 5 }, 5 },
                    { 6, "6 Bien Xanh Street, Phu Quoc", true, new DateTime(1991, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Korean", "/uploads/passports/user-06.jpg", new byte[] { 86, 71, 89, 89, 6 }, 6 },
                    { 7, "7 Bien Xanh Street, Phu Quoc", true, new DateTime(1992, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Japanese", "/uploads/passports/user-07.jpg", new byte[] { 87, 72, 90, 90, 7 }, 7 },
                    { 8, "8 Bien Xanh Street, Phu Quoc", true, new DateTime(1993, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "American", "/uploads/passports/user-08.jpg", new byte[] { 88, 73, 91, 91, 8 }, 8 },
                    { 9, "9 Bien Xanh Street, Phu Quoc", true, new DateTime(1994, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "French", "/uploads/passports/user-09.jpg", new byte[] { 89, 74, 92, 92, 9 }, 9 },
                    { 10, "10 Bien Xanh Street, Phu Quoc", true, new DateTime(1995, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "German", "/uploads/passports/user-10.jpg", new byte[] { 90, 75, 93, 93, 10 }, 10 },
                    { 11, "11 Bien Xanh Street, Phu Quoc", true, new DateTime(1996, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Australian", "/uploads/passports/user-11.jpg", new byte[] { 91, 76, 94, 94, 11 }, 11 },
                    { 12, "12 Bien Xanh Street, Phu Quoc", true, new DateTime(1997, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Singaporean", "/uploads/passports/user-12.jpg", new byte[] { 92, 77, 95, 95, 12 }, 12 },
                    { 13, "13 Bien Xanh Street, Phu Quoc", true, new DateTime(1998, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thai", "/uploads/passports/user-13.jpg", new byte[] { 93, 78, 96, 96, 13 }, 13 }
                });

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "ProfileId", "Address", "DateOfBirth", "DeletionRequested", "Nationality", "PassportImage", "PassportNumber", "UserId" },
                values: new object[] { 14, "14 Bien Xanh Street, Phu Quoc", new DateTime(1999, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Malaysian", "/uploads/passports/user-14.jpg", new byte[] { 94, 79, 97, 97, 14 }, 14 });

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "ProfileId", "Address", "ConsentGiven", "DateOfBirth", "Nationality", "PassportImage", "PassportNumber", "UserId" },
                values: new object[,]
                {
                    { 15, "15 Bien Xanh Street, Phu Quoc", true, new DateTime(2000, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "British", "/uploads/passports/user-15.jpg", new byte[] { 95, 80, 98, 98, 15 }, 15 },
                    { 16, "16 Bien Xanh Street, Phu Quoc", true, new DateTime(2001, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Canadian", "/uploads/passports/user-16.jpg", new byte[] { 96, 81, 99, 99, 16 }, 16 },
                    { 17, "17 Bien Xanh Street, Phu Quoc", true, new DateTime(2002, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Indian", "/uploads/passports/user-17.jpg", new byte[] { 97, 82, 100, 100, 17 }, 17 },
                    { 18, "18 Bien Xanh Street, Phu Quoc", true, new DateTime(2003, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vietnamese", "/uploads/passports/user-18.jpg", new byte[] { 98, 83, 101, 101, 18 }, 18 },
                    { 19, "19 Bien Xanh Street, Phu Quoc", true, new DateTime(2004, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vietnamese", "/uploads/passports/user-19.jpg", new byte[] { 99, 84, 102, 102, 19 }, 19 },
                    { 20, "20 Bien Xanh Street, Phu Quoc", true, new DateTime(1985, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vietnamese", "/uploads/passports/user-20.jpg", new byte[] { 100, 85, 103, 103, 20 }, 20 }
                });

            migrationBuilder.InsertData(
                table: "BookingGuests",
                columns: new[] { "BookingGuestId", "BookingId", "DateOfBirth", "FullName", "Nationality", "PassportNumber" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(1991, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 01", "Korean", "P20260001" },
                    { 2, 2, new DateTime(1992, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 02", "Japanese", "P20260002" },
                    { 3, 3, new DateTime(1993, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 03", "American", "P20260003" },
                    { 4, 4, new DateTime(1994, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 04", "French", "P20260004" },
                    { 5, 5, new DateTime(1995, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 05", "German", "P20260005" },
                    { 6, 6, new DateTime(1996, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 06", "Australian", "P20260006" },
                    { 7, 7, new DateTime(1997, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 07", "Singaporean", "P20260007" },
                    { 8, 8, new DateTime(1998, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 08", "Thai", "P20260008" },
                    { 9, 9, new DateTime(1999, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 09", "Malaysian", "P20260009" },
                    { 10, 10, new DateTime(2000, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 10", "British", "P20260010" },
                    { 11, 11, new DateTime(2001, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 11", "Canadian", "P20260011" },
                    { 12, 12, new DateTime(2002, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 12", "Indian", "P20260012" },
                    { 13, 13, new DateTime(2003, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 13", "Vietnamese", "P20260013" },
                    { 14, 14, new DateTime(2004, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 14", "Vietnamese", "P20260014" },
                    { 15, 15, new DateTime(1990, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 15", "Vietnamese", "P20260015" },
                    { 16, 16, new DateTime(1991, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 16", "Vietnamese", "P20260016" },
                    { 17, 17, new DateTime(1992, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 17", "Vietnamese", "P20260017" },
                    { 18, 18, new DateTime(1993, 9, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 18", "Vietnamese", "P20260018" },
                    { 19, 19, new DateTime(1994, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 19", "Vietnamese", "P20260019" },
                    { 20, 20, new DateTime(1995, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest Companion 20", "Vietnamese", "P20260020" }
                });

            migrationBuilder.InsertData(
                table: "FoodOrders",
                columns: new[] { "FoodOrderId", "BookingId", "ChargedAt", "OrderedAt", "TotalAmount" },
                values: new object[] { 1, 1, null, new DateTime(2026, 6, 3, 13, 15, 0, 0, DateTimeKind.Unspecified), 170000.00m });

            migrationBuilder.InsertData(
                table: "FoodOrders",
                columns: new[] { "FoodOrderId", "BookingId", "ChargedAt", "IsChargedToRoom", "OrderStatus", "OrderedAt", "TotalAmount" },
                values: new object[] { 2, 2, new DateTime(2026, 6, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), true, "Preparing", new DateTime(2026, 6, 4, 14, 15, 0, 0, DateTimeKind.Unspecified), 330000.00m });

            migrationBuilder.InsertData(
                table: "FoodOrders",
                columns: new[] { "FoodOrderId", "BookingId", "ChargedAt", "OrderStatus", "OrderedAt", "TotalAmount" },
                values: new object[,]
                {
                    { 3, 3, null, "Ready", new DateTime(2026, 6, 5, 15, 15, 0, 0, DateTimeKind.Unspecified), 540000.00m },
                    { 4, 4, null, "Served", new DateTime(2026, 6, 6, 16, 15, 0, 0, DateTimeKind.Unspecified), 160000.00m },
                    { 5, 5, null, "Cancelled", new DateTime(2026, 6, 7, 17, 15, 0, 0, DateTimeKind.Unspecified), 370000.00m }
                });

            migrationBuilder.InsertData(
                table: "FoodOrders",
                columns: new[] { "FoodOrderId", "BookingId", "ChargedAt", "OrderedAt", "TotalAmount" },
                values: new object[] { 6, 6, null, new DateTime(2026, 6, 8, 18, 15, 0, 0, DateTimeKind.Unspecified), 630000.00m });

            migrationBuilder.InsertData(
                table: "FoodOrders",
                columns: new[] { "FoodOrderId", "BookingId", "ChargedAt", "OrderStatus", "OrderedAt", "TotalAmount" },
                values: new object[] { 7, 7, null, "Preparing", new DateTime(2026, 6, 9, 19, 15, 0, 0, DateTimeKind.Unspecified), 940000.00m });

            migrationBuilder.InsertData(
                table: "FoodOrders",
                columns: new[] { "FoodOrderId", "BookingId", "ChargedAt", "IsChargedToRoom", "OrderStatus", "OrderedAt", "TotalAmount" },
                values: new object[] { 8, 8, new DateTime(2026, 6, 24, 10, 30, 0, 0, DateTimeKind.Unspecified), true, "Ready", new DateTime(2026, 6, 10, 12, 15, 0, 0, DateTimeKind.Unspecified), 260000.00m });

            migrationBuilder.InsertData(
                table: "FoodOrders",
                columns: new[] { "FoodOrderId", "BookingId", "ChargedAt", "OrderStatus", "OrderedAt", "TotalAmount" },
                values: new object[,]
                {
                    { 9, 9, null, "Served", new DateTime(2026, 6, 11, 13, 15, 0, 0, DateTimeKind.Unspecified), 570000.00m },
                    { 10, 10, null, "Cancelled", new DateTime(2026, 6, 12, 14, 15, 0, 0, DateTimeKind.Unspecified), 930000.00m }
                });

            migrationBuilder.InsertData(
                table: "FoodOrders",
                columns: new[] { "FoodOrderId", "BookingId", "ChargedAt", "OrderedAt", "TotalAmount" },
                values: new object[] { 11, 11, null, new DateTime(2026, 6, 13, 15, 15, 0, 0, DateTimeKind.Unspecified), 1340000.00m });

            migrationBuilder.InsertData(
                table: "FoodOrders",
                columns: new[] { "FoodOrderId", "BookingId", "ChargedAt", "OrderStatus", "OrderedAt", "TotalAmount" },
                values: new object[,]
                {
                    { 12, 12, null, "Preparing", new DateTime(2026, 6, 14, 16, 15, 0, 0, DateTimeKind.Unspecified), 360000.00m },
                    { 13, 13, null, "Ready", new DateTime(2026, 6, 15, 17, 15, 0, 0, DateTimeKind.Unspecified), 770000.00m }
                });

            migrationBuilder.InsertData(
                table: "FoodOrders",
                columns: new[] { "FoodOrderId", "BookingId", "ChargedAt", "IsChargedToRoom", "OrderStatus", "OrderedAt", "TotalAmount" },
                values: new object[] { 14, 14, new DateTime(2026, 6, 30, 10, 30, 0, 0, DateTimeKind.Unspecified), true, "Served", new DateTime(2026, 6, 16, 18, 15, 0, 0, DateTimeKind.Unspecified), 1230000.00m });

            migrationBuilder.InsertData(
                table: "FoodOrders",
                columns: new[] { "FoodOrderId", "BookingId", "ChargedAt", "OrderStatus", "OrderedAt", "TotalAmount" },
                values: new object[] { 15, 15, null, "Cancelled", new DateTime(2026, 6, 17, 19, 15, 0, 0, DateTimeKind.Unspecified), 1740000.00m });

            migrationBuilder.InsertData(
                table: "FoodOrders",
                columns: new[] { "FoodOrderId", "BookingId", "ChargedAt", "OrderedAt", "TotalAmount" },
                values: new object[] { 16, 16, null, new DateTime(2026, 6, 18, 12, 15, 0, 0, DateTimeKind.Unspecified), 460000.00m });

            migrationBuilder.InsertData(
                table: "FoodOrders",
                columns: new[] { "FoodOrderId", "BookingId", "ChargedAt", "OrderStatus", "OrderedAt", "TotalAmount" },
                values: new object[,]
                {
                    { 17, 17, null, "Preparing", new DateTime(2026, 6, 19, 13, 15, 0, 0, DateTimeKind.Unspecified), 970000.00m },
                    { 18, 18, null, "Ready", new DateTime(2026, 6, 20, 14, 15, 0, 0, DateTimeKind.Unspecified), 1530000.00m },
                    { 19, 19, null, "Served", new DateTime(2026, 6, 21, 15, 15, 0, 0, DateTimeKind.Unspecified), 2140000.00m }
                });

            migrationBuilder.InsertData(
                table: "FoodOrders",
                columns: new[] { "FoodOrderId", "BookingId", "ChargedAt", "IsChargedToRoom", "OrderStatus", "OrderedAt", "TotalAmount" },
                values: new object[] { 20, 20, new DateTime(2026, 7, 6, 10, 30, 0, 0, DateTimeKind.Unspecified), true, "Cancelled", new DateTime(2026, 6, 22, 16, 15, 0, 0, DateTimeKind.Unspecified), 560000.00m });

            migrationBuilder.InsertData(
                table: "GuestFolios",
                columns: new[] { "FolioId", "BookingId", "TotalAmount", "TotalFoodCharges", "TotalRoomCharges", "TotalTourCharges" },
                values: new object[] { 1, 1, 3070000.00m, 170000.00m, 2050000.00m, 850000.00m });

            migrationBuilder.InsertData(
                table: "GuestFolios",
                columns: new[] { "FolioId", "BookingId", "Status", "TotalAmount", "TotalFoodCharges", "TotalRoomCharges", "TotalTourCharges" },
                values: new object[,]
                {
                    { 2, 2, "PartialPaid", 4130000.00m, 330000.00m, 2300000.00m, 1500000.00m },
                    { 3, 3, "Paid", 5390000.00m, 540000.00m, 2550000.00m, 2300000.00m },
                    { 4, 4, "Refunded", 6210000.00m, 160000.00m, 2800000.00m, 3250000.00m },
                    { 5, 5, "Cancelled", 4145000.00m, 370000.00m, 3050000.00m, 725000.00m }
                });

            migrationBuilder.InsertData(
                table: "GuestFolios",
                columns: new[] { "FolioId", "BookingId", "TotalAmount", "TotalFoodCharges", "TotalRoomCharges", "TotalTourCharges" },
                values: new object[] { 6, 6, 5530000.00m, 630000.00m, 3300000.00m, 1600000.00m });

            migrationBuilder.InsertData(
                table: "GuestFolios",
                columns: new[] { "FolioId", "BookingId", "Status", "TotalAmount", "TotalFoodCharges", "TotalRoomCharges", "TotalTourCharges" },
                values: new object[,]
                {
                    { 7, 7, "PartialPaid", 7115000.00m, 940000.00m, 3550000.00m, 2625000.00m },
                    { 8, 8, "Paid", 7860000.00m, 260000.00m, 3800000.00m, 3800000.00m },
                    { 9, 9, "Refunded", 9745000.00m, 570000.00m, 4050000.00m, 5125000.00m },
                    { 10, 10, "Cancelled", 6330000.00m, 930000.00m, 4300000.00m, 1100000.00m }
                });

            migrationBuilder.InsertData(
                table: "GuestFolios",
                columns: new[] { "FolioId", "BookingId", "TotalAmount", "TotalFoodCharges", "TotalRoomCharges", "TotalTourCharges" },
                values: new object[] { 11, 11, 8240000.00m, 1340000.00m, 4550000.00m, 2350000.00m });

            migrationBuilder.InsertData(
                table: "GuestFolios",
                columns: new[] { "FolioId", "BookingId", "Status", "TotalAmount", "TotalFoodCharges", "TotalRoomCharges", "TotalTourCharges" },
                values: new object[,]
                {
                    { 12, 12, "PartialPaid", 8910000.00m, 360000.00m, 4800000.00m, 3750000.00m },
                    { 13, 13, "Paid", 11120000.00m, 770000.00m, 5050000.00m, 5300000.00m },
                    { 14, 14, "Refunded", 13530000.00m, 1230000.00m, 5300000.00m, 7000000.00m },
                    { 15, 15, "Cancelled", 8765000.00m, 1740000.00m, 5550000.00m, 1475000.00m }
                });

            migrationBuilder.InsertData(
                table: "GuestFolios",
                columns: new[] { "FolioId", "BookingId", "TotalAmount", "TotalFoodCharges", "TotalRoomCharges", "TotalTourCharges" },
                values: new object[] { 16, 16, 9360000.00m, 460000.00m, 5800000.00m, 3100000.00m });

            migrationBuilder.InsertData(
                table: "GuestFolios",
                columns: new[] { "FolioId", "BookingId", "Status", "TotalAmount", "TotalFoodCharges", "TotalRoomCharges", "TotalTourCharges" },
                values: new object[,]
                {
                    { 17, 17, "PartialPaid", 11895000.00m, 970000.00m, 6050000.00m, 4875000.00m },
                    { 18, 18, "Paid", 14630000.00m, 1530000.00m, 6300000.00m, 6800000.00m },
                    { 19, 19, "Refunded", 17565000.00m, 2140000.00m, 6550000.00m, 8875000.00m },
                    { 20, 20, "Cancelled", 9210000.00m, 560000.00m, 6800000.00m, 1850000.00m }
                });

            migrationBuilder.InsertData(
                table: "Refunds",
                columns: new[] { "RefundId", "BookingId", "CreatedAt", "RefundAmount", "RefundReason", "RefundStatus" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 6, 21, 11, 0, 0, 0, DateTimeKind.Unspecified), 125000.00m, "Room issue compensation", "Pending" },
                    { 2, 2, new DateTime(2026, 6, 22, 11, 0, 0, 0, DateTimeKind.Unspecified), 150000.00m, "Tour cancelled by weather", "Approved" },
                    { 3, 3, new DateTime(2026, 6, 23, 11, 0, 0, 0, DateTimeKind.Unspecified), 175000.00m, "Payment duplicate", "Rejected" },
                    { 4, 4, new DateTime(2026, 6, 24, 11, 0, 0, 0, DateTimeKind.Unspecified), 200000.00m, "Guest cancelled early", "Completed" },
                    { 5, 5, new DateTime(2026, 6, 25, 11, 0, 0, 0, DateTimeKind.Unspecified), 225000.00m, "Room issue compensation", "Pending" },
                    { 6, 6, new DateTime(2026, 6, 26, 11, 0, 0, 0, DateTimeKind.Unspecified), 250000.00m, "Tour cancelled by weather", "Approved" },
                    { 7, 7, new DateTime(2026, 6, 27, 11, 0, 0, 0, DateTimeKind.Unspecified), 275000.00m, "Payment duplicate", "Rejected" },
                    { 8, 8, new DateTime(2026, 6, 28, 11, 0, 0, 0, DateTimeKind.Unspecified), 300000.00m, "Guest cancelled early", "Completed" },
                    { 9, 9, new DateTime(2026, 6, 29, 11, 0, 0, 0, DateTimeKind.Unspecified), 325000.00m, "Room issue compensation", "Pending" },
                    { 10, 10, new DateTime(2026, 6, 30, 11, 0, 0, 0, DateTimeKind.Unspecified), 350000.00m, "Tour cancelled by weather", "Approved" },
                    { 11, 11, new DateTime(2026, 7, 1, 11, 0, 0, 0, DateTimeKind.Unspecified), 375000.00m, "Payment duplicate", "Rejected" },
                    { 12, 12, new DateTime(2026, 7, 2, 11, 0, 0, 0, DateTimeKind.Unspecified), 400000.00m, "Guest cancelled early", "Completed" },
                    { 13, 13, new DateTime(2026, 7, 3, 11, 0, 0, 0, DateTimeKind.Unspecified), 425000.00m, "Room issue compensation", "Pending" },
                    { 14, 14, new DateTime(2026, 7, 4, 11, 0, 0, 0, DateTimeKind.Unspecified), 450000.00m, "Tour cancelled by weather", "Approved" },
                    { 15, 15, new DateTime(2026, 7, 5, 11, 0, 0, 0, DateTimeKind.Unspecified), 475000.00m, "Payment duplicate", "Rejected" },
                    { 16, 16, new DateTime(2026, 7, 6, 11, 0, 0, 0, DateTimeKind.Unspecified), 500000.00m, "Guest cancelled early", "Completed" },
                    { 17, 17, new DateTime(2026, 7, 7, 11, 0, 0, 0, DateTimeKind.Unspecified), 525000.00m, "Room issue compensation", "Pending" },
                    { 18, 18, new DateTime(2026, 7, 8, 11, 0, 0, 0, DateTimeKind.Unspecified), 550000.00m, "Tour cancelled by weather", "Approved" },
                    { 19, 19, new DateTime(2026, 7, 9, 11, 0, 0, 0, DateTimeKind.Unspecified), 575000.00m, "Payment duplicate", "Rejected" },
                    { 20, 20, new DateTime(2026, 7, 10, 11, 0, 0, 0, DateTimeKind.Unspecified), 600000.00m, "Guest cancelled early", "Completed" }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "ReviewId", "BookingId", "Comment", "CreatedAt", "IsVisible", "Rating", "TourId", "UserId" },
                values: new object[,]
                {
                    { 2, 2, "Sample review 02 for resort service scenario.", new DateTime(2026, 6, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), true, 5, null, 2 },
                    { 4, 4, "Sample review 04 for resort service scenario.", new DateTime(2026, 6, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), true, 2, null, 4 },
                    { 6, 6, "Sample review 06 for resort service scenario.", new DateTime(2026, 6, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), true, 4, null, 6 },
                    { 8, 8, "Sample review 08 for resort service scenario.", new DateTime(2026, 6, 23, 18, 0, 0, 0, DateTimeKind.Unspecified), true, 1, null, 8 },
                    { 10, 10, "Sample review 10 for resort service scenario.", new DateTime(2026, 6, 25, 18, 0, 0, 0, DateTimeKind.Unspecified), true, 3, null, 10 },
                    { 12, 12, "Sample review 12 for resort service scenario.", new DateTime(2026, 6, 27, 18, 0, 0, 0, DateTimeKind.Unspecified), true, 5, null, 12 },
                    { 14, 14, "Sample review 14 for resort service scenario.", new DateTime(2026, 6, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), true, 2, null, 14 }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "ReviewId", "BookingId", "Comment", "CreatedAt", "Rating", "TourId", "UserId" },
                values: new object[] { 16, 16, "Sample review 16 for resort service scenario.", new DateTime(2026, 7, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), 4, null, 16 });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "ReviewId", "BookingId", "Comment", "CreatedAt", "IsVisible", "Rating", "TourId", "UserId" },
                values: new object[,]
                {
                    { 18, 18, "Sample review 18 for resort service scenario.", new DateTime(2026, 7, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), true, 1, null, 18 },
                    { 20, 20, "Sample review 20 for resort service scenario.", new DateTime(2026, 7, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), true, 3, null, 20 }
                });

            migrationBuilder.InsertData(
                table: "TourBookings",
                columns: new[] { "TourBookingId", "BookingId", "Participants", "ScheduleId", "TotalAmount" },
                values: new object[] { 1, 1, 2, 1, 850000.00m });

            migrationBuilder.InsertData(
                table: "TourBookings",
                columns: new[] { "TourBookingId", "BookingId", "BookingStatus", "Participants", "ScheduleId", "TotalAmount" },
                values: new object[,]
                {
                    { 2, 2, "Confirmed", 3, 2, 1500000.00m },
                    { 3, 3, "Completed", 4, 3, 2300000.00m },
                    { 4, 4, "Cancelled", 5, 4, 3250000.00m }
                });

            migrationBuilder.InsertData(
                table: "TourBookings",
                columns: new[] { "TourBookingId", "BookingId", "Participants", "ScheduleId", "TotalAmount" },
                values: new object[] { 5, 5, 1, 5, 725000.00m });

            migrationBuilder.InsertData(
                table: "TourBookings",
                columns: new[] { "TourBookingId", "BookingId", "BookingStatus", "Participants", "ScheduleId", "TotalAmount" },
                values: new object[,]
                {
                    { 6, 6, "Confirmed", 2, 6, 1600000.00m },
                    { 7, 7, "Completed", 3, 7, 2625000.00m },
                    { 8, 8, "Cancelled", 4, 8, 3800000.00m }
                });

            migrationBuilder.InsertData(
                table: "TourBookings",
                columns: new[] { "TourBookingId", "BookingId", "Participants", "ScheduleId", "TotalAmount" },
                values: new object[] { 9, 9, 5, 9, 5125000.00m });

            migrationBuilder.InsertData(
                table: "TourBookings",
                columns: new[] { "TourBookingId", "BookingId", "BookingStatus", "Participants", "ScheduleId", "TotalAmount" },
                values: new object[,]
                {
                    { 10, 10, "Confirmed", 1, 10, 1100000.00m },
                    { 11, 11, "Completed", 2, 11, 2350000.00m },
                    { 12, 12, "Cancelled", 3, 12, 3750000.00m }
                });

            migrationBuilder.InsertData(
                table: "TourBookings",
                columns: new[] { "TourBookingId", "BookingId", "Participants", "ScheduleId", "TotalAmount" },
                values: new object[] { 13, 13, 4, 13, 5300000.00m });

            migrationBuilder.InsertData(
                table: "TourBookings",
                columns: new[] { "TourBookingId", "BookingId", "BookingStatus", "Participants", "ScheduleId", "TotalAmount" },
                values: new object[,]
                {
                    { 14, 14, "Confirmed", 5, 14, 7000000.00m },
                    { 15, 15, "Completed", 1, 15, 1475000.00m },
                    { 16, 16, "Cancelled", 2, 16, 3100000.00m }
                });

            migrationBuilder.InsertData(
                table: "TourBookings",
                columns: new[] { "TourBookingId", "BookingId", "Participants", "ScheduleId", "TotalAmount" },
                values: new object[] { 17, 17, 3, 17, 4875000.00m });

            migrationBuilder.InsertData(
                table: "TourBookings",
                columns: new[] { "TourBookingId", "BookingId", "BookingStatus", "Participants", "ScheduleId", "TotalAmount" },
                values: new object[,]
                {
                    { 18, 18, "Confirmed", 4, 18, 6800000.00m },
                    { 19, 19, "Completed", 5, 19, 8875000.00m },
                    { 20, 20, "Cancelled", 1, 20, 1850000.00m }
                });

            migrationBuilder.InsertData(
                table: "FolioTransactions",
                columns: new[] { "TransactionId", "Amount", "CreatedAt", "FolioId", "ReferenceId", "TransactionType" },
                values: new object[,]
                {
                    { 1, 350000.00m, new DateTime(2026, 6, 17, 10, 30, 0, 0, DateTimeKind.Unspecified), 1, 1, "RoomCharge" },
                    { 2, 450000.00m, new DateTime(2026, 6, 18, 10, 30, 0, 0, DateTimeKind.Unspecified), 2, 2, "FoodCharge" },
                    { 3, 550000.00m, new DateTime(2026, 6, 19, 10, 30, 0, 0, DateTimeKind.Unspecified), 3, 3, "TourCharge" },
                    { 4, 650000.00m, new DateTime(2026, 6, 20, 10, 30, 0, 0, DateTimeKind.Unspecified), 4, 4, "Payment" },
                    { 5, 750000.00m, new DateTime(2026, 6, 21, 10, 30, 0, 0, DateTimeKind.Unspecified), 5, 5, "Refund" },
                    { 6, 850000.00m, new DateTime(2026, 6, 22, 10, 30, 0, 0, DateTimeKind.Unspecified), 6, 6, "Adjustment" },
                    { 7, 950000.00m, new DateTime(2026, 6, 23, 10, 30, 0, 0, DateTimeKind.Unspecified), 7, 7, "RoomCharge" },
                    { 8, 1050000.00m, new DateTime(2026, 6, 24, 10, 30, 0, 0, DateTimeKind.Unspecified), 8, 8, "FoodCharge" },
                    { 9, 1150000.00m, new DateTime(2026, 6, 25, 10, 30, 0, 0, DateTimeKind.Unspecified), 9, 9, "TourCharge" },
                    { 10, 1250000.00m, new DateTime(2026, 6, 26, 10, 30, 0, 0, DateTimeKind.Unspecified), 10, 10, "Payment" },
                    { 11, 1350000.00m, new DateTime(2026, 6, 27, 10, 30, 0, 0, DateTimeKind.Unspecified), 11, 11, "Refund" },
                    { 12, 1450000.00m, new DateTime(2026, 6, 28, 10, 30, 0, 0, DateTimeKind.Unspecified), 12, 12, "Adjustment" },
                    { 13, 1550000.00m, new DateTime(2026, 6, 29, 10, 30, 0, 0, DateTimeKind.Unspecified), 13, 13, "RoomCharge" },
                    { 14, 1650000.00m, new DateTime(2026, 6, 30, 10, 30, 0, 0, DateTimeKind.Unspecified), 14, 14, "FoodCharge" },
                    { 15, 1750000.00m, new DateTime(2026, 7, 1, 10, 30, 0, 0, DateTimeKind.Unspecified), 15, 15, "TourCharge" },
                    { 16, 1850000.00m, new DateTime(2026, 7, 2, 10, 30, 0, 0, DateTimeKind.Unspecified), 16, 16, "Payment" },
                    { 17, 1950000.00m, new DateTime(2026, 7, 3, 10, 30, 0, 0, DateTimeKind.Unspecified), 17, 17, "Refund" },
                    { 18, 2050000.00m, new DateTime(2026, 7, 4, 10, 30, 0, 0, DateTimeKind.Unspecified), 18, 18, "Adjustment" },
                    { 19, 2150000.00m, new DateTime(2026, 7, 5, 10, 30, 0, 0, DateTimeKind.Unspecified), 19, 19, "RoomCharge" },
                    { 20, 2250000.00m, new DateTime(2026, 7, 6, 10, 30, 0, 0, DateTimeKind.Unspecified), 20, 20, "FoodCharge" }
                });

            migrationBuilder.InsertData(
                table: "FoodOrderItems",
                columns: new[] { "OrderItemId", "FoodOrderId", "MenuItemId", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 1, 1, 2, 85000.00m },
                    { 2, 2, 2, 3, 110000.00m },
                    { 3, 3, 3, 4, 135000.00m },
                    { 4, 4, 4, 1, 160000.00m },
                    { 5, 5, 5, 2, 185000.00m },
                    { 6, 6, 6, 3, 210000.00m },
                    { 7, 7, 7, 4, 235000.00m },
                    { 8, 8, 8, 1, 260000.00m },
                    { 9, 9, 9, 2, 285000.00m },
                    { 10, 10, 10, 3, 310000.00m },
                    { 11, 11, 11, 4, 335000.00m },
                    { 12, 12, 12, 1, 360000.00m },
                    { 13, 13, 13, 2, 385000.00m },
                    { 14, 14, 14, 3, 410000.00m },
                    { 15, 15, 15, 4, 435000.00m },
                    { 16, 16, 16, 1, 460000.00m },
                    { 17, 17, 17, 2, 485000.00m },
                    { 18, 18, 18, 3, 510000.00m },
                    { 19, 19, 19, 4, 535000.00m },
                    { 20, 20, 20, 1, 560000.00m }
                });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "InvoiceId", "FinalAmount", "FolioId", "InvoiceNumber", "IssuedAt", "TaxAmount", "TotalAmount" },
                values: new object[] { 1, 3078000.00m, 1, "INV-2026-0001", new DateTime(2026, 6, 19, 9, 0, 0, 0, DateTimeKind.Unspecified), 228000.00m, 2850000.00m });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "InvoiceId", "FinalAmount", "FolioId", "InvoiceNumber", "InvoiceStatus", "IssuedAt", "TaxAmount", "TotalAmount" },
                values: new object[,]
                {
                    { 2, 3456000.00m, 2, "INV-2026-0002", "Issued", new DateTime(2026, 6, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), 256000.00m, 3200000.00m },
                    { 3, 3834000.00m, 3, "INV-2026-0003", "Paid", new DateTime(2026, 6, 21, 9, 0, 0, 0, DateTimeKind.Unspecified), 284000.00m, 3550000.00m },
                    { 4, 4212000.00m, 4, "INV-2026-0004", "Cancelled", new DateTime(2026, 6, 22, 9, 0, 0, 0, DateTimeKind.Unspecified), 312000.00m, 3900000.00m }
                });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "InvoiceId", "FinalAmount", "FolioId", "InvoiceNumber", "IssuedAt", "TaxAmount", "TotalAmount" },
                values: new object[] { 5, 4590000.00m, 5, "INV-2026-0005", new DateTime(2026, 6, 23, 9, 0, 0, 0, DateTimeKind.Unspecified), 340000.00m, 4250000.00m });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "InvoiceId", "FinalAmount", "FolioId", "InvoiceNumber", "InvoiceStatus", "IssuedAt", "TaxAmount", "TotalAmount" },
                values: new object[,]
                {
                    { 6, 4968000.00m, 6, "INV-2026-0006", "Issued", new DateTime(2026, 6, 24, 9, 0, 0, 0, DateTimeKind.Unspecified), 368000.00m, 4600000.00m },
                    { 7, 5346000.00m, 7, "INV-2026-0007", "Paid", new DateTime(2026, 6, 25, 9, 0, 0, 0, DateTimeKind.Unspecified), 396000.00m, 4950000.00m },
                    { 8, 5724000.00m, 8, "INV-2026-0008", "Cancelled", new DateTime(2026, 6, 26, 9, 0, 0, 0, DateTimeKind.Unspecified), 424000.00m, 5300000.00m }
                });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "InvoiceId", "FinalAmount", "FolioId", "InvoiceNumber", "IssuedAt", "TaxAmount", "TotalAmount" },
                values: new object[] { 9, 6102000.00m, 9, "INV-2026-0009", new DateTime(2026, 6, 27, 9, 0, 0, 0, DateTimeKind.Unspecified), 452000.00m, 5650000.00m });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "InvoiceId", "FinalAmount", "FolioId", "InvoiceNumber", "InvoiceStatus", "IssuedAt", "TaxAmount", "TotalAmount" },
                values: new object[,]
                {
                    { 10, 6480000.00m, 10, "INV-2026-0010", "Issued", new DateTime(2026, 6, 28, 9, 0, 0, 0, DateTimeKind.Unspecified), 480000.00m, 6000000.00m },
                    { 11, 6858000.00m, 11, "INV-2026-0011", "Paid", new DateTime(2026, 6, 29, 9, 0, 0, 0, DateTimeKind.Unspecified), 508000.00m, 6350000.00m },
                    { 12, 7236000.00m, 12, "INV-2026-0012", "Cancelled", new DateTime(2026, 6, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), 536000.00m, 6700000.00m }
                });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "InvoiceId", "FinalAmount", "FolioId", "InvoiceNumber", "IssuedAt", "TaxAmount", "TotalAmount" },
                values: new object[] { 13, 7614000.00m, 13, "INV-2026-0013", new DateTime(2026, 7, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), 564000.00m, 7050000.00m });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "InvoiceId", "FinalAmount", "FolioId", "InvoiceNumber", "InvoiceStatus", "IssuedAt", "TaxAmount", "TotalAmount" },
                values: new object[,]
                {
                    { 14, 7992000.00m, 14, "INV-2026-0014", "Issued", new DateTime(2026, 7, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), 592000.00m, 7400000.00m },
                    { 15, 8370000.00m, 15, "INV-2026-0015", "Paid", new DateTime(2026, 7, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), 620000.00m, 7750000.00m },
                    { 16, 8748000.00m, 16, "INV-2026-0016", "Cancelled", new DateTime(2026, 7, 4, 9, 0, 0, 0, DateTimeKind.Unspecified), 648000.00m, 8100000.00m }
                });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "InvoiceId", "FinalAmount", "FolioId", "InvoiceNumber", "IssuedAt", "TaxAmount", "TotalAmount" },
                values: new object[] { 17, 9126000.00m, 17, "INV-2026-0017", new DateTime(2026, 7, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), 676000.00m, 8450000.00m });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "InvoiceId", "FinalAmount", "FolioId", "InvoiceNumber", "InvoiceStatus", "IssuedAt", "TaxAmount", "TotalAmount" },
                values: new object[,]
                {
                    { 18, 9504000.00m, 18, "INV-2026-0018", "Issued", new DateTime(2026, 7, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), 704000.00m, 8800000.00m },
                    { 19, 9882000.00m, 19, "INV-2026-0019", "Paid", new DateTime(2026, 7, 7, 9, 0, 0, 0, DateTimeKind.Unspecified), 732000.00m, 9150000.00m },
                    { 20, 10260000.00m, 20, "INV-2026-0020", "Cancelled", new DateTime(2026, 7, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), 760000.00m, 9500000.00m }
                });

            migrationBuilder.InsertData(
                table: "KOT",
                columns: new[] { "KotId", "CreatedAt", "FoodOrderId" },
                values: new object[] { 1, new DateTime(2026, 6, 3, 13, 20, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "KOT",
                columns: new[] { "KotId", "CreatedAt", "FoodOrderId", "Status" },
                values: new object[,]
                {
                    { 2, new DateTime(2026, 6, 4, 14, 20, 0, 0, DateTimeKind.Unspecified), 2, "Cooking" },
                    { 3, new DateTime(2026, 6, 5, 15, 20, 0, 0, DateTimeKind.Unspecified), 3, "Ready" },
                    { 4, new DateTime(2026, 6, 6, 16, 20, 0, 0, DateTimeKind.Unspecified), 4, "Completed" },
                    { 5, new DateTime(2026, 6, 7, 17, 20, 0, 0, DateTimeKind.Unspecified), 5, "Cancelled" }
                });

            migrationBuilder.InsertData(
                table: "KOT",
                columns: new[] { "KotId", "CreatedAt", "FoodOrderId" },
                values: new object[] { 6, new DateTime(2026, 6, 8, 18, 20, 0, 0, DateTimeKind.Unspecified), 6 });

            migrationBuilder.InsertData(
                table: "KOT",
                columns: new[] { "KotId", "CreatedAt", "FoodOrderId", "Status" },
                values: new object[,]
                {
                    { 7, new DateTime(2026, 6, 9, 19, 20, 0, 0, DateTimeKind.Unspecified), 7, "Cooking" },
                    { 8, new DateTime(2026, 6, 10, 12, 20, 0, 0, DateTimeKind.Unspecified), 8, "Ready" },
                    { 9, new DateTime(2026, 6, 11, 13, 20, 0, 0, DateTimeKind.Unspecified), 9, "Completed" },
                    { 10, new DateTime(2026, 6, 12, 14, 20, 0, 0, DateTimeKind.Unspecified), 10, "Cancelled" }
                });

            migrationBuilder.InsertData(
                table: "KOT",
                columns: new[] { "KotId", "CreatedAt", "FoodOrderId" },
                values: new object[] { 11, new DateTime(2026, 6, 13, 15, 20, 0, 0, DateTimeKind.Unspecified), 11 });

            migrationBuilder.InsertData(
                table: "KOT",
                columns: new[] { "KotId", "CreatedAt", "FoodOrderId", "Status" },
                values: new object[,]
                {
                    { 12, new DateTime(2026, 6, 14, 16, 20, 0, 0, DateTimeKind.Unspecified), 12, "Cooking" },
                    { 13, new DateTime(2026, 6, 15, 17, 20, 0, 0, DateTimeKind.Unspecified), 13, "Ready" },
                    { 14, new DateTime(2026, 6, 16, 18, 20, 0, 0, DateTimeKind.Unspecified), 14, "Completed" },
                    { 15, new DateTime(2026, 6, 17, 19, 20, 0, 0, DateTimeKind.Unspecified), 15, "Cancelled" }
                });

            migrationBuilder.InsertData(
                table: "KOT",
                columns: new[] { "KotId", "CreatedAt", "FoodOrderId" },
                values: new object[] { 16, new DateTime(2026, 6, 18, 12, 20, 0, 0, DateTimeKind.Unspecified), 16 });

            migrationBuilder.InsertData(
                table: "KOT",
                columns: new[] { "KotId", "CreatedAt", "FoodOrderId", "Status" },
                values: new object[,]
                {
                    { 17, new DateTime(2026, 6, 19, 13, 20, 0, 0, DateTimeKind.Unspecified), 17, "Cooking" },
                    { 18, new DateTime(2026, 6, 20, 14, 20, 0, 0, DateTimeKind.Unspecified), 18, "Ready" },
                    { 19, new DateTime(2026, 6, 21, 15, 20, 0, 0, DateTimeKind.Unspecified), 19, "Completed" },
                    { 20, new DateTime(2026, 6, 22, 16, 20, 0, 0, DateTimeKind.Unspecified), 20, "Cancelled" }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "InvoiceId", "PaidAt", "PaymentMethod", "PaymentStatus", "TransactionCode" },
                values: new object[,]
                {
                    { 1, 1150000.00m, 1, null, "Cash", "Pending", "TXN202600001" },
                    { 2, 1300000.00m, 2, new DateTime(2026, 6, 21, 14, 45, 0, 0, DateTimeKind.Unspecified), "CreditCard", "Success", "TXN202600002" },
                    { 3, 1450000.00m, 3, new DateTime(2026, 6, 22, 14, 45, 0, 0, DateTimeKind.Unspecified), "DebitCard", "Failed", "TXN202600003" },
                    { 4, 1600000.00m, 4, new DateTime(2026, 6, 23, 14, 45, 0, 0, DateTimeKind.Unspecified), "VNPay", "Refunded", "TXN202600004" },
                    { 5, 1750000.00m, 5, null, "MoMo", "Pending", "TXN202600005" },
                    { 6, 1900000.00m, 6, new DateTime(2026, 6, 25, 14, 45, 0, 0, DateTimeKind.Unspecified), "BankTransfer", "Success", "TXN202600006" },
                    { 7, 2050000.00m, 7, new DateTime(2026, 6, 26, 14, 45, 0, 0, DateTimeKind.Unspecified), "Cash", "Failed", "TXN202600007" },
                    { 8, 2200000.00m, 8, new DateTime(2026, 6, 27, 14, 45, 0, 0, DateTimeKind.Unspecified), "CreditCard", "Refunded", "TXN202600008" },
                    { 9, 2350000.00m, 9, null, "DebitCard", "Pending", "TXN202600009" },
                    { 10, 2500000.00m, 10, new DateTime(2026, 6, 29, 14, 45, 0, 0, DateTimeKind.Unspecified), "VNPay", "Success", "TXN202600010" },
                    { 11, 2650000.00m, 11, new DateTime(2026, 6, 30, 14, 45, 0, 0, DateTimeKind.Unspecified), "MoMo", "Failed", "TXN202600011" },
                    { 12, 2800000.00m, 12, new DateTime(2026, 7, 1, 14, 45, 0, 0, DateTimeKind.Unspecified), "BankTransfer", "Refunded", "TXN202600012" },
                    { 13, 2950000.00m, 13, null, "Cash", "Pending", "TXN202600013" },
                    { 14, 3100000.00m, 14, new DateTime(2026, 7, 3, 14, 45, 0, 0, DateTimeKind.Unspecified), "CreditCard", "Success", "TXN202600014" },
                    { 15, 3250000.00m, 15, new DateTime(2026, 7, 4, 14, 45, 0, 0, DateTimeKind.Unspecified), "DebitCard", "Failed", "TXN202600015" },
                    { 16, 3400000.00m, 16, new DateTime(2026, 7, 5, 14, 45, 0, 0, DateTimeKind.Unspecified), "VNPay", "Refunded", "TXN202600016" },
                    { 17, 3550000.00m, 17, null, "MoMo", "Pending", "TXN202600017" },
                    { 18, 3700000.00m, 18, new DateTime(2026, 7, 7, 14, 45, 0, 0, DateTimeKind.Unspecified), "BankTransfer", "Success", "TXN202600018" },
                    { 19, 3850000.00m, 19, new DateTime(2026, 7, 8, 14, 45, 0, 0, DateTimeKind.Unspecified), "Cash", "Failed", "TXN202600019" },
                    { 20, 4000000.00m, 20, new DateTime(2026, 7, 9, 14, 45, 0, 0, DateTimeKind.Unspecified), "CreditCard", "Refunded", "TXN202600020" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingGuests_BookingId",
                table: "BookingGuests",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FolioTransactions_FolioId",
                table: "FolioTransactions",
                column: "FolioId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodOrderItems_FoodOrderId",
                table: "FoodOrderItems",
                column: "FoodOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodOrderItems_MenuItemId",
                table: "FoodOrderItems",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodOrders_BookingId",
                table: "FoodOrders",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestFolios_BookingId",
                table: "GuestFolios",
                column: "BookingId",
                unique: true,
                filter: "[BookingId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_FolioId",
                table: "Invoices",
                column: "FolioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KOT_FoodOrderId",
                table: "KOT",
                column: "FoodOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategories_CategoryName",
                table: "MenuCategories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_CategoryId_ItemName",
                table: "MenuItems",
                columns: new[] { "CategoryId", "ItemName" },
                unique: true,
                filter: "[CategoryId] IS NOT NULL AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_InvoiceId",
                table: "Payments",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_BookingId",
                table: "Refunds",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BookingId",
                table: "Reviews",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_TourId",
                table: "Reviews",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleName",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomCategoryImages_CategoryId",
                table: "RoomCategoryImages",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_CategoryId",
                table: "Rooms",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomNumber",
                table: "Rooms",
                column: "RoomNumber",
                unique: true,
                filter: "[RoomNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TableReservations_TableId",
                table: "TableReservations",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_TableReservations_UserId",
                table: "TableReservations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TourBookings_BookingId",
                table: "TourBookings",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_TourBookings_ScheduleId",
                table: "TourBookings",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_TourSchedules_GuideId",
                table: "TourSchedules",
                column: "GuideId");

            migrationBuilder.CreateIndex(
                name: "IX_TourSchedules_TourId",
                table: "TourSchedules",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingGuests");

            migrationBuilder.DropTable(
                name: "FolioTransactions");

            migrationBuilder.DropTable(
                name: "FoodOrderItems");

            migrationBuilder.DropTable(
                name: "KOT");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Refunds");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "RoomCategoryImages");

            migrationBuilder.DropTable(
                name: "TableReservations");

            migrationBuilder.DropTable(
                name: "TourBookings");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "FoodOrders");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "DiningTables");

            migrationBuilder.DropTable(
                name: "TourSchedules");

            migrationBuilder.DropTable(
                name: "MenuCategories");

            migrationBuilder.DropTable(
                name: "GuestFolios");

            migrationBuilder.DropTable(
                name: "Tours");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "RoomCategories");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
