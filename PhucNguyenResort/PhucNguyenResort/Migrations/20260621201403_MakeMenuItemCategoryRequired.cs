using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Migrations
{
    /// <inheritdoc />
    public partial class MakeMenuItemCategoryRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_MenuCategories_CategoryId",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_CategoryId_ItemName",
                table: "MenuItems");

            migrationBuilder.DropCheckConstraint(
                name: "CK_MenuItems_AvailableRequiresCategory",
                table: "MenuItems");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "MenuItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_CategoryId_ItemName",
                table: "MenuItems",
                columns: new[] { "CategoryId", "ItemName" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_MenuCategories_CategoryId",
                table: "MenuItems",
                column: "CategoryId",
                principalTable: "MenuCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_MenuCategories_CategoryId",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_CategoryId_ItemName",
                table: "MenuItems");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "MenuItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 9, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5789), new DateTime(2026, 6, 11, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5796), new DateTime(2026, 6, 4, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5804) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 24, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5806), new DateTime(2026, 6, 27, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5806), new DateTime(2026, 6, 17, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5808) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 29, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5810), new DateTime(2026, 7, 1, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5811), new DateTime(2026, 6, 18, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5813) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 14, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5814), new DateTime(2026, 6, 16, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5815), new DateTime(2026, 6, 9, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5816) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 20, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5818), new DateTime(2026, 6, 23, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5818), new DateTime(2026, 6, 14, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5820) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 19, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5822), new DateTime(2026, 5, 21, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5826), new DateTime(2026, 5, 14, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5828) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 19, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5830), new DateTime(2026, 4, 22, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5830), new DateTime(2026, 4, 9, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5833) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5835), new DateTime(2026, 6, 1, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5835), new DateTime(2026, 5, 25, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5836) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 4, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5838), new DateTime(2026, 6, 9, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5838), new DateTime(2026, 5, 30, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5840) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 9, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5841), new DateTime(2026, 7, 11, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5842), new DateTime(2026, 6, 18, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5843) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 4, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5845), new DateTime(2026, 7, 7, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5845), new DateTime(2026, 6, 19, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5847) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 19, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5848), new DateTime(2026, 2, 24, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5849), new DateTime(2026, 2, 9, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5851) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 19, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5853), new DateTime(2026, 1, 20, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5854), new DateTime(2026, 1, 16, 9, 22, 51, 799, DateTimeKind.Local).AddTicks(5855) });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_CategoryId_ItemName",
                table: "MenuItems",
                columns: new[] { "CategoryId", "ItemName" },
                unique: true,
                filter: "[CategoryId] IS NOT NULL AND [IsDeleted] = 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_MenuItems_AvailableRequiresCategory",
                table: "MenuItems",
                sql: "[IsAvailable] = 0 OR [CategoryId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_MenuCategories_CategoryId",
                table: "MenuItems",
                column: "CategoryId",
                principalTable: "MenuCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
