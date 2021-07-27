using Microsoft.EntityFrameworkCore.Migrations;

namespace DM.Services.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ProfilePictures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "Users",
                maxLength: 200,
                nullable: true);
            
            migrationBuilder.AddColumn<string>(
                name: "MediumProfilePictureUrl",
                table: "Users",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmallProfilePictureUrl",
                table: "Users",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Original",
                table: "Uploads",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "Users");
            
            migrationBuilder.DropColumn(
                name: "MediumProfilePictureUrl",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SmallProfilePictureUrl",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Original",
                table: "Uploads");
        }
    }
}
