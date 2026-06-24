using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomHandoverTimeAndArrivedStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExpectedArrivalTime",
                table: "Bookings",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RoomHandoverTime",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { new DateTime(2026, 6, 14, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3831), new DateTime(2026, 6, 16, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3838), new DateTime(2026, 6, 9, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3841), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { new DateTime(2026, 6, 29, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3843), new DateTime(2026, 7, 2, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3844), new DateTime(2026, 6, 22, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3846), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { new DateTime(2026, 7, 4, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3848), new DateTime(2026, 7, 6, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3848), new DateTime(2026, 6, 23, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3850), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { new DateTime(2026, 6, 19, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3871), new DateTime(2026, 6, 21, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3872), new DateTime(2026, 6, 14, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3874), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { new DateTime(2026, 6, 25, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3876), new DateTime(2026, 6, 28, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3877), new DateTime(2026, 6, 19, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3878), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { new DateTime(2026, 5, 24, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3881), new DateTime(2026, 5, 26, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3886), new DateTime(2026, 5, 19, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3888), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { new DateTime(2026, 4, 24, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3890), new DateTime(2026, 4, 27, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3891), new DateTime(2026, 4, 14, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3894), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { new DateTime(2026, 6, 4, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3896), new DateTime(2026, 6, 6, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3896), new DateTime(2026, 5, 30, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3898), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { new DateTime(2026, 6, 9, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3900), new DateTime(2026, 6, 14, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3900), new DateTime(2026, 6, 4, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3902), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { new DateTime(2026, 7, 14, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3903), new DateTime(2026, 7, 16, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3904), new DateTime(2026, 6, 23, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3906), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { new DateTime(2026, 7, 9, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3907), new DateTime(2026, 7, 12, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3908), new DateTime(2026, 6, 24, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3909), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { new DateTime(2026, 2, 24, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3911), new DateTime(2026, 3, 1, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3911), new DateTime(2026, 2, 14, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3914), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt", "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { new DateTime(2026, 1, 24, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3916), new DateTime(2026, 1, 25, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3917), new DateTime(2026, 1, 21, 9, 37, 29, 979, DateTimeKind.Local).AddTicks(3919), null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 14,
                columns: new[] { "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 15,
                columns: new[] { "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 16,
                columns: new[] { "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 17,
                columns: new[] { "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 18,
                columns: new[] { "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 19,
                columns: new[] { "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 20,
                columns: new[] { "ExpectedArrivalTime", "RoomHandoverTime" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedArrivalTime",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RoomHandoverTime",
                table: "Bookings");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 12, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1859), new DateTime(2026, 6, 14, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1867), new DateTime(2026, 6, 7, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1871) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 27, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1874), new DateTime(2026, 6, 30, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1875), new DateTime(2026, 6, 20, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1877) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 2, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1879), new DateTime(2026, 7, 4, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1879), new DateTime(2026, 6, 21, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1881) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 17, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1883), new DateTime(2026, 6, 19, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1883), new DateTime(2026, 6, 12, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1885) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 23, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1887), new DateTime(2026, 6, 26, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1887), new DateTime(2026, 6, 17, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1889) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 22, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1891), new DateTime(2026, 5, 24, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1896), new DateTime(2026, 5, 17, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1898) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 22, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1899), new DateTime(2026, 4, 25, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1900), new DateTime(2026, 4, 12, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1903) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 2, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1905), new DateTime(2026, 6, 4, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1905), new DateTime(2026, 5, 28, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1907) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 7, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1908), new DateTime(2026, 6, 12, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1909), new DateTime(2026, 6, 2, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1910) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 12, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1912), new DateTime(2026, 7, 14, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1913), new DateTime(2026, 6, 21, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1915) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 7, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1916), new DateTime(2026, 7, 10, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1917), new DateTime(2026, 6, 22, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1918) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 22, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1920), new DateTime(2026, 2, 27, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1920), new DateTime(2026, 2, 12, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1923) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 22, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1925), new DateTime(2026, 1, 23, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1925), new DateTime(2026, 1, 19, 15, 19, 4, 498, DateTimeKind.Local).AddTicks(1928) });
        }
    }
}
