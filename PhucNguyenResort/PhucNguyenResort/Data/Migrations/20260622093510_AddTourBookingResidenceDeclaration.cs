using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTourBookingResidenceDeclaration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookerFullName",
                table: "TourBookings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookerIdentityNumber",
                table: "TourBookings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookerPhone",
                table: "TourBookings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 12, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7500), new DateTime(2026, 6, 14, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7503), new DateTime(2026, 6, 7, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7506) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 27, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7508), new DateTime(2026, 6, 30, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7508), new DateTime(2026, 6, 20, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7510) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 2, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7512), new DateTime(2026, 7, 4, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7512), new DateTime(2026, 6, 21, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7514) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 17, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7516), new DateTime(2026, 6, 19, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7516), new DateTime(2026, 6, 12, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7518) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 23, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7566), new DateTime(2026, 6, 26, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7567), new DateTime(2026, 6, 17, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7568) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7570), new DateTime(2026, 5, 24, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7574), new DateTime(2026, 5, 17, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7576) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7577), new DateTime(2026, 4, 25, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7578), new DateTime(2026, 4, 12, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7580) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 2, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7582), new DateTime(2026, 6, 4, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7582), new DateTime(2026, 5, 28, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7584) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 7, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7585), new DateTime(2026, 6, 12, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7586), new DateTime(2026, 6, 2, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7587) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 12, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7588), new DateTime(2026, 7, 14, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7589), new DateTime(2026, 6, 21, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7590) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 7, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7592), new DateTime(2026, 7, 10, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7592), new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7594) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7595), new DateTime(2026, 2, 27, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7596), new DateTime(2026, 2, 12, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7598) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7600), new DateTime(2026, 1, 23, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7600), new DateTime(2026, 1, 19, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(7602) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 1,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8356) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 2,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8360) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 3,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8362) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 4,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8364) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 5,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8365) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 6,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8367) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 7,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8391) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 8,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8393) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 9,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8394) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 10,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8396) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 11,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8397) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 12,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8399) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 13,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8401) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 14,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8402) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 15,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8403) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 16,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8405) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 17,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8407) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 18,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8408) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 19,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8410) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 20,
                columns: new[] { "BookerFullName", "BookerIdentityNumber", "BookerPhone", "CreatedAt" },
                values: new object[] { null, null, null, new DateTime(2026, 6, 22, 16, 35, 10, 333, DateTimeKind.Local).AddTicks(8411) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookerFullName",
                table: "TourBookings");

            migrationBuilder.DropColumn(
                name: "BookerIdentityNumber",
                table: "TourBookings");

            migrationBuilder.DropColumn(
                name: "BookerPhone",
                table: "TourBookings");

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
    }
}
