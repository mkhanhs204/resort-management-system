using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddPreCheckinFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ConsentTimestamp",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PdpdConsentChecked",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "BookingGuests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimaryBooker",
                table: "BookingGuests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 1,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 2,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 3,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 4,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 5,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 6,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 7,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 8,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 9,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 10,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 11,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 12,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 13,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 14,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 15,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 16,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 17,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 18,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 19,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "BookingGuests",
                keyColumn: "BookingGuestId",
                keyValue: 20,
                columns: new[] { "Gender", "IsPrimaryBooker" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "ConsentTimestamp", "CreatedAt", "PdpdConsentChecked" },
                values: new object[] { new DateTime(2026, 6, 4, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(949), new DateTime(2026, 6, 6, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(956), null, new DateTime(2026, 5, 30, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(959), false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "ConsentTimestamp", "CreatedAt", "PdpdConsentChecked" },
                values: new object[] { new DateTime(2026, 6, 19, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(962), new DateTime(2026, 6, 22, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(963), null, new DateTime(2026, 6, 12, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(965), false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "ConsentTimestamp", "CreatedAt", "PdpdConsentChecked" },
                values: new object[] { new DateTime(2026, 6, 24, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(967), new DateTime(2026, 6, 26, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(968), null, new DateTime(2026, 6, 13, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(970), false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "ConsentTimestamp", "CreatedAt", "PdpdConsentChecked" },
                values: new object[] { new DateTime(2026, 6, 9, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(973), new DateTime(2026, 6, 11, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(973), null, new DateTime(2026, 6, 4, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(977), false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "ConsentTimestamp", "CreatedAt", "PdpdConsentChecked" },
                values: new object[] { new DateTime(2026, 6, 15, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(979), new DateTime(2026, 6, 18, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(979), null, new DateTime(2026, 6, 9, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(981), false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "ConsentTimestamp", "CreatedAt", "PdpdConsentChecked" },
                values: new object[] { new DateTime(2026, 5, 14, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(983), new DateTime(2026, 5, 16, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(988), null, new DateTime(2026, 5, 9, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(990), false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "ConsentTimestamp", "CreatedAt", "PdpdConsentChecked" },
                values: new object[] { new DateTime(2026, 4, 14, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1008), new DateTime(2026, 4, 17, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1009), null, new DateTime(2026, 4, 4, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1011), false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "ConsentTimestamp", "CreatedAt", "PdpdConsentChecked" },
                values: new object[] { new DateTime(2026, 5, 25, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1014), new DateTime(2026, 5, 27, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1014), null, new DateTime(2026, 5, 20, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1016), false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "ConsentTimestamp", "CreatedAt", "PdpdConsentChecked" },
                values: new object[] { new DateTime(2026, 5, 30, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1020), new DateTime(2026, 6, 4, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1020), null, new DateTime(2026, 5, 25, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1022), false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "ConsentTimestamp", "CreatedAt", "PdpdConsentChecked" },
                values: new object[] { new DateTime(2026, 7, 4, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1024), new DateTime(2026, 7, 6, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1025), null, new DateTime(2026, 6, 13, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1027), false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "ConsentTimestamp", "CreatedAt", "PdpdConsentChecked" },
                values: new object[] { new DateTime(2026, 6, 29, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1029), new DateTime(2026, 7, 2, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1029), null, new DateTime(2026, 6, 14, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1031), false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "ConsentTimestamp", "CreatedAt", "PdpdConsentChecked" },
                values: new object[] { new DateTime(2026, 2, 14, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1033), new DateTime(2026, 2, 19, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1034), null, new DateTime(2026, 2, 4, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1036), false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "ConsentTimestamp", "CreatedAt", "PdpdConsentChecked" },
                values: new object[] { new DateTime(2026, 1, 14, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1038), new DateTime(2026, 1, 15, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1038), null, new DateTime(2026, 1, 11, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1040), false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 14,
                columns: new[] { "ConsentTimestamp", "PdpdConsentChecked" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 15,
                columns: new[] { "ConsentTimestamp", "PdpdConsentChecked" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 16,
                columns: new[] { "ConsentTimestamp", "PdpdConsentChecked" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 17,
                columns: new[] { "ConsentTimestamp", "PdpdConsentChecked" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 18,
                columns: new[] { "ConsentTimestamp", "PdpdConsentChecked" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 19,
                columns: new[] { "ConsentTimestamp", "PdpdConsentChecked" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 20,
                columns: new[] { "ConsentTimestamp", "PdpdConsentChecked" },
                values: new object[] { null, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsentTimestamp",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PdpdConsentChecked",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "BookingGuests");

            migrationBuilder.DropColumn(
                name: "IsPrimaryBooker",
                table: "BookingGuests");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6319), new DateTime(2026, 6, 5, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6326), new DateTime(2026, 5, 29, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6329) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 18, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6332), new DateTime(2026, 6, 21, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6332), new DateTime(2026, 6, 11, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6334) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 23, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6336), new DateTime(2026, 6, 25, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6336), new DateTime(2026, 6, 12, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6338) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 8, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6341), new DateTime(2026, 6, 10, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6342), new DateTime(2026, 6, 3, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6344) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 14, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6345), new DateTime(2026, 6, 17, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6345), new DateTime(2026, 6, 8, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6347) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 13, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6348), new DateTime(2026, 5, 15, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6353), new DateTime(2026, 5, 8, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6355) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 13, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6356), new DateTime(2026, 4, 16, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6357), new DateTime(2026, 4, 3, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6359) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 24, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6360), new DateTime(2026, 5, 26, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6361), new DateTime(2026, 5, 19, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6362) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 29, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6365), new DateTime(2026, 6, 3, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6365), new DateTime(2026, 5, 24, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6366) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 3, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6368), new DateTime(2026, 7, 5, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6369), new DateTime(2026, 6, 12, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6370) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 28, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6372), new DateTime(2026, 7, 1, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6372), new DateTime(2026, 6, 13, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6374) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 13, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6375), new DateTime(2026, 2, 18, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6376), new DateTime(2026, 2, 3, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6377) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 13, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6379), new DateTime(2026, 1, 14, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6379), new DateTime(2026, 1, 10, 18, 46, 40, 311, DateTimeKind.Local).AddTicks(6381) });
        }
    }
}
