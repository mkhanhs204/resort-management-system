using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTableReservationBusinessFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE TableReservations
                SET ReservationDate = '2026-06-01'
                WHERE ReservationDate IS NULL;

                UPDATE TableReservations
                SET ReservationTime = '18:00:00'
                WHERE ReservationTime IS NULL;

                UPDATE TableReservations
                SET PartySize = 1
                WHERE PartySize IS NULL OR PartySize < 1;
                """);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "ReservationTime",
                table: "TableReservations",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 18, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReservationDate",
                table: "TableReservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PartySize",
                table: "TableReservations",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancellationReason",
                table: "TableReservations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledAt",
                table: "TableReservations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedAt",
                table: "TableReservations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConfirmedByUserId",
                table: "TableReservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TableReservations",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "TableReservations",
                type: "int",
                nullable: false,
                defaultValue: 120);

            migrationBuilder.AddColumn<string>(
                name: "ReservationCode",
                table: "TableReservations",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialRequests",
                table: "TableReservations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE TableReservations
                SET
                    ReservationCode = CASE ReservationId
                        WHEN 1 THEN 'TR-20260606-0001'
                        WHEN 2 THEN 'TR-20260607-0002'
                        WHEN 3 THEN 'TR-20260608-0003'
                        WHEN 4 THEN 'TR-20260609-0004'
                        WHEN 5 THEN 'TR-20260610-0005'
                        WHEN 6 THEN 'TR-20260611-0006'
                        WHEN 7 THEN 'TR-20260612-0007'
                        WHEN 8 THEN 'TR-20260613-0008'
                        WHEN 9 THEN 'TR-20260614-0009'
                        WHEN 10 THEN 'TR-20260615-0010'
                        WHEN 11 THEN 'TR-20260616-0011'
                        WHEN 12 THEN 'TR-20260617-0012'
                        WHEN 13 THEN 'TR-20260618-0013'
                        WHEN 14 THEN 'TR-20260619-0014'
                        WHEN 15 THEN 'TR-20260620-0015'
                        WHEN 16 THEN 'TR-20260621-0016'
                        WHEN 17 THEN 'TR-20260622-0017'
                        WHEN 18 THEN 'TR-20260623-0018'
                        WHEN 19 THEN 'TR-20260624-0019'
                        WHEN 20 THEN 'TR-20260625-0020'
                        ELSE ReservationCode
                    END,
                    CreatedAt = CASE ReservationId
                        WHEN 1 THEN '2026-06-01T09:00:00'
                        WHEN 2 THEN '2026-06-02T09:00:00'
                        WHEN 3 THEN '2026-06-03T09:00:00'
                        WHEN 4 THEN '2026-06-04T09:00:00'
                        WHEN 5 THEN '2026-06-05T09:00:00'
                        WHEN 6 THEN '2026-06-06T09:00:00'
                        WHEN 7 THEN '2026-06-07T09:00:00'
                        WHEN 8 THEN '2026-06-08T09:00:00'
                        WHEN 9 THEN '2026-06-09T09:00:00'
                        WHEN 10 THEN '2026-06-10T09:00:00'
                        WHEN 11 THEN '2026-06-11T09:00:00'
                        WHEN 12 THEN '2026-06-12T09:00:00'
                        WHEN 13 THEN '2026-06-13T09:00:00'
                        WHEN 14 THEN '2026-06-14T09:00:00'
                        WHEN 15 THEN '2026-06-15T09:00:00'
                        WHEN 16 THEN '2026-06-16T09:00:00'
                        WHEN 17 THEN '2026-06-17T09:00:00'
                        WHEN 18 THEN '2026-06-18T09:00:00'
                        WHEN 19 THEN '2026-06-19T09:00:00'
                        WHEN 20 THEN '2026-06-20T09:00:00'
                        ELSE CreatedAt
                    END,
                    SpecialRequests = CASE ReservationId
                        WHEN 1 THEN N'Window seat if available.'
                        WHEN 2 THEN N'Quiet table.'
                        WHEN 3 THEN N'Birthday dessert note.'
                        WHEN 4 THEN N'Near entrance.'
                        WHEN 5 THEN N'High chair requested.'
                        WHEN 7 THEN N'Outdoor area preferred.'
                        WHEN 9 THEN N'Anniversary setup.'
                        WHEN 11 THEN N'Family dinner.'
                        WHEN 13 THEN N'Near window.'
                        WHEN 15 THEN N'Vegetarian menu request.'
                        WHEN 17 THEN N'Sea view preferred.'
                        WHEN 19 THEN N'Late arrival possible.'
                        ELSE SpecialRequests
                    END,
                    ConfirmedAt = CASE ReservationId
                        WHEN 2 THEN '2026-06-02T10:00:00'
                        WHEN 3 THEN '2026-06-03T10:00:00'
                        WHEN 6 THEN '2026-06-06T10:00:00'
                        WHEN 7 THEN '2026-06-07T10:00:00'
                        WHEN 10 THEN '2026-06-10T10:00:00'
                        WHEN 11 THEN '2026-06-11T10:00:00'
                        WHEN 14 THEN '2026-06-14T10:00:00'
                        WHEN 15 THEN '2026-06-15T10:00:00'
                        WHEN 18 THEN '2026-06-18T10:00:00'
                        WHEN 19 THEN '2026-06-19T10:00:00'
                        ELSE ConfirmedAt
                    END,
                    ConfirmedByUserId = CASE
                        WHEN ReservationId IN (2, 3, 6, 7, 10, 11, 14, 15, 18, 19) THEN 5
                        ELSE ConfirmedByUserId
                    END,
                    CancelledAt = CASE ReservationId
                        WHEN 4 THEN '2026-06-05T10:00:00'
                        WHEN 8 THEN '2026-06-09T10:00:00'
                        WHEN 12 THEN '2026-06-13T10:00:00'
                        WHEN 16 THEN '2026-06-17T10:00:00'
                        WHEN 20 THEN '2026-06-21T10:00:00'
                        ELSE CancelledAt
                    END,
                    CancellationReason = CASE ReservationId
                        WHEN 4 THEN N'Customer requested cancellation.'
                        WHEN 8 THEN N'Table unavailable due to maintenance.'
                        WHEN 12 THEN N'Customer changed schedule.'
                        WHEN 16 THEN N'Restaurant capacity adjusted.'
                        WHEN 20 THEN N'Customer requested cancellation.'
                        ELSE CancellationReason
                    END,
                    PartySize = CASE ReservationId
                        WHEN 5 THEN 2
                        WHEN 10 THEN 2
                        WHEN 11 THEN 4
                        WHEN 15 THEN 2
                        WHEN 16 THEN 4
                        WHEN 17 THEN 6
                        WHEN 20 THEN 2
                        ELSE PartySize
                    END
                WHERE ReservationId BETWEEN 1 AND 20;

                UPDATE TableReservations
                SET ReservationCode = CONCAT('TR-LEGACY-', ReservationId)
                WHERE ReservationCode IS NULL OR LTRIM(RTRIM(ReservationCode)) = '';
                """);

            migrationBuilder.AlterColumn<string>(
                name: "ReservationCode",
                table: "TableReservations",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TableReservations_ConfirmedByUserId",
                table: "TableReservations",
                column: "ConfirmedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TableReservations_ReservationCode",
                table: "TableReservations",
                column: "ReservationCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TableReservations_Users_ConfirmedByUserId",
                table: "TableReservations",
                column: "ConfirmedByUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TableReservations_Users_ConfirmedByUserId",
                table: "TableReservations");

            migrationBuilder.DropIndex(
                name: "IX_TableReservations_ConfirmedByUserId",
                table: "TableReservations");

            migrationBuilder.DropIndex(
                name: "IX_TableReservations_ReservationCode",
                table: "TableReservations");

            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "TableReservations");

            migrationBuilder.DropColumn(
                name: "CancelledAt",
                table: "TableReservations");

            migrationBuilder.DropColumn(
                name: "ConfirmedAt",
                table: "TableReservations");

            migrationBuilder.DropColumn(
                name: "ConfirmedByUserId",
                table: "TableReservations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TableReservations");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "TableReservations");

            migrationBuilder.DropColumn(
                name: "ReservationCode",
                table: "TableReservations");

            migrationBuilder.DropColumn(
                name: "SpecialRequests",
                table: "TableReservations");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "ReservationTime",
                table: "TableReservations",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReservationDate",
                table: "TableReservations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "PartySize",
                table: "TableReservations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.Sql("""
                UPDATE TableReservations
                SET PartySize = CASE ReservationId
                    WHEN 5 THEN 7
                    WHEN 10 THEN 6
                    WHEN 11 THEN 7
                    WHEN 15 THEN 3
                    WHEN 16 THEN 5
                    WHEN 17 THEN 7
                    WHEN 20 THEN 6
                    ELSE PartySize
                END
                WHERE ReservationId IN (5, 10, 11, 15, 16, 17, 20);
                """);
        }
    }
}
