﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeApi.Migrations
{
    /// <inheritdoc />
    public partial class IngredientsAndRecipes1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Recipe_Ingredients_IngredientId",
                table: "Recipe_Ingredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_Ingredients_Units_ingredientId",
                table: "Recipe_Ingredients",
                column: "Units_ingredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Ingredients_Ingredients_IngredientId",
                table: "Recipe_Ingredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Ingredients_Units_ingredients_Units_ingredientId",
                table: "Recipe_Ingredients",
                column: "Units_ingredientId",
                principalTable: "Units_ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Ingredients_Ingredients_IngredientId",
                table: "Recipe_Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Ingredients_Units_ingredients_Units_ingredientId",
                table: "Recipe_Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_Ingredients_IngredientId",
                table: "Recipe_Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_Ingredients_Units_ingredientId",
                table: "Recipe_Ingredients");
        }
    }
}
