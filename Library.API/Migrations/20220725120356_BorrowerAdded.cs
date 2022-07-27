using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.API.Migrations
{
    public partial class BorrowerAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BorrowerID",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_BorrowerID",
                table: "Books",
                column: "BorrowerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_User_BorrowerID",
                table: "Books",
                column: "BorrowerID",
                principalTable: "User",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_User_BorrowerID",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BorrowerID",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BorrowerID",
                table: "Books");
        }
    }
}
