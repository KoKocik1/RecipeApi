using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeApi.IService;
using RecipeApi.Models;

namespace RecipeApi.Controllers
{
    [Route("recipes")]
    [ApiController]
    public class RecipeController : Controller
    {
        private readonly ILogger<RecipeController> _logger;
        private readonly IRecipeService _recipeService;

        public RecipeController(ILogger<RecipeController> logger, IRecipeService recipeService)
        {
            _logger = logger;
            _recipeService = recipeService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RecipeDto>> GetRecipes()
        {
            var recipes = _recipeService.GetRecipes();
            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public ActionResult<RecipeDetailsDto> GetRecipe(int id)
        {
            var recipe = _recipeService.GetRecipe(id);
            return Ok(recipe);
        }

        [HttpGet("author/{authorId}")]
        public ActionResult<IEnumerable<RecipeDto>> GetRecipesByAuthor(int authorId)
        {
            var recipes = _recipeService.GetRecipesByAuthor(authorId);
            return Ok(recipes);
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateRecipeDto recipe)
        {
            var id = _recipeService.AddRecipe(recipe);
            return Created($"/recipes/{id}", null);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] UpdateRecipeDto recipe)
        {
            _recipeService.UpdateRecipe(id, recipe);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _recipeService.DeleteRecipe(id);
            return NoContent();
        }
    }
}