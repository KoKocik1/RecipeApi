using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeApi.Database;
using RecipeApi.IService;
using RecipeApi.Models;

namespace RecipeApi.Controllers
{
    [Route("Recipe")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly ILogger<RecipeController> _logger;
        private readonly IRecipeService _recipeService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecipeController(
            ILogger<RecipeController> logger,
            IRecipeService recipeService,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _recipeService = recipeService;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<RecipeDto>> GetRecipes()
        {
            var recipes = _recipeService.GetRecipes();
            return Ok(recipes);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<RecipeDetailsDto> GetRecipe(int id)
        {
            var recipe = _recipeService.GetRecipe(id);
            return Ok(recipe);
        }

        [HttpGet("author/{authorId}")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<RecipeDto>> GetRecipesByAuthor(string authorId)
        {
            var recipes = _recipeService.GetRecipesByAuthor(authorId);
            return Ok(recipes);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create([FromBody] CreateRecipeDto recipe)
        {
            var user = await _userManager.GetUserAsync(User);
            var id = _recipeService.AddRecipe(recipe, user.Id);
            return Created($"/recipes/{id}", null);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateRecipeDto recipe)
        {
            var user = await _userManager.GetUserAsync(User);
            _recipeService.UpdateRecipe(id, recipe, user.Id);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            _recipeService.DeleteRecipe(id, user.Id);
            return NoContent();
        }
    }
}