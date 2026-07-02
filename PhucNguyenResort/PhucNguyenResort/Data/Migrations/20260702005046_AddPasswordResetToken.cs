using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordResetToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordTokenExpiry",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "Description",
                value: "Phòng cao cấp hướng biển");

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "Description",
                value: "Phòng view vườn yên tĩnh");

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "Description",
                value: "Suite rộng rãi cho gia đình");

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "Description",
                value: "Biệt thự siêu cấp sang trọng");

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "Description",
                value: "Biệt thự lãng mạn cho cặp đôi");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 4,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 5,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 8,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 9,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 10,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 11,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 12,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 13,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 14,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 15,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 16,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 17,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 18,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 19,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 20,
                columns: new[] { "ResetPasswordToken", "ResetPasswordTokenExpiry" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResetPasswordTokenExpiry",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "Description",
                value: "Phòng cao c?p hu?ng bi?n");

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "Description",
                value: "Phòng view vu?n yên tinh");

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "Description",
                value: "Suite r?ng rãi cho gia dình");

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "Description",
                value: "Bi?t th? siêu c?p sang tr?ng");

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "Description",
                value: "Bi?t th? lãng m?n cho c?p dôi");
        }
    }
}
