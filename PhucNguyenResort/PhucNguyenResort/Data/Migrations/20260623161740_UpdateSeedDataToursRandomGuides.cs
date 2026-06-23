using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedDataToursRandomGuides : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                column: "RoleId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 19,
                column: "RoleId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 20,
                column: "RoleId",
                value: 7);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 13, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(7988), new DateTime(2026, 6, 15, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(7996), new DateTime(2026, 6, 8, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8002) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 28, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8004), new DateTime(2026, 7, 1, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8005), new DateTime(2026, 6, 21, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8007) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 3, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8009), new DateTime(2026, 7, 5, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8010), new DateTime(2026, 6, 22, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8011) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 18, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8013), new DateTime(2026, 6, 20, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8014), new DateTime(2026, 6, 13, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8015) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 24, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8017), new DateTime(2026, 6, 27, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8018), new DateTime(2026, 6, 18, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8020) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8021), new DateTime(2026, 5, 25, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8031), new DateTime(2026, 5, 18, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8034) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8036), new DateTime(2026, 4, 26, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8036), new DateTime(2026, 4, 13, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8038) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8040), new DateTime(2026, 6, 5, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8040), new DateTime(2026, 5, 29, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8042) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 8, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8043), new DateTime(2026, 6, 13, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8044), new DateTime(2026, 6, 3, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8046) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 13, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8047), new DateTime(2026, 7, 15, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8048), new DateTime(2026, 6, 22, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8050) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 8, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8051), new DateTime(2026, 7, 11, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8052), new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8053) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8055), new DateTime(2026, 2, 28, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8056), new DateTime(2026, 2, 13, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8059) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8061), new DateTime(2026, 1, 24, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8061), new DateTime(2026, 1, 20, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(8063) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9004));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9009));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9011));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9013));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9014));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9017));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9018));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9020));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9021));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9023));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9024));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9026));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9027));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9029));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9030));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9033));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9034));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9035));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9068));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 23, 22, 45, 52, 622, DateTimeKind.Local).AddTicks(9070));

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 1,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 2,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 3,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 4,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 5,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 6,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 7,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 8,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 9,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 10,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 11,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 12,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 13,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 14,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 15,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 16,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 17,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 18,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 19,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 20,
                column: "ManagerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 18,
                column: "RoleId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 19,
                column: "RoleId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 20,
                column: "RoleId",
                value: 3);
        }
    }
}
