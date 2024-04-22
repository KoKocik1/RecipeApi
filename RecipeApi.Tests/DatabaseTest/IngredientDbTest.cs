using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecipeApi.Database;
using Xunit;

namespace RecipeApi.Tests
{
    public class IngredientDbTest
    {
        [Fact]
        public void DbContext_CanAddAndRetrieveIngredient()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: "DatabaseTest")
                .Options;

            // Act
            using (var context = new RecipeDbContext(options))
            {
                // Add a new ingredient
                var ingredient = new Ingredient { Name = "Test Ingredient"};
                context.Ingredients.Add(ingredient);
                context.SaveChanges();
            
                var retrievedIngredient = context.Ingredients.FirstOrDefault(i => i.Name == "Test Ingredient");
                Assert.NotNull(retrievedIngredient);

                context.Ingredients.Remove(retrievedIngredient);
            }
        }

    }
}