using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeApi.IService;
using RecipeApi.Models;

namespace RecipeApi.Controllers
{
    [Route("api/recipe-ingredient")]
    [ApiController]
    [Authorize]
    public class RecipeIngredientController : Controller
    {
        private readonly ILogger<RecipeIngredientController> _logger;
        private readonly IRecipeIngredientService _ingredientService;

        public RecipeIngredientController(ILogger<RecipeIngredientController> logger, IRecipeIngredientService ingredientService)
        {
            _logger = logger;
            _ingredientService = ingredientService;
        }

        [HttpGet("recipe/{recipeId}")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<RecipeIngredientDto>> GetByRecipeId(int recipeId)
        {
                var ingredients = _ingredientService.GetRecipeIngredients(recipeId);
                return Ok(ingredients);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<RecipeIngredientDto> GetById(int id)
        {
            var ingredient = _ingredientService.GetRecipeIngredient(id);
            return Ok(ingredient);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create([FromBody] CreateRecipeIngredientToExistingRecipeDto ingredient)
        {
            var id = _ingredientService.AddRecipeIngredient(ingredient);
            return Created($"/recipe-ingredient/{id}", null);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete(int id)
        {
            _ingredientService.DeleteRecipeIngredient(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult Update(int id, [FromBody] UpdateRecipeIngredientDto ingredient)
        {
            _ingredientService.UpdateRecipeIngredient(id, ingredient);
            return Ok();
        }
        
    }
}