using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lumen.Modules.NotABot.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "NotABot");

            migrationBuilder.CreateTable(
                name: "ServerChannelStats",
                schema: "NotABot",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChannelId = table.Column<long>(type: "bigint", nullable: false),
                    MessageCount = table.Column<int>(type: "integer", nullable: false),
                    ReactionCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerChannelStats", x => new { x.Id, x.Date, x.ChannelId });
                });

            migrationBuilder.CreateTable(
                name: "ServerReactionStats",
                schema: "NotABot",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reaction = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerReactionStats", x => new { x.Id, x.Date, x.Reaction });
                });

            migrationBuilder.CreateTable(
                name: "ServerStats",
                schema: "NotABot",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MemberLeft = table.Column<int>(type: "integer", nullable: false),
                    MemberJoined = table.Column<int>(type: "integer", nullable: false),
                    MemberCount = table.Column<int>(type: "integer", nullable: false),
                    MessageCount = table.Column<int>(type: "integer", nullable: false),
                    ReactionAdded = table.Column<int>(type: "integer", nullable: false),
                    ReactionRemoved = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerStats", x => new { x.Id, x.Date });
                });

            migrationBuilder.CreateTable(
                name: "ServerUserStats",
                schema: "NotABot",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    MessageCount = table.Column<int>(type: "integer", nullable: false),
                    ReactionCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerUserStats", x => new { x.Id, x.Date, x.UserId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServerChannelStats",
                schema: "NotABot");

            migrationBuilder.DropTable(
                name: "ServerReactionStats",
                schema: "NotABot");

            migrationBuilder.DropTable(
                name: "ServerStats",
                schema: "NotABot");

            migrationBuilder.DropTable(
                name: "ServerUserStats",
                schema: "NotABot");
        }
    }
}
