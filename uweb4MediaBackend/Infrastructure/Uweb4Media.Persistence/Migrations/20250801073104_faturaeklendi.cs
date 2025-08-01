using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uweb4Media.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class faturaeklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvoiceId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoicePdfUrl",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceStatus",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "InvoicePdfUrl",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "InvoiceStatus",
                table: "Payments");
        }
    }
}
