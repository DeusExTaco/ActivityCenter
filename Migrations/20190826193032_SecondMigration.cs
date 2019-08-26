using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivityCenter.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "time",
                table: "Events",
                newName: "end_date");

            migrationBuilder.AddColumn<int>(
                name: "normalized_duration",
                table: "Events",
                type: "INT",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "normalized_duration",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "end_date",
                table: "Events",
                newName: "time");
        }
    }
}
