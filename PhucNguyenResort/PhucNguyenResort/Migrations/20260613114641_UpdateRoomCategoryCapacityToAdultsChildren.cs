using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomCategoryCapacityToAdultsChildren : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Capacity",
                table: "RoomCategories",
                newName: "MaxChildren");

            migrationBuilder.RenameColumn(
                name: "GuestCount",
                table: "Bookings",
                newName: "NumberOfChildren");

            migrationBuilder.AddColumn<int>(
                name: "MaxAdults",
                table: "RoomCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfAdults",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "NumberOfAdults", "NumberOfChildren", "TotalAmount", "UserId" },
                values: new object[] { "CheckedOut", new DateTime(2026, 6, 3, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6319), new DateTime(2026, 6, 5, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6326), new DateTime(2026, 5, 29, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6329), 1200000m, 2, 0, 2400000m, 2 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "NumberOfAdults", "NumberOfChildren", "TotalAmount" },
                values: new object[] { "Reserved", new DateTime(2026, 6, 18, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6332), new DateTime(2026, 6, 21, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6332), new DateTime(2026, 6, 11, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6334), 1500000m, 2, 0, 3000000m });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "NumberOfAdults", "NumberOfChildren", "RoomId", "TotalAmount" },
                values: new object[] { "PendingPayment", new DateTime(2026, 6, 23, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6336), new DateTime(2026, 6, 25, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6336), new DateTime(2026, 6, 12, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6338), 2500000m, 4, 0, 10, 5000000m });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "NumberOfAdults", "NumberOfChildren", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { new DateTime(2026, 6, 8, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6341), new DateTime(2026, 6, 10, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6342), new DateTime(2026, 6, 3, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6344), 1000000m, 2, 0, 11, 2000000m, 3 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "NumberOfAdults", "NumberOfChildren", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { "Reserved", new DateTime(2026, 6, 14, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6345), new DateTime(2026, 6, 17, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6345), new DateTime(2026, 6, 8, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6347), 12000000m, 6, 0, 13, 24000000m, 2 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "NumberOfAdults", "NumberOfChildren", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { "CheckedOut", new DateTime(2026, 5, 13, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6348), new DateTime(2026, 5, 15, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6353), new DateTime(2026, 5, 8, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6355), 1200000m, 2, 0, 1, 2400000m, 2 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "NumberOfAdults", "NumberOfChildren", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { "CheckedOut", new DateTime(2026, 4, 13, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6356), new DateTime(2026, 4, 16, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6357), new DateTime(2026, 4, 3, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6359), 1500000m, 2, 0, 2, 3000000m, 3 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "NumberOfAdults", "NumberOfChildren", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { "Cancelled", new DateTime(2026, 5, 24, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6360), new DateTime(2026, 5, 26, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6361), new DateTime(2026, 5, 19, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6362), 1200000m, 2, 0, 4, 2400000m, 3 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "NumberOfAdults", "NumberOfChildren", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { "CheckedOut", new DateTime(2026, 5, 29, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6365), new DateTime(2026, 6, 3, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6365), new DateTime(2026, 5, 24, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6366), 3000000m, 2, 0, 5, 6000000m, 2 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "NumberOfAdults", "NumberOfChildren", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { "Reserved", new DateTime(2026, 7, 3, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6368), new DateTime(2026, 7, 5, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6369), new DateTime(2026, 6, 12, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6370), 8000000m, 5, 0, 13, 16000000m, 3 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "NumberOfAdults", "NumberOfChildren", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { "PendingPayment", new DateTime(2026, 6, 28, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6372), new DateTime(2026, 7, 1, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6372), new DateTime(2026, 6, 13, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6374), 12000000m, 6, 0, 14, 24000000m, 2 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "NumberOfAdults", "NumberOfChildren", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { "CheckedOut", new DateTime(2026, 2, 13, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6375), new DateTime(2026, 2, 18, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6376), new DateTime(2026, 2, 3, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6377), 3000000m, 2, 0, 6, 6000000m, 3 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "NumberOfAdults", "NumberOfChildren", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { new DateTime(2026, 1, 13, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6379), new DateTime(2026, 1, 14, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6379), new DateTime(2026, 1, 10, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6381), 600000m, 1, 0, 1, 1200000m, 3 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 14,
                columns: new[] { "NumberOfAdults", "NumberOfChildren" },
                values: new object[] { 2, 0 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 15,
                columns: new[] { "NumberOfAdults", "NumberOfChildren" },
                values: new object[] { 2, 0 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 16,
                columns: new[] { "NumberOfAdults", "NumberOfChildren" },
                values: new object[] { 2, 0 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 17,
                columns: new[] { "NumberOfAdults", "NumberOfChildren" },
                values: new object[] { 2, 0 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 18,
                columns: new[] { "NumberOfAdults", "NumberOfChildren" },
                values: new object[] { 2, 0 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 19,
                columns: new[] { "NumberOfAdults", "NumberOfChildren" },
                values: new object[] { 2, 0 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 20,
                columns: new[] { "NumberOfAdults", "NumberOfChildren" },
                values: new object[] { 2, 0 });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 1,
                columns: new[] { "CategoryName", "Description", "ImageUrl", "MaxAdults", "MaxChildren", "PricePerNight" },
                values: new object[] { "Deluxe Ocean View", "Phòng cao cấp hướng biển", "/img/room/room-1.jpg", 2, 1, 1200000m });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 2,
                columns: new[] { "CategoryName", "Description", "ImageUrl", "MaxAdults", "MaxChildren", "PricePerNight" },
                values: new object[] { "Premium Garden View", "Phòng view vườn yên tĩnh", "/img/room/room-2.jpg", 2, 1, 1000000m });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 3,
                columns: new[] { "CategoryName", "Description", "ImageUrl", "MaxAdults", "PricePerNight" },
                values: new object[] { "Family Suite", "Suite rộng rãi cho gia đình", "/img/room/room-3.jpg", 4, 2500000m });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 4,
                columns: new[] { "CategoryName", "Description", "ImageUrl", "MaxAdults", "MaxChildren", "PricePerNight" },
                values: new object[] { "Presidential Villa", "Biệt thự siêu cấp sang trọng", "/img/room/room-4.jpg", 6, 4, 8000000m });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 5,
                columns: new[] { "MaxAdults", "MaxChildren" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 6,
                columns: new[] { "MaxAdults", "MaxChildren" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 7,
                columns: new[] { "MaxAdults", "MaxChildren" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 8,
                columns: new[] { "MaxAdults", "MaxChildren" },
                values: new object[] { 2, 0 });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 9,
                columns: new[] { "MaxAdults", "MaxChildren" },
                values: new object[] { 2, 0 });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 10,
                columns: new[] { "MaxAdults", "MaxChildren" },
                values: new object[] { 3, 0 });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 11,
                columns: new[] { "MaxAdults", "MaxChildren" },
                values: new object[] { 2, 0 });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 12,
                columns: new[] { "MaxAdults", "MaxChildren" },
                values: new object[] { 3, 0 });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 13,
                columns: new[] { "MaxAdults", "MaxChildren" },
                values: new object[] { 2, 0 });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 14,
                columns: new[] { "MaxAdults", "MaxChildren" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 15,
                columns: new[] { "MaxAdults", "MaxChildren" },
                values: new object[] { 2, 0 });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 16,
                columns: new[] { "MaxAdults", "MaxChildren" },
                values: new object[] { 2, 0 });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 17,
                columns: new[] { "MaxAdults", "MaxChildren" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 18,
                columns: new[] { "MaxAdults", "MaxChildren" },
                values: new object[] { 5, 0 });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 19,
                columns: new[] { "MaxAdults", "MaxChildren" },
                values: new object[] { 2, 0 });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 20,
                columns: new[] { "MaxAdults", "MaxChildren" },
                values: new object[] { 2, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxAdults",
                table: "RoomCategories");

            migrationBuilder.DropColumn(
                name: "NumberOfAdults",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "MaxChildren",
                table: "RoomCategories",
                newName: "Capacity");

            migrationBuilder.RenameColumn(
                name: "NumberOfChildren",
                table: "Bookings",
                newName: "GuestCount");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "TotalAmount", "UserId" },
                values: new object[] { "Reserved", new DateTime(2026, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), 550000.00m, 2, 2050000.00m, 1 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "TotalAmount" },
                values: new object[] { "CheckedIn", new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), 600000.00m, 3, 2300000.00m });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount" },
                values: new object[] { "CheckedOut", new DateTime(2026, 6, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 4, 9, 0, 0, 0, DateTimeKind.Unspecified), 650000.00m, 4, 3, 2550000.00m });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), 700000.00m, 5, 4, 2800000.00m, 4 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { "NoShow", new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), 750000.00m, 1, 5, 3050000.00m, 5 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { "Reserved", new DateTime(2026, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 7, 9, 0, 0, 0, DateTimeKind.Unspecified), 800000.00m, 2, 6, 3300000.00m, 6 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { "CheckedIn", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), 850000.00m, 3, 7, 3550000.00m, 7 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { "CheckedOut", new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 9, 9, 0, 0, 0, DateTimeKind.Unspecified), 900000.00m, 4, 8, 3800000.00m, 8 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { "Cancelled", new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), 950000.00m, 5, 9, 4050000.00m, 9 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { "NoShow", new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 11, 9, 0, 0, 0, DateTimeKind.Unspecified), 1000000.00m, 1, 10, 4300000.00m, 10 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { "Reserved", new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), 1050000.00m, 2, 11, 4550000.00m, 11 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "BookingStatus", "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { "CheckedIn", new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 13, 9, 0, 0, 0, DateTimeKind.Unspecified), 1100000.00m, 3, 12, 4800000.00m, 12 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "DepositAmount", "GuestCount", "RoomId", "TotalAmount", "UserId" },
                values: new object[] { new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 14, 9, 0, 0, 0, DateTimeKind.Unspecified), 1150000.00m, 4, 13, 5050000.00m, 13 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 14,
                column: "GuestCount",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 15,
                column: "GuestCount",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 16,
                column: "GuestCount",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 17,
                column: "GuestCount",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 18,
                column: "GuestCount",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 19,
                column: "GuestCount",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 20,
                column: "GuestCount",
                value: 1);

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 1,
                columns: new[] { "Capacity", "CategoryName", "Description", "ImageUrl", "PricePerNight" },
                values: new object[] { 2, "Standard Garden", "Standard Garden category with capacity 2 guest(s).", "/images/rooms/category-01.jpg", 1080000.00m });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 2,
                columns: new[] { "Capacity", "CategoryName", "Description", "ImageUrl", "PricePerNight" },
                values: new object[] { 2, "Superior Sea View", "Superior Sea View category with capacity 2 guest(s).", "/images/rooms/category-02.jpg", 1260000.00m });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 3,
                columns: new[] { "CategoryName", "Description", "ImageUrl", "PricePerNight" },
                values: new object[] { "Deluxe Pool View", "Deluxe Pool View category with capacity 2 guest(s).", "/images/rooms/category-03.jpg", 1440000.00m });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 4,
                columns: new[] { "Capacity", "CategoryName", "Description", "ImageUrl", "PricePerNight" },
                values: new object[] { 3, "Premium Ocean", "Premium Ocean category with capacity 3 guest(s).", "/images/rooms/category-04.jpg", 1620000.00m });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "Capacity",
                value: 4);

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 6,
                column: "Capacity",
                value: 4);

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 7,
                column: "Capacity",
                value: 6);

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 8,
                column: "Capacity",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 9,
                column: "Capacity",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 10,
                column: "Capacity",
                value: 3);

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 11,
                column: "Capacity",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 12,
                column: "Capacity",
                value: 3);

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 13,
                column: "Capacity",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 14,
                column: "Capacity",
                value: 4);

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 15,
                column: "Capacity",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 16,
                column: "Capacity",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 17,
                column: "Capacity",
                value: 4);

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 18,
                column: "Capacity",
                value: 5);

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 19,
                column: "Capacity",
                value: 2);

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 20,
                column: "Capacity",
                value: 2);
        }
    }
}
