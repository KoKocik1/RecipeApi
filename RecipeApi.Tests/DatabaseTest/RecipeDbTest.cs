using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecipeApi.Database;
using Xunit;
namespace RecipeApi.Tests
{
    public class RecipeDbTests : TestDbContextBase
    {
        [Fact]
        public void DbContext_CanAddAndRetrieveRecipes()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: "DatabaseTest")
                .Options;

            // Act
            using (var context = new RecipeDbContext(options))
            {
                // Add a new recipe
                var recipe = new Recipe { Title = "Test Recipe", Portions = 4, TimeToCook = "30 min", CreatedAt=DateTime.Now, Description = "Test Description"};
                context.Recipes.Add(recipe);
                context.SaveChanges();

                var retrievedRecipe = context.Recipes.FirstOrDefault(r => r.Title == "Test Recipe");
                Assert.NotNull(retrievedRecipe);
                Assert.Equal("Test Description", retrievedRecipe.Description);
                Assert.Equal("Test Recipe", retrievedRecipe.Title);

                context.Recipes.Remove(retrievedRecipe);
            }
        }

        [Fact]
        public void DbContext_CanAddAndRetrieveRecipesWithIngredients()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: "DbContext_CanAddAndRetrieveRecipesWithIngredients")
                .Options;

            // Act
            using (var context = new RecipeDbContext(options))
            {

                // Create ingredients
                var ingredient1 = new Ingredient { Name = "Test Ingredient 1", Verified=false};
                var ingredient2 = new Ingredient { Name = "Test Ingredient 2", Verified=false};
                context.Ingredients.AddRange(ingredient1, ingredient2);
                context.SaveChanges();

                //Create Unit
                var unit1 = new UnitIngredient { Type = "Test Unit 1"};
                var unit2 = new UnitIngredient { Type = "Test Unit 2"};
                context.UnitIngredients.AddRange(unit1, unit2);
                context.SaveChanges();

                // Create instructions
                var instruction1 = new RecipeInstruction { Instruction = "Test Step 1", Order = 1, RecipeId = 1};
                var instruction2 = new RecipeInstruction { Instruction = "Test Step 2", Order = 2, RecipeId = 1};
                context.RecipeInstructions.AddRange(instruction1, instruction2);

                // Add a new recipe
                var recipe = new Recipe { Title = "Test Recipe", Description = "Test Description", Instructions = new List<RecipeInstruction> { instruction1, instruction2 }, Portions = 4, TimeToCook = "30 min", CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now};
                context.Recipes.Add(recipe);
                context.SaveChanges();

                var recipeIngredient1 = new RecipeIngredient { Ingredient = ingredient1, Quantity = 1, IngredientId = ingredient1.Id, RecipeId = recipe.Id, UnitIngredient = unit1, UnitIngredientId = unit1.Id};
                var recipeIngredient2 = new RecipeIngredient { Ingredient = ingredient2, Quantity = 2, IngredientId = ingredient2.Id, RecipeId = recipe.Id, UnitIngredient = unit2, UnitIngredientId = unit2.Id};
                context.RecipeIngredients.AddRange(recipeIngredient1, recipeIngredient2);
                context.SaveChanges();

                // connect ingredients with recipe
                recipe.Ingredients = new List<RecipeIngredient> { recipeIngredient1, recipeIngredient2 };
                context.Recipes.Update(recipe);
                context.SaveChanges();                
            }

            // Assert
            using (var context = new RecipeDbContext(options))
            {
                var retrievedRecipe = context.Recipes.Include(r => r.Ingredients).Include(r=>r.Instructions).FirstOrDefault(r => r.Title == "Test Recipe");
                Assert.NotNull(retrievedRecipe);
                Assert.Equal("Test Description", retrievedRecipe.Description);
                Assert.NotNull(retrievedRecipe.Instructions);
                Assert.Equal(2, retrievedRecipe.Instructions.Count);
                Assert.Equal("Test Step 1", retrievedRecipe.Instructions.FirstOrDefault(r=>r.Order==1).Instruction);
                Assert.Equal("Test Step 2", retrievedRecipe.Instructions.FirstOrDefault(r=>r.Order==2).Instruction);

                var retrievedRecipeIngredients = context.RecipeIngredients.Include(r => r.Ingredient).Include(r => r.UnitIngredient).Where(r => r.RecipeId == retrievedRecipe.Id).ToList();

                Assert.NotNull(retrievedRecipeIngredients);
                Assert.Equal(2, retrievedRecipeIngredients.Count);
                Assert.NotNull(retrievedRecipeIngredients.FirstOrDefault(r=>r.Ingredient.Name == "Test Ingredient 1"));
                Assert.NotNull(retrievedRecipeIngredients.FirstOrDefault(r=>r.Ingredient.Name == "Test Ingredient 2"));
                Assert.NotNull(retrievedRecipeIngredients.FirstOrDefault(r=>r.Quantity == 1));
                Assert.NotNull(retrievedRecipeIngredients.FirstOrDefault(r=>r.Quantity == 2));
                Assert.NotNull(retrievedRecipeIngredients.FirstOrDefault(r=>r.UnitIngredient.Type == "Test Unit 1"));
                Assert.NotNull(retrievedRecipeIngredients.FirstOrDefault(r=>r.UnitIngredient.Type == "Test Unit 2"));
            }
        }

    }
}