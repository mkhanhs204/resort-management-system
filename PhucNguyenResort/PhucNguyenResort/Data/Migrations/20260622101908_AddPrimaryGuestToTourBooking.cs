using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPrimaryGuestToTourBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TourBookingGuests");

            migrationBuilder.AddColumn<DateTime>(
                name: "PrimaryGuestDateOfBirth",
                table: "TourBookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrimaryGuestGender",
                table: "TourBookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryGuestName",
                table: "TourBookings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryGuestNationality",
                table: "TourBookings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryGuestPassportNumber",
                table: "TourBookings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 12, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4892), new DateTime(2026, 6, 14, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4897), new DateTime(2026, 6, 7, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4899) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 27, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4902), new DateTime(2026, 6, 30, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4902), new DateTime(2026, 6, 20, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4904) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 2, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4906), new DateTime(2026, 7, 4, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4906), new DateTime(2026, 6, 21, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4908) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 17, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4910), new DateTime(2026, 6, 19, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4910), new DateTime(2026, 6, 12, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4912) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 23, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4913), new DateTime(2026, 6, 26, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4914), new DateTime(2026, 6, 17, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4916) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4917), new DateTime(2026, 5, 24, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4920), new DateTime(2026, 5, 17, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4922) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4953), new DateTime(2026, 4, 25, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4954), new DateTime(2026, 4, 12, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4956) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 2, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4958), new DateTime(2026, 6, 4, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4958), new DateTime(2026, 5, 28, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4960) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 7, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4961), new DateTime(2026, 6, 12, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4962), new DateTime(2026, 6, 2, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4964) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 12, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4965), new DateTime(2026, 7, 14, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4966), new DateTime(2026, 6, 21, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4968) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 7, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4969), new DateTime(2026, 7, 10, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4970), new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4972) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4973), new DateTime(2026, 2, 27, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4974), new DateTime(2026, 2, 12, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4976) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4978), new DateTime(2026, 1, 23, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4978), new DateTime(2026, 1, 19, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(4980) });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5815), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5820), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5822), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 4,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5823), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 5,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5825), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5826), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5828), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 8,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5830), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 9,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5863), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 10,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5864), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 11,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5866), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 12,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5868), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 13,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5870), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 14,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5871), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 15,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5873), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 16,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5875), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 17,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5877), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 18,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5879), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 19,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5880), null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "TourBookings",
                keyColumn: "TourBookingId",
                keyValue: 20,
                columns: new[] { "CreatedAt", "PrimaryGuestDateOfBirth", "PrimaryGuestGender", "PrimaryGuestName", "PrimaryGuestNationality", "PrimaryGuestPassportNumber" },
                values: new object[] { new DateTime(2026, 6, 22, 17, 19, 6, 2, DateTimeKind.Local).AddTicks(5881), null, null, null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryGuestDateOfBirth",
                table: "TourBookings");

            migrationBuilder.DropColumn(
                name: "PrimaryGuestGender",
                table: "TourBookings");

            migrationBuilder.DropColumn(
                name: "PrimaryGuestName",
                table: "TourBookings");

            migrationBuilder.DropColumn(
                name: "PrimaryGuestNationality",
                table: "TourBookings");

            migrationBuilder.DropColumn(
                name: "PrimaryGuestPassportNumber",
                table: "TourBookings");

            migrationBuilder.CreateTable(
                name: "TourBookingGuests",
                columns: table => new
                {
                    TourBookingGuestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourBookingId = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    IsPrimaryBooker = table.Column<bool>(type: "bit", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PassportNumber = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
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
    }
}
