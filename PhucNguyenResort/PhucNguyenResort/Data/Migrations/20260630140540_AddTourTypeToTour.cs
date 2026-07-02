using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTourTypeToTour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TourType",
                table: "Tours",
                type: "nvarchar(100)",
                maxLength: 100,
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
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 1,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 2,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 3,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 4,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 5,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 6,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 7,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 8,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 9,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 10,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 11,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 12,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 13,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 14,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 15,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 16,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 17,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 18,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 19,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 20,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 21,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 22,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 23,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 24,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 25,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 26,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 27,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 28,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 29,
                column: "TourType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 30,
                column: "TourType",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TourType",
                table: "Tours");

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "Description",
                value: "Pḥng cao c?p hu?ng bi?n");

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "Description",
                value: "Pḥng view vu?n yên tinh");

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "Description",
                value: "Suite r?ng răi cho gia d́nh");

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
                value: "Bi?t th? lăng m?n cho c?p dôi");
        }
    }
}
