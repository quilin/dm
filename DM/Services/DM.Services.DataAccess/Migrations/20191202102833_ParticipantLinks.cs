using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DM.Services.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ParticipantLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterRoomLinks");

            migrationBuilder.CreateTable(
                name: "ParticipantRoomLinks",
                columns: table => new
                {
                    ParticipantRoomLinkId = table.Column<Guid>(nullable: false),
                    ParticipantId = table.Column<Guid>(nullable: false),
                    RoomId = table.Column<Guid>(nullable: false),
                    Policy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantRoomLinks", x => x.ParticipantRoomLinkId);
                    table.ForeignKey(
                        name: "FK_ParticipantRoomLinks_Characters_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParticipantRoomLinks_Readers_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Readers",
                        principalColumn: "ReaderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParticipantRoomLinks_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantRoomLinks_ParticipantId",
                table: "ParticipantRoomLinks",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantRoomLinks_RoomId",
                table: "ParticipantRoomLinks",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParticipantRoomLinks");

            migrationBuilder.CreateTable(
                name: "CharacterRoomLinks",
                columns: table => new
                {
                    CharacterRoomLinkId = table.Column<Guid>(nullable: false),
                    CharacterId = table.Column<Guid>(nullable: false),
                    RoomId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterRoomLinks", x => x.CharacterRoomLinkId);
                    table.ForeignKey(
                        name: "FK_CharacterRoomLinks_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterRoomLinks_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterRoomLinks_CharacterId",
                table: "CharacterRoomLinks",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterRoomLinks_RoomId",
                table: "CharacterRoomLinks",
                column: "RoomId");
        }
    }
}
