using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveManagerIdFromTour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tours_Users_ManagerId",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_Tours_ManagerId",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Tours");

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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 18,
                column: "FullName",
                value: "Cao Dat");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 19,
                column: "FullName",
                value: "Ta Anh");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 20,
                column: "FullName",
                value: "Lam Bao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "Tours",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 13, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(7994), new DateTime(2026, 6, 15, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(7999), new DateTime(2026, 6, 8, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8002) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 28, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8004), new DateTime(2026, 7, 1, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8005), new DateTime(2026, 6, 21, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8007) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 3, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8009), new DateTime(2026, 7, 5, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8009), new DateTime(2026, 6, 22, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8011) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 18, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8013), new DateTime(2026, 6, 20, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8013), new DateTime(2026, 6, 13, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8015) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 24, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8016), new DateTime(2026, 6, 27, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8017), new DateTime(2026, 6, 18, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8018) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8020), new DateTime(2026, 5, 25, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8023), new DateTime(2026, 5, 18, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8026) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8027), new DateTime(2026, 4, 26, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8028), new DateTime(2026, 4, 13, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8031) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8032), new DateTime(2026, 6, 5, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8033), new DateTime(2026, 5, 29, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8034) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 8, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8036), new DateTime(2026, 6, 13, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8037), new DateTime(2026, 6, 3, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8038) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 13, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8040), new DateTime(2026, 7, 15, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8040), new DateTime(2026, 6, 22, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8042) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 8, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8065), new DateTime(2026, 7, 11, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8065), new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8067) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8069), new DateTime(2026, 2, 28, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8069), new DateTime(2026, 2, 13, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8072) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8074), new DateTime(2026, 1, 24, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8075), new DateTime(2026, 1, 20, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8077) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8891));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8895));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8897));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8899));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8901));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8902));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8904));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8905));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8907));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8937));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8938));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8940));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8942));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8944));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8946));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8948));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8949));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8951));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8953));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 23, 17, 39, 765, DateTimeKind.Local).AddTicks(8954));

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 1,
                column: "ManagerId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 2,
                column: "ManagerId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 3,
                column: "ManagerId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 4,
                column: "ManagerId",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 5,
                column: "ManagerId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 6,
                column: "ManagerId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 7,
                column: "ManagerId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 8,
                column: "ManagerId",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 9,
                column: "ManagerId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 10,
                column: "ManagerId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 11,
                column: "ManagerId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 12,
                column: "ManagerId",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 13,
                column: "ManagerId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 14,
                column: "ManagerId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 15,
                column: "ManagerId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 16,
                column: "ManagerId",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 17,
                column: "ManagerId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 18,
                column: "ManagerId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 19,
                column: "ManagerId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 20,
                column: "ManagerId",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 18,
                column: "FullName",
                value: "Cao Tien Dat");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 19,
                column: "FullName",
                value: "Ta Minh Anh");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 20,
                column: "FullName",
                value: "Lam Gia Bao");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_ManagerId",
                table: "Tours",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_Users_ManagerId",
                table: "Tours",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
