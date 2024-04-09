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
    [Route("api/recipeingredient")]
    [ApiController]
    public class RecipeIngredientController : Controller
    {
        private readonly ILogger<RecipeIngredientController> _logger;
        private readonly IRecipeIngredientService _ingredientService;

        public RecipeIngredientController(ILogger<RecipeIngredientController> logger, IRecipeIngredientService ingredientService)
        {
            _logger = logger;
            _ingredientService = ingredientService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RecipeIngredientDto>> GetByRecipeId([FromQuery] int recipeId = 0)
        {
                var ingredients = _ingredientService.GetRecipeIngredients(recipeId);
                return Ok(ingredients);
        }

        [HttpGet("{id}")]
        public ActionResult<RecipeIngredientDto> GetById(int id)
        {
            var ingredient = _ingredientService.GetRecipeIngredient(id);
            return Ok(ingredient);
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateRecipeIngredientDto ingredient)
        {
            var id = _ingredientService.AddRecipeIngredient(ingredient);
            return Created($"/recipeingredient/{id}", null);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _ingredientService.DeleteRecipeIngredient(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] CreateRecipeIngredientDto ingredient)
        {
            _ingredientService.UpdateRecipeIngredient(id, ingredient);
            return Ok();
        }
        
    }
}