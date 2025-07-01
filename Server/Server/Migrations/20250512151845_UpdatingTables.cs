using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Gifts",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "Gifts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Gifts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserWinnerId",
                table: "Gifts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WinnerId",
                table: "Gifts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowMe",
                table: "Donors",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GiftId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isWin = table.Column<bool>(type: "bit", nullable: false),
                    isPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Gifts_GiftId",
                        column: x => x.GiftId,
                        principalTable: "Gifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_WinnerId",
                table: "Gifts",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_GiftId",
                table: "Tickets",
                column: "GiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Users_WinnerId",
                table: "Gifts",
                column: "WinnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Users_WinnerId",
                table: "Gifts");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_WinnerId",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Details",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "UserWinnerId",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "ShowMe",
                table: "Donors");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Gifts",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    GiftId = table.Column<int>(type: "int", nullable: false),
                    IsDraft = table.Column<bool>(type: "bit", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchases_Gifts_GiftId",
                        column: x => x.GiftId,
                        principalTable: "Gifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Purchases_Users_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_CustomerId",
                table: "Purchases",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_GiftId",
                table: "Purchases",
                column: "GiftId");
        }
    }
}
