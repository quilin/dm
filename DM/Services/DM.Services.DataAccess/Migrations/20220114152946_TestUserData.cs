using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DM.Services.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class TestUserData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "AccessPolicy", "Activated", "CanMerge", "Email", "Icq", "Info", "IsRemoved", "LastVisitDate", "Location", "Login", "MediumProfilePictureUrl", "MergeRequested", "Name", "PasswordHash", "ProfilePictureUrl", "QualityRating", "QuantityRating", "RatingDisabled", "RegistrationDate", "Role", "Salt", "Skype", "SmallProfilePictureUrl", "Status", "TimezoneId" },
                values: new object[,]
                {
                    { new Guid("5be34a7d-22db-4768-b211-4dc7654e261c"), 0, true, false, "admin@dm.am", null, null, false, null, null, "admin", null, null, null, "lUTrrT5dd82qIuamjL8cRRfxXpOa5Y/3ABUILuX+MRs=", null, 0, 0, false, new DateTimeOffset(new DateTime(2022, 1, 14, 15, 29, 45, 588, DateTimeKind.Unspecified).AddTicks(8371), new TimeSpan(0, 0, 0, 0, 0)), 19, "A8ZmMTaApByo5FqrDE+G3F7lYXCfvbFCBkuYmr4kkl0eVlHEvmm8IOECdc+KgxnMeeeLTscP1PJEIgRETKs508POLFd7XOOwS0oP", null, null, "My password is pwd_admin", null },
                    { new Guid("b80c3ca4-8ccf-463b-a13d-b829da404fe4"), 0, true, false, "player@dm.am", null, null, false, null, null, "player", null, null, null, "Es0VRSPBgkmTC7N/S5mNesFvn9e9c1bQEd/A7gt+6LA=", null, 0, 0, false, new DateTimeOffset(new DateTime(2022, 1, 14, 15, 29, 45, 588, DateTimeKind.Unspecified).AddTicks(9176), new TimeSpan(0, 0, 0, 0, 0)), 1, "HuuqgzvaET31lWiqqf11nkVXA18b3fx/aF93dTaLZnmIT63kFU8O1RRAi9pG7R9xERDVh+nkoiWaUqMp/RFyQRIBSjOcr9O8RuMD", null, null, "My password is pwd_player", null },
                    { new Guid("ad89a190-1f46-4718-b929-af5dfc8acd1e"), 0, true, false, "player_alt@dm.am", null, null, false, null, null, "player_alt", null, null, null, "Dg1QPgYLgM5Q67w/j5Jj/4z+Gzzk0Y3+jEnpkgkeqYY=", null, 0, 0, false, new DateTimeOffset(new DateTime(2022, 1, 14, 15, 29, 45, 588, DateTimeKind.Unspecified).AddTicks(9248), new TimeSpan(0, 0, 0, 0, 0)), 1, "3m7iJpyDBt3fuCKTlsLM8XdnuX8/0srof0/0qqmyNo7dzc6PazajxjuOPmZfUR9DCTlSO8xQ+b1r9i32LFLFsIP2HQtsaNxUV4+T", null, null, "My password is pwd_player_alt", null },
                    { new Guid("060384bc-1001-4449-bd24-4db2e6de8773"), 0, true, false, "moderator_general@dm.am", null, null, false, null, null, "moderator_general", null, null, null, "KbXOIktGkaTUETHU3w+7SrW7NvrUU/S/6AAgoJBZqTU=", null, 0, 0, false, new DateTimeOffset(new DateTime(2022, 1, 14, 15, 29, 45, 588, DateTimeKind.Unspecified).AddTicks(9251), new TimeSpan(0, 0, 0, 0, 0)), 9, "pXoay10W9EsX2jQZh6B6UyyDOHuqY5aXiTzLKfOGmNf9MH4J55KqVESCEDJGhqaZH5dFLTxib8Ga4nZAvU93VINJxztES0TN7/Bd", null, null, "My password is pwd_moderator_general", null },
                    { new Guid("d01a4eb1-af59-4585-95fa-2c98696c35fa"), 0, true, false, "nanny@dm.am", null, null, false, null, null, "nanny", null, null, null, "YdFyoApgb/ZEXbiPLzi45j5hY9M0yvexM3oqwDFfiNk=", null, 0, 0, false, new DateTimeOffset(new DateTime(2022, 1, 14, 15, 29, 45, 588, DateTimeKind.Unspecified).AddTicks(9254), new TimeSpan(0, 0, 0, 0, 0)), 5, "GfIyR4NZ03z9K40u+Rk0sWH59+krnd7dE22Y+kL7v7dulUV5Tvqeu27SDqjL/lYAaZmwsOz1A/Fx3HGs3fN+Zk0GPEtNeKOS6Scp", null, null, "My password is pwd_nanny", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("060384bc-1001-4449-bd24-4db2e6de8773"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("5be34a7d-22db-4768-b211-4dc7654e261c"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("ad89a190-1f46-4718-b929-af5dfc8acd1e"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("b80c3ca4-8ccf-463b-a13d-b829da404fe4"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("d01a4eb1-af59-4585-95fa-2c98696c35fa"));
        }
    }
}
