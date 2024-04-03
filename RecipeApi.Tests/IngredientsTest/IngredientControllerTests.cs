using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RecipeApi.Controllers;
using RecipeApi.Exceptions;
using RecipeApi.IService;
using RecipeApi.Models;

namespace RecipeApi.Tests.IngredientsTest
{
    public class IngredientControllerTests
    {

        private readonly Mock<IIngrededientService> _mockService;
        private readonly IngredientController _controller;
        public IngredientControllerTests()
        {
            _mockService = new Mock<IIngrededientService>();
            _controller = new IngredientController(_mockService.Object);
        }

        [Fact]
        public void GetIngredients_ReturnsOkObjectResult_WithListOfIngredients()
        {
            // Arrange
            var mockIngredients = new List<IngredientDto>
            {
                new IngredientDto { Id = 1, Name = "Salt" },
                new IngredientDto { Id = 2, Name = "Sugar" }
            };

            _mockService.Setup(s => s.GetIngredients()).Returns(mockIngredients);

            // Act
            var result = _controller.GetIngredients();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<IngredientDto>>(actionResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public void GetIngredient_ReturnsOkObjectResult_WithIngredient()
        {
            // Arrange
            var ingredientId = 1;
            var mockIngredient = new IngredientDto { Id = ingredientId, Name = "Salt" };

            _mockService.Setup(s => s.GetIngredient(ingredientId)).Returns(mockIngredient);

            // Act
            var result = _controller.GetIngredient(ingredientId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<IngredientDto>(actionResult.Value);
            Assert.Equal(ingredientId, returnValue.Id);
        }

        [Fact]
        public void GetIngredient_NotFound_ReturnsNotFoundResult()
        {
            try
            {
                // Arrange
                _mockService.Setup(s => s.GetIngredient(It.IsAny<int>())).Throws(new NotFoundException("Ingredient not found"));

                // Act
                var result = _controller.GetIngredient(999);
            }
            catch (NotFoundException e)
            {
                Assert.Equal("Ingredient not found", e.Message);
            }
        }

        [Fact]
        public void AddIngredient_ReturnsCreatedResult()
        {
            // Arrange
            var newIngredient = new CreateIngredientDto { Name = "Pepper" };
            _mockService.Setup(s => s.AddIngredient(newIngredient)).Returns(1); // Assuming the new ID is 1

            // Act
            var result = _controller.AddIngredient(newIngredient);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.NotNull(createdResult.Location);
            Assert.Equal($"/ingredient/1", createdResult.Location);
        }

        [Fact]
        public void DeleteIngredient_NotFound_ReturnsNotFoundResult()
        {
            try{
                // Arrange
                _mockService.Setup(s => s.DeleteIngredient(It.IsAny<int>())).Throws(new NotFoundException("Ingredient not found"));

                // Act
                var result = _controller.DeleteIngredient(999);
            }
            catch (NotFoundException e)
            {
                Assert.Equal("Ingredient not found", e.Message);
            }            
        }
    }
}