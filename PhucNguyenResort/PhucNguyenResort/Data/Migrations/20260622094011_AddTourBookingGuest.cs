using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTourBookingGuest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "TourBookingGuests",
                columns: table => new
                {
                    TourBookingGuestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourBookingId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PassportNumber = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    IsPrimaryBooker = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourBookingGuests", x => x.TourBookingGuestId);
                    table.ForeignKey(
                        name: "FK_TourBookingGuests_TourBookings_TourBookingId",
                        column: x => x.TourBookingId,
                        principalTable: "TourBookings",
                        principalColumn: "TourBookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 12, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6100), new DateTime(2026, 6, 14, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6107), new DateTime(2026, 6, 7, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6109) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 27, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6111), new DateTime(2026, 6, 30, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6112), new DateTime(2026, 6, 20, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6114) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 2, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6115), new DateTime(2026, 7, 4, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6116), new DateTime(2026, 6, 21, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6118) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 17, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6119), new DateTime(2026, 6, 19, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6120), new DateTime(2026, 6, 12, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6122) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 23, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6147), new DateTime(2026, 6, 26, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6148), new DateTime(2026, 6, 17, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6149) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6151), new DateTime(2026, 5, 24, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6154), new DateTime(2026, 5, 17, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6156) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6158), new DateTime(2026, 4, 25, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6158), new DateTime(2026, 4, 12, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6161) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 2, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6163), new DateTime(2026, 6, 4, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6163), new DateTime(2026, 5, 28, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6165) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 7, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6167), new DateTime(2026, 6, 12, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6168), new DateTime(2026, 6, 2, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6170) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 12, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6171), new DateTime(2026, 7, 14, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6172), new DateTime(2026, 6, 21, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6173) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 7, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6175), new DateTime(2026, 7, 10, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6175), new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6177) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6178), new DateTime(2026, 2, 27, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6179), new DateTime(2026, 2, 12, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6182) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6183), new DateTime(2026, 1, 23, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6184), new DateTime(2026, 1, 19, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(6185) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7002));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7007));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7009));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7011));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7046));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7049));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7052));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7054));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7056));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7058));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7060));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7062));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7064));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7066));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7068));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7070));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7073));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7074));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7076));

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 22, 16, 40, 10, 682, DateTimeKind.Local).AddTicks(7078));

            migrationBuilder.CreateIndex(
                name: "IX_TourBookingGuests_TourBookingId",
                table: "TourBookingGuests",
                column: "TourBookingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TourBookingGuests");

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
    }
}
