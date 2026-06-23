using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTourManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "Tours",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tours_ManagerId",
                table: "Tours",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_Users_ManagerId",
                table: "Tours",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tours_Users_ManagerId",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_Tours_ManagerId",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Tours");
        }
    }
}
