using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Air_Skypiea.Migrations
{
    public partial class addTarget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TargetId",
                table: "Travels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Travels_TargetId",
                table: "Travels",
                column: "TargetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Travels_Cities_TargetId",
                table: "Travels",
                column: "TargetId",
                principalTable: "Cities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Travels_Cities_TargetId",
                table: "Travels");

            migrationBuilder.DropIndex(
                name: "IX_Travels_TargetId",
                table: "Travels");

            migrationBuilder.DropColumn(
                name: "TargetId",
                table: "Travels");
        }
    }
}
