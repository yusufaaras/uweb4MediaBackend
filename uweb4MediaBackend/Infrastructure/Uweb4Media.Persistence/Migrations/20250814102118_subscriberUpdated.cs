using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uweb4Media.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class subscriberUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_SubscriberUserId_AuthorUserId",
                table: "Subscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorUserId",
                table: "Subscriptions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AuthorCompanyId",
                table: "Subscriptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_AuthorCompanyId",
                table: "Subscriptions",
                column: "AuthorCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SubscriberUserId_AuthorUserId",
                table: "Subscriptions",
                columns: new[] { "SubscriberUserId", "AuthorUserId" },
                unique: true,
                filter: "[AuthorUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Companies_AuthorCompanyId",
                table: "Subscriptions",
                column: "AuthorCompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Companies_AuthorCompanyId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_AuthorCompanyId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_SubscriberUserId_AuthorUserId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "AuthorCompanyId",
                table: "Subscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorUserId",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SubscriberUserId_AuthorUserId",
                table: "Subscriptions",
                columns: new[] { "SubscriberUserId", "AuthorUserId" },
                unique: true);
        }
    }
}
