using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTableReservationRoomCharge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "TableReservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ChargedAmount",
                table: "TableReservations",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ChargedAt",
                table: "TableReservations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsChargedToRoom",
                table: "TableReservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 1,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 2,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 3,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 4,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 5,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 6,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 7,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 8,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 9,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 10,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 11,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 12,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 13,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 14,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 15,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 16,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 17,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 18,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 19,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TableReservations",
                keyColumn: "ReservationId",
                keyValue: 20,
                columns: new[] { "BookingId", "ChargedAt" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_TableReservations_BookingId",
                table: "TableReservations",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_TableReservations_Bookings_BookingId",
                table: "TableReservations",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TableReservations_Bookings_BookingId",
                table: "TableReservations");

            migrationBuilder.DropIndex(
                name: "IX_TableReservations_BookingId",
                table: "TableReservations");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "TableReservations");

            migrationBuilder.DropColumn(
                name: "ChargedAmount",
                table: "TableReservations");

            migrationBuilder.DropColumn(
                name: "ChargedAt",
                table: "TableReservations");

            migrationBuilder.DropColumn(
                name: "IsChargedToRoom",
                table: "TableReservations");
        }
    }
}
