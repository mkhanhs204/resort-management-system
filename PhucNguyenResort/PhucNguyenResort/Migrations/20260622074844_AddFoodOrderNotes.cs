using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddFoodOrderNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerNote",
                table: "FoodOrders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedDeliveryAt",
                table: "FoodOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemNote",
                table: "FoodOrderItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 12, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8407), new DateTime(2026, 6, 14, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8412), new DateTime(2026, 6, 7, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8415) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 27, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8418), new DateTime(2026, 6, 30, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8419), new DateTime(2026, 6, 20, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8420) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 2, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8422), new DateTime(2026, 7, 4, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8422), new DateTime(2026, 6, 21, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8424) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 17, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8426), new DateTime(2026, 6, 19, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8427), new DateTime(2026, 6, 12, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8428) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 23, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8430), new DateTime(2026, 6, 26, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8431), new DateTime(2026, 6, 17, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8432) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 22, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8434), new DateTime(2026, 5, 24, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8439), new DateTime(2026, 5, 17, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8441) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 22, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8443), new DateTime(2026, 4, 25, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8444), new DateTime(2026, 4, 12, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8445) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 2, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8447), new DateTime(2026, 6, 4, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8448), new DateTime(2026, 5, 28, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8449) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 7, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8451), new DateTime(2026, 6, 12, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8452), new DateTime(2026, 6, 2, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8453) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 12, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8455), new DateTime(2026, 7, 14, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8455), new DateTime(2026, 6, 21, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8457) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 7, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8490), new DateTime(2026, 7, 10, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8490), new DateTime(2026, 6, 22, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8492) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 22, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8494), new DateTime(2026, 2, 27, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8495), new DateTime(2026, 2, 12, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8496) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 22, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8498), new DateTime(2026, 1, 23, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8499), new DateTime(2026, 1, 19, 14, 48, 43, 744, DateTimeKind.Local).AddTicks(8500) });

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 1,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 2,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 3,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 4,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 5,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 6,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 7,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 8,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 9,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 10,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 11,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 12,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 13,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 14,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 15,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 16,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 17,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 18,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 19,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrderItems",
                keyColumn: "OrderItemId",
                keyValue: 20,
                column: "ItemNote",
                value: null);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 1,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 2,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 3,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 4,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 5,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 6,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 7,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 8,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 9,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 10,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 11,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 12,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 13,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 14,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 15,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 16,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 17,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 18,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 19,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 20,
                columns: new[] { "CustomerNote", "RequestedDeliveryAt" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerNote",
                table: "FoodOrders");

            migrationBuilder.DropColumn(
                name: "RequestedDeliveryAt",
                table: "FoodOrders");

            migrationBuilder.DropColumn(
                name: "ItemNote",
                table: "FoodOrderItems");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 12, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4012), new DateTime(2026, 6, 14, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4020), new DateTime(2026, 6, 7, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4024) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 27, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4027), new DateTime(2026, 6, 30, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4028), new DateTime(2026, 6, 20, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4030) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 2, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4034), new DateTime(2026, 7, 4, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4034), new DateTime(2026, 6, 21, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4036) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 17, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4039), new DateTime(2026, 6, 19, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4040), new DateTime(2026, 6, 12, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4042) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 23, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4045), new DateTime(2026, 6, 26, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4045), new DateTime(2026, 6, 17, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4048) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 22, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4051), new DateTime(2026, 5, 24, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4056), new DateTime(2026, 5, 17, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4059) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 22, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4061), new DateTime(2026, 4, 25, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4062), new DateTime(2026, 4, 12, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4064) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 2, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4066), new DateTime(2026, 6, 4, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4067), new DateTime(2026, 5, 28, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4068) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 7, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4129), new DateTime(2026, 6, 12, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4129), new DateTime(2026, 6, 2, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4131) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 12, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4134), new DateTime(2026, 7, 14, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4134), new DateTime(2026, 6, 21, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4137) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 7, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4139), new DateTime(2026, 7, 10, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4140), new DateTime(2026, 6, 22, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4142) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 22, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4144), new DateTime(2026, 2, 27, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4145), new DateTime(2026, 2, 12, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4147) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 22, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4151), new DateTime(2026, 1, 23, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4151), new DateTime(2026, 1, 19, 3, 34, 16, 557, DateTimeKind.Local).AddTicks(4153) });
        }
    }
}
