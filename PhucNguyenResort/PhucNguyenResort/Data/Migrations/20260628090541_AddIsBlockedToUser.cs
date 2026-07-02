using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsBlockedToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);



            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, new DateTime(2026, 6, 18, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7074), new DateTime(2026, 6, 20, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7080), new DateTime(2026, 6, 13, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7086), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, new DateTime(2026, 7, 3, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7092), new DateTime(2026, 7, 6, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7093), new DateTime(2026, 6, 26, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7095), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, new DateTime(2026, 7, 8, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7097), new DateTime(2026, 7, 10, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7098), new DateTime(2026, 6, 27, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7100), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, new DateTime(2026, 6, 23, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7102), new DateTime(2026, 6, 25, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7103), new DateTime(2026, 6, 18, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7105), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, new DateTime(2026, 6, 29, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7107), new DateTime(2026, 7, 2, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7107), new DateTime(2026, 6, 23, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7109), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, new DateTime(2026, 5, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7111), new DateTime(2026, 5, 30, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7116), new DateTime(2026, 5, 23, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7119), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, new DateTime(2026, 4, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7121), new DateTime(2026, 5, 1, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7122), new DateTime(2026, 4, 18, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7125), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, new DateTime(2026, 6, 8, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7127), new DateTime(2026, 6, 10, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7128), new DateTime(2026, 6, 3, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7129), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, new DateTime(2026, 6, 13, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7132), new DateTime(2026, 6, 18, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7132), new DateTime(2026, 6, 8, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7134), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, new DateTime(2026, 7, 18, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7136), new DateTime(2026, 7, 20, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7136), new DateTime(2026, 6, 27, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7138), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, new DateTime(2026, 7, 13, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7140), new DateTime(2026, 7, 16, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7140), new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7142), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, new DateTime(2026, 2, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7144), new DateTime(2026, 3, 5, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7144), new DateTime(2026, 2, 18, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7147), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, new DateTime(2026, 1, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7149), new DateTime(2026, 1, 29, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7150), new DateTime(2026, 1, 25, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(7151), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 14,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 15,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 16,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 17,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 18,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 19,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 20,
                columns: new[] { "ActualCheckInTime", "ActualCheckOutTime", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8097));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8102));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8104));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8105));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8109));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8111));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8112));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8114));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8115));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8117));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8119));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8121));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8123));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8124));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8126));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8129));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8130));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8132));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8133));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 28, 16, 5, 41, 77, DateTimeKind.Local).AddTicks(8135));


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ActualCheckInTime",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ActualCheckOutTime",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ExpectedArrivalTime",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RoomHandoverTime",
                table: "Bookings");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 14, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3458), new DateTime(2026, 6, 16, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3462), new DateTime(2026, 6, 9, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3465) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 29, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3467), new DateTime(2026, 7, 2, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3467), new DateTime(2026, 6, 22, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3469) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 4, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3471), new DateTime(2026, 7, 6, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3471), new DateTime(2026, 6, 23, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3473) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 19, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3475), new DateTime(2026, 6, 21, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3475), new DateTime(2026, 6, 14, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3477) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 25, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3478), new DateTime(2026, 6, 28, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3479), new DateTime(2026, 6, 19, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3480) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3482), new DateTime(2026, 5, 26, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3484), new DateTime(2026, 5, 19, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3486) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3487), new DateTime(2026, 4, 27, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3488), new DateTime(2026, 4, 14, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3490) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 4, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3492), new DateTime(2026, 6, 6, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3492), new DateTime(2026, 5, 30, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3494) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 9, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3495), new DateTime(2026, 6, 14, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3495), new DateTime(2026, 6, 4, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3497) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 14, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3498), new DateTime(2026, 7, 16, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3499), new DateTime(2026, 6, 23, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3500) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 9, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3502), new DateTime(2026, 7, 12, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3502), new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3503) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3525), new DateTime(2026, 3, 1, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3525), new DateTime(2026, 2, 14, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3527) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3529), new DateTime(2026, 1, 25, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3530), new DateTime(2026, 1, 21, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(3531) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4446));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4449));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4451));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4453));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4455));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4457));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4459));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4460));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4462));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4463));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4464));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4466));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4467));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4469));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4491));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4494));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4496));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4497));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4499));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 24, 10, 50, 4, 756, DateTimeKind.Local).AddTicks(4500));
        }
    }
}
