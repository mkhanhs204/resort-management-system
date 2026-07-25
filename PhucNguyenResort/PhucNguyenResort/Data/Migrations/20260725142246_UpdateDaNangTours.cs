using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDaNangTours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE TourBookings NOCHECK CONSTRAINT ALL;");
            migrationBuilder.Sql("ALTER TABLE TourSchedules NOCHECK CONSTRAINT ALL;");
            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 30);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 11,
                column: "TourId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 12,
                column: "TourId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 13,
                column: "TourId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 14,
                column: "TourId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 15,
                column: "TourId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 16,
                column: "TourId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 17,
                column: "TourId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 18,
                column: "TourId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 19,
                column: "TourId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 20,
                column: "TourId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 21,
                column: "TourId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 22,
                column: "TourId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 23,
                column: "TourId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 24,
                column: "TourId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 25,
                column: "TourId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 26,
                column: "TourId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 27,
                column: "TourId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 28,
                column: "TourId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 29,
                column: "TourId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 30,
                column: "TourId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 1,
                columns: new[] { "Description", "DurationHours", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Khám phá chốn bồng lai tiên cảnh Bà Nà Hills với Cầu Vàng nổi tiếng.", 8, "Bà Nà", 25, 1250000.00m, "Tour Bà Nà Hills" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 2,
                columns: new[] { "Description", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Tham quan Chùa Linh Ứng và ngắm cảnh từ bán đảo Sơn Trà.", "Sơn Trà", 15, 450000.00m, "Bán Đảo Sơn Trà - Chùa Linh Ứng" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 3,
                columns: new[] { "Description", "DurationHours", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Trải nghiệm đi thúng tại Rừng Dừa và dạo đêm phố cổ Hội An.", 7, "Hội An", 20, 750000.00m, "Phố Cổ Hội An - Rừng Dừa Bảy Mẫu" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 4,
                columns: new[] { "Description", "DurationHours", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Ngắm thành phố Đà Nẵng lung linh về đêm từ du thuyền trên sông Hàn.", 2, "Sông Hàn", 50, 250000.00m, "Du Thuyền Sông Hàn" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 5,
                columns: new[] { "Description", "DurationHours", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Vui chơi không giới hạn tại công viên giải trí Asia Park.", 5, "Hải Châu", 100, 350000.00m, "Công Viên Châu Á - Asia Park" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 6,
                columns: new[] { "Description", "DurationHours", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Thư giãn tắm bùn và tắm khoáng nóng tại Núi Thần Tài.", 6, "Hòa Phú", 30, 850000.00m, "Suối Khoáng Nóng Núi Thần Tài" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 7,
                columns: new[] { "Description", "DurationHours", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Đi cano cao tốc ra đảo, lặn ngắm san hô tại Cù Lao Chàm.", 7, "Cù Lao Chàm", 20, 650000.00m, "Khám Phá Cù Lao Chàm" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 8,
                columns: new[] { "Description", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Tham quan danh thắng Ngũ Hành Sơn và làng đá mỹ nghệ Non Nước.", "Ngũ Hành Sơn", 25, 300000.00m, "Làng Đá Mỹ Nghệ Non Nước" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 9,
                columns: new[] { "Description", "DurationHours", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Chinh phục đệ nhất hùng quan Đèo Hải Vân và tắm biển Vịnh Lăng Cô.", 8, "Hải Vân", 15, 950000.00m, "Đèo Hải Vân - Vịnh Lăng Cô" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 10,
                columns: new[] { "Description", "DurationHours", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Trải nghiệm trượt thác phao mạo hiểm vô cùng kỳ thú.", 6, "Hòa Phú Thành", 30, 550000.00m, "Trượt Thác Hòa Phú Thành" });
            migrationBuilder.Sql("ALTER TABLE TourSchedules WITH CHECK CHECK CONSTRAINT ALL;");
            migrationBuilder.Sql("ALTER TABLE TourBookings WITH CHECK CHECK CONSTRAINT ALL;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE TourBookings NOCHECK CONSTRAINT ALL;");
            migrationBuilder.Sql("ALTER TABLE TourSchedules NOCHECK CONSTRAINT ALL;");
            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 11,
                column: "TourId",
                value: 11);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 12,
                column: "TourId",
                value: 12);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 13,
                column: "TourId",
                value: 13);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 14,
                column: "TourId",
                value: 14);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 15,
                column: "TourId",
                value: 15);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 16,
                column: "TourId",
                value: 16);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 17,
                column: "TourId",
                value: 17);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 18,
                column: "TourId",
                value: 18);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 19,
                column: "TourId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 20,
                column: "TourId",
                value: 20);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 21,
                column: "TourId",
                value: 21);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 22,
                column: "TourId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 23,
                column: "TourId",
                value: 23);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 24,
                column: "TourId",
                value: 24);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 25,
                column: "TourId",
                value: 25);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 26,
                column: "TourId",
                value: 26);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 27,
                column: "TourId",
                value: 27);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 28,
                column: "TourId",
                value: 28);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 29,
                column: "TourId",
                value: 29);

            migrationBuilder.UpdateData(
                table: "TourSchedules",
                keyColumn: "ScheduleId",
                keyValue: 30,
                column: "TourId",
                value: 30);

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 1,
                columns: new[] { "Description", "DurationHours", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Island Hopping experience for resort guests.", 3, "Phu Quoc Area 1", 10, 425000.00m, "Island Hopping" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 2,
                columns: new[] { "Description", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Snorkeling Coral Reef experience for resort guests.", "Phu Quoc Area 2", 12, 500000.00m, "Snorkeling Coral Reef" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 3,
                columns: new[] { "Description", "DurationHours", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Sunset Fishing experience for resort guests.", 5, "Phu Quoc Area 3", 14, 575000.00m, "Sunset Fishing" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 4,
                columns: new[] { "Description", "DurationHours", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Pearl Farm Visit experience for resort guests.", 6, "Phu Quoc Area 4", 16, 650000.00m, "Pearl Farm Visit" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 5,
                columns: new[] { "Description", "DurationHours", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Forest Trekking experience for resort guests.", 7, "Phu Quoc Area 5", 18, 725000.00m, "Forest Trekking" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 6,
                columns: new[] { "Description", "DurationHours", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Night Market Food Tour experience for resort guests.", 8, "Phu Quoc Area 6", 20, 800000.00m, "Night Market Food Tour" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 7,
                columns: new[] { "Description", "DurationHours", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Kayak Lagoon experience for resort guests.", 2, "Phu Quoc Area 7", 22, 875000.00m, "Kayak Lagoon" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 8,
                columns: new[] { "Description", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Luxury Yacht Trip experience for resort guests.", "Phu Quoc Area 8", 8, 950000.00m, "Luxury Yacht Trip" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 9,
                columns: new[] { "Description", "DurationHours", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Historical Prison Tour experience for resort guests.", 4, "Phu Quoc Area 9", 10, 1025000.00m, "Historical Prison Tour" });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 10,
                columns: new[] { "Description", "DurationHours", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[] { "Pepper Farm Tour experience for resort guests.", 5, "Phu Quoc Area 10", 12, 1100000.00m, "Pepper Farm Tour" });

            migrationBuilder.InsertData(
                table: "Tours",
                columns: new[] { "TourId", "Description", "DurationHours", "ImageUrl", "Location", "MaxCapacity", "Price", "TourName" },
                values: new object[,]
                {
                    { 11, "Cable Car Adventure experience for resort guests.", 6, "/images/tours/tour-11.jpg", "Phu Quoc Area 11", 14, 1175000.00m, "Cable Car Adventure" },
                    { 12, "Diving Beginner experience for resort guests.", 7, "/images/tours/tour-12.jpg", "Phu Quoc Area 12", 16, 1250000.00m, "Diving Beginner" },
                    { 13, "Diving Advanced experience for resort guests.", 8, "/images/tours/tour-13.jpg", "Phu Quoc Area 13", 18, 1325000.00m, "Diving Advanced" },
                    { 14, "Private Beach Picnic experience for resort guests.", 2, "/images/tours/tour-14.jpg", "Phu Quoc Area 14", 20, 1400000.00m, "Private Beach Picnic" },
                    { 15, "Photography Tour experience for resort guests.", 3, "/images/tours/tour-15.jpg", "Phu Quoc Area 15", 22, 1475000.00m, "Photography Tour" },
                    { 16, "Wellness Retreat experience for resort guests.", 4, "/images/tours/tour-16.jpg", "Phu Quoc Area 16", 8, 1550000.00m, "Wellness Retreat" },
                    { 17, "Cooking Class Tour experience for resort guests.", 5, "/images/tours/tour-17.jpg", "Phu Quoc Area 17", 10, 1625000.00m, "Cooking Class Tour" },
                    { 18, "Cycling Countryside experience for resort guests.", 6, "/images/tours/tour-18.jpg", "Phu Quoc Area 18", 12, 1700000.00m, "Cycling Countryside" },
                    { 19, "Bird Watching experience for resort guests.", 7, "/images/tours/tour-19.jpg", "Phu Quoc Area 19", 14, 1775000.00m, "Bird Watching" },
                    { 20, "Family Discovery experience for resort guests.", 8, "/images/tours/tour-20.jpg", "Phu Quoc Area 20", 16, 1850000.00m, "Family Discovery" },
                    { 21, "Scuba Diving Master experience for resort guests.", 9, "/images/tours/tour-21.jpg", "Phu Quoc Area 21", 18, 1925000.00m, "Scuba Diving Master" },
                    { 22, "Traditional Village Tour experience for resort guests.", 4, "/images/tours/tour-22.jpg", "Phu Quoc Area 22", 20, 2000000.00m, "Traditional Village Tour" },
                    { 23, "Sunrise Yoga on Beach experience for resort guests.", 2, "/images/tours/tour-23.jpg", "Phu Quoc Area 23", 22, 2075000.00m, "Sunrise Yoga on Beach" },
                    { 24, "Seafood BBQ Night experience for resort guests.", 3, "/images/tours/tour-24.jpg", "Phu Quoc Area 24", 8, 2150000.00m, "Seafood BBQ Night" },
                    { 25, "Jungle Safari experience for resort guests.", 4, "/images/tours/tour-25.jpg", "Phu Quoc Area 25", 10, 2225000.00m, "Jungle Safari" },
                    { 26, "Star Gazing Tour experience for resort guests.", 5, "/images/tours/tour-26.jpg", "Phu Quoc Area 26", 12, 2300000.00m, "Star Gazing Tour" },
                    { 27, "Local Craft Workshop experience for resort guests.", 6, "/images/tours/tour-27.jpg", "Phu Quoc Area 27", 14, 2375000.00m, "Local Craft Workshop" },
                    { 28, "Wine Tasting Experience for resort guests.", 7, "/images/tours/tour-28.jpg", "Phu Quoc Area 28", 16, 2450000.00m, "Wine Tasting Experience" },
                    { 29, "Waterfall Hiking experience for resort guests.", 8, "/images/tours/tour-29.jpg", "Phu Quoc Area 29", 18, 2525000.00m, "Waterfall Hiking" },
                    { 30, "Island Cycling Tour experience for resort guests.", 2, "/images/tours/tour-30.jpg", "Phu Quoc Area 30", 20, 2600000.00m, "Island Cycling Tour" }
                });

            migrationBuilder.Sql("ALTER TABLE TourSchedules WITH CHECK CHECK CONSTRAINT ALL;");
            migrationBuilder.Sql("ALTER TABLE TourBookings WITH CHECK CHECK CONSTRAINT ALL;");
        }
    }
}
