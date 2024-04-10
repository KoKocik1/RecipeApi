using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RecipeApi.Controllers;
using RecipeApi.Database;
using RecipeApi.Exceptions;
using RecipeApi.IService;
using RecipeApi.Mapping;
using RecipeApi.Models;
using RecipeApi.Service;

namespace RecipeApi.Tests.IngredientsTest
{
    public class IngredientControllerTests
    {
        private readonly IMapper _mapper;
        private readonly ILogger<IngredientService> _logger;

        public IngredientControllerTests()
        {
            // Create a mapper instance (you can use AutoMapper or a similar library)
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<RecipeMappingProfile>()));

            // Create a logger instance
            _logger = new LoggerFactory().CreateLogger<IngredientService>();

        }

        //get all ingredients
        [Fact]
        public void GetIngredients_ReturnsOkObjectResult_WithListOfIngredients()
        {
            // Set up in-memory database options
            DbContextOptions<RecipeDbContext> _options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: "GetIngredients_ReturnsOkObjectResult_WithListOfIngredients")
                .Options;

            using (var context = new RecipeDbContext(_options))
            {
                // Arrange
                var service = new IngredientService(context, _mapper, _logger);
                var controller = new IngredientController(service);


                controller.AddIngredient(new CreateIngredientDto { Name = "Salt" });
                controller.AddIngredient(new CreateIngredientDto { Name = "Sugar" });

                // Act
                var result = controller.GetIngredients();

                // Assert
                var actionResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnValue = Assert.IsType<List<IngredientDto>>(actionResult.Value);
                Assert.Equal(2, returnValue.Count);
            }
        }

        //get specific ingredient
        [Fact]
        public void GetIngredient_ReturnsOkObjectResult_WithIngredient()
        {
            // Set up in-memory database options
            DbContextOptions<RecipeDbContext> _options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: "GetIngredient_ReturnsOkObjectResult_WithIngredient") //TODO
                .Options;

            // Arrange
            var mockIngredient = new CreateIngredientDto { Name = "Salt" };

            using (var context = new RecipeDbContext(_options))
            {
                var service = new IngredientService(context, _mapper, _logger);
                var controller = new IngredientController(service);

                var addResult = controller.AddIngredient(mockIngredient) as CreatedResult;
                var ingredientId = int.Parse(addResult.Location.Split("/").Last());  //TODO

                // Act
                var result = controller.GetIngredient(ingredientId);

                // Assert
                var actionResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnValue = Assert.IsType<IngredientDto>(actionResult.Value);
                Assert.Equal(ingredientId, returnValue.Id);
            }
        }

        //get ingredient not found
        [Fact]
        public void GetIngredient_NotFound_ReturnsNotFoundResult()
        {
            // Set up in-memory database options
            DbContextOptions<RecipeDbContext> _options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: "GetIngredient_NotFound_ReturnsNotFoundResult")
                .Options;

            // Arrange
            var ingredientId = 999;

            using (var context = new RecipeDbContext(_options))
            {
                var service = new IngredientService(context, _mapper, _logger);
                var controller = new IngredientController(service);

                // Act & Assert
                var ex = Assert.Throws<NotFoundException>(() => controller.GetIngredient(ingredientId));
                Assert.Equal("Ingredient not found", ex.Message);
            }
        }

        //add ingredient created
        [Fact]
        public void AddIngredient_ReturnsCreatedResult()
        {
            // Set up in-memory database options
            DbContextOptions<RecipeDbContext> _options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: "AddIngredient_ReturnsCreatedResult")
                .Options;

            // Arrange
            var newIngredient = new CreateIngredientDto { Name = "Pepper" };

            using (var context = new RecipeDbContext(_options))
            {
                var service = new IngredientService(context, _mapper, _logger);
                var controller = new IngredientController(service);

                // Act
                var result = controller.AddIngredient(newIngredient);

                // Assert
                var createdResult = Assert.IsType<CreatedResult>(result);
                Assert.NotNull(createdResult.Location);
                Assert.Equal($"/ingredient/1", createdResult.Location);
            }
        }

        //add ingredient already exists
        [Fact]
        public void AddIngredient_ReturnsBadRequestException()
        {
            // Set up in-memory database options
            DbContextOptions<RecipeDbContext> _options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: "AddIngredient_ReturnsBadRequestException")
                .Options;

            // Arrange
            var newIngredient = new CreateIngredientDto { Name = "Pepper" };

            using (var context = new RecipeDbContext(_options))
            {
                var service = new IngredientService(context, _mapper, _logger);
                var controller = new IngredientController(service);

                controller.AddIngredient(newIngredient);

                // Act & Assert
                var ex = Assert.Throws<BadRequestException>(() => controller.AddIngredient(newIngredient));
                Assert.Equal("Ingredient already exists", ex.Message);
            }
        }

        //delete ingredient not found
        [Fact]
        public void DeleteIngredient_NotFound_ReturnsNotFoundResult()
        {
            // Set up in-memory database options
            DbContextOptions<RecipeDbContext> _options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteIngredient_NotFound_ReturnsNotFoundResult")
                .Options;

            // Arrange
            var ingredientId = 999;

            using (var context = new RecipeDbContext(_options))
            {
                var service = new IngredientService(context, _mapper, _logger);
                var controller = new IngredientController(service);

                // Act & Assert
                var ex = Assert.Throws<NotFoundException>(() => controller.DeleteIngredient(ingredientId));
                Assert.Equal("Ingredient not found", ex.Message);
            }
        }

        //delete ingredient found
        [Fact]
        public void DeleteIngredient_ReturnsNoContentResult()
        {
            // Set up in-memory database options
            DbContextOptions<RecipeDbContext> _options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteIngredient_ReturnsNoContentResult")
                .Options;

            // Arrange
            var mockIngredient = new CreateIngredientDto { Name = "Salt" };

            using (var context = new RecipeDbContext(_options))
            {
                var service = new IngredientService(context, _mapper, _logger);
                var controller = new IngredientController(service);

                var addResult = controller.AddIngredient(mockIngredient) as CreatedResult;
                var ingredientId = int.Parse(addResult.Location.Split("/").Last());  //TODO

                // Act
                var result = controller.DeleteIngredient(ingredientId);

                // Assert
                Assert.IsType<NoContentResult>(result);
                var ex1 = Assert.Throws<NotFoundException>(() => controller.GetIngredient(ingredientId));
                Assert.Equal("Ingredient not found", ex1.Message);
            }
        }

        //update ingredient not found
        [Fact]
        public void UpdateIngredient_NotFound_ReturnsNotFoundResult()
        {
            // Set up in-memory database options
            DbContextOptions<RecipeDbContext> _options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: "UpdateIngredient_NotFound_ReturnsNotFoundResult")
                .Options;

            // Arrange
            var ingredientDto = new UpdateIngredientDto { Name = "Pepper" };
            var notExistingId = 999;

            using (var context = new RecipeDbContext(_options))
            {
                var service = new IngredientService(context, _mapper, _logger);
                var controller = new IngredientController(service);

                // Act & Assert
                var ex = Assert.Throws<NotFoundException>(() => controller.UpdateIngredient(notExistingId, ingredientDto));
                Assert.Equal("Ingredient not found", ex.Message);
            }
        }

        //update ingredient found
        [Fact]
        public void UpdateIngredient_ReturnsNoContentResult()
        {
            // Set up in-memory database options
            DbContextOptions<RecipeDbContext> _options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: "UpdateIngredient_ReturnsNoContentResult")
                .Options;

            // Arrange
            var name1= "Salt";
            var name2 = "Pepper";
            var createIngredientDto = new CreateIngredientDto { Name = name1 };
            using (var context = new RecipeDbContext(_options))
            {
                var service = new IngredientService(context, _mapper, _logger);
                var controller = new IngredientController(service);

                var addResult = controller.AddIngredient(createIngredientDto) as CreatedResult;
                var ingredientId = int.Parse(addResult.Location.Split("/").Last());  //TODO

                var updatedIngredient = new UpdateIngredientDto { Name = name2 };

                // Act
                var resultUpdate = controller.UpdateIngredient(ingredientId, updatedIngredient);
                var resultGet = controller.GetIngredient(ingredientId);

                // Assert
                Assert.IsType<OkResult>(resultUpdate);
                var actionResult = Assert.IsType<OkObjectResult>(resultGet.Result);
                var returnValue = Assert.IsType<IngredientDto>(actionResult.Value);
                Assert.NotEqual(name1, returnValue.Name);
                Assert.Equal(name2, returnValue.Name); 
            }
        }

    }
}