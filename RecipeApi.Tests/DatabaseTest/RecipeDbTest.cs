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
        // [Fact]
        // public void DbContext_CanAddAndRetrieveRecipes()
        // {
        //     // Arrange
        //     var options = new DbContextOptionsBuilder<RecipeDbContext>()
        //         .UseInMemoryDatabase(databaseName: "TestDatabase")
        //         .Options;

        //     // Act
        //     using (var context = new RecipeDbContext(options))
        //     {
        //         // Add a new recipe
        //         var recipe = new Recipe { Title = "Test Recipe", Ins = "Test Description", Instructions = "Test Instructions" };
        //         context.Recipes.Add(recipe);
        //         context.SaveChanges();
        //     }

        //     // Assert
        //     using (var context = new RecipeDbContext(options))
        //     {
        //         var retrievedRecipe = context.Recipes.FirstOrDefault(r => r.Name == "Test Recipe");
        //         Assert.NotNull(retrievedRecipe);
        //         Assert.Equal("Test Description", retrievedRecipe.Description);
        //         Assert.Equal("Test Instructions", retrievedRecipe.Instructions);
        //     }
        // }

        // [Fact]
        // public void DbContext_CanAddAndRetrieveRecipesWithIngredients()
        // {
        //     // Arrange
        //     var options = new DbContextOptionsBuilder<RecipeDbContext>()
        //         .UseInMemoryDatabase(databaseName: "TestDatabase")
        //         .Options;

        //     // Act
        //     using (var context = new RecipeDbContext(options))
        //     {

        //         // Create ingredients
        //         var ingredient1 = new Ingredient { Name = "Test Ingredient 1", Quantity = 1.0, Unit = "Test Unit 1" };
        //         var ingredient2 = new Ingredient { Name = "Test Ingredient 2", Quantity = 2.0, Unit = "Test Unit 2" };

        //         context.Ingredients.AddRange(ingredient1, ingredient2);
        //         context.SaveChanges();

        //         // Add a new recipe
        //         var recipe = new Recipe { Name = "Test Recipe", Description = "Test Description", Instructions = "Test Instructions"};
        //         recipe.Ingredients = new List<Ingredient> { ingredient1, ingredient2 };

        //         context.Recipes.Add(recipe);
        //         context.SaveChanges();
        //     }

        //     // Assert
        //     using (var context = new RecipeDbContext(options))
        //     {
        //         var retrievedRecipe = context.Recipes.Include(r => r.Ingredients).FirstOrDefault(r => r.Name == "Test Recipe");
        //         Assert.NotNull(retrievedRecipe);
        //         Assert.Equal("Test Description", retrievedRecipe.Description);
        //         Assert.Equal("Test Instructions", retrievedRecipe.Instructions);
        //         Assert.NotNull(retrievedRecipe.Ingredients);
        //         Assert.Equal(2, retrievedRecipe.Ingredients.Count);
        //         Assert.Contains(retrievedRecipe.Ingredients, i => i.Name == "Test Ingredient 1");
        //         Assert.Contains(retrievedRecipe.Ingredients, i => i.Name == "Test Ingredient 2");
        //         Assert.Contains(retrievedRecipe.Ingredients, i => i.Quantity == 1);
        //         Assert.Contains(retrievedRecipe.Ingredients, i => i.Quantity == 2);
        //         Assert.Contains(retrievedRecipe.Ingredients, i => i.Unit == "Test Unit 1");
        //         Assert.Contains(retrievedRecipe.Ingredients, i => i.Unit == "Test Unit 2");
        //     }
        // }

    }
}