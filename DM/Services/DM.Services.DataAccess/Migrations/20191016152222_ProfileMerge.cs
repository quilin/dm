using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DM.Services.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ProfileMerge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDatas");

            migrationBuilder.AddColumn<string>(
                name: "Icq",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Info",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowEmail",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Skype",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Users",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icq",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Info",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ShowEmail",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Skype",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserDatas",
                columns: table => new
                {
                    UserProfileId = table.Column<Guid>(nullable: false),
                    Icq = table.Column<string>(nullable: true),
                    Info = table.Column<string>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ShowEmail = table.Column<bool>(nullable: false),
                    Skype = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDatas", x => x.UserProfileId);
                    table.ForeignKey(
                        name: "FK_UserDatas_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDatas_UserId",
                table: "UserDatas",
                column: "UserId",
                unique: true);
        }
    }
}
