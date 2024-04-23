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

        [HttpGet("{recipeId}")]
        [AllowAnonymous]
        public ActionResult<RecipeDetailsDto> GetRecipe(int recipeId)
        {
            var recipe = _recipeService.GetRecipe(recipeId);
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
            var recipeId = _recipeService.AddRecipe(recipe, user.Id);
            return Created($"/recipes/{recipeId}", null);
        }

        [HttpPost("Clone/{recipeId}")]
        [Authorize]
        public async Task<ActionResult> Clone(int recipeId)
        {
            var user = await _userManager.GetUserAsync(User);
            var newRecipeId = _recipeService.CloneRecipe(recipeId, user.Id);
            return Created($"/Recipe/{newRecipeId}", null);
        }

        [HttpPut("{recipeId}")]
        [Authorize]
        public async Task<ActionResult> Update(int recipeId, [FromBody] UpdateRecipeDto recipe)
        {
            var user = await _userManager.GetUserAsync(User);
            _recipeService.UpdateRecipe(recipeId, recipe, user.Id);
            return Ok();
        }

        [HttpDelete("{recipeId}")]
        [Authorize]
        public async Task<ActionResult> Delete(int recipeId)
        {
            var user = await _userManager.GetUserAsync(User);
            _recipeService.DeleteRecipe(recipeId, user.Id);
            return NoContent();
        }
    }
}