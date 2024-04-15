using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecipeApi.Controllers;
using RecipeApi.Database;
using RecipeApi.Mapping;
using RecipeApi.Models;
using RecipeApi.Service;

namespace RecipeApi.Tests.RecipeIngredientTest
{
    public class RecipeIngredientControllerTest
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RecipeIngredientService> _logger;
        private readonly ILogger<RecipeIngredientController> _loggerController;
        DbContextOptions<RecipeDbContext> _options;
        public RecipeIngredientControllerTest()
        {

            _options = new DbContextOptionsBuilder<RecipeDbContext>()
                        .UseInMemoryDatabase(databaseName: "DatabaseTest")
                        .Options;

            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<RecipeMappingProfile>()));
            _loggerController = new LoggerFactory().CreateLogger<RecipeIngredientController>();
            _logger = new LoggerFactory().CreateLogger<RecipeIngredientService>();

            cleanDatabase();
            createTestData();

        }
        private void cleanDatabase()
        {

        }
        private void createTestData()
        {

            UnitIngredient unitIngredient1 = new UnitIngredient {Type = "Kg" };
            UnitIngredient unitIngredient2 = new UnitIngredient {Type = "g" };

            Ingredient ingredient1 = new Ingredient { Name = "Ingredient1", Verified = false };
            Ingredient ingredient2 = new Ingredient { Name = "Ingredient2", Verified = false };
            Ingredient ingredient3 = new Ingredient { Name = "Ingredient3", Verified = false };
            Ingredient ingredient4 = new Ingredient { Name = "Ingredient4", Verified = false };

            User user = new User{
                    Email = "recipeIngredient@recipeIngredient.pl",
                    PasswordHash = "1234",
                    RoleId = 1,
                    FirstName = "RecipeIngredient",
                    LastName = "RecipeIngredient",
                    UserName = "RecipeIngredient",
                    DateOfBirth = DateTime.Now.ToUniversalTime(),
                    Nationality = "Test"
            };

            using (var context = new RecipeDbContext(_options))
            {
                context.UnitIngredients.AddRange(new List<UnitIngredient>
                {
                    unitIngredient1,
                    unitIngredient2
                });
                context.Ingredients.AddRange(new List<Ingredient>
                {
                    ingredient1,
                    ingredient2,
                    ingredient3,
                    ingredient4
                });
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        //get all recipeIngredients
        [Fact]
        public void GetRecipeIngredients_ReturnsOkObjectResult_WithListOfRecipeIngredients()
        {
            // Set up in-memory database options
            using (var context = new RecipeDbContext(_options))
            {
                // Arrange
                var service = new RecipeIngredientService(context, _mapper, _logger);
                var controller = new RecipeIngredientController(_loggerController, service);

                controller.Create(new CreateRecipeIngredientToExistingRecipeDto { RecipeId = 1, IngredientId = 1, UnitIngredientId = 1, Quantity = 1 });
                controller.Create(new CreateRecipeIngredientToExistingRecipeDto { RecipeId = 2, IngredientId = 2, UnitIngredientId = 2, Quantity = 2 });
                controller.Create(new CreateRecipeIngredientToExistingRecipeDto { RecipeId = 1, IngredientId = 3, UnitIngredientId = 1, Quantity = 3 });
                controller.Create(new CreateRecipeIngredientToExistingRecipeDto { RecipeId = 2, IngredientId = 4, UnitIngredientId = 2, Quantity = 4 });

                // Act
                var result = controller.GetByRecipeId(1);

                // Assert
                var actionResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnValue = Assert.IsType<List<RecipeIngredientDto>>(actionResult.Value);
                Assert.Equal(2, returnValue.Count);
            }
        }
    }
}