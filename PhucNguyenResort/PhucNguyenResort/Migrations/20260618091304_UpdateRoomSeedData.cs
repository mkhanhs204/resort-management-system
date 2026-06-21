using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PhucNguyenResort.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 2,
                columns: new[] { "Price", "RoomCategoryId" },
                values: new object[] { 1200000m, 1 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 3,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 1200000m, 1, 3 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 4,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 1200000m, 1, 4 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 5,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 1000000m, 2, 5 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 6,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 1000000m, 2, 6 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 7,
                column: "RoomId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 8,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 1000000m, 2, 8 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 9,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 2500000m, 3, 9 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 10,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 2500000m, 3, 10 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 11,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 2500000m, 3, 11 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 12,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 2500000m, 3, 12 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 13,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 8000000m, 4, 13 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 14,
                columns: new[] { "Price", "RoomCategoryId" },
                values: new object[] { 8000000m, 4 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 15,
                columns: new[] { "Price", "RoomCategoryId" },
                values: new object[] { 8000000m, 4 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 16,
                columns: new[] { "Price", "RoomCategoryId" },
                values: new object[] { 8000000m, 4 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 17,
                columns: new[] { "Price", "RoomCategoryId" },
                values: new object[] { 3500000m, 5 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 18,
                columns: new[] { "Price", "RoomCategoryId" },
                values: new object[] { 3500000m, 5 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 19,
                columns: new[] { "Price", "RoomCategoryId" },
                values: new object[] { 3500000m, 5 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 20,
                columns: new[] { "Price", "RoomCategoryId" },
                values: new object[] { 3500000m, 5 });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 8, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3575), new DateTime(2026, 6, 10, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3582), new DateTime(2026, 6, 3, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3586) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 23, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3589), new DateTime(2026, 6, 26, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3590), new DateTime(2026, 6, 16, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3591) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 28, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3593), new DateTime(2026, 6, 30, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3594), new DateTime(2026, 6, 17, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3596) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 13, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3597), new DateTime(2026, 6, 15, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3598), new DateTime(2026, 6, 8, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3600) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 19, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3602), new DateTime(2026, 6, 22, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3602), new DateTime(2026, 6, 13, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3604) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 18, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3606), new DateTime(2026, 5, 20, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3614), new DateTime(2026, 5, 13, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3616) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3618), new DateTime(2026, 4, 21, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3619), new DateTime(2026, 4, 8, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3622) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 29, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3624), new DateTime(2026, 5, 31, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3624), new DateTime(2026, 5, 24, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3626) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 3, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3628), new DateTime(2026, 6, 8, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3628), new DateTime(2026, 5, 29, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3630) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 8, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3654), new DateTime(2026, 7, 10, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3654), new DateTime(2026, 6, 17, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3656) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 3, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3658), new DateTime(2026, 7, 6, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3659), new DateTime(2026, 6, 18, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3660) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 18, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3662), new DateTime(2026, 2, 23, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3662), new DateTime(2026, 2, 8, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3665) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 18, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3667), new DateTime(2026, 1, 19, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3668), new DateTime(2026, 1, 15, 16, 13, 2, 463, DateTimeKind.Local).AddTicks(3670) });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 5,
                columns: new[] { "CategoryName", "Description", "ImageUrl", "MaxAdults", "PricePerNight" },
                values: new object[] { "Honeymoon Villa", "Biệt thự lãng mạn cho cặp đôi", "/images/rooms/category-08.jpg", 2, 3500000m });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 2,
                column: "CategoryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 3,
                columns: new[] { "CategoryId", "Status" },
                values: new object[] { 1, "Available" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 4,
                columns: new[] { "CategoryId", "Status" },
                values: new object[] { 1, "Available" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 5,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber" },
                values: new object[] { 2, 2, "201" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 6,
                columns: new[] { "CategoryId", "RoomNumber", "Status" },
                values: new object[] { 2, "202", "Maintenance" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 7,
                columns: new[] { "CategoryId", "RoomNumber", "Status" },
                values: new object[] { 2, "203", "Available" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 8,
                columns: new[] { "CategoryId", "RoomNumber", "Status" },
                values: new object[] { 2, "204", "Occupied" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 9,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber" },
                values: new object[] { 3, 3, "301" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 10,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber", "Status" },
                values: new object[] { 3, 3, "302", "Available" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 11,
                columns: new[] { "CategoryId", "RoomNumber", "Status" },
                values: new object[] { 3, "303", "NeedsCleaning" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 12,
                columns: new[] { "CategoryId", "RoomNumber", "Status" },
                values: new object[] { 3, "304", "Available" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 13,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber" },
                values: new object[] { 4, 4, "401" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 14,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber" },
                values: new object[] { 4, 4, "402" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 15,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber", "Status" },
                values: new object[] { 4, 4, "403", "Available" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 16,
                columns: new[] { "CategoryId", "RoomNumber", "Status" },
                values: new object[] { 4, "404", "Available" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 17,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber" },
                values: new object[] { 5, 5, "501" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 18,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber", "Status" },
                values: new object[] { 5, 5, "502", "Available" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 19,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber", "Status" },
                values: new object[] { 5, 5, "503", "Available" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 20,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber" },
                values: new object[] { 5, 5, "504" });

            migrationBuilder.DeleteData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 2,
                columns: new[] { "Price", "RoomCategoryId" },
                values: new object[] { 1000000m, 2 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 3,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 2700000m, 10, 10 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 4,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 2880000m, 11, 11 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 5,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 3240000m, 13, 13 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 6,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 1200000m, 1, 1 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 7,
                column: "RoomId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 8,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 8000000m, 4, 4 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 9,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 1800000m, 5, 5 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 10,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 3240000m, 13, 13 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 11,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 3420000m, 14, 14 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 12,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 1980000m, 6, 6 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 13,
                columns: new[] { "Price", "RoomCategoryId", "RoomId" },
                values: new object[] { 1200000m, 1, 1 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 14,
                columns: new[] { "Price", "RoomCategoryId" },
                values: new object[] { 3420000m, 14 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 15,
                columns: new[] { "Price", "RoomCategoryId" },
                values: new object[] { 3600000m, 15 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 16,
                columns: new[] { "Price", "RoomCategoryId" },
                values: new object[] { 3780000m, 16 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 17,
                columns: new[] { "Price", "RoomCategoryId" },
                values: new object[] { 3960000m, 17 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 18,
                columns: new[] { "Price", "RoomCategoryId" },
                values: new object[] { 4140000m, 18 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 19,
                columns: new[] { "Price", "RoomCategoryId" },
                values: new object[] { 4320000m, 19 });

            migrationBuilder.UpdateData(
                table: "BookingDetails",
                keyColumn: "BookingDetailId",
                keyValue: 20,
                columns: new[] { "Price", "RoomCategoryId" },
                values: new object[] { 4500000m, 20 });

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

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 5,
                columns: new[] { "CategoryName", "Description", "ImageUrl", "MaxAdults", "PricePerNight" },
                values: new object[] { "Family Suite", "Family Suite category with capacity 4 guest(s).", "/images/rooms/category-05.jpg", 4, 1800000.00m });

            migrationBuilder.InsertData(
                table: "RoomCategories",
                columns: new[] { "CategoryId", "CategoryName", "Description", "ImageUrl", "MaxAdults", "MaxChildren", "PricePerNight" },
                values: new object[,]
                {
                    { 6, "Executive Suite", "Executive Suite category with capacity 4 guest(s).", "/images/rooms/category-06.jpg", 4, 0, 1980000.00m },
                    { 7, "Presidential Villa", "Presidential Villa category with capacity 6 guest(s).", "/images/rooms/category-07.jpg", 6, 0, 2160000.00m },
                    { 8, "Honeymoon Villa", "Honeymoon Villa category with capacity 2 guest(s).", "/images/rooms/category-08.jpg", 2, 0, 2340000.00m },
                    { 9, "Bungalow Garden", "Bungalow Garden category with capacity 2 guest(s).", "/images/rooms/category-09.jpg", 2, 0, 2520000.00m },
                    { 10, "Bungalow Beach", "Bungalow Beach category with capacity 3 guest(s).", "/images/rooms/category-10.jpg", 3, 0, 2700000.00m },
                    { 11, "Twin Standard", "Twin Standard category with capacity 2 guest(s).", "/images/rooms/category-11.jpg", 2, 0, 2880000.00m },
                    { 12, "Triple Deluxe", "Triple Deluxe category with capacity 3 guest(s).", "/images/rooms/category-12.jpg", 3, 0, 3060000.00m },
                    { 13, "Accessible Room", "Accessible Room category with capacity 2 guest(s).", "/images/rooms/category-13.jpg", 2, 0, 3240000.00m },
                    { 14, "Connecting Room", "Connecting Room category with capacity 4 guest(s).", "/images/rooms/category-14.jpg", 4, 0, 3420000.00m },
                    { 15, "Long Stay Studio", "Long Stay Studio category with capacity 2 guest(s).", "/images/rooms/category-15.jpg", 2, 0, 3600000.00m },
                    { 16, "Business Room", "Business Room category with capacity 2 guest(s).", "/images/rooms/category-16.jpg", 2, 0, 3780000.00m },
                    { 17, "Penthouse Suite", "Penthouse Suite category with capacity 4 guest(s).", "/images/rooms/category-17.jpg", 4, 0, 3960000.00m },
                    { 18, "Lagoon Villa", "Lagoon Villa category with capacity 5 guest(s).", "/images/rooms/category-18.jpg", 5, 0, 4140000.00m },
                    { 19, "Mountain View", "Mountain View category with capacity 2 guest(s).", "/images/rooms/category-19.jpg", 2, 0, 4320000.00m },
                    { 20, "Budget Room", "Budget Room category with capacity 2 guest(s).", "/images/rooms/category-20.jpg", 2, 0, 4500000.00m }
                });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 2,
                column: "CategoryId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 3,
                columns: new[] { "CategoryId", "Status" },
                values: new object[] { 3, "Maintenance" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 4,
                columns: new[] { "CategoryId", "Status" },
                values: new object[] { 4, "NeedsCleaning" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 5,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber" },
                values: new object[] { 5, 1, "105" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 6,
                columns: new[] { "CategoryId", "RoomNumber", "Status" },
                values: new object[] { 6, "106", "Occupied" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 7,
                columns: new[] { "CategoryId", "RoomNumber", "Status" },
                values: new object[] { 7, "107", "Maintenance" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 8,
                columns: new[] { "CategoryId", "RoomNumber", "Status" },
                values: new object[] { 8, "108", "NeedsCleaning" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 9,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber" },
                values: new object[] { 9, 2, "109" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 10,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber", "Status" },
                values: new object[] { 10, 2, "110", "Occupied" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 11,
                columns: new[] { "CategoryId", "RoomNumber", "Status" },
                values: new object[] { 11, "111", "Maintenance" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 12,
                columns: new[] { "CategoryId", "RoomNumber", "Status" },
                values: new object[] { 12, "112", "NeedsCleaning" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 13,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber" },
                values: new object[] { 13, 3, "113" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 14,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber" },
                values: new object[] { 14, 3, "114" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 15,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber", "Status" },
                values: new object[] { 15, 3, "115", "Maintenance" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 16,
                columns: new[] { "CategoryId", "RoomNumber", "Status" },
                values: new object[] { 16, "116", "NeedsCleaning" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 17,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber" },
                values: new object[] { 17, 4, "117" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 18,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber", "Status" },
                values: new object[] { 18, 4, "118", "Occupied" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 19,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber", "Status" },
                values: new object[] { 19, 4, "119", "Maintenance" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 20,
                columns: new[] { "CategoryId", "FloorNumber", "RoomNumber" },
                values: new object[] { 20, 4, "120" });
        }
    }
}
