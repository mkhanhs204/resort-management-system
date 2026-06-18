using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentMethodToTourBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "TourBookings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 8, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8078), new DateTime(2026, 6, 10, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8083), new DateTime(2026, 6, 3, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8085) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 23, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8087), new DateTime(2026, 6, 26, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8088), new DateTime(2026, 6, 16, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8089) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 28, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8116), new DateTime(2026, 6, 30, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8116), new DateTime(2026, 6, 17, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8118) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 13, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8121), new DateTime(2026, 6, 15, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8121), new DateTime(2026, 6, 8, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8123) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 19, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8125), new DateTime(2026, 6, 22, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8125), new DateTime(2026, 6, 13, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8127) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 18, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8129), new DateTime(2026, 5, 20, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8132), new DateTime(2026, 5, 13, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8134) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 18, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8135), new DateTime(2026, 4, 21, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8136), new DateTime(2026, 4, 8, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8138) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 29, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8139), new DateTime(2026, 5, 31, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8140), new DateTime(2026, 5, 24, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8141) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8144), new DateTime(2026, 6, 8, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8145), new DateTime(2026, 5, 29, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8147) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 8, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8148), new DateTime(2026, 7, 10, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8149), new DateTime(2026, 6, 17, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8150) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 3, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8152), new DateTime(2026, 7, 6, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8153), new DateTime(2026, 6, 18, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8154) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 18, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8156), new DateTime(2026, 2, 23, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8156), new DateTime(2026, 2, 8, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8158) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 18, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8160), new DateTime(2026, 1, 19, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8160), new DateTime(2026, 1, 15, 5, 30, 41, 643, DateTimeKind.Local).AddTicks(8162) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 1,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 2,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 3,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 4,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 5,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 6,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 7,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 8,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 9,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 10,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 11,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 12,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 13,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 14,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 15,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 16,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 17,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 18,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 19,
                column: "PaymentMethod",
                value: null);

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 20,
                column: "PaymentMethod",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "TourBookings");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7729), new DateTime(2026, 6, 7, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7736), new DateTime(2026, 5, 31, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7740) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 20, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7743), new DateTime(2026, 6, 23, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7744), new DateTime(2026, 6, 13, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7746) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 25, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7747), new DateTime(2026, 6, 27, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7748), new DateTime(2026, 6, 14, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7749) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 10, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7752), new DateTime(2026, 6, 12, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7753), new DateTime(2026, 6, 5, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7755) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7757), new DateTime(2026, 6, 19, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7757), new DateTime(2026, 6, 10, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7759) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 15, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7761), new DateTime(2026, 5, 17, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7766), new DateTime(2026, 5, 10, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7768) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 15, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7770), new DateTime(2026, 4, 18, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7770), new DateTime(2026, 4, 5, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7772) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 26, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7774), new DateTime(2026, 5, 28, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7774), new DateTime(2026, 5, 21, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7777) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 31, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7778), new DateTime(2026, 6, 5, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7779), new DateTime(2026, 5, 26, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7780) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 5, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7782), new DateTime(2026, 7, 7, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7784), new DateTime(2026, 6, 14, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7785) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 30, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7787), new DateTime(2026, 7, 3, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7788), new DateTime(2026, 6, 15, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7790) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 15, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7792), new DateTime(2026, 2, 20, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7792), new DateTime(2026, 2, 5, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7794) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7796), new DateTime(2026, 1, 16, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7796), new DateTime(2026, 1, 12, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7798) });
        }
    }
}
