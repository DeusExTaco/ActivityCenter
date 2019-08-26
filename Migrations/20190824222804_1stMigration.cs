using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivityCenter.Migrations
{
    public partial class _1stMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Users",
                table => new
                {
                    id = table.Column<int>()
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    first_name = table.Column<string>("VARCHAR(45)", maxLength: 45),
                    last_name = table.Column<string>("VARCHAR(45)", maxLength: 45),
                    email = table.Column<string>("VARCHAR(255)"),
                    password = table.Column<string>("VARCHAR(255)"),
                    birthday = table.Column<DateTime>("DATETIME"),
                    created_at = table.Column<DateTime>("DATETIME"),
                    updated_at = table.Column<DateTime>("DATETIME")
                },
                constraints: table => { table.PrimaryKey("PK_Users", x => x.id); });

            migrationBuilder.CreateTable(
                "Events",
                table => new
                {
                    id = table.Column<int>()
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(),
                    event_title = table.Column<string>("VARCHAR(45)", maxLength: 45),
                    date = table.Column<DateTime>("DATETIME"),
                    time = table.Column<DateTime>("DATETIME"),
                    duration = table.Column<int>(),
                    duration_units = table.Column<string>("VARCHAR(10)"),
                    description = table.Column<string>("TEXT"),
                    created_at = table.Column<DateTime>("DATETIME"),
                    updated_at = table.Column<DateTime>("DATETIME")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.id);
                    table.ForeignKey(
                        "FK_Events_Users_user_id",
                        x => x.user_id,
                        "Users",
                        "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Participants",
                table => new
                {
                    id = table.Column<int>()
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(),
                    event_id = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.id);
                    table.ForeignKey(
                        "FK_Participants_Events_event_id",
                        x => x.event_id,
                        "Events",
                        "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Participants_Users_user_id",
                        x => x.user_id,
                        "Users",
                        "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Events_user_id",
                "Events",
                "user_id");

            migrationBuilder.CreateIndex(
                "IX_Participants_event_id",
                "Participants",
                "event_id");

            migrationBuilder.CreateIndex(
                "IX_Participants_user_id",
                "Participants",
                "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Participants");

            migrationBuilder.DropTable(
                "Events");

            migrationBuilder.DropTable(
                "Users");
        }
    }
}