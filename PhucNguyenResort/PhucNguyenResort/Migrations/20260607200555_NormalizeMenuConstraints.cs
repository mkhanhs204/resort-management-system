using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeMenuConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE MenuCategories
                SET CategoryName = LTRIM(RTRIM(CategoryName))
                WHERE CategoryName IS NOT NULL;
                """);

            migrationBuilder.Sql("""
                UPDATE MenuCategories
                SET CategoryName = CONCAT('Unnamed Category ', CategoryId)
                WHERE CategoryName IS NULL OR CategoryName = '';
                """);

            migrationBuilder.Sql("""
                WITH DuplicateCategories AS
                (
                    SELECT
                        CategoryId,
                        ROW_NUMBER() OVER (
                            PARTITION BY LOWER(CategoryName)
                            ORDER BY CategoryId) AS RowNumber
                    FROM MenuCategories
                )
                UPDATE c
                SET CategoryName = CONCAT(c.CategoryName, ' ', c.CategoryId)
                FROM MenuCategories c
                INNER JOIN DuplicateCategories d ON c.CategoryId = d.CategoryId
                WHERE d.RowNumber > 1;
                """);

            migrationBuilder.Sql("""
                UPDATE MenuItems
                SET ItemName = LTRIM(RTRIM(ItemName))
                WHERE ItemName IS NOT NULL;
                """);

            migrationBuilder.Sql("""
                UPDATE MenuItems
                SET ItemName = CONCAT('Unnamed Item ', MenuItemId)
                WHERE ItemName IS NULL OR ItemName = '';
                """);

            migrationBuilder.Sql("""
                UPDATE MenuItems
                SET Price = 0
                WHERE Price IS NULL;
                """);

            migrationBuilder.Sql("""
                UPDATE MenuItems
                SET IsAvailable = 0
                WHERE CategoryId IS NULL AND IsAvailable = 1;
                """);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "MenuItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ItemName",
                table: "MenuItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "MenuCategories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_MenuItems_AvailableRequiresCategory",
                table: "MenuItems",
                sql: "[IsAvailable] = 0 OR [CategoryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategories_CategoryName",
                table: "MenuCategories",
                column: "CategoryName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_MenuItems_AvailableRequiresCategory",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuCategories_CategoryName",
                table: "MenuCategories");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "MenuItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "ItemName",
                table: "MenuItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "MenuCategories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
