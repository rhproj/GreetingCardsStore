using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GCard.DataAccess.Migrations
{
    public partial class DisplayOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Occasion",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "ItemType",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Occasion");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "ItemType",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
