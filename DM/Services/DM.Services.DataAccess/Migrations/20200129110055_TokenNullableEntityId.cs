using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DM.Services.DataAccess.Migrations
{
    internal partial class TokenNullableEntityId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "EntityId",
                table: "Tokens",
                nullable: true,
                oldClrType: typeof(Guid));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DELETE FROM ""Tokens""
WHERE ""EntityId"" is null;
");
            migrationBuilder.AlterColumn<Guid>(
                name: "EntityId",
                table: "Tokens",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
