using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtToTourBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TourBookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TourBookings");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 9, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5789), new DateTime(2026, 6, 11, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5796), new DateTime(2026, 6, 4, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5804) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 24, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5806), new DateTime(2026, 6, 27, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5806), new DateTime(2026, 6, 17, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5808) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 29, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5810), new DateTime(2026, 7, 1, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5811), new DateTime(2026, 6, 18, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5813) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 14, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5814), new DateTime(2026, 6, 16, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5815), new DateTime(2026, 6, 9, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5816) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 20, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5818), new DateTime(2026, 6, 23, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5818), new DateTime(2026, 6, 14, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5820) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 19, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5822), new DateTime(2026, 5, 21, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5826), new DateTime(2026, 5, 14, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5828) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 19, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5830), new DateTime(2026, 4, 22, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5830), new DateTime(2026, 4, 9, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5833) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5835), new DateTime(2026, 6, 1, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5835), new DateTime(2026, 5, 25, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5836) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 4, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5838), new DateTime(2026, 6, 9, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5838), new DateTime(2026, 5, 30, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5840) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 9, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5841), new DateTime(2026, 7, 11, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5842), new DateTime(2026, 6, 18, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5843) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 4, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5845), new DateTime(2026, 7, 7, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5845), new DateTime(2026, 6, 19, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5847) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 19, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5848), new DateTime(2026, 2, 24, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5849), new DateTime(2026, 2, 9, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5851) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5853), new DateTime(2026, 1, 20, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5854), new DateTime(2026, 1, 16, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5855) });
        }
    }
}
