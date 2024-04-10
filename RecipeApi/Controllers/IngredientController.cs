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
    [Route("api/ingredient")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngrededientService _ingredientService;

        public IngredientController(IIngrededientService ingrededientService)
        {
            _ingredientService = ingrededientService;
        }

        [HttpGet]
        // [AllowAnonymous]
        public ActionResult<IEnumerable<IngredientDto>> GetIngredients()
        {
            var ingredients = _ingredientService.GetIngredients();
            return Ok(ingredients);
        }

        [HttpGet("{id}")]
        //  [AllowAnonymous]
        public ActionResult<IngredientDto> GetIngredient(int id)
        {
            var ingredient = _ingredientService.GetIngredient(id);
            return Ok(ingredient);
        }

        [HttpPost]
        // [Authorize]
        public ActionResult AddIngredient([FromBody] CreateIngredientDto ingredient)
        {
            var id=_ingredientService.AddIngredient(ingredient);
            return Created($"/ingredient/{id}",null);
        }

        [HttpPut("{id}")]
        // [Authorize]
        public ActionResult UpdateIngredient(int id, [FromBody] UpdateIngredientDto ingredient)
        {
            _ingredientService.UpdateIngredient(id, ingredient);
            return Ok();
        }

        [HttpDelete("{id}")]
        // [Authorize]
        public ActionResult DeleteIngredient(int id)
        {
            _ingredientService.DeleteIngredient(id);
            return NoContent();
        }
    }
}