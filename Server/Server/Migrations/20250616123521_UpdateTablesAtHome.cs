using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTablesAtHome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPaid",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "isWin",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Gifts",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Categories",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Gifts",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categories",
                newName: "CategoryName");

            migrationBuilder.AddColumn<bool>(
                name: "isPaid",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isWin",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
