using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeApi.Database;
using RecipeApi.IService;
using RecipeApi.Models;

namespace RecipeApi.Controllers
{
    [Route("api/recipe-instruction")]
    [ApiController]
    [Authorize]
    public class RecipeInstructionController : ControllerBase
    {
        private readonly ILogger<RecipeInstructionController> _logger;
        private readonly IRecipeInstructionService _instructionService;

        public RecipeInstructionController(ILogger<RecipeInstructionController> logger, IRecipeInstructionService instructionService)
        {
            _logger = logger;
            _instructionService = instructionService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<RecipeInstruction> Get(int id)
        {
            var instruction = _instructionService.GetRecipeInstruction(id);
            return Ok(instruction);
        }

        [HttpGet("recipe/{recipeId}")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<RecipeInstruction>> GetInstructionsForRecipe(int recipeId)
        {
            var instructions = _instructionService.GetRecipeInstructionsByRecipeId(recipeId);
            return Ok(instructions);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Create([FromBody] CreateRecipeInstructionToExistingRecipeDto instruction)
        {
            var id = _instructionService.AddRecipeInstruction(instruction);
            return Created($"/recipe-instruction/{id}", null);
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult Update(int id, [FromBody] UpdateRecipeInstructionDto instruction)
        {
            _instructionService.UpdateRecipeInstruction(id, instruction);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete(int id)
        {
            _instructionService.DeleteRecipeInstruction(id);
            return Ok();
        }

    }
}