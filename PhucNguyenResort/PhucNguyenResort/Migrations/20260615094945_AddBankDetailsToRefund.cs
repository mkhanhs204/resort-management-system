using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhucNguyenResort.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddBankDetailsToRefund : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankDetails",
                table: "Refunds",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7729), new DateTime(2026, 6, 7, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7736), new DateTime(2026, 5, 31, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7740) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 20, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7743), new DateTime(2026, 6, 23, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7744), new DateTime(2026, 6, 13, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7746) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 25, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7747), new DateTime(2026, 6, 27, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7748), new DateTime(2026, 6, 14, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7749) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 10, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7752), new DateTime(2026, 6, 12, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7753), new DateTime(2026, 6, 5, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7755) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7757), new DateTime(2026, 6, 19, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7757), new DateTime(2026, 6, 10, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7759) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 15, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7761), new DateTime(2026, 5, 17, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7766), new DateTime(2026, 5, 10, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7768) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 15, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7770), new DateTime(2026, 4, 18, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7770), new DateTime(2026, 4, 5, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7772) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 26, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7774), new DateTime(2026, 5, 28, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7774), new DateTime(2026, 5, 21, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7777) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 31, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7778), new DateTime(2026, 6, 5, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7779), new DateTime(2026, 5, 26, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7780) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 5, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7782), new DateTime(2026, 7, 7, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7784), new DateTime(2026, 6, 14, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7785) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 30, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7787), new DateTime(2026, 7, 3, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7788), new DateTime(2026, 6, 15, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7790) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 15, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7792), new DateTime(2026, 2, 20, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7792), new DateTime(2026, 2, 5, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7794) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7796), new DateTime(2026, 1, 16, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7796), new DateTime(2026, 1, 12, 16, 49, 45, 7, DateTimeKind.Local).AddTicks(7798) });

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 1,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 2,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 3,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 4,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 5,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 6,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 7,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 8,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 9,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 10,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 11,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 12,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 13,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 14,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 15,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 16,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 17,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 18,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 19,
                column: "BankDetails",
                value: null);

            migrationBuilder.UpdateData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 20,
                column: "BankDetails",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankDetails",
                table: "Refunds");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 4, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2845), new DateTime(2026, 6, 6, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2853), new DateTime(2026, 5, 30, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2856) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 19, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2859), new DateTime(2026, 6, 22, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2860), new DateTime(2026, 6, 12, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2862) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 24, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2864), new DateTime(2026, 6, 26, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2865), new DateTime(2026, 6, 13, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2866) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 9, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2870), new DateTime(2026, 6, 11, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2870), new DateTime(2026, 6, 4, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2872) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 15, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2874), new DateTime(2026, 6, 18, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2875), new DateTime(2026, 6, 9, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2876) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 6,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 14, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2878), new DateTime(2026, 5, 16, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2883), new DateTime(2026, 5, 9, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2885) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 7,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 4, 14, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2918), new DateTime(2026, 4, 17, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2918), new DateTime(2026, 4, 4, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2921) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 25, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2923), new DateTime(2026, 5, 27, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2923), new DateTime(2026, 5, 20, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2925) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 5, 30, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2928), new DateTime(2026, 6, 4, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2928), new DateTime(2026, 5, 25, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2930) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 7, 4, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2932), new DateTime(2026, 7, 6, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2933), new DateTime(2026, 6, 13, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2935) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 11,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 6, 29, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2937), new DateTime(2026, 7, 2, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2937), new DateTime(2026, 6, 14, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2939) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 12,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 2, 14, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2941), new DateTime(2026, 2, 19, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2941), new DateTime(2026, 2, 4, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2944) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 13,
                columns: new[] { "CheckInDate", "CheckOutDate", "CreatedAt" },
                values: new object[] { new DateTime(2026, 1, 14, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2945), new DateTime(2026, 1, 15, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2946), new DateTime(2026, 1, 11, 17, 4, 24, 614, DateTimeKind.Local).AddTicks(2948) });
        }
    }
}
