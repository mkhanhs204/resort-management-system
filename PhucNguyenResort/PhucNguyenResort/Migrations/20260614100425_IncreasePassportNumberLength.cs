using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Migrations
{
    /// <inheritdoc />
    public partial class IncreasePassportNumberLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PassportNumber",
                table: "BookingGuests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 4, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2845), new DateTime(2026, 6, 6, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2853), new DateTime(2026, 5, 30, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2856) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 19, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2859), new DateTime(2026, 6, 22, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2860), new DateTime(2026, 6, 12, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2862) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 24, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2864), new DateTime(2026, 6, 26, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2865), new DateTime(2026, 6, 13, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2866) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 9, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2870), new DateTime(2026, 6, 11, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2870), new DateTime(2026, 6, 4, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2872) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 15, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2874), new DateTime(2026, 6, 18, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2875), new DateTime(2026, 6, 9, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2876) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 14, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2878), new DateTime(2026, 5, 16, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2883), new DateTime(2026, 5, 9, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2885) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 14, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2918), new DateTime(2026, 4, 17, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2918), new DateTime(2026, 4, 4, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2921) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 25, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2923), new DateTime(2026, 5, 27, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2923), new DateTime(2026, 5, 20, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2925) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2928), new DateTime(2026, 6, 4, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2928), new DateTime(2026, 5, 25, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2930) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 4, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2932), new DateTime(2026, 7, 6, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2933), new DateTime(2026, 6, 13, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2935) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 29, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2937), new DateTime(2026, 7, 2, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2937), new DateTime(2026, 6, 14, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2939) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 14, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2941), new DateTime(2026, 2, 19, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2941), new DateTime(2026, 2, 4, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2944) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 14, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2945), new DateTime(2026, 1, 15, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2946), new DateTime(2026, 1, 11, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2948) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PassportNumber",
                table: "BookingGuests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 4, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(949), new DateTime(2026, 6, 6, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(956), new DateTime(2026, 5, 30, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(959) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 19, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(962), new DateTime(2026, 6, 22, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(963), new DateTime(2026, 6, 12, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(965) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 24, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(967), new DateTime(2026, 6, 26, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(968), new DateTime(2026, 6, 13, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(970) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 9, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(973), new DateTime(2026, 6, 11, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(973), new DateTime(2026, 6, 4, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(977) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 15, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(979), new DateTime(2026, 6, 18, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(979), new DateTime(2026, 6, 9, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(981) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 14, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(983), new DateTime(2026, 5, 16, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(988), new DateTime(2026, 5, 9, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(990) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 14, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1008), new DateTime(2026, 4, 17, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1009), new DateTime(2026, 4, 4, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1011) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 25, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1014), new DateTime(2026, 5, 27, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1014), new DateTime(2026, 5, 20, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1016) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1020), new DateTime(2026, 6, 4, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1020), new DateTime(2026, 5, 25, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1022) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 4, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1024), new DateTime(2026, 7, 6, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1025), new DateTime(2026, 6, 13, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1027) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 29, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1029), new DateTime(2026, 7, 2, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1029), new DateTime(2026, 6, 14, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1031) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 14, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1033), new DateTime(2026, 2, 19, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1034), new DateTime(2026, 2, 4, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1036) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 14, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1038), new DateTime(2026, 1, 15, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1038), new DateTime(2026, 1, 11, 16, 45, 39, 928, DateTimeKind.Local).AddTicks(1040) });
        }
    }
}
