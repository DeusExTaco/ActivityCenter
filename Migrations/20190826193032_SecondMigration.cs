using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivityCenter.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                "time",
                "Events",
                "end_date");

            migrationBuilder.AddColumn<int>(
                "normalized_duration",
                "Events",
                "INT",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "normalized_duration",
                "Events");

            migrationBuilder.RenameColumn(
                "end_date",
                "Events",
                "time");
        }
    }
}