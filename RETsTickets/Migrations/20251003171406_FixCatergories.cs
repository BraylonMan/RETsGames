using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RETsGames.Migrations
{
    /// <inheritdoc />
    public partial class FixCatergories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Game_GameId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_GameId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Game_CategoryId",
                table: "Game",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Category_CategoryId",
                table: "Game",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Category_CategoryId",
                table: "Game");

            migrationBuilder.DropIndex(
                name: "IX_Game_CategoryId",
                table: "Game");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Category_GameId",
                table: "Category",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Game_GameId",
                table: "Category",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
