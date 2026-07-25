using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableSeedData_VN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 1,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Sân Vườn", "Bàn ăn thư giãn cạnh sân vườn, thích hợp cho nhóm nhỏ thích không gian yên tĩnh.", "Gần lối đi sân vườn nhiệt đới", "Bàn Hướng Sân Vườn", "Tiêu Chuẩn" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 2,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Hồ Bơi", "Chỗ ngồi rộng rãi cạnh hồ bơi, phù hợp cho bữa ăn gia đình và thư giãn tại resort.", "Gần khu vực hồ bơi", "Bàn Gia Đình Cạnh Hồ Bơi", "Gia Đình" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 3,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Trong Nhà", "Bàn ăn thoải mái trong nhà dành cho nhóm đông người, dễ dàng tiếp cận quầy phục vụ.", "Sảnh ăn chính", "Bàn Nhóm Trong Nhà", "Khách Đoàn" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 4,
                columns: new[] { "Area", "Description", "LocationNote", "TableName" },
                values: new object[] { "Phòng VIP", "Không gian phòng riêng dành cho các buổi tiệc kỷ niệm, gặp mặt hoặc ăn tối cao cấp.", "Khu vực phòng VIP", "Bàn Ăn Phòng Riêng" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 5,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Trong Nhà", "Bàn ăn lãng mạn dành cho 2 người với không gian sáng sủa cạnh cửa sổ.", "Cạnh cửa sổ", "Bàn Đôi Cạnh Cửa Sổ", "Cặp Đôi" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 6,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Ngoài Trời", "Bàn ăn không gian mở ngoài hiên với tầm nhìn thoáng đãng và dễ chịu.", "Khu vực hiên có mái che", "Bàn Ngoài Hiên", "Tiêu Chuẩn" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 7,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Sân Vườn", "Bàn ăn dành cho gia đình với khung cảnh sân vườn êm đềm.", "Góc sân vườn", "Bàn Gia Đình Ngoài Vườn", "Gia Đình" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 8,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Trong Nhà", "Bàn kích thước lớn được sử dụng cho khách đoàn và chuẩn bị tiệc lớn.", "Khu vực tiệc lớn", "Bàn Tiệc Lớn", "Khách Đoàn" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 9,
                columns: new[] { "Area", "Description", "LocationNote", "TableName" },
                values: new object[] { "Sân Vườn", "Khu vực chòi cao cấp ngoài vườn mang đến trải nghiệm ăn uống riêng tư, đẳng cấp.", "Chòi sân vườn", "Chòi VIP Ngoài Vườn" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 10,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Trong Nhà", "Khu vực ghế sofa êm ái dành cho 2 người thích sự yên tĩnh và riêng tư.", "Góc yên tĩnh", "Khu Vực Riêng Cho Cặp Đôi", "Sofa Góc" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 11,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Trong Nhà", "Ghế ngồi sofa thoải mái sát cạnh cửa sổ ngắm cảnh.", "Dọc cửa sổ", "Bàn Sofa Cạnh Cửa Sổ", "Sofa Góc" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 12,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Trong Nhà", "Bàn dự phòng dành riêng cho nhân viên vận hành trong giờ cao điểm.", "Gần trạm phục vụ", "Bàn Dự Phòng (Nhân Viên)", "Nội Bộ" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 13,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Ngoài Trời", "Bàn ăn ngoài trời cỡ lớn cho các buổi họp mặt và dùng bữa nhóm.", "Khu ăn uống ngoài trời", "Bàn Nhóm Ngoài Trời", "Khách Đoàn" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 14,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Phòng VIP", "Bàn riêng tư rộng rãi phù hợp cho gia đình hoặc các dịp kỷ niệm nhỏ.", "Phòng ăn riêng tư", "Bàn Gia Đình Khu Riêng Tư", "Gia Đình" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 15,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Ngoài Trời", "Bàn nhỏ nhắn ngoài ban công dành cho 2 người với không khí nghỉ dưỡng mát mẻ.", "Khu vực ban công", "Bàn Đôi Ngoài Ban Công", "Cặp Đôi" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 16,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Trong Nhà", "Bàn dự phòng dùng cho mục đích điều hành, không nhận đặt bàn online.", "Khu vực ăn uống phía sau", "Bàn Dự Phòng Hệ Thống", "Nội Bộ" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 17,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Hồ Bơi", "Khu vực ghế sofa bán không gian riêng gần khu nhà hàng hồ bơi.", "Sảnh chờ hồ bơi", "Bàn Sofa Cạnh Hồ Bơi", "Sofa Góc" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 18,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Sân Vườn", "Bàn ăn hướng ra sân vườn cho nhóm đông người với bố cục mở thoáng mát.", "Gần đài phun nước sân vườn", "Bàn Nhóm Ngoài Vườn", "Khách Đoàn" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 19,
                columns: new[] { "Area", "Description", "LocationNote", "TableName" },
                values: new object[] { "Phòng VIP", "Bàn cao cấp cỡ lớn thích hợp cho sinh nhật, lễ kỷ niệm và tiệc tại resort.", "Khu vực setup lễ kỷ niệm", "Bàn Tiệc Kỷ Niệm Premium" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 20,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Trong Nhà", "Bàn 2 chỗ linh hoạt để dự phòng khi nhà hàng quá tải.", "Khu vực ghế ngồi linh hoạt", "Bàn Đôi Dự Phòng", "Cặp Đôi" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 1,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Garden", "A relaxed garden-side table for small groups who prefer a quiet dining experience.", "Near tropical garden walkway", "Garden View Table", "Standard" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 2,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Poolside", "Spacious poolside seating suitable for family meals and casual resort dining.", "Close to pool deck", "Poolside Family Table", "Family" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 3,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Indoor", "Comfortable indoor table for larger groups with easy access to restaurant service.", "Main dining hall", "Indoor Group Table", "Group" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 4,
                columns: new[] { "Area", "Description", "LocationNote", "TableName" },
                values: new object[] { "Private Room", "Private dining setup for celebrations, meetings, or premium group dinners.", "Private room section", "Private Dining Table" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 5,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Indoor", "An intimate two-seat table with a bright window-side atmosphere.", "Near window", "Couple Window Table", "Couple" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 6,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Outdoor", "Open-air terrace table with a comfortable resort dining setting.", "Covered terrace", "Terrace Table", "Standard" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 7,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Garden", "Family-friendly garden table with a calm outdoor view.", "Garden corner", "Garden Family Table", "Family" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 8,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Indoor", "Large table used for group dining and banquet preparation.", "Banquet zone", "Banquet Prep Table", "Group" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 9,
                columns: new[] { "Area", "Description", "LocationNote", "TableName" },
                values: new object[] { "Garden", "Premium garden pavilion table for private resort dining experiences.", "Garden pavilion", "VIP Garden Pavilion" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 10,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Indoor", "Cozy booth seating for two guests who prefer privacy.", "Quiet corner", "Quiet Couple Booth", "Booth" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 11,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Indoor", "Comfortable booth table with window-side seating.", "Window line", "Window Booth Table", "Booth" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 12,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Indoor", "Reserved operational table used by staff during peak service periods.", "Near service station", "Service Hold Table", "Staff Hold" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 13,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Outdoor", "Large outdoor table for group meals and casual gatherings.", "Open-air dining zone", "Outdoor Group Table", "Group" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 14,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Private Room", "Large private table suitable for families or small celebrations.", "Private dining room", "Private Family Table", "Family" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 15,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Outdoor", "Small balcony table for two with a relaxed resort atmosphere.", "Balcony side", "Balcony Couple Table", "Couple" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 16,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Indoor", "Operational buffer table kept out of online reservations.", "Back dining area", "Setup Buffer Table", "Staff Hold" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 17,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Poolside", "Semi-private booth near the poolside restaurant area.", "Poolside lounge", "Poolside Booth", "Booth" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 18,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Garden", "Garden-facing table for group dining with a more open layout.", "Near garden fountain", "Garden Group Table", "Group" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 19,
                columns: new[] { "Area", "Description", "LocationNote", "TableName" },
                values: new object[] { "Private Room", "Premium large table for birthdays, anniversaries, and special resort meals.", "Celebration setup area", "Premium Celebration Table" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 20,
                columns: new[] { "Area", "Description", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Indoor", "Backup two-seat table used when the restaurant needs extra flexibility.", "Flexible seating area", "Couple Backup Table", "Couple" });
        }
    }
}
