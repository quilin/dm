using Microsoft.EntityFrameworkCore.Migrations;

namespace DM.Services.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NullableAlignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Alignment",
                table: "Characters",
                nullable: true,
                oldClrType: typeof(int));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Alignment",
                table: "Characters",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
