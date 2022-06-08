using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Air_Skypiea.Migrations
{
    public partial class updateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Peoples_Peopleid",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "Peoples");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_Peopleid",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Peopleid",
                table: "Reservations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Peopleid",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Peoples",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeDoc = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    doc = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peoples", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Peopleid",
                table: "Reservations",
                column: "Peopleid");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Peoples_Peopleid",
                table: "Reservations",
                column: "Peopleid",
                principalTable: "Peoples",
                principalColumn: "id");
        }
    }
}
