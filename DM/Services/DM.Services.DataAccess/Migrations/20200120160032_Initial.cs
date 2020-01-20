using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DM.Services.DataAccess.Migrations
{
    internal partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    ConversationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.ConversationId);
                });

            migrationBuilder.CreateTable(
                name: "Fora",
                columns: table => new
                {
                    ForumId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    ViewPolicy = table.Column<int>(nullable: false),
                    CreateTopicPolicy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fora", x => x.ForumId);
                });

            migrationBuilder.CreateTable(
                name: "TagGroups",
                columns: table => new
                {
                    TagGroupId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagGroups", x => x.TagGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    Login = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    RegistrationDate = table.Column<DateTimeOffset>(nullable: false),
                    LastVisitDate = table.Column<DateTimeOffset>(nullable: true),
                    TimezoneId = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false),
                    AccessPolicy = table.Column<int>(nullable: false),
                    Salt = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    RatingDisabled = table.Column<bool>(nullable: false),
                    QualityRating = table.Column<int>(nullable: false),
                    QuantityRating = table.Column<int>(nullable: false),
                    Activated = table.Column<bool>(nullable: false),
                    CanMerge = table.Column<bool>(nullable: false),
                    MergeRequested = table.Column<Guid>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Icq = table.Column<string>(nullable: true),
                    Skype = table.Column<string>(nullable: true),
                    ShowEmail = table.Column<bool>(nullable: false),
                    Info = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<Guid>(nullable: false),
                    TagGroupId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                    table.ForeignKey(
                        name: "FK_Tags_TagGroups_TagGroupId",
                        column: x => x.TagGroupId,
                        principalTable: "TagGroups",
                        principalColumn: "TagGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bans",
                columns: table => new
                {
                    BanId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ModeratorId = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTimeOffset>(nullable: false),
                    EndDate = table.Column<DateTimeOffset>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    AccessRestrictionPolicy = table.Column<int>(nullable: false),
                    IsVoluntary = table.Column<bool>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bans", x => x.BanId);
                    table.ForeignKey(
                        name: "FK_Bans_Users_ModeratorId",
                        column: x => x.ModeratorId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bans_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumModerators",
                columns: table => new
                {
                    ForumModeratorId = table.Column<Guid>(nullable: false),
                    ForumId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumModerators", x => x.ForumModeratorId);
                    table.ForeignKey(
                        name: "FK_ForumModerators_Fora_ForumId",
                        column: x => x.ForumId,
                        principalTable: "Fora",
                        principalColumn: "ForumId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForumModerators_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false),
                    ReleaseDate = table.Column<DateTimeOffset>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    MasterId = table.Column<Guid>(nullable: false),
                    AssistantId = table.Column<Guid>(nullable: true),
                    NannyId = table.Column<Guid>(nullable: true),
                    AttributeSchemaId = table.Column<Guid>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    SystemName = table.Column<string>(nullable: true),
                    SettingName = table.Column<string>(nullable: true),
                    Info = table.Column<string>(nullable: true),
                    HideTemper = table.Column<bool>(nullable: false),
                    HideSkills = table.Column<bool>(nullable: false),
                    HideInventory = table.Column<bool>(nullable: false),
                    HideStory = table.Column<bool>(nullable: false),
                    DisableAlignment = table.Column<bool>(nullable: false),
                    HideDiceResult = table.Column<bool>(nullable: false),
                    ShowPrivateMessages = table.Column<bool>(nullable: false),
                    CommentariesAccessMode = table.Column<int>(nullable: false),
                    Notepad = table.Column<string>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_Games_Users_AssistantId",
                        column: x => x.AssistantId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Users_MasterId",
                        column: x => x.MasterId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_Users_NannyId",
                        column: x => x.NannyId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ConversationId = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "ConversationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    TargetId = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false),
                    ReportText = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    AnswerAuthorId = table.Column<Guid>(nullable: true),
                    Answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Reports_Users_AnswerAuthorId",
                        column: x => x.AnswerAuthorId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_Users_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserConversationLinks",
                columns: table => new
                {
                    UserConversationLinkId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ConversationId = table.Column<Guid>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConversationLinks", x => x.UserConversationLinkId);
                    table.ForeignKey(
                        name: "FK_UserConversationLinks_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "ConversationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserConversationLinks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlackListLinks",
                columns: table => new
                {
                    BlackListLinkId = table.Column<Guid>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackListLinks", x => x.BlackListLinkId);
                    table.ForeignKey(
                        name: "FK_BlackListLinks_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlackListLinks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    CharacterId = table.Column<Guid>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdateDate = table.Column<DateTimeOffset>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Race = table.Column<string>(nullable: true),
                    Class = table.Column<string>(nullable: true),
                    Alignment = table.Column<int>(nullable: true),
                    Appearance = table.Column<string>(nullable: true),
                    Temper = table.Column<string>(nullable: true),
                    Story = table.Column<string>(nullable: true),
                    Skills = table.Column<string>(nullable: true),
                    Inventory = table.Column<string>(nullable: true),
                    IsNpc = table.Column<bool>(nullable: false),
                    AccessPolicy = table.Column<int>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.CharacterId);
                    table.ForeignKey(
                        name: "FK_Characters_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameTags",
                columns: table => new
                {
                    GameTagId = table.Column<Guid>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTags", x => x.GameTagId);
                    table.ForeignKey(
                        name: "FK_GameTags_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Readers",
                columns: table => new
                {
                    ReaderId = table.Column<Guid>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readers", x => x.ReaderId);
                    table.ForeignKey(
                        name: "FK_Readers_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Readers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    AccessType = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    OrderNumber = table.Column<double>(nullable: false),
                    PreviousRoomId = table.Column<Guid>(nullable: true),
                    NextRoomId = table.Column<Guid>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_Rooms_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rooms_Rooms_NextRoomId",
                        column: x => x.NextRoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rooms_Rooms_PreviousRoomId",
                        column: x => x.PreviousRoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    TokenId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.TokenId);
                    table.ForeignKey(
                        name: "FK_Tokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterAttributes",
                columns: table => new
                {
                    CharacterAttributeId = table.Column<Guid>(nullable: false),
                    AttributeId = table.Column<Guid>(nullable: false),
                    CharacterId = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterAttributes", x => x.CharacterAttributeId);
                    table.ForeignKey(
                        name: "FK_CharacterAttributes_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PendingPosts",
                columns: table => new
                {
                    PendingPostId = table.Column<Guid>(nullable: false),
                    AwaitingUserId = table.Column<Guid>(nullable: false),
                    PendingUserId = table.Column<Guid>(nullable: false),
                    RoomId = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingPosts", x => x.PendingPostId);
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
                name: "Posts",
                columns: table => new
                {
                    PostId = table.Column<Guid>(nullable: false),
                    RoomId = table.Column<Guid>(nullable: false),
                    CharacterId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdateUserId = table.Column<Guid>(nullable: true),
                    LastUpdateDate = table.Column<DateTimeOffset>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Commentary = table.Column<string>(nullable: true),
                    MasterMessage = table.Column<string>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Posts_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_Users_LastUpdateUserId",
                        column: x => x.LastUpdateUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
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
                        name: "FK_RoomClaims_Characters_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomClaims_Readers_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Readers",
                        principalColumn: "ReaderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomClaims_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Uploads",
                columns: table => new
                {
                    UploadId = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    VirtualPath = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uploads", x => x.UploadId);
                    table.ForeignKey(
                        name: "FK_Uploads_Characters_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Uploads_Games_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Uploads_Posts_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Uploads_Users_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Uploads_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    VoteId = table.Column<Guid>(nullable: false),
                    PostId = table.Column<Guid>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    TargetUserId = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    SignValue = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.VoteId);
                    table.ForeignKey(
                        name: "FK_Votes_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votes_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votes_Users_TargetUserId",
                        column: x => x.TargetUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumTopics",
                columns: table => new
                {
                    ForumTopicId = table.Column<Guid>(nullable: false),
                    ForumId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Attached = table.Column<bool>(nullable: false),
                    Closed = table.Column<bool>(nullable: false),
                    LastCommentId = table.Column<Guid>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumTopics", x => x.ForumTopicId);
                    table.ForeignKey(
                        name: "FK_ForumTopics_Fora_ForumId",
                        column: x => x.ForumId,
                        principalTable: "Fora",
                        principalColumn: "ForumId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForumTopics_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdateDate = table.Column<DateTimeOffset>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    LikeId = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.LikeId);
                    table.ForeignKey(
                        name: "FK_Likes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Warnings",
                columns: table => new
                {
                    WarningId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ModeratorId = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Points = table.Column<int>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warnings", x => x.WarningId);
                    table.ForeignKey(
                        name: "FK_Warnings_Users_ModeratorId",
                        column: x => x.ModeratorId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Warnings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bans_ModeratorId",
                table: "Bans",
                column: "ModeratorId");

            migrationBuilder.CreateIndex(
                name: "IX_Bans_UserId",
                table: "Bans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlackListLinks_GameId",
                table: "BlackListLinks",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_BlackListLinks_UserId",
                table: "BlackListLinks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterAttributes_CharacterId",
                table: "CharacterAttributes",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_GameId",
                table: "Characters",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_UserId",
                table: "Characters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_EntityId",
                table: "Comments",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumModerators_ForumId",
                table: "ForumModerators",
                column: "ForumId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumModerators_UserId",
                table: "ForumModerators",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopics_ForumId",
                table: "ForumTopics",
                column: "ForumId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopics_LastCommentId",
                table: "ForumTopics",
                column: "LastCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopics_UserId",
                table: "ForumTopics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_AssistantId",
                table: "Games",
                column: "AssistantId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_MasterId",
                table: "Games",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_NannyId",
                table: "Games",
                column: "NannyId");

            migrationBuilder.CreateIndex(
                name: "IX_GameTags_GameId",
                table: "GameTags",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameTags_TagId",
                table: "GameTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_EntityId",
                table: "Likes",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId",
                table: "Likes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId",
                table: "Messages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");

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
                name: "IX_Posts_CharacterId",
                table: "Posts",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_LastUpdateUserId",
                table: "Posts",
                column: "LastUpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_RoomId",
                table: "Posts",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_GameId",
                table: "Readers",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_UserId",
                table: "Readers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AnswerAuthorId",
                table: "Reports",
                column: "AnswerAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TargetId",
                table: "Reports",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserId",
                table: "Reports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomClaims_ParticipantId",
                table: "RoomClaims",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomClaims_RoomId",
                table: "RoomClaims",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_GameId",
                table: "Rooms",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_NextRoomId",
                table: "Rooms",
                column: "NextRoomId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_PreviousRoomId",
                table: "Rooms",
                column: "PreviousRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_TagGroupId",
                table: "Tags",
                column: "TagGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_EntityId",
                table: "Tokens",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_UserId",
                table: "Tokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_EntityId",
                table: "Uploads",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_UserId",
                table: "Uploads",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserConversationLinks_ConversationId",
                table: "UserConversationLinks",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserConversationLinks_UserId",
                table: "UserConversationLinks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_GameId",
                table: "Votes",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_PostId",
                table: "Votes",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_TargetUserId",
                table: "Votes",
                column: "TargetUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UserId",
                table: "Votes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Warnings_EntityId",
                table: "Warnings",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Warnings_ModeratorId",
                table: "Warnings",
                column: "ModeratorId");

            migrationBuilder.CreateIndex(
                name: "IX_Warnings_UserId",
                table: "Warnings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumTopics_Comments_LastCommentId",
                table: "ForumTopics",
                column: "LastCommentId",
                principalTable: "Comments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumTopics_Users_UserId",
                table: "ForumTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_AssistantId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_MasterId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_NannyId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Games_EntityId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ForumTopics_EntityId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "Bans");

            migrationBuilder.DropTable(
                name: "BlackListLinks");

            migrationBuilder.DropTable(
                name: "CharacterAttributes");

            migrationBuilder.DropTable(
                name: "ForumModerators");

            migrationBuilder.DropTable(
                name: "GameTags");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "PendingPosts");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "RoomClaims");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Uploads");

            migrationBuilder.DropTable(
                name: "UserConversationLinks");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropTable(
                name: "Warnings");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Readers");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "TagGroups");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "ForumTopics");

            migrationBuilder.DropTable(
                name: "Fora");

            migrationBuilder.DropTable(
                name: "Comments");
        }
    }
}
