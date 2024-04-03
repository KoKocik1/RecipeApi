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
using RecipeApi.Service;

namespace RecipeApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public IngredientController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<IngredientDto>> GetIngredients()
        {
            var ingredients = _recipeService.GetIngredients();
            return Ok(ingredients);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<IngredientDto> GetIngredient(int id)
        {
            var ingredient = _recipeService.GetIngredient(id);
            return Ok(ingredient);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddIngredient([FromBody] IngredientDto ingredient)
        {
            var id=_recipeService.AddIngredient(ingredient);
            return Created($"ingredient/{id}",null);
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult UpdateIngredient(int id, [FromBody] IngredientDto ingredient)
        {
            _recipeService.UpdateIngredient(id, ingredient);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeleteIngredient(int id)
        {
            _recipeService.DeleteIngredient(id);
            return NoContent();
        }
    }
}