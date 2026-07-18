using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomCleaningLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoomCleaningLogs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    AssignedStaffName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomCleaningLogs", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_RoomCleaningLogs_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 1,
                column: "Comment",
                value: "Tour khám phá đảo rất thú vị. Hướng dẫn viên nhiệt tình, tuy nhiên xe đón xuất phát trễ khoảng 10 phút. Tổng thể vẫn rất đáng trải nghiệm.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 2,
                column: "Comment",
                value: "Trải nghiệm lưu trú tuyệt vời! Phòng ốc chuẩn 5 sao, view hướng biển cực đẹp. Bữa tối tại nhà hàng rất ngon, nhân viên phục vụ tại bàn chu đáo.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 3,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Chuyến đi lặn ngắm san hô thật sự ấn tượng. Dịch vụ cano riêng rất êm, có chuẩn bị sẵn khăn tắm, đồ ăn nhẹ và nước uống vô cùng cẩn thận.", 5 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 4,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Dịch vụ phòng (Room service) cực kỳ nhanh chóng. Tôi gọi bữa sáng mang lên phòng và mọi thứ đều nóng hổi, trình bày đẹp mắt như ăn ở nhà hàng.", 5 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 5,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Lịch trình tour thám hiểm rừng quốc gia sắp xếp hợp lý, không bị mệt. Cảnh quan đẹp tuyệt vời và các bạn guide rất am hiểu kiến thức bản địa.", 5 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 6,
                column: "Comment",
                value: "Đồ ăn tại nhà hàng buffet phong phú, tôm hùm tươi ngon. Tuy nhiên hồ bơi chung hơi đông vào buổi chiều. Nhân viên hỗ trợ lấy khăn rất nhiệt tình.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 7,
                column: "Comment",
                value: "Trải nghiệm tour du thuyền ngắm hoàng hôn ngoài sức tưởng tượng. Rượu vang và hải sản phục vụ trên tàu đều là loại hảo hạng, sang trọng.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 8,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Đúng chuẩn resort 5 sao! Không gian yên tĩnh, thiết kế đẳng cấp. Tôi rất thích bữa tối lãng mạn được setup riêng tư ngay trên bãi biển.", 5 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 9,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Xe limousine đưa đón đi tour đời mới rất êm ái. Lịch trình tham quan linh hoạt theo sức khỏe của gia dịch tôi, một dịch vụ rất đáng tiền.", 5 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 10,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Mọi thứ đều hoàn hảo từ lúc check-in đến check-out. Giường ngủ cực kỳ êm ái. Gọi cocktail phục vụ tại hồ bơi riêng rất nhanh.", 5 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 11,
                column: "Comment",
                value: "Tour đạp xe quanh bản làng không khí rất trong lành. Đồ ăn trưa set menu ngon nhưng hơi ít lựa chọn cho người ăn thuần chay.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 12,
                column: "Comment",
                value: "Gia đình tôi đã có kỳ nghỉ trọn vẹn. Gọi món ăn phục vụ tận phòng lúc 1 giờ sáng nhưng chất lượng steak và pasta vẫn cực kỳ xuất sắc.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 13,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Chuyến đi chèo kayak rất an toàn và chuyên nghiệp. Các trang thiết bị bảo hộ, áo phao của resort đều còn rất mới và chất lượng cao.", 5 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 14,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Nhà hàng fine-dining mang đến hương vị bùng nổ. Các bạn phục vụ bàn cực kỳ tinh tế, luôn để ý rót thêm rượu và giải thích rõ từng món.", 5 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 15,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Tour câu mực đêm rất vui, tuy nhiên hôm đó sóng hơi lớn nên tôi bị say sóng chút xíu. Bù lại nhân viên pha trà gừng chăm sóc rất ân cần.", 4 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 16,
                column: "Comment",
                value: "Phòng rộng, sạch sẽ, bồn tắm nằm siêu to. Hệ thống điện smart-home trong phòng đôi lúc hơi khó điều khiển. Dịch vụ dọn phòng chu đáo.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 17,
                column: "Comment",
                value: "Tour đi tham quan vườn quốc gia và suối thác vô cùng tuyệt diệu. Bữa trưa picnic do resort chuẩn bị mang theo thực sự xuất sắc và đầy đặn.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 18,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Thích nhất là dịch vụ khay bữa sáng nổi (floating breakfast) ở hồ bơi riêng trong villa. Góc nào trong phòng cũng chụp được ảnh sống ảo.", 5 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 19,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Chương trình tour tham quan làng nghề địa phương khá hay và nhiều thông tin. Mong resort cân đối cho thêm khoảng 30 phút để tự do mua sắm.", 4 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 20,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Chất lượng ẩm thực, tiện ích spa và dịch vụ phòng thật sự vượt cả kỳ vọng. Thái độ nhân viên chuẩn 5 sao. Chắc chắn sẽ quay lại vào năm sau!", 5 });

            migrationBuilder.CreateIndex(
                name: "IX_RoomCleaningLogs_RoomId",
                table: "RoomCleaningLogs",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomCleaningLogs");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 1,
                column: "Comment",
                value: "Sample review 01 for resort service scenario.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 2,
                column: "Comment",
                value: "Sample review 02 for resort service scenario.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 3,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Sample review 03 for resort service scenario.", 1 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 4,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Sample review 04 for resort service scenario.", 2 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 5,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Sample review 05 for resort service scenario.", 3 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 6,
                column: "Comment",
                value: "Sample review 06 for resort service scenario.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 7,
                column: "Comment",
                value: "Sample review 07 for resort service scenario.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 8,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Sample review 08 for resort service scenario.", 1 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 9,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Sample review 09 for resort service scenario.", 2 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 10,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Sample review 10 for resort service scenario.", 3 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 11,
                column: "Comment",
                value: "Sample review 11 for resort service scenario.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 12,
                column: "Comment",
                value: "Sample review 12 for resort service scenario.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 13,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Sample review 13 for resort service scenario.", 1 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 14,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Sample review 14 for resort service scenario.", 2 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 15,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Sample review 15 for resort service scenario.", 3 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 16,
                column: "Comment",
                value: "Sample review 16 for resort service scenario.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 17,
                column: "Comment",
                value: "Sample review 17 for resort service scenario.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 18,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Sample review 18 for resort service scenario.", 1 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 19,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Sample review 19 for resort service scenario.", 2 });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 20,
                columns: new[] { "Comment", "Rating" },
                values: new object[] { "Sample review 20 for resort service scenario.", 3 });
        }
    }
}