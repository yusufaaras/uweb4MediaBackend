using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uweb4Media.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class videoisMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_MediaContents_MediaContentId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_MediaContentId_UserId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "VideoLocalizedString");

            migrationBuilder.AddColumn<int>(
                name: "CommentsCount",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPremium",
                table: "Videos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Videos",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MediaContentId",
                table: "Likes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "VideoId",
                table: "Likes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MediaContentId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "VideoId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_UserId",
                table: "Videos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_MediaContentId",
                table: "Likes",
                column: "MediaContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_VideoId_UserId",
                table: "Likes",
                columns: new[] { "VideoId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_VideoId",
                table: "Comments",
                column: "VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Videos_VideoId",
                table: "Comments",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_MediaContents_MediaContentId",
                table: "Likes",
                column: "MediaContentId",
                principalTable: "MediaContents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Videos_VideoId",
                table: "Likes",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_AppUsers_UserId",
                table: "Videos",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Videos_VideoId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_MediaContents_MediaContentId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Videos_VideoId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_AppUsers_UserId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_UserId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Likes_MediaContentId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_VideoId_UserId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Comments_VideoId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CommentsCount",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "IsPremium",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "Comments");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                table: "Videos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "VideoLocalizedString",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "MediaContentId",
                table: "Likes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MediaContentId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_MediaContentId_UserId",
                table: "Likes",
                columns: new[] { "MediaContentId", "UserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_MediaContents_MediaContentId",
                table: "Likes",
                column: "MediaContentId",
                principalTable: "MediaContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
