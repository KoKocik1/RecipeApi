using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameWithoutUnderline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Ingredients_Units_ingredients_Units_ingredientId",
                table: "Recipe_Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_Ingredients_Units_ingredientId",
                table: "Recipe_Ingredients");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Recipe_Ingredients",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_Ingredients_UnitId",
                table: "Recipe_Ingredients",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Ingredients_Units_ingredients_UnitId",
                table: "Recipe_Ingredients",
                column: "UnitId",
                principalTable: "Units_ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Ingredients_Units_ingredients_UnitId",
                table: "Recipe_Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_Ingredients_UnitId",
                table: "Recipe_Ingredients");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Recipe_Ingredients");

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_Ingredients_Units_ingredientId",
                table: "Recipe_Ingredients",
                column: "Units_ingredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Ingredients_Units_ingredients_Units_ingredientId",
                table: "Recipe_Ingredients",
                column: "Units_ingredientId",
                principalTable: "Units_ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
