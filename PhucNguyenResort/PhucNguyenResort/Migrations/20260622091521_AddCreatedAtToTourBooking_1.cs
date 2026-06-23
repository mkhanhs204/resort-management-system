using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtToTourBooking_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 12, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8061), new DateTime(2026, 6, 14, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8066), new DateTime(2026, 6, 7, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8070) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 27, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8071), new DateTime(2026, 6, 30, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8072), new DateTime(2026, 6, 20, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8074) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 2, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8076), new DateTime(2026, 7, 4, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8076), new DateTime(2026, 6, 21, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8078) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 17, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8079), new DateTime(2026, 6, 19, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8080), new DateTime(2026, 6, 12, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8081) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 23, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8083), new DateTime(2026, 6, 26, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8083), new DateTime(2026, 6, 17, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8085) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8087), new DateTime(2026, 5, 24, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8089), new DateTime(2026, 5, 17, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8091) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8093), new DateTime(2026, 4, 25, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8093), new DateTime(2026, 4, 12, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8096) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 2, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8097), new DateTime(2026, 6, 4, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8098), new DateTime(2026, 5, 28, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8099) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 7, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8101), new DateTime(2026, 6, 12, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8101), new DateTime(2026, 6, 2, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8102) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 12, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8104), new DateTime(2026, 7, 14, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8104), new DateTime(2026, 6, 21, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8106) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 7, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8148), new DateTime(2026, 7, 10, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8148), new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8150) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8151), new DateTime(2026, 2, 27, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8152), new DateTime(2026, 2, 12, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8154) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8155), new DateTime(2026, 1, 23, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8156), new DateTime(2026, 1, 19, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8158) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8865));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8869));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8871));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8873));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8874));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8875));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8877));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8879));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8880));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8881));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8883));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8884));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8885));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8886));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8888));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8889));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8892));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8896));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8899));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 15, 20, 837, DateTimeKind.Local).AddTicks(8900));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 12, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9920), new DateTime(2026, 6, 14, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9925), new DateTime(2026, 6, 7, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9929) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 27, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9934), new DateTime(2026, 6, 30, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9934), new DateTime(2026, 6, 20, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9938) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 2, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9940), new DateTime(2026, 7, 4, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9941), new DateTime(2026, 6, 21, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9944) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 17, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9946), new DateTime(2026, 6, 19, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9947), new DateTime(2026, 6, 12, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9950) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 23, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9952), new DateTime(2026, 6, 26, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9953), new DateTime(2026, 6, 17, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9955) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 22, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9958), new DateTime(2026, 5, 24, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9961), new DateTime(2026, 5, 17, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9964) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 22, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9966), new DateTime(2026, 4, 25, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9967), new DateTime(2026, 4, 12, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9971) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 2, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9973), new DateTime(2026, 6, 4, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9974), new DateTime(2026, 5, 28, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9976) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 7, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9979), new DateTime(2026, 6, 12, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9980), new DateTime(2026, 6, 2, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9982) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 12, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9985), new DateTime(2026, 7, 14, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9985), new DateTime(2026, 6, 21, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9988) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 7, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9991), new DateTime(2026, 7, 10, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9991), new DateTime(2026, 6, 22, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9993) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 22, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9996), new DateTime(2026, 2, 27, 16, 3, 12, 794, DateTimeKind.Local).AddTicks(9996), new DateTime(2026, 2, 12, 16, 3, 12, 795, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(3), new DateTime(2026, 1, 23, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(4), new DateTime(2026, 1, 19, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(7) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1450));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1460));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1463));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1466));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1469));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1472));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1476));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1479));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1482));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1485));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1487));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1490));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1492));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1495));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1497));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1500));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1504));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1506));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1508));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 3, 12, 795, DateTimeKind.Local).AddTicks(1511));
        }
    }
}
