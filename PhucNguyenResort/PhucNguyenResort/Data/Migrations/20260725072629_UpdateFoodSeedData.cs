using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFoodSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE MenuItems NOCHECK CONSTRAINT ALL;");
            migrationBuilder.DeleteData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 20);
            // Removed AddColumn for HasBeenEdited
            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CategoryName",
                value: "Khai Vị");

            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CategoryName",
                value: "Hải Sản");

            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CategoryName",
                value: "Món Việt");

            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CategoryName",
                value: "Món Âu");

            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CategoryName",
                value: "Lẩu & Nướng");

            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 6,
                column: "CategoryName",
                value: "Cơm & Bún & Phở");

            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 7,
                column: "CategoryName",
                value: "Món Chay");

            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 8,
                column: "CategoryName",
                value: "Tráng Miệng");

            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 9,
                column: "CategoryName",
                value: "Đồ Uống");

            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 10,
                column: "CategoryName",
                value: "Món Trẻ Em");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 1,
                columns: new[] { "Description", "ItemName", "Price" },
                values: new object[] { "Cuốn bánh tráng tươi với tôm, thịt heo, bún và rau sống, chấm tương đậu phộng.", "Gỏi Cuốn Tôm Thịt", 75000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 2,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 1, "Chả giò nhân thịt heo, nấm mèo, miến chiên vàng giòn, ăn kèm nước mắm chua ngọt.", "Chả Giò Chiên Giòn", 85000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 3,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 1, "Bánh xèo giòn nhân tôm, thịt, giá đỗ, ăn kèm rau sống và nước chấm.", "Bánh Xèo Miền Tây", 95000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 4,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 1, "Gỏi ngó sen trộn tôm và thịt ba chỉ, rắc đậu phộng rang và hành phi.", "Gỏi Ngó Sen Tôm Thịt", 90000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 5,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 1, "Bánh khọt nhân tôm tươi, ăn kèm rau sống và nước mắm pha.", "Bánh Khọt Vũng Tàu", 80000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 6,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 1, "Súp bào ngư nấu với vi cá, nấm đông cô, thơm ngon bổ dưỡng.", "Súp Bào Ngư Vi Cá", 185000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 7,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 1, "Rau xà lách, cà chua bi, bơ, trứng luộc trộn sốt mè rang.", "Salad Trộn Kiểu Resort", 75000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 8,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 1, "Tôm sú chiên bơ tỏi thơm lừng, dọn kèm bánh mì nướng.", "Tôm Chiên Bơ Tỏi", 130000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 9,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 1, "Mực ống tẩm bột chiên giòn, ăn kèm sốt mayonnaise và tương ớt.", "Mực Chiên Giòn", 120000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 10,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 1, "Nấm đùi gà và nấm kim châm xào bơ tỏi, rắc hành lá.", "Nấm Xào Bơ Tỏi", 85000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 11,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 2, "Tôm hùm tươi nướng phô mai, dọn kèm khoai tây nghiền và salad.", "Tôm Hùm Nướng Phô Mai", 850000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 12,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 2, "Cua biển rang me chua ngọt đậm đà, thơm lừng.", "Cua Rang Me", 450000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 13,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 2, "Ghẹ tươi hấp bia, giữ nguyên vị ngọt tự nhiên, chấm muối tiêu chanh.", "Ghẹ Hấp Bia", 380000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 14,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 2, "Mực ống nướng sa tế cay nồng, ăn kèm rau răm và đồ chua.", "Mực Nướng Sa Tế", 220000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 15,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 2, "Sò điệp tươi nướng mỡ hành và đậu phộng giã nhỏ.", "Sò Điệp Nướng Mỡ Hành", 195000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 16,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 2, "Cá chẽm biển hấp xì dầu kiểu Hồng Kông, thịt cá thơm mềm.", "Cá Chẽm Hấp Hồng Kông", 350000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 17,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 2, "Tôm sú nướng than hoa, phết muối ớt cay nhẹ.", "Tôm Sú Nướng Muối Ớt", 280000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 18,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 2, "Ốc hương rang bơ tỏi thơm phức, vị béo ngậy.", "Ốc Hương Rang Bơ", 250000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 19,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 2, "Bạch tuộc baby nướng xiên que, phết sốt teriyaki.", "Bạch Tuộc Nướng Xiên", 180000m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 20,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 2, "Hàu sữa Phú Quốc nướng phô mai, béo ngậy thơm lừng.", "Hàu Nướng Phô Mai", 220000m });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "MenuItemId", "CategoryId", "Description", "ImageUrl", "IsAvailable", "ItemName", "Price" },
                values: new object[,]
                {
                    { 21, 3, "Cá lóc kho tộ đất với nước màu dừa, tiêu và hành lá.", "/images/menu/item-21.jpg", true, "Cá Kho Tộ", 150000m },
                    { 22, 3, "Thịt ba chỉ kho nước dừa với trứng vịt, vị mặn ngọt đậm đà.", "/images/menu/item-22.jpg", true, "Thịt Kho Tàu", 130000m },
                    { 23, 3, "Canh chua miền Tây nấu với cá lóc, me, thơm, đậu bắp.", "/images/menu/item-23.jpg", true, "Canh Chua Cá Lóc", 120000m },
                    { 24, 3, "Đùi gà ta kho gừng tươi, nước mắm thắng đường thơm lừng.", "/images/menu/item-24.jpg", true, "Gà Kho Gừng", 140000m },
                    { 25, 3, "Rau muống xào tỏi phi thơm, giòn ngọt tự nhiên.", "/images/menu/item-25.jpg", true, "Rau Muống Xào Tỏi", 65000m },
                    { 26, 3, "Đậu hũ chiên vàng sốt cà chua với thịt bằm và hành lá.", "/images/menu/item-26.jpg", true, "Đậu Hũ Sốt Cà Chua", 75000m },
                    { 27, 3, "Sườn heo non rim nước mắm, tỏi ớt, caramel nhẹ.", "/images/menu/item-27.jpg", true, "Sườn Non Rim Mặn", 160000m },
                    { 28, 3, "Thịt bò Úc xào lúc lắc với ớt chuông, hành tây, ăn kèm cơm trắng.", "/images/menu/item-28.jpg", true, "Bò Lúc Lắc", 195000m },
                    { 29, 3, "Tôm sú rim nước cốt dừa béo ngậy, hành lá và tiêu xanh.", "/images/menu/item-29.jpg", true, "Tôm Rim Nước Cốt Dừa", 175000m },
                    { 30, 3, "Gà ta nướng lá chanh thơm nức, ăn kèm muối tiêu chanh.", "/images/menu/item-30.jpg", true, "Gà Nướng Lá Chanh", 185000m },
                    { 31, 4, "Thịt bò Úc áp chảo tái vừa, rưới sốt tiêu đen, kèm khoai tây chiên.", "/images/menu/item-31.jpg", true, "Bò Bít Tết Sốt Tiêu Đen", 285000m },
                    { 32, 4, "Cá hồi Na Uy áp chảo giòn da, dọn kèm rau củ hấp và sốt chanh dây.", "/images/menu/item-32.jpg", true, "Cá Hồi Áp Chảo", 320000m },
                    { 33, 4, "Spaghetti sốt Bolognese với thịt bò bằm, cà chua và phô mai Parmesan.", "/images/menu/item-33.jpg", true, "Mì Ý Sốt Bò Bằm", 165000m },
                    { 34, 4, "Fettuccine sốt kem trắng với nấm rừng và phô mai.", "/images/menu/item-34.jpg", true, "Mì Ý Sốt Kem Nấm", 175000m },
                    { 35, 4, "Pizza đế mỏng với tôm, mực, phô mai Mozzarella và sốt cà chua.", "/images/menu/item-35.jpg", true, "Pizza Hải Sản", 220000m },
                    { 36, 4, "Sườn cừu New Zealand nướng hương thảo, kèm khoai tây nghiền.", "/images/menu/item-36.jpg", true, "Sườn Cừu Nướng Rosemary", 395000m },
                    { 37, 4, "Súp kem nấm với dầu truffle đen, bánh mì nướng giòn.", "/images/menu/item-37.jpg", true, "Súp Kem Nấm Truffle", 135000m },
                    { 38, 4, "Hamburger bò Úc nướng than, phô mai Cheddar, rau xà lách, cà chua.", "/images/menu/item-38.jpg", true, "Hamburger Bò Phô Mai", 185000m },
                    { 39, 4, "Đùi gà nướng sốt BBQ mật ong, kèm bắp nướng và coleslaw.", "/images/menu/item-39.jpg", true, "Gà Nướng Sốt BBQ", 195000m },
                    { 40, 4, "Salad Caesar truyền thống với gà nướng thái lát, bánh mì giòn, phô mai.", "/images/menu/item-40.jpg", true, "Salad Caesar Gà Nướng", 145000m },
                    { 41, 5, "Lẩu Tom Yum chua cay với tôm, mực, cá, nấm và rau tươi (2-3 người).", "/images/menu/item-41.jpg", true, "Lẩu Thái Tom Yum Hải Sản", 450000m },
                    { 42, 5, "Lẩu dấm với thịt bò tái, rau sống, bún và mắm nêm (2-3 người).", "/images/menu/item-42.jpg", true, "Lẩu Bò Nhúng Dấm", 380000m },
                    { 43, 5, "Lẩu gà ta nấu lá é thơm đặc trưng Phú Quốc (2-3 người).", "/images/menu/item-43.jpg", true, "Lẩu Gà Lá É", 350000m },
                    { 44, 5, "Lẩu cá kèo nấu lá giang chua thanh, mùi thơm đặc trưng (2-3 người).", "/images/menu/item-44.jpg", true, "Lẩu Cá Kèo Lá Giang", 320000m },
                    { 45, 5, "Thịt bò bằm cuốn lá lốt nướng than hoa thơm lừng.", "/images/menu/item-45.jpg", true, "Bò Nướng Lá Lốt", 145000m },
                    { 46, 5, "Sườn heo ướp mật ong nướng than, thơm ngọt đậm vị.", "/images/menu/item-46.jpg", true, "Sườn Heo Nướng Mật Ong", 195000m },
                    { 47, 5, "Gà ta nướng ngũ vị hương, da giòn thịt mềm.", "/images/menu/item-47.jpg", true, "Gà Nướng Ngũ Vị", 220000m },
                    { 48, 5, "Tôm, mực, sò điệp, cá nướng than hoa phục vụ 2-4 người.", "/images/menu/item-48.jpg", true, "Combo Nướng Hải Sản", 550000m },
                    { 49, 5, "Bê thui đặc sản, ăn kèm rau sống, bánh tráng và mắm nêm.", "/images/menu/item-49.jpg", true, "Bê Thui Cầu Mống", 280000m },
                    { 50, 5, "Lẩu nấm thập cẩm với đậu hũ, rau củ tươi, nước dùng chay (2-3 người).", "/images/menu/item-50.jpg", true, "Lẩu Nấm Chay", 280000m },
                    { 51, 6, "Phở bò truyền thống với nước dùng hầm xương 12 tiếng, thịt bò tái và nạm.", "/images/menu/item-51.jpg", true, "Phở Bò Tái Nạm", 95000m },
                    { 52, 6, "Bún bò Huế cay nồng với giò heo, huyết và chả cua.", "/images/menu/item-52.jpg", true, "Bún Bò Huế", 105000m },
                    { 53, 6, "Bún chả nướng than hoa ăn kèm nước mắm pha, rau sống và nem cua bể.", "/images/menu/item-53.jpg", true, "Bún Chả Hà Nội", 110000m },
                    { 54, 6, "Cơm tấm với sườn nướng, bì, chả trứng và nước mắm pha.", "/images/menu/item-54.jpg", true, "Cơm Tấm Sườn Bì Chả", 95000m },
                    { 55, 6, "Cơm chiên với tôm, lạp xưởng, trứng, đậu Hà Lan và cà rốt.", "/images/menu/item-55.jpg", true, "Cơm Chiên Dương Châu", 105000m },
                    { 56, 6, "Cơm nghệ vàng ươm ăn kèm gà xé phay, rau sống và canh.", "/images/menu/item-56.jpg", true, "Cơm Gà Hội An", 110000m },
                    { 57, 6, "Hủ tiếu nước trong với tôm, thịt bằm, gan và trứng cút.", "/images/menu/item-57.jpg", true, "Hủ Tiếu Nam Vang", 90000m },
                    { 58, 6, "Mì Quảng với tôm, thịt heo, trứng cút và bánh tráng nướng.", "/images/menu/item-58.jpg", true, "Mì Quảng Đà Nẵng", 100000m },
                    { 59, 6, "Bún riêu cua đồng với chả cua, đậu hũ chiên, cà chua.", "/images/menu/item-59.jpg", true, "Bún Riêu Cua", 95000m },
                    { 60, 6, "Cơm niêu đất nóng hổi ăn kèm cá kho tộ và canh rau.", "/images/menu/item-60.jpg", true, "Cơm Niêu Cá Kho Tộ", 135000m },
                    { 61, 7, "Cơm chiên với đậu hũ, nấm, bắp, cà rốt và đậu que.", "/images/menu/item-61.jpg", true, "Cơm Chiên Chay Thập Cẩm", 85000m },
                    { 62, 7, "Phở chay nước dùng rau củ với nấm đông cô, đậu hũ và rau thơm.", "/images/menu/item-62.jpg", true, "Phở Chay Nấm", 80000m },
                    { 63, 7, "Bún chay kiểu Huế với nước dùng sả, nấm và chả chay.", "/images/menu/item-63.jpg", true, "Bún Chay Huế", 85000m },
                    { 64, 7, "Đậu hũ chiên giòn xào sả ớt, thơm cay hấp dẫn.", "/images/menu/item-64.jpg", true, "Đậu Hũ Chiên Sả Ớt", 70000m },
                    { 65, 7, "Nấm đùi gà, nấm bào ngư xào với bông cải, cà rốt.", "/images/menu/item-65.jpg", true, "Nấm Xào Rau Củ", 75000m },
                    { 66, 7, "Canh bí đỏ nấu nước dùng chay với đậu hũ non và rau ngót.", "/images/menu/item-66.jpg", true, "Canh Bí Đỏ Nấu Chay", 65000m },
                    { 67, 7, "Cuốn bánh tráng với đậu hũ chiên, bún, rau sống và nấm.", "/images/menu/item-67.jpg", true, "Gỏi Cuốn Chay", 65000m },
                    { 68, 7, "Mì trứng xào với rau cải, nấm và đậu hũ chiên.", "/images/menu/item-68.jpg", true, "Mì Xào Chay", 80000m },
                    { 69, 7, "Cà ri nước cốt dừa với khoai tây, cà rốt, đậu que và đậu hũ.", "/images/menu/item-69.jpg", true, "Cà Ri Chay Rau Củ", 90000m },
                    { 70, 7, "Cơm cuộn kiểu Nhật với bơ, dưa leo, cà rốt và đậu hũ.", "/images/menu/item-70.jpg", true, "Cơm Cuộn Rong Biển Chay", 95000m },
                    { 71, 8, "Chè khúc bạch mát lạnh với nhãn, vải và nước đường hoa nhài.", "/images/menu/item-71.jpg", true, "Chè Khúc Bạch", 55000m },
                    { 72, 8, "Bánh flan mềm mịn với caramel đắng nhẹ và kem tươi.", "/images/menu/item-72.jpg", true, "Bánh Flan Caramel", 50000m },
                    { 73, 8, "Kem dừa tươi Phú Quốc phục vụ trong trái dừa non.", "/images/menu/item-73.jpg", true, "Kem Dừa Tươi", 75000m },
                    { 74, 8, "Bánh chocolate nóng nhân chảy, ăn kèm kem vanilla.", "/images/menu/item-74.jpg", true, "Bánh Chocolate Lava", 95000m },
                    { 75, 8, "Đĩa trái cây tươi theo mùa: xoài, thanh long, dưa hấu, đu đủ.", "/images/menu/item-75.jpg", true, "Trái Cây Tươi Theo Mùa", 85000m },
                    { 76, 8, "Chè Thái với sầu riêng, mít, dừa non và nước cốt dừa.", "/images/menu/item-76.jpg", true, "Chè Thái Sầu Riêng", 65000m },
                    { 77, 8, "Tiramisu biến tấu với cà phê phin Việt Nam đậm đà.", "/images/menu/item-77.jpg", true, "Tiramisu Cà Phê Việt", 90000m },
                    { 78, 8, "Sữa chua dầm với xoài, dâu, việt quất và granola.", "/images/menu/item-78.jpg", true, "Sữa Chua Dầm Trái Cây", 60000m },
                    { 79, 8, "Bánh chuối nướng nước cốt dừa, mặt vàng ruộm, thơm béo.", "/images/menu/item-79.jpg", true, "Bánh Chuối Nướng", 55000m },
                    { 80, 8, "Panna Cotta kem sữa với coulis xoài tươi Phú Quốc.", "/images/menu/item-80.jpg", true, "Panna Cotta Xoài", 75000m },
                    { 81, 9, "Cà phê phin truyền thống Việt Nam pha với sữa đặc, đá viên.", "/images/menu/item-81.jpg", true, "Cà Phê Sữa Đá", 45000m },
                    { 82, 9, "Cà phê đen đá đậm đà, nguyên chất không đường.", "/images/menu/item-82.jpg", true, "Cà Phê Đen Đá", 40000m },
                    { 83, 9, "Trà sen đặc biệt ướp hương sen tự nhiên, thưởng thức nóng hoặc đá.", "/images/menu/item-83.jpg", true, "Trà Sen Phúc Nguyên", 55000m },
                    { 84, 9, "Sinh tố xoài Phú Quốc tươi nguyên, ngọt thanh mát lạnh.", "/images/menu/item-84.jpg", true, "Sinh Tố Xoài", 65000m },
                    { 85, 9, "Nước dừa tươi xiêm lạnh mát, phục vụ nguyên trái.", "/images/menu/item-85.jpg", true, "Nước Dừa Tươi", 50000m },
                    { 86, 9, "Nước ép cam tươi 100%, không đường, giàu vitamin C.", "/images/menu/item-86.jpg", true, "Nước Ép Cam Tươi", 55000m },
                    { 87, 9, "Trà đào cam sả mát lạnh, thơm hương trái cây tự nhiên.", "/images/menu/item-87.jpg", true, "Trà Đào Cam Sả", 60000m },
                    { 88, 9, "Mocktail Mojito biến tấu với chanh, bạc hà, soda và đường mía.", "/images/menu/item-88.jpg", true, "Mojito Việt Nam", 75000m },
                    { 89, 9, "Bia tươi craft brewing tại resort, vị nhẹ thanh mát.", "/images/menu/item-89.jpg", true, "Bia Tươi Resort", 65000m },
                    { 90, 9, "Cocktail đặc biệt với rượu rum, nước cam, grenadine và đá xay.", "/images/menu/item-90.jpg", true, "Cocktail Hoàng Hôn", 120000m },
                    { 91, 10, "Spaghetti sốt cà chua thịt bò bằm, phần nhỏ phù hợp trẻ em.", "/images/menu/item-91.jpg", true, "Mì Ý Bò Bằm Cho Bé", 75000m },
                    { 92, 10, "Cơm chiên trứng đơn giản, bổ dưỡng, phù hợp khẩu vị trẻ nhỏ.", "/images/menu/item-92.jpg", true, "Cơm Chiên Trứng", 65000m },
                    { 93, 10, "Gà viên tẩm bột chiên giòn, ăn kèm sốt cà chua.", "/images/menu/item-93.jpg", true, "Gà Viên Chiên Giòn", 70000m },
                    { 94, 10, "Khoai tây chiên giòn rắc phô mai Parmesan thơm lừng.", "/images/menu/item-94.jpg", true, "Khoai Tây Chiên Phô Mai", 60000m },
                    { 95, 10, "Bánh mì sandwich nhân gà xé, rau xà lách và sốt mayonnaise.", "/images/menu/item-95.jpg", true, "Sandwich Gà Cho Bé", 65000m },
                    { 96, 10, "Súp bí đỏ nấu kem sữa mịn màng, bổ dưỡng cho trẻ.", "/images/menu/item-96.jpg", true, "Súp Bí Đỏ Kem Sữa", 55000m },
                    { 97, 10, "Bánh pancake mềm xốp phết bơ và mật ong, kèm trái cây tươi.", "/images/menu/item-97.jpg", true, "Pancake Mật Ong", 70000m },
                    { 98, 10, "Cháo gà hạt sen nấu nhừ, bổ dưỡng và dễ tiêu hóa.", "/images/menu/item-98.jpg", true, "Cháo Gà Hạt Sen", 60000m },
                    { 99, 10, "Sinh tố chuối sữa mát lạnh, bổ sung năng lượng cho bé.", "/images/menu/item-99.jpg", true, "Sinh Tố Chuối Sữa", 45000m },
                    { 100, 10, "Kem ba vị vanilla, chocolate, dâu phục vụ trong ly nhỏ.", "/images/menu/item-100.jpg", true, "Kem Ba Vị Cho Bé", 55000m }
                });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 1,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 2,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 3,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 4,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 5,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 6,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 7,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 8,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 9,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 10,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 11,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 12,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 13,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 14,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 15,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 16,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 17,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 18,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 19,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 20,
                column: "HasBeenEdited",
                value: false);

            migrationBuilder.Sql("ALTER TABLE MenuItems CHECK CONSTRAINT ALL;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE MenuItems NOCHECK CONSTRAINT ALL;");
            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 100);
            // Removed DropColumn for HasBeenEdited
            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CategoryName",
                value: "Breakfast");

            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CategoryName",
                value: "Appetizer");

            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CategoryName",
                value: "Soup");

            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CategoryName",
                value: "Salad");

            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CategoryName",
                value: "Seafood");

            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 6,
                column: "CategoryName",
                value: "Vietnamese Food");

            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 7,
                column: "CategoryName",
                value: "Western Food");

            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 8,
                column: "CategoryName",
                value: "Vegetarian");

            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 9,
                column: "CategoryName",
                value: "Dessert");

            migrationBuilder.UpdateData(
                table: "MenuCategories",
                keyColumn: "CategoryId",
                keyValue: 10,
                column: "CategoryName",
                value: "Coffee");

            migrationBuilder.InsertData(
                table: "MenuCategories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 11, "Tea" },
                    { 12, "Juice" },
                    { 13, "Cocktail" },
                    { 14, "Mocktail" },
                    { 15, "Kids Meal" },
                    { 16, "Room Service Set" },
                    { 17, "BBQ" },
                    { 18, "Noodle" },
                    { 19, "Rice Dish" },
                    { 20, "Special Combo" }
                });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 1,
                columns: new[] { "Description", "ItemName", "Price" },
                values: new object[] { "Pho Bo prepared by resort kitchen.", "Pho Bo", 85000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 2,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 2, "Bun Cha prepared by resort kitchen.", "Bun Cha", 110000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 3,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 3, "Spring Rolls prepared by resort kitchen.", "Spring Rolls", 135000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 4,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 4, "Seafood Soup prepared by resort kitchen.", "Seafood Soup", 160000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 5,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 5, "Caesar Salad prepared by resort kitchen.", "Caesar Salad", 185000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 6,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 6, "Grilled Prawns prepared by resort kitchen.", "Grilled Prawns", 210000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 7,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 7, "Beef Steak prepared by resort kitchen.", "Beef Steak", 235000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 8,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 8, "Vegetable Curry prepared by resort kitchen.", "Vegetable Curry", 260000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 9,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 9, "Chocolate Cake prepared by resort kitchen.", "Chocolate Cake", 285000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 10,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 10, "Vietnamese Coffee prepared by resort kitchen.", "Vietnamese Coffee", 310000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 11,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 11, "Lotus Tea prepared by resort kitchen.", "Lotus Tea", 335000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 12,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 12, "Mango Juice prepared by resort kitchen.", "Mango Juice", 360000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 13,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 13, "Sunset Cocktail prepared by resort kitchen.", "Sunset Cocktail", 385000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 14,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 14, "Blue Lagoon Mocktail prepared by resort kitchen.", "Blue Lagoon Mocktail", 410000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 15,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 15, "Kids Pasta prepared by resort kitchen.", "Kids Pasta", 435000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 16,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 16, "Club Sandwich prepared by resort kitchen.", "Club Sandwich", 460000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 17,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 17, "BBQ Platter prepared by resort kitchen.", "BBQ Platter", 485000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 18,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 18, "Seafood Noodle prepared by resort kitchen.", "Seafood Noodle", 510000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 19,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 19, "Chicken Fried Rice prepared by resort kitchen.", "Chicken Fried Rice", 535000.00m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 20,
                columns: new[] { "CategoryId", "Description", "ItemName", "Price" },
                values: new object[] { 20, "Resort Combo prepared by resort kitchen.", "Resort Combo", 560000.00m });

            migrationBuilder.Sql("ALTER TABLE MenuItems CHECK CONSTRAINT ALL;");
        }
    }
}
