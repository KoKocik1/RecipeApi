using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeApi.Database;
using RecipeApi.IService;
using RecipeApi.Models;

namespace RecipeApi.Controllers
{
    [Route("api/recipe-instruction")]
    [ApiController]
    public class RecipeInstructionController : ControllerBase
    {
        private readonly ILogger<RecipeInstructionController> _logger;
        private readonly IRecipeInstructionService _instructionService;

        public RecipeInstructionController(ILogger<RecipeInstructionController> logger, IRecipeInstructionService instructionService)
        {
            _logger = logger;
            _instructionService = instructionService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RecipeInstructionDto>> Get()
        {
            var instructions= _instructionService.GetRecipeInstructions();
            return Ok(instructions);
        }

        [HttpGet("{id}")]
        public ActionResult<RecipeInstruction> Get(int id)
        {
            var instruction = _instructionService.GetRecipeInstruction(id);
            return Ok(instruction);
        }

        [HttpGet("recipe/{recipeId}")]
        public ActionResult<IEnumerable<RecipeInstruction>> GetInstructionsForRecipe(int recipeId)
        {
            var instructions = _instructionService.GetRecipeInstructionsByRecipeId(recipeId);
            return Ok(instructions);
        }
        [HttpPost]
        public ActionResult Create([FromBody] CreateRecipeInstructionDto instruction)
        {
            var id = _instructionService.AddRecipeInstruction(instruction);
            return Created($"/instruction/{id}", null);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] RecipeInstructionDto instruction)
        {
            _instructionService.UpdateRecipeInstruction(id, instruction);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _instructionService.DeleteRecipeInstruction(id);
            return Ok();
        }

    }
}