using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uweb4Media.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class google : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleId",
                table: "AppUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleId",
                table: "AppUsers");
        }
    }
}
