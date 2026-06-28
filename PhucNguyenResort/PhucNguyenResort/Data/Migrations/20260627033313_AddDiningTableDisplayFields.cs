using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDiningTableDisplayFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "DiningTables",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DiningTables",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "DiningTables",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "DiningTables",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReservable",
                table: "DiningTables",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationNote",
                table: "DiningTables",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TableName",
                table: "DiningTables",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TableType",
                table: "DiningTables",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 1,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "IsReservable", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Garden", "A relaxed garden-side table for small groups who prefer a quiet dining experience.", 1, "/images/tables/table-01.jpg", true, "Near tropical garden walkway", "Garden View Table", "Standard" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 2,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "IsReservable", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Poolside", "Spacious poolside seating suitable for family meals and casual resort dining.", 2, "/images/tables/table-02.jpg", true, "Close to pool deck", "Poolside Family Table", "Family" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 3,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "IsReservable", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Indoor", "Comfortable indoor table for larger groups with easy access to restaurant service.", 3, "/images/tables/table-03.jpg", true, "Main dining hall", "Indoor Group Table", "Group" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 4,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Private Room", "Private dining setup for celebrations, meetings, or premium group dinners.", 4, "/images/tables/table-04.jpg", "Private room section", "Private Dining Table", "VIP" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 5,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "IsReservable", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Indoor", "An intimate two-seat table with a bright window-side atmosphere.", 5, "/images/tables/table-05.jpg", true, "Near window", "Couple Window Table", "Couple" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 6,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "IsReservable", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Outdoor", "Open-air terrace table with a comfortable resort dining setting.", 6, "/images/tables/table-06.jpg", true, "Covered terrace", "Terrace Table", "Standard" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 7,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "IsReservable", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Garden", "Family-friendly garden table with a calm outdoor view.", 7, "/images/tables/table-07.jpg", true, "Garden corner", "Garden Family Table", "Family" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 8,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Indoor", "Large table used for group dining and banquet preparation.", 8, "/images/tables/table-08.jpg", "Banquet zone", "Banquet Prep Table", "Group" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 9,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "IsReservable", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Garden", "Premium garden pavilion table for private resort dining experiences.", 9, "/images/tables/table-09.jpg", true, "Garden pavilion", "VIP Garden Pavilion", "VIP" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 10,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "IsReservable", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Indoor", "Cozy booth seating for two guests who prefer privacy.", 10, "/images/tables/table-10.jpg", true, "Quiet corner", "Quiet Couple Booth", "Booth" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 11,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "IsReservable", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Indoor", "Comfortable booth table with window-side seating.", 11, "/images/tables/table-11.jpg", true, "Window line", "Window Booth Table", "Booth" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 12,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Indoor", "Reserved operational table used by staff during peak service periods.", 12, "/images/tables/table-12.jpg", "Near service station", "Service Hold Table", "Staff Hold" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 13,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "IsReservable", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Outdoor", "Large outdoor table for group meals and casual gatherings.", 13, "/images/tables/table-13.jpg", true, "Open-air dining zone", "Outdoor Group Table", "Group" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 14,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "IsReservable", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Private Room", "Large private table suitable for families or small celebrations.", 14, "/images/tables/table-14.jpg", true, "Private dining room", "Private Family Table", "Family" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 15,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "IsReservable", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Outdoor", "Small balcony table for two with a relaxed resort atmosphere.", 15, "/images/tables/table-15.jpg", true, "Balcony side", "Balcony Couple Table", "Couple" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 16,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Indoor", "Operational buffer table kept out of online reservations.", 16, "/images/tables/table-16.jpg", "Back dining area", "Setup Buffer Table", "Staff Hold" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 17,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "IsReservable", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Poolside", "Semi-private booth near the poolside restaurant area.", 17, "/images/tables/table-17.jpg", true, "Poolside lounge", "Poolside Booth", "Booth" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 18,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "IsReservable", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Garden", "Garden-facing table for group dining with a more open layout.", 18, "/images/tables/table-18.jpg", true, "Near garden fountain", "Garden Group Table", "Group" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 19,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "IsReservable", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Private Room", "Premium large table for birthdays, anniversaries, and special resort meals.", 19, "/images/tables/table-19.jpg", true, "Celebration setup area", "Premium Celebration Table", "VIP" });

            migrationBuilder.UpdateData(
                table: "DiningTables",
                keyColumn: "TableId",
                keyValue: 20,
                columns: new[] { "Area", "Description", "DisplayOrder", "ImageUrl", "LocationNote", "TableName", "TableType" },
                values: new object[] { "Indoor", "Backup two-seat table used when the restaurant needs extra flexibility.", 20, "/images/tables/table-20.jpg", "Flexible seating area", "Couple Backup Table", "Couple" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "DiningTables");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "DiningTables");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "DiningTables");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "DiningTables");

            migrationBuilder.DropColumn(
                name: "IsReservable",
                table: "DiningTables");

            migrationBuilder.DropColumn(
                name: "LocationNote",
                table: "DiningTables");

            migrationBuilder.DropColumn(
                name: "TableName",
                table: "DiningTables");

            migrationBuilder.DropColumn(
                name: "TableType",
                table: "DiningTables");
        }
    }
}
