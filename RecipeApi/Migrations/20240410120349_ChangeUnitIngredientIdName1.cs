using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUnitIngredientIdName1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_UnitsIngredients_UnitIngredientId",
                table: "RecipeIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UnitsIngredients",
                table: "UnitsIngredients");

            migrationBuilder.RenameTable(
                name: "UnitsIngredients",
                newName: "UnitIngredients");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UnitIngredients",
                table: "UnitIngredients",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_UnitIngredients_UnitIngredientId",
                table: "RecipeIngredients",
                column: "UnitIngredientId",
                principalTable: "UnitIngredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_UnitIngredients_UnitIngredientId",
                table: "RecipeIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UnitIngredients",
                table: "UnitIngredients");

            migrationBuilder.RenameTable(
                name: "UnitIngredients",
                newName: "UnitsIngredients");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UnitsIngredients",
                table: "UnitsIngredients",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_UnitsIngredients_UnitIngredientId",
                table: "RecipeIngredients",
                column: "UnitIngredientId",
                principalTable: "UnitsIngredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
