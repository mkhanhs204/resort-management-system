using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTourImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // TourBookingId already exists in database
            // MaintenanceRequests already exists in database

            migrationBuilder.CreateTable(
                name: "TourImages",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TourId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourImages", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_TourImages_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "TourId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 1,
                column: "TourBookingId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 2,
                column: "TourBookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 3,
                column: "TourBookingId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 4,
                column: "TourBookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 5,
                column: "TourBookingId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 6,
                column: "TourBookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 7,
                column: "TourBookingId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 8,
                column: "TourBookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 9,
                column: "TourBookingId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 10,
                column: "TourBookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 11,
                column: "TourBookingId",
                value: 11);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 12,
                column: "TourBookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 13,
                column: "TourBookingId",
                value: 13);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 14,
                column: "TourBookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 15,
                column: "TourBookingId",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 16,
                column: "TourBookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 17,
                column: "TourBookingId",
                value: 17);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 18,
                column: "TourBookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 19,
                column: "TourBookingId",
                value: 19);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 20,
                column: "TourBookingId",
                value: null);

            // Index for TourBookingId already exists
            // Index for MaintenanceRequests already exists

            migrationBuilder.CreateIndex(
                name: "IX_TourImages_TourId",
                table: "TourImages",
                column: "TourId");

            // Foreign key for TourBookingId already exists
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_TourBookings_TourBookingId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "MaintenanceRequests");

            migrationBuilder.DropTable(
                name: "TourImages");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_TourBookingId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "TourBookingId",
                table: "Reviews");
        }
    }
}
