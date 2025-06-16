using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdatigTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Categories_CategoryID",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "UserWinnerId",
                table: "Gifts");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "Gifts",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Gifts_CategoryID",
                table: "Gifts",
                newName: "IX_Gifts_CategoryId");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Categories_CategoryId",
                table: "Gifts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Categories_CategoryId",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Gifts",
                newName: "CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Gifts_CategoryId",
                table: "Gifts",
                newName: "IX_Gifts_CategoryID");

            migrationBuilder.AddColumn<int>(
                name: "UserWinnerId",
                table: "Gifts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Categories_CategoryID",
                table: "Gifts",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
