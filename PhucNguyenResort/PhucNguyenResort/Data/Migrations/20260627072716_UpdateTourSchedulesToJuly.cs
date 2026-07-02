using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTourSchedulesToJuly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 17, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(1984), new DateTime(2026, 6, 19, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(1990), new DateTime(2026, 6, 12, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(1995) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 2, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(1998), new DateTime(2026, 7, 5, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(1999), new DateTime(2026, 6, 25, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2002) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 7, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2004), new DateTime(2026, 7, 9, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2005), new DateTime(2026, 6, 26, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2007) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 22, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2009), new DateTime(2026, 6, 24, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2010), new DateTime(2026, 6, 17, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2012) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 28, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2014), new DateTime(2026, 7, 1, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2015), new DateTime(2026, 6, 22, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2017) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2051), new DateTime(2026, 5, 29, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2057), new DateTime(2026, 5, 22, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2060) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2062), new DateTime(2026, 4, 30, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2063), new DateTime(2026, 4, 17, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2066) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 7, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2068), new DateTime(2026, 6, 9, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2069), new DateTime(2026, 6, 2, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2071) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 12, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2073), new DateTime(2026, 6, 17, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2073), new DateTime(2026, 6, 7, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2076) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 17, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2078), new DateTime(2026, 7, 19, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2079), new DateTime(2026, 6, 26, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2081) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 12, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2083), new DateTime(2026, 7, 15, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2084), new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2086) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2088), new DateTime(2026, 3, 4, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2088), new DateTime(2026, 2, 17, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2092) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2094), new DateTime(2026, 1, 28, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2095), new DateTime(2026, 1, 24, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(2097) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3251));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3256));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3259));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3261));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3263));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3265));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3267));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3269));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3271));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3275));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3277));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3279));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3281));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3283));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3285));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3290));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3292));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3294));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 27, 14, 27, 15, 425, DateTimeKind.Local).AddTicks(3296));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 1,
                column: "TourDate",
                value: new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 2,
                column: "TourDate",
                value: new DateTime(2026, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 3,
                column: "TourDate",
                value: new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 4,
                column: "TourDate",
                value: new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 5,
                column: "TourDate",
                value: new DateTime(2026, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 6,
                column: "TourDate",
                value: new DateTime(2026, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 7,
                column: "TourDate",
                value: new DateTime(2026, 7, 17, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 8,
                column: "TourDate",
                value: new DateTime(2026, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 9,
                column: "TourDate",
                value: new DateTime(2026, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 10,
                column: "TourDate",
                value: new DateTime(2026, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 11,
                column: "TourDate",
                value: new DateTime(2026, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 12,
                column: "TourDate",
                value: new DateTime(2026, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 13,
                column: "TourDate",
                value: new DateTime(2026, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 14,
                column: "TourDate",
                value: new DateTime(2026, 7, 24, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 15,
                column: "TourDate",
                value: new DateTime(2026, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 16,
                column: "TourDate",
                value: new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 17,
                column: "TourDate",
                value: new DateTime(2026, 7, 27, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 18,
                column: "TourDate",
                value: new DateTime(2026, 7, 28, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 19,
                column: "TourDate",
                value: new DateTime(2026, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 20,
                column: "TourDate",
                value: new DateTime(2026, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "TourSchedules",
                columns: new[] { "ScheduleId", "AvailableSlots", "GuideId", "StartTime", "TourDate", "TourId" },
                values: new object[,]
                {
                    { 21, 10, 7, new TimeSpan(0, 9, 0, 0, 0), new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 22, 10, 7, new TimeSpan(0, 9, 0, 0, 0), new DateTime(2026, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 22 },
                    { 23, 10, 7, new TimeSpan(0, 9, 0, 0, 0), new DateTime(2026, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 23 },
                    { 24, 10, 7, new TimeSpan(0, 9, 0, 0, 0), new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 24 },
                    { 25, 10, 7, new TimeSpan(0, 9, 0, 0, 0), new DateTime(2026, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 25 },
                    { 26, 10, 7, new TimeSpan(0, 9, 0, 0, 0), new DateTime(2026, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 26 },
                    { 27, 10, 7, new TimeSpan(0, 9, 0, 0, 0), new DateTime(2026, 7, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 27 },
                    { 28, 10, 7, new TimeSpan(0, 9, 0, 0, 0), new DateTime(2026, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 28 },
                    { 29, 10, 7, new TimeSpan(0, 9, 0, 0, 0), new DateTime(2026, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 29 },
                    { 30, 10, 7, new TimeSpan(0, 9, 0, 0, 0), new DateTime(2026, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 30 }
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

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 1,
                column: "TourDate",
                value: new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 2,
                column: "TourDate",
                value: new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 3,
                column: "TourDate",
                value: new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 4,
                column: "TourDate",
                value: new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 5,
                column: "TourDate",
                value: new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 6,
                column: "TourDate",
                value: new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 7,
                column: "TourDate",
                value: new DateTime(2026, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 8,
                column: "TourDate",
                value: new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 9,
                column: "TourDate",
                value: new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 10,
                column: "TourDate",
                value: new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 11,
                column: "TourDate",
                value: new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 12,
                column: "TourDate",
                value: new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 13,
                column: "TourDate",
                value: new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 14,
                column: "TourDate",
                value: new DateTime(2026, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 15,
                column: "TourDate",
                value: new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 16,
                column: "TourDate",
                value: new DateTime(2026, 6, 26, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 17,
                column: "TourDate",
                value: new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 18,
                column: "TourDate",
                value: new DateTime(2026, 6, 28, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 19,
                column: "TourDate",
                value: new DateTime(2026, 6, 29, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 20,
                column: "TourDate",
                value: new DateTime(2026, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
