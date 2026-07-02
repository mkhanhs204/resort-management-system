using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCancelReasonToTourBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 30);

            migrationBuilder.AddColumn<string>(
                name: "CancelReason",
                table: "TourBookings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

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
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2697) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 2,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2702) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 3,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2704) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 4,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2705) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 5,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2707) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 6,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2708) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 7,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2710) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 8,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2712) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 9,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2713) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 10,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2714) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 11,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2716) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 12,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2717) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 13,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2719) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 14,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2720) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 15,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2723) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 16,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2724) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 17,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2725) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 18,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2727) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 19,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2728) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 20,
                columns: new[] { "CancelReason", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 6, 27, 14, 14, 13, 661, DateTimeKind.Local).AddTicks(2730) });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 21,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Scuba Diving Master experience for resort guests.", 9, "/images/tours/tour-21.jpg", 18, 1925000.00m, "Scuba Diving Master" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 22,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Traditional Village Tour experience for resort guests.", 4, "/images/tours/tour-22.jpg", 20, 2000000.00m, "Traditional Village Tour" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 23,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Sunrise Yoga on Beach experience for resort guests.", 2, "/images/tours/tour-23.jpg", 22, 2075000.00m, "Sunrise Yoga on Beach" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 24,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "Price", "TourName" },
                values: new object[] { "Seafood BBQ Night experience for resort guests.", 3, "/images/tours/tour-24.jpg", 2150000.00m, "Seafood BBQ Night" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 25,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Jungle Safari experience for resort guests.", 4, "/images/tours/tour-25.jpg", 10, 2225000.00m, "Jungle Safari" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 26,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Star Gazing Tour experience for resort guests.", 5, "/images/tours/tour-26.jpg", 12, 2300000.00m, "Star Gazing Tour" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 27,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Local Craft Workshop experience for resort guests.", 6, "/images/tours/tour-27.jpg", 14, 2375000.00m, "Local Craft Workshop" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 28,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Wine Tasting Experience for resort guests.", 7, "/images/tours/tour-28.jpg", 16, 2450000.00m, "Wine Tasting Experience" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 29,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Waterfall Hiking experience for resort guests.", 8, "/images/tours/tour-29.jpg", 18, 2525000.00m, "Waterfall Hiking" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 30,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Island Cycling Tour experience for resort guests.", 2, "/images/tours/tour-30.jpg", 20, 2600000.00m, "Island Cycling Tour" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelReason",
                table: "TourBookings");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2101), new DateTime(2026, 6, 18, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2106), new DateTime(2026, 6, 11, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2108) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 1, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2110), new DateTime(2026, 7, 4, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2111), new DateTime(2026, 6, 24, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2113) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 6, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2115), new DateTime(2026, 7, 8, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2115), new DateTime(2026, 6, 25, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2117) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 21, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2119), new DateTime(2026, 6, 23, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2119), new DateTime(2026, 6, 16, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2121) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 27, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2123), new DateTime(2026, 6, 30, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2124), new DateTime(2026, 6, 21, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2125) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2144), new DateTime(2026, 5, 28, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2147), new DateTime(2026, 5, 21, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2149) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2151), new DateTime(2026, 4, 29, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2152), new DateTime(2026, 4, 16, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2155) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 6, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2157), new DateTime(2026, 6, 8, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2157), new DateTime(2026, 6, 1, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2159) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 11, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2161), new DateTime(2026, 6, 16, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2161), new DateTime(2026, 6, 6, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2163) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 16, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2165), new DateTime(2026, 7, 18, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2166), new DateTime(2026, 6, 25, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2173) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 11, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2174), new DateTime(2026, 7, 14, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2175), new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2176) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2178), new DateTime(2026, 3, 3, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2179), new DateTime(2026, 2, 16, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2182) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2185), new DateTime(2026, 1, 27, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2185), new DateTime(2026, 1, 23, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(2187) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3065));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3068));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3070));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3072));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3074));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3076));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3078));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3079));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3081));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3083));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3085));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3086));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3089));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3091));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3092));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3093));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3094));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3096));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3097));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 26, 18, 57, 30, 579, DateTimeKind.Local).AddTicks(3099));

            migrationBuilder.InsertData(
                table: "TourSchedules",
                columns: new[] { "ScheduleId", "AvailableSlots", "GuideId", "StartTime", "TourDate", "TourId" },
                values: new object[,]
                {
                    { 21, 10, 7, new TimeSpan(0, 10, 30, 0, 0), new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 22, 15, 7, new TimeSpan(0, 11, 0, 0, 0), new DateTime(2026, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 22 },
                    { 23, 20, 7, new TimeSpan(0, 12, 30, 0, 0), new DateTime(2026, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 23 },
                    { 24, 8, 7, new TimeSpan(0, 7, 0, 0, 0), new DateTime(2026, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 24 },
                    { 25, 6, 7, new TimeSpan(0, 8, 30, 0, 0), new DateTime(2026, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 25 },
                    { 26, 10, 7, new TimeSpan(0, 9, 0, 0, 0), new DateTime(2026, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 26 },
                    { 27, 30, 7, new TimeSpan(0, 10, 30, 0, 0), new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 27 },
                    { 28, 15, 7, new TimeSpan(0, 11, 0, 0, 0), new DateTime(2026, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 28 },
                    { 29, 12, 7, new TimeSpan(0, 12, 30, 0, 0), new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 29 },
                    { 30, 4, 7, new TimeSpan(0, 7, 0, 0, 0), new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 30 }
                });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 21,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Mangrove Forest Canoe experience for resort guests.", 4, "/images/tours/tour-01.jpg", 10, 600000.00m, "Mangrove Forest Canoe" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 22,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Squid Fishing Night experience for resort guests.", 5, "/images/tours/tour-02.jpg", 15, 750000.00m, "Squid Fishing Night" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 23,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Safari Wildlife Tour experience for resort guests.", 6, "/images/tours/tour-03.jpg", 20, 1200000.00m, "Safari Wildlife Tour" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 24,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "Price", "TourName" },
                values: new object[] { "Local Village Homestay experience for resort guests.", 8, "/images/tours/tour-04.jpg", 950000.00m, "Local Village Homestay" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 25,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Scuba Diving Deep Sea experience for resort guests.", 6, "/images/tours/tour-05.jpg", 6, 2500000.00m, "Scuba Diving Deep Sea" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 26,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Motorbike Island Tour experience for resort guests.", 4, "/images/tours/tour-06.jpg", 10, 450000.00m, "Motorbike Island Tour" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 27,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Beach BBQ Party experience for resort guests.", 3, "/images/tours/tour-07.jpg", 30, 850000.00m, "Beach BBQ Party" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 28,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Yoga Sunrise Retreat experience for resort guests.", 2, "/images/tours/tour-08.jpg", 15, 500000.00m, "Yoga Sunrise Retreat" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 29,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Hidden Waterfall Trek experience for resort guests.", 5, "/images/tours/tour-09.jpg", 12, 700000.00m, "Hidden Waterfall Trek" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 30,
                columns: new[] { "Description", "DurationHours", "ImageUrl", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Helicopter Sightseeing experience for resort guests.", 1, "/images/tours/tour-10.jpg", 4, 5500000.00m, "Helicopter Sightseeing" });
        }
    }
}
