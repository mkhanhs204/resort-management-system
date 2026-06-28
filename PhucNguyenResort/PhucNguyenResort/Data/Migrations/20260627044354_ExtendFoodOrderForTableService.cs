using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendFoodOrderForTableService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodOrders_Bookings_BookingId",
                table: "FoodOrders");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "FoodOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaidAtRestaurant",
                table: "FoodOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OrderType",
                table: "FoodOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "RoomService");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidAt",
                table: "FoodOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TableReservationId",
                table: "FoodOrders",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 1,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 2,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 3,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 4,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 5,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 6,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 7,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 8,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 9,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 10,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 11,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 12,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 13,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 14,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 15,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 16,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 17,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 18,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 19,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 20,
                columns: new[] { "PaidAt", "TableReservationId" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_FoodOrders_TableReservationId",
                table: "FoodOrders",
                column: "TableReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodOrders_Bookings_BookingId",
                table: "FoodOrders",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodOrders_TableReservations_TableReservationId",
                table: "FoodOrders",
                column: "TableReservationId",
                principalTable: "TableReservations",
                principalColumn: "ReservationId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodOrders_Bookings_BookingId",
                table: "FoodOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodOrders_TableReservations_TableReservationId",
                table: "FoodOrders");

            migrationBuilder.DropIndex(
                name: "IX_FoodOrders_TableReservationId",
                table: "FoodOrders");

            migrationBuilder.DropColumn(
                name: "IsPaidAtRestaurant",
                table: "FoodOrders");

            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "FoodOrders");

            migrationBuilder.DropColumn(
                name: "PaidAt",
                table: "FoodOrders");

            migrationBuilder.DropColumn(
                name: "TableReservationId",
                table: "FoodOrders");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "FoodOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodOrders_Bookings_BookingId",
                table: "FoodOrders",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
