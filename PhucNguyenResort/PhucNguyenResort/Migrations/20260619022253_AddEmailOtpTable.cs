using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailOtpTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "TourBookings");

            migrationBuilder.CreateTable(
                name: "EmailOtps",
                columns: table => new
                {
                    OtpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OtpCode = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: false),
                    ExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Attempts = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailOtps", x => x.OtpId);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_EmailOtps_Email",
                table: "EmailOtps",
                column: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailOtps");

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
                values: new object[] { new DateTime(2026, 6, 8, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3575), new DateTime(2026, 6, 10, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3582), new DateTime(2026, 6, 3, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3586) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 23, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3589), new DateTime(2026, 6, 26, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3590), new DateTime(2026, 6, 16, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3591) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 28, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3593), new DateTime(2026, 6, 30, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3594), new DateTime(2026, 6, 17, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3596) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 13, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3597), new DateTime(2026, 6, 15, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3598), new DateTime(2026, 6, 8, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3600) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 19, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3602), new DateTime(2026, 6, 22, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3602), new DateTime(2026, 6, 13, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3604) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 18, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3606), new DateTime(2026, 5, 20, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3614), new DateTime(2026, 5, 13, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3616) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3618), new DateTime(2026, 4, 21, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3619), new DateTime(2026, 4, 8, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3622) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 29, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3624), new DateTime(2026, 5, 31, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3624), new DateTime(2026, 5, 24, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3626) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3628), new DateTime(2026, 6, 8, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3628), new DateTime(2026, 5, 29, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3630) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 8, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3654), new DateTime(2026, 7, 10, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3654), new DateTime(2026, 6, 17, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3656) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 3, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3658), new DateTime(2026, 7, 6, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3659), new DateTime(2026, 6, 18, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3660) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 18, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3662), new DateTime(2026, 2, 23, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3662), new DateTime(2026, 2, 8, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3665) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 18, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3667), new DateTime(2026, 1, 19, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3668), new DateTime(2026, 1, 15, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3670) });

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
    }
}
