using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreToursToSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "Tours",
                columns: new[] { "TourId", "Description", "DurationHours", "ImageUrl", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[,]
                {
                    { 21, "Mangrove Forest Canoe experience for resort guests.", 4, "/images/tours/tour-01.jpg", "Phu Quoc Area 21", 10, 600000.00m, "Mangrove Forest Canoe" },
                    { 22, "Squid Fishing Night experience for resort guests.", 5, "/images/tours/tour-02.jpg", "Phu Quoc Area 22", 15, 750000.00m, "Squid Fishing Night" },
                    { 23, "Safari Wildlife Tour experience for resort guests.", 6, "/images/tours/tour-03.jpg", "Phu Quoc Area 23", 20, 1200000.00m, "Safari Wildlife Tour" },
                    { 24, "Local Village Homestay experience for resort guests.", 8, "/images/tours/tour-04.jpg", "Phu Quoc Area 24", 8, 950000.00m, "Local Village Homestay" },
                    { 25, "Scuba Diving Deep Sea experience for resort guests.", 6, "/images/tours/tour-05.jpg", "Phu Quoc Area 25", 6, 2500000.00m, "Scuba Diving Deep Sea" },
                    { 26, "Motorbike Island Tour experience for resort guests.", 4, "/images/tours/tour-06.jpg", "Phu Quoc Area 26", 10, 450000.00m, "Motorbike Island Tour" },
                    { 27, "Beach BBQ Party experience for resort guests.", 3, "/images/tours/tour-07.jpg", "Phu Quoc Area 27", 30, 850000.00m, "Beach BBQ Party" },
                    { 28, "Yoga Sunrise Retreat experience for resort guests.", 2, "/images/tours/tour-08.jpg", "Phu Quoc Area 28", 15, 500000.00m, "Yoga Sunrise Retreat" },
                    { 29, "Hidden Waterfall Trek experience for resort guests.", 5, "/images/tours/tour-09.jpg", "Phu Quoc Area 29", 12, 700000.00m, "Hidden Waterfall Trek" },
                    { 30, "Helicopter Sightseeing experience for resort guests.", 1, "/images/tours/tour-10.jpg", "Phu Quoc Area 30", 4, 5500000.00m, "Helicopter Sightseeing" }
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 30);

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
