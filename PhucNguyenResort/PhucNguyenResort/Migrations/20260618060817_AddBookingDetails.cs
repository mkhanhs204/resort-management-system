using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PhucNguyenResort.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Rooms_RoomId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Bookings");

            migrationBuilder.CreateTable(
                name: "BookingDetails",
                columns: table => new
                {
                    BookingDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    RoomCategoryId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingDetails", x => x.BookingDetailId);
                    table.ForeignKey(
                        name: "FK_BookingDetails_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingDetails_RoomCategories_RoomCategoryId",
                        column: x => x.RoomCategoryId,
                        principalTable: "RoomCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookingDetails_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "BookingDetails",
                columns: new[] { "BookingDetailId", "BookingId", "Price", "RoomCategoryId", "RoomId" },
                values: new object[,]
                {
                    { 1, 1, 1200000m, 1, 1 },
                    { 2, 2, 1000000m, 2, 2 },
                    { 3, 3, 2700000m, 10, 10 },
                    { 4, 4, 2880000m, 11, 11 },
                    { 5, 5, 3240000m, 13, 13 },
                    { 6, 6, 1200000m, 1, 1 },
                    { 7, 7, 1000000m, 2, 2 },
                    { 8, 8, 8000000m, 4, 4 },
                    { 9, 9, 1800000m, 5, 5 },
                    { 10, 10, 3240000m, 13, 13 },
                    { 11, 11, 3420000m, 14, 14 },
                    { 12, 12, 1980000m, 6, 6 },
                    { 13, 13, 1200000m, 1, 1 },
                    { 14, 14, 3420000m, 14, 14 },
                    { 15, 15, 3600000m, 15, 15 },
                    { 16, 16, 3780000m, 16, 16 },
                    { 17, 17, 3960000m, 17, 17 },
                    { 18, 18, 4140000m, 18, 18 },
                    { 19, 19, 4320000m, 19, 19 },
                    { 20, 20, 4500000m, 20, 20 }
                });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 8, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4580), new DateTime(2026, 6, 10, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4590), new DateTime(2026, 6, 3, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4595) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 23, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4598), new DateTime(2026, 6, 26, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4599), new DateTime(2026, 6, 16, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4601) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 28, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4602), new DateTime(2026, 6, 30, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4603), new DateTime(2026, 6, 17, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4605) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 13, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4607), new DateTime(2026, 6, 15, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4607), new DateTime(2026, 6, 8, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4609) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 19, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4611), new DateTime(2026, 6, 22, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4611), new DateTime(2026, 6, 13, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4613) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 18, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4615), new DateTime(2026, 5, 20, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4619), new DateTime(2026, 5, 13, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4621) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 18, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4623), new DateTime(2026, 4, 21, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4624), new DateTime(2026, 4, 8, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4625) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 29, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4627), new DateTime(2026, 5, 31, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4628), new DateTime(2026, 5, 24, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4630) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4631), new DateTime(2026, 6, 8, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4632), new DateTime(2026, 5, 29, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4634) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 8, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4636), new DateTime(2026, 7, 10, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4637), new DateTime(2026, 6, 17, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4638) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 3, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4703), new DateTime(2026, 7, 6, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4704), new DateTime(2026, 6, 18, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4705) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 18, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4707), new DateTime(2026, 2, 23, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4708), new DateTime(2026, 2, 8, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4710) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 18, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4712), new DateTime(2026, 1, 19, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4713), new DateTime(2026, 1, 15, 13, 8, 15, 855, DateTimeKind.Local).AddTicks(4714) });

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_BookingId",
                table: "BookingDetails",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_RoomCategoryId",
                table: "BookingDetails",
                column: "RoomCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_RoomId",
                table: "BookingDetails",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingDetails");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "RoomId" },
                values: new object[] { new DateTime(2026, 6, 5, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7729), new DateTime(2026, 6, 7, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7736), new DateTime(2026, 5, 31, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7740), 1 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "RoomId" },
                values: new object[] { new DateTime(2026, 6, 20, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7743), new DateTime(2026, 6, 23, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7744), new DateTime(2026, 6, 13, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7746), 2 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "RoomId" },
                values: new object[] { new DateTime(2026, 6, 25, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7747), new DateTime(2026, 6, 27, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7748), new DateTime(2026, 6, 14, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7749), 10 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "RoomId" },
                values: new object[] { new DateTime(2026, 6, 10, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7752), new DateTime(2026, 6, 12, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7753), new DateTime(2026, 6, 5, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7755), 11 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "RoomId" },
                values: new object[] { new DateTime(2026, 6, 16, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7757), new DateTime(2026, 6, 19, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7757), new DateTime(2026, 6, 10, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7759), 13 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "RoomId" },
                values: new object[] { new DateTime(2026, 5, 15, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7761), new DateTime(2026, 5, 17, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7766), new DateTime(2026, 5, 10, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7768), 1 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "RoomId" },
                values: new object[] { new DateTime(2026, 4, 15, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7770), new DateTime(2026, 4, 18, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7770), new DateTime(2026, 4, 5, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7772), 2 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "RoomId" },
                values: new object[] { new DateTime(2026, 5, 26, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7774), new DateTime(2026, 5, 28, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7774), new DateTime(2026, 5, 21, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7777), 4 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "RoomId" },
                values: new object[] { new DateTime(2026, 5, 31, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7778), new DateTime(2026, 6, 5, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7779), new DateTime(2026, 5, 26, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7780), 5 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "RoomId" },
                values: new object[] { new DateTime(2026, 7, 5, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7782), new DateTime(2026, 7, 7, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7784), new DateTime(2026, 6, 14, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7785), 13 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "RoomId" },
                values: new object[] { new DateTime(2026, 6, 30, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7787), new DateTime(2026, 7, 3, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7788), new DateTime(2026, 6, 15, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7790), 14 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "RoomId" },
                values: new object[] { new DateTime(2026, 2, 15, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7792), new DateTime(2026, 2, 20, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7792), new DateTime(2026, 2, 5, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7794), 6 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "RoomId" },
                values: new object[] { new DateTime(2026, 1, 15, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7796), new DateTime(2026, 1, 16, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7796), new DateTime(2026, 1, 12, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7798), 1 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 14,
                column: "RoomId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 15,
                column: "RoomId",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 16,
                column: "RoomId",
                value: 16);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 17,
                column: "RoomId",
                value: 17);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 18,
                column: "RoomId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 19,
                column: "RoomId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 20,
                column: "RoomId",
                value: 20);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Rooms_RoomId",
                table: "Bookings",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
