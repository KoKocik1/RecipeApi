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
    public class IngredientControllerTests //: IDisposable
    {
        // private readonly IMapper _mapper;
        // private readonly ILogger<IngredientService> _logger;

        // DbContextOptions<RecipeDbContext> _options;

        // CreateIngredientDto mockIngredient1;
        // CreateIngredientDto mockIngredient2;
        // public IngredientControllerTests()
        // {
        //     mockIngredient1 = new CreateIngredientDto { Name = "IngredientControllerTests1" };
        //     mockIngredient2 = new CreateIngredientDto { Name = "IngredientControllerTests2" };

        //     _options = new DbContextOptionsBuilder<RecipeDbContext>()
        //                 .UseInMemoryDatabase(databaseName: "DatabaseTest")
        //                 .Options;

        //     _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<RecipeMappingProfile>()));
        //     _logger = new LoggerFactory().CreateLogger<IngredientService>();
        //     cleanDatabase();


        // }

        // private void cleanDatabase()
        // {
        //     using (var context = new RecipeDbContext(_options))
        //     {
        //         var ingredientToRemove = context.Ingredients.Where(i => i.Name == mockIngredient1.Name);
        //         context.Ingredients.RemoveRange(ingredientToRemove);
        //         ingredientToRemove = context.Ingredients.Where(i => i.Name == mockIngredient2.Name);
        //         context.Ingredients.RemoveRange(ingredientToRemove);
        //         context.SaveChanges();
        //     }
        // }
        // public void Dispose()
        // {
        // }
        
        // //get all ingredients
        // [Fact]
        // public void GetIngredients_ReturnsOkObjectResult()
        // {
        //     // Set up in-memory database options
        //     using (var context = new RecipeDbContext(_options))
        //     {
        //         // Arrange
        //         var service = new IngredientService(context, _mapper, _logger);
        //         var controller = new IngredientController(service);

        //         var countOfIngredients = context.Ingredients.Count();

        //         controller.AddIngredient(mockIngredient1);
        //         controller.AddIngredient(mockIngredient2);

        //         // Act
        //         var result = controller.GetIngredients();

        //         // Assert
        //         var actionResult = Assert.IsType<OkObjectResult>(result.Result);
        //         var returnValue = Assert.IsType<List<IngredientDto>>(actionResult.Value);
        //         Assert.Equal(countOfIngredients + 2, returnValue.Count);
        //     }
        // }

        // //get specific ingredient
        // [Fact]
        // public void GetIngredient_ReturnsOkObjectResult()
        // {
        //     using (var context = new RecipeDbContext(_options))
        //     {
        //         var service = new IngredientService(context, _mapper, _logger);
        //         var controller = new IngredientController(service);

        //         var result = controller.AddIngredient(mockIngredient1);
        //         var addResult=result as CreatedResult;
        //         var ingredientId = int.Parse(addResult.Location.Split("/").Last());  //TODO

        //         // Act
        //         var result1 = controller.GetIngredient(ingredientId);

        //         // Assert
        //         var actionResult = Assert.IsType<OkObjectResult>(result1.Result);
        //         var returnValue = Assert.IsType<IngredientDto>(actionResult.Value);
        //         Assert.Equal(mockIngredient1.Name, returnValue.Name);
        //     }
        // }

        // //get ingredient not found
        // [Fact]
        // public void GetIngredient_NotFound_ReturnsNotFoundResult()
        // {
        //     // Arrange
        //     var ingredientId = 999;

        //     using (var context = new RecipeDbContext(_options))
        //     {
        //         var service = new IngredientService(context, _mapper, _logger);
        //         var controller = new IngredientController(service);

        //         // Act & Assert
        //         var ex = Assert.Throws<NotFoundException>(() => controller.GetIngredient(ingredientId));
        //         Assert.Equal("Ingredient not found", ex.Message);
        //     }
        // }

        // //add ingredient created
        // [Fact]
        // public void AddIngredient_ReturnsCreatedResult()
        // {
        //     // Arrange
        //     //var newIngredient = new CreateIngredientDto { Name = "Pepper" };

        //     using (var context = new RecipeDbContext(_options))
        //     {
        //         var service = new IngredientService(context, _mapper, _logger);
        //         var controller = new IngredientController(service);

        //         // Act
        //         var result = controller.AddIngredient(mockIngredient1);
        //         var addResult = result as CreatedResult;
        //         var ingredientId = int.Parse(addResult.Location.Split("/").Last());  //TODO

        //         // Assert
        //         var createdResult = Assert.IsType<CreatedResult>(result);
        //         Assert.NotNull(createdResult.Location);
        //         Assert.Equal($"/ingredient/{ingredientId}", createdResult.Location);
        //     }
        // }

        // //add ingredient already exists
        // [Fact]
        // public void AddIngredient_ReturnsBadRequestException()
        // {
        //     // Arrange
        //     //var newIngredient = new CreateIngredientDto { Name = "Pepper" };

        //     using (var context = new RecipeDbContext(_options))
        //     {
        //         var service = new IngredientService(context, _mapper, _logger);
        //         var controller = new IngredientController(service);

        //         controller.AddIngredient(mockIngredient1);

        //         // Act & Assert
        //         var ex = Assert.Throws<BadRequestException>(() => controller.AddIngredient(mockIngredient1));
        //         Assert.Equal("Ingredient already exists", ex.Message);
        //     }
        // }

        // //delete ingredient not found
        // [Fact]
        // public void DeleteIngredient_NotFound_ReturnsNotFoundResult()
        // {
        //     // Arrange
        //     var ingredientId = 999;

        //     using (var context = new RecipeDbContext(_options))
        //     {
        //         var service = new IngredientService(context, _mapper, _logger);
        //         var controller = new IngredientController(service);

        //         // Act & Assert
        //         var ex = Assert.Throws<NotFoundException>(() => controller.DeleteIngredient(ingredientId));
        //         Assert.Equal("Ingredient not found", ex.Message);
        //     }
        // }

        // //delete ingredient found
        // [Fact]
        // public void DeleteIngredient_ReturnsNoContentResult()
        // {
        //     using (var context = new RecipeDbContext(_options))
        //     {
        //         var service = new IngredientService(context, _mapper, _logger);
        //         var controller = new IngredientController(service);

        //         var addResult = controller.AddIngredient(mockIngredient1) as CreatedResult;
        //         var ingredientId = int.Parse(addResult.Location.Split("/").Last());  //TODO

        //         // Act
        //         var result = controller.DeleteIngredient(ingredientId);

        //         // Assert
        //         Assert.IsType<NoContentResult>(result);
        //         var ex1 = Assert.Throws<NotFoundException>(() => controller.GetIngredient(ingredientId));
        //         Assert.Equal("Ingredient not found", ex1.Message);
        //     }
        // }

        // //update ingredient not found
        // [Fact]
        // public void UpdateIngredient_NotFound_ReturnsNotFoundResult()
        // {
        //     // Arrange
        //     var ingredientDto = new UpdateIngredientDto { Name = "NoFoundExample" };
        //     var notExistingId = 999;

        //     using (var context = new RecipeDbContext(_options))
        //     {
        //         var service = new IngredientService(context, _mapper, _logger);
        //         var controller = new IngredientController(service);

        //         // Act & Assert
        //         var ex = Assert.Throws<NotFoundException>(() => controller.UpdateIngredient(notExistingId, ingredientDto));
        //         Assert.Equal("Ingredient not found", ex.Message);
        //     }
        // }

        // //update ingredient found
        // [Fact]
        // public void UpdateIngredient_ReturnsNoContentResult()
        // {
        //     // Arrange
        //     using (var context = new RecipeDbContext(_options))
        //     {
        //         var service = new IngredientService(context, _mapper, _logger);
        //         var controller = new IngredientController(service);

        //         var addResult = controller.AddIngredient(mockIngredient1) as CreatedResult;
        //         var ingredientId = int.Parse(addResult.Location.Split("/").Last());  //TODO

        //         var updatedIngredient = new UpdateIngredientDto { Name = mockIngredient2.Name };

        //         // Act
        //         var resultUpdate = controller.UpdateIngredient(ingredientId, updatedIngredient);
        //         var resultGet = controller.GetIngredient(ingredientId);

        //         // Assert
        //         Assert.IsType<OkResult>(resultUpdate);
        //         var actionResult = Assert.IsType<OkObjectResult>(resultGet.Result);
        //         var returnValue = Assert.IsType<IngredientDto>(actionResult.Value);
        //         Assert.Equal(mockIngredient2.Name, returnValue.Name);
        //     }
        // }

    }
}