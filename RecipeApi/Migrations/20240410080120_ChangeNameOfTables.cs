using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RecipeApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameOfTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Ingredients_Ingredients_IngredientId",
                table: "Recipe_Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Ingredients_Recipes_RecipeId",
                table: "Recipe_Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Ingredients_Units_ingredients_UnitId",
                table: "Recipe_Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Instructions_Recipes_RecipeId",
                table: "Recipe_Instructions");

            migrationBuilder.DropTable(
                name: "Units_ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipe_Instructions",
                table: "Recipe_Instructions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipe_Ingredients",
                table: "Recipe_Ingredients");

            migrationBuilder.RenameTable(
                name: "Recipe_Instructions",
                newName: "RecipeInstructions");

            migrationBuilder.RenameTable(
                name: "Recipe_Ingredients",
                newName: "RecipeIngredients");

            migrationBuilder.RenameIndex(
                name: "IX_Recipe_Instructions_RecipeId",
                table: "RecipeInstructions",
                newName: "IX_RecipeInstructions_RecipeId");

            migrationBuilder.RenameColumn(
                name: "Units_ingredientId",
                table: "RecipeIngredients",
                newName: "Unit_ingredientId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipe_Ingredients_UnitId",
                table: "RecipeIngredients",
                newName: "IX_RecipeIngredients_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipe_Ingredients_RecipeId",
                table: "RecipeIngredients",
                newName: "IX_RecipeIngredients_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipe_Ingredients_IngredientId",
                table: "RecipeIngredients",
                newName: "IX_RecipeIngredients_IngredientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeInstructions",
                table: "RecipeInstructions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UnitsIngredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitsIngredients", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Ingredients_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Recipes_RecipeId",
                table: "RecipeIngredients",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_UnitsIngredients_UnitId",
                table: "RecipeIngredients",
                column: "UnitId",
                principalTable: "UnitsIngredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeInstructions_Recipes_RecipeId",
                table: "RecipeInstructions",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Ingredients_IngredientId",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Recipes_RecipeId",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_UnitsIngredients_UnitId",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeInstructions_Recipes_RecipeId",
                table: "RecipeInstructions");

            migrationBuilder.DropTable(
                name: "UnitsIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeInstructions",
                table: "RecipeInstructions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients");

            migrationBuilder.RenameTable(
                name: "RecipeInstructions",
                newName: "Recipe_Instructions");

            migrationBuilder.RenameTable(
                name: "RecipeIngredients",
                newName: "Recipe_Ingredients");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeInstructions_RecipeId",
                table: "Recipe_Instructions",
                newName: "IX_Recipe_Instructions_RecipeId");

            migrationBuilder.RenameColumn(
                name: "Unit_ingredientId",
                table: "Recipe_Ingredients",
                newName: "Units_ingredientId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredients_UnitId",
                table: "Recipe_Ingredients",
                newName: "IX_Recipe_Ingredients_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredients_RecipeId",
                table: "Recipe_Ingredients",
                newName: "IX_Recipe_Ingredients_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredients_IngredientId",
                table: "Recipe_Ingredients",
                newName: "IX_Recipe_Ingredients_IngredientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipe_Instructions",
                table: "Recipe_Instructions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipe_Ingredients",
                table: "Recipe_Ingredients",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Units_ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units_ingredients", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Ingredients_Ingredients_IngredientId",
                table: "Recipe_Ingredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Ingredients_Recipes_RecipeId",
                table: "Recipe_Ingredients",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Ingredients_Units_ingredients_UnitId",
                table: "Recipe_Ingredients",
                column: "UnitId",
                principalTable: "Units_ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Instructions_Recipes_RecipeId",
                table: "Recipe_Instructions",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
