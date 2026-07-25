using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomSeedData_VN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 1,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Phòng Deluxe Hướng Biển", "Tận hưởng không gian sang trọng với ban công riêng biệt hướng ra đại dương xanh mát, trang bị bồn tắm nằm cao cấp và giường ngủ êm ái chuẩn 5 sao." });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 2,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Phòng Premium Hướng Vườn", "Không gian nghỉ dưỡng yên bình giữa khu vườn nhiệt đới xanh tươi, trang bị nội thất gỗ tự nhiên, phù hợp cho những ai tìm kiếm sự tĩnh lặng và thư giãn tuyệt đối." });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 3,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Phòng Suite Gia Đình", "Thiết kế rộng rãi với 2 phòng ngủ riêng biệt và khu vực phòng khách chung, ban công lớn, lý tưởng cho những kỳ nghỉ gắn kết yêu thương của cả gia đình." });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 4,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Biệt Thự Tổng Thống", "Tuyệt tác nghỉ dưỡng diện tích khủng với hồ bơi vô cực riêng, quản gia phục vụ 24/7, phòng khách xa hoa và tiện nghi đẳng cấp bậc nhất dành cho giới thượng lưu." });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 5,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Biệt Thự Trăng Mật", "Chốn hẹn hò lãng mạn cho các cặp uyên ương với không gian hoàn toàn riêng tư, hồ bơi cá nhân nhỏ, setup hoa hồng, nến và dịch vụ chăm sóc tại phòng mang đến kỉ niệm khó quên." });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "AssignedStaffName", "CategoryId", "FloorNumber", "RoomNumber" },
                values: new object[,]
                {
                    { 21, null, 1, 1, "105" },
                    { 22, null, 1, 1, "106" },
                    { 23, null, 2, 2, "205" },
                    { 24, null, 2, 2, "206" },
                    { 25, null, 3, 3, "305" },
                    { 26, null, 3, 3, "306" },
                    { 27, null, 4, 4, "405" },
                    { 28, null, 4, 4, "406" },
                    { 29, null, 5, 5, "505" },
                    { 30, null, 5, 5, "506" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 30);

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 1,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Deluxe Ocean View", "Phòng cao cấp hướng biển" });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 2,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Premium Garden View", "Phòng view vườn yên tĩnh" });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 3,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Family Suite", "Suite rộng rãi cho gia đình" });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 4,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Presidential Villa", "Biệt thự siêu cấp sang trọng" });

            migrationBuilder.UpdateData(
                table: "RoomCategories",
                keyColumn: "CategoryId",
                keyValue: 5,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Honeymoon Villa", "Biệt thự lãng mạn cho cặp đôi" });
        }
    }
}
