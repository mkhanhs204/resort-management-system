using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingDetailToFoodOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingDetailId",
                table: "FoodOrders",
                type: "int",
                nullable: true);

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

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 1,
                column: "BookingDetailId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 2,
                column: "BookingDetailId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 3,
                column: "BookingDetailId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 4,
                column: "BookingDetailId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 5,
                column: "BookingDetailId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 6,
                column: "BookingDetailId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 7,
                column: "BookingDetailId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 8,
                column: "BookingDetailId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 9,
                column: "BookingDetailId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 10,
                column: "BookingDetailId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 11,
                column: "BookingDetailId",
                value: 11);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 12,
                column: "BookingDetailId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 13,
                column: "BookingDetailId",
                value: 13);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 14,
                column: "BookingDetailId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 15,
                column: "BookingDetailId",
                value: 15);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 16,
                column: "BookingDetailId",
                value: 16);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 17,
                column: "BookingDetailId",
                value: 17);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 18,
                column: "BookingDetailId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 19,
                column: "BookingDetailId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "FoodOrders",
                keyColumn: "FoodOrderId",
                keyValue: 20,
                column: "BookingDetailId",
                value: 20);

            migrationBuilder.CreateIndex(
                name: "IX_FoodOrders_BookingDetailId",
                table: "FoodOrders",
                column: "BookingDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodOrders_BookingDetails_BookingDetailId",
                table: "FoodOrders",
                column: "BookingDetailId",
                principalTable: "BookingDetails",
                principalColumn: "BookingDetailId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodOrders_BookingDetails_BookingDetailId",
                table: "FoodOrders");

            migrationBuilder.DropIndex(
                name: "IX_FoodOrders_BookingDetailId",
                table: "FoodOrders");

            migrationBuilder.DropColumn(
                name: "BookingDetailId",
                table: "FoodOrders");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 12, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3809), new DateTime(2026, 6, 14, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3822), new DateTime(2026, 6, 7, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3825) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 27, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3828), new DateTime(2026, 6, 30, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3829), new DateTime(2026, 6, 20, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3831) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 2, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3841), new DateTime(2026, 7, 4, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3865), new DateTime(2026, 6, 21, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3867) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 17, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3870), new DateTime(2026, 6, 19, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3871), new DateTime(2026, 6, 12, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3873) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 23, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3876), new DateTime(2026, 6, 26, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3876), new DateTime(2026, 6, 17, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3879) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 22, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3881), new DateTime(2026, 5, 24, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3886), new DateTime(2026, 5, 17, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3889) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 22, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3892), new DateTime(2026, 4, 25, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3892), new DateTime(2026, 4, 12, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3895) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 2, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3897), new DateTime(2026, 6, 4, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3898), new DateTime(2026, 5, 28, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3900) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 7, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3903), new DateTime(2026, 6, 12, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3904), new DateTime(2026, 6, 2, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3906) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 12, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3908), new DateTime(2026, 7, 14, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3909), new DateTime(2026, 6, 21, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3911) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 7, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3914), new DateTime(2026, 7, 10, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3914), new DateTime(2026, 6, 22, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3923) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 22, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3925), new DateTime(2026, 2, 27, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3926), new DateTime(2026, 2, 12, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3928) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 22, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3931), new DateTime(2026, 1, 23, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3932), new DateTime(2026, 1, 19, 3, 14, 0, 365, DateTimeKind.Local).AddTicks(3948) });
        }
    }
}
