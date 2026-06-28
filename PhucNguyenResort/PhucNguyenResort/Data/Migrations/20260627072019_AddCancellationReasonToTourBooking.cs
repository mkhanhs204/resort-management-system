using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCancellationReasonToTourBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CancelReason",
                table: "TourBookings",
                newName: "CancellationReason");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 17, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(427), new DateTime(2026, 6, 19, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(432), new DateTime(2026, 6, 12, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(435) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 2, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(437), new DateTime(2026, 7, 5, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(437), new DateTime(2026, 6, 25, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(440) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 7, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(441), new DateTime(2026, 7, 9, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(442), new DateTime(2026, 6, 26, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(444) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 22, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(445), new DateTime(2026, 6, 24, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(446), new DateTime(2026, 6, 17, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(448) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 28, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(450), new DateTime(2026, 7, 1, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(450), new DateTime(2026, 6, 22, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(452) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(453), new DateTime(2026, 5, 29, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(456), new DateTime(2026, 5, 22, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(458) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(461), new DateTime(2026, 4, 30, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(461), new DateTime(2026, 4, 17, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(464) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 7, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(466), new DateTime(2026, 6, 9, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(467), new DateTime(2026, 6, 2, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(468) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 12, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(470), new DateTime(2026, 6, 17, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(471), new DateTime(2026, 6, 7, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(472) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 17, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(502), new DateTime(2026, 7, 19, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(502), new DateTime(2026, 6, 26, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(504) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 12, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(506), new DateTime(2026, 7, 15, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(506), new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(508) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(510), new DateTime(2026, 3, 4, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(510), new DateTime(2026, 2, 17, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(513) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(515), new DateTime(2026, 1, 28, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(516), new DateTime(2026, 1, 24, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(518) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1384));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1388));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1390));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1391));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1393));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1394));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1396));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1397));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1399));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1400));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1402));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1403));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1404));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1406));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1408));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1410));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1411));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1412));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1414));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 20, 17, 307, DateTimeKind.Local).AddTicks(1415));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CancellationReason",
                table: "TourBookings",
                newName: "CancelReason");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 17, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1737), new DateTime(2026, 6, 19, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1742), new DateTime(2026, 6, 12, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1745) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 2, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1747), new DateTime(2026, 7, 5, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1747), new DateTime(2026, 6, 25, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1749) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 7, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1751), new DateTime(2026, 7, 9, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1752), new DateTime(2026, 6, 26, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1753) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 22, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1755), new DateTime(2026, 6, 24, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1755), new DateTime(2026, 6, 17, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1757) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 28, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1758), new DateTime(2026, 7, 1, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1759), new DateTime(2026, 6, 22, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1760) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1793), new DateTime(2026, 5, 29, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1796), new DateTime(2026, 5, 22, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1798) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1800), new DateTime(2026, 4, 30, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1800), new DateTime(2026, 4, 17, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1803) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 7, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1805), new DateTime(2026, 6, 9, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1806), new DateTime(2026, 6, 2, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1808) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 12, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1809), new DateTime(2026, 6, 17, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1810), new DateTime(2026, 6, 7, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1812) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 17, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1813), new DateTime(2026, 7, 19, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1814), new DateTime(2026, 6, 26, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1815) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 12, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1817), new DateTime(2026, 7, 15, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1818), new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1819) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1821), new DateTime(2026, 3, 4, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1822), new DateTime(2026, 2, 17, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1825) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1826), new DateTime(2026, 1, 28, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1827), new DateTime(2026, 1, 24, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(1829) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2697));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2702));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2704));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2705));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2707));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2708));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2710));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2712));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2713));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2714));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2716));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2717));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2719));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2720));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2723));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2724));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2725));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2727));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2728));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2730));
        }
    }
}
