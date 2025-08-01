using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uweb4Media.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class paymentservice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentCode",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentCodeGeneratedAt",
                table: "Payments",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentCode",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentCodeGeneratedAt",
                table: "Payments");
        }
    }
}
