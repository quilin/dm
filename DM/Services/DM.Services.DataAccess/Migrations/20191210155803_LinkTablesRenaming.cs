using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DM.Services.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class LinkTablesRenaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParticipantRoomLinks");

            migrationBuilder.DropTable(
                name: "PostWaitNotifications");

            migrationBuilder.CreateTable(
                name: "PendingPosts",
                columns: table => new
                {
                    PostPendingId = table.Column<Guid>(nullable: false),
                    AwaitingUserId = table.Column<Guid>(nullable: false),
                    PendingUserId = table.Column<Guid>(nullable: false),
                    RoomId = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingPosts", x => x.PostPendingId);
                    table.ForeignKey(
                        name: "FK_PendingPosts_Users_AwaitingUserId",
                        column: x => x.AwaitingUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PendingPosts_Users_PendingUserId",
                        column: x => x.PendingUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PendingPosts_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomClaims",
                columns: table => new
                {
                    RoomClaimId = table.Column<Guid>(nullable: false),
                    ParticipantId = table.Column<Guid>(nullable: false),
                    RoomId = table.Column<Guid>(nullable: false),
                    Policy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomClaims", x => x.RoomClaimId);
                    table.ForeignKey(
                        name: "FK_RoomClaims_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PendingPosts_AwaitingUserId",
                table: "PendingPosts",
                column: "AwaitingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingPosts_PendingUserId",
                table: "PendingPosts",
                column: "PendingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingPosts_RoomId",
                table: "PendingPosts",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomClaims_ParticipantId",
                table: "RoomClaims",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomClaims_RoomId",
                table: "RoomClaims",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PendingPosts");

            migrationBuilder.DropTable(
                name: "RoomClaims");

            migrationBuilder.CreateTable(
                name: "ParticipantRoomLinks",
                columns: table => new
                {
                    ParticipantRoomLinkId = table.Column<Guid>(nullable: false),
                    ParticipantId = table.Column<Guid>(nullable: false),
                    Policy = table.Column<int>(nullable: false),
                    RoomId = table.Column<Guid>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "PostWaitNotifications",
                columns: table => new
                {
                    PostAnticipationId = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false),
                    RoomId = table.Column<Guid>(nullable: false),
                    TargetId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostWaitNotifications", x => x.PostAnticipationId);
                    table.ForeignKey(
                        name: "FK_PostWaitNotifications_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostWaitNotifications_Users_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostWaitNotifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
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

            migrationBuilder.CreateIndex(
                name: "IX_PostWaitNotifications_RoomId",
                table: "PostWaitNotifications",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_PostWaitNotifications_TargetId",
                table: "PostWaitNotifications",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_PostWaitNotifications_UserId",
                table: "PostWaitNotifications",
                column: "UserId");
        }
    }
}
