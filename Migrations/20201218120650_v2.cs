using Microsoft.EntityFrameworkCore.Migrations;

namespace temperature.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_outside",
                table: "outside");

            migrationBuilder.DropPrimaryKey(
                name: "PK_inside",
                table: "inside");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "outside");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "inside");

            migrationBuilder.RenameTable(
                name: "outside",
                newName: "Outside");

            migrationBuilder.RenameTable(
                name: "inside",
                newName: "Inside");

            migrationBuilder.RenameColumn(
                name: "temp",
                table: "Outside",
                newName: "Temperature");

            migrationBuilder.RenameColumn(
                name: "moist",
                table: "Outside",
                newName: "Moisture");

            migrationBuilder.RenameColumn(
                name: "temp",
                table: "Inside",
                newName: "Temperature");

            migrationBuilder.RenameColumn(
                name: "moist",
                table: "Inside",
                newName: "Moisture");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Outside",
                table: "Outside",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inside",
                table: "Inside",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Outside",
                table: "Outside");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inside",
                table: "Inside");

            migrationBuilder.RenameTable(
                name: "Outside",
                newName: "outside");

            migrationBuilder.RenameTable(
                name: "Inside",
                newName: "inside");

            migrationBuilder.RenameColumn(
                name: "Temperature",
                table: "outside",
                newName: "temp");

            migrationBuilder.RenameColumn(
                name: "Moisture",
                table: "outside",
                newName: "moist");

            migrationBuilder.RenameColumn(
                name: "Temperature",
                table: "inside",
                newName: "temp");

            migrationBuilder.RenameColumn(
                name: "Moisture",
                table: "inside",
                newName: "moist");

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "outside",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "inside",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_outside",
                table: "outside",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_inside",
                table: "inside",
                column: "id");
        }
    }
}
