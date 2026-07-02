using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddMaintenanceTableFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaintenanceRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRequests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 12, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6819), new DateTime(2026, 6, 14, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6827), new DateTime(2026, 6, 7, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6831) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 27, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6833), new DateTime(2026, 6, 30, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6833), new DateTime(2026, 6, 20, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6835) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 2, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6837), new DateTime(2026, 7, 4, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6838), new DateTime(2026, 6, 21, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6839) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 17, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6841), new DateTime(2026, 6, 19, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6842), new DateTime(2026, 6, 12, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6843) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 23, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6845), new DateTime(2026, 6, 26, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6846), new DateTime(2026, 6, 17, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6847) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 22, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6849), new DateTime(2026, 5, 24, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6852), new DateTime(2026, 5, 17, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6854) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 22, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6856), new DateTime(2026, 4, 25, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6856), new DateTime(2026, 4, 12, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6858) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 2, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6860), new DateTime(2026, 6, 4, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6860), new DateTime(2026, 5, 28, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6862) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 7, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6894), new DateTime(2026, 6, 12, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6894), new DateTime(2026, 6, 2, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6896) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 12, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6898), new DateTime(2026, 7, 14, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6899), new DateTime(2026, 6, 21, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6900) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 7, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6902), new DateTime(2026, 7, 10, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6903), new DateTime(2026, 6, 22, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6904) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 22, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6906), new DateTime(2026, 2, 27, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6907), new DateTime(2026, 2, 12, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6908) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 22, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6910), new DateTime(2026, 1, 23, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6911), new DateTime(2026, 1, 19, 20, 32, 2, 624, DateTimeKind.Local).AddTicks(6913) });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_RoomId",
                table: "MaintenanceRequests",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenanceRequests");

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
