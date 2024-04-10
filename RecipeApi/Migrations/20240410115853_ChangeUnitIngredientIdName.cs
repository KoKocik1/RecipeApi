using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUnitIngredientIdName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_UnitsIngredients_UnitId",
                table: "RecipeIngredients");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredients_UnitId",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "RecipeIngredients");

            migrationBuilder.RenameColumn(
                name: "Unit_ingredientId",
                table: "RecipeIngredients",
                newName: "UnitIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_UnitIngredientId",
                table: "RecipeIngredients",
                column: "UnitIngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_UnitsIngredients_UnitIngredientId",
                table: "RecipeIngredients",
                column: "UnitIngredientId",
                principalTable: "UnitsIngredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_UnitsIngredients_UnitIngredientId",
                table: "RecipeIngredients");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredients_UnitIngredientId",
                table: "RecipeIngredients");

            migrationBuilder.RenameColumn(
                name: "UnitIngredientId",
                table: "RecipeIngredients",
                newName: "Unit_ingredientId");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "RecipeIngredients",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_UnitId",
                table: "RecipeIngredients",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_UnitsIngredients_UnitId",
                table: "RecipeIngredients",
                column: "UnitId",
                principalTable: "UnitsIngredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
